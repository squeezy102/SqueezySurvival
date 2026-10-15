#Requires -Version 5.1
<#
.SYNOPSIS
    Builds, tests, packages and publishes a SqueezySurvival release.
.DESCRIPTION
    Implements Docs/Engineering/ReleasePipeline.md. Any failing step stops the release.
.PARAMETER Version
    Release version in MAJOR.MINOR.PATCH form. Defaults to the contents of the VERSION file.
#>
[CmdletBinding()]
param(
    [string] $Version
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$RepositoryRoot = Split-Path -Parent $PSScriptRoot

if (-not $Version) {
    $Version = (Get-Content -LiteralPath (Join-Path $RepositoryRoot 'VERSION') -TotalCount 1).Trim()
}
if ($Version -notmatch '^\d+\.\d+\.\d+$') {
    throw "Version must be MAJOR.MINOR.PATCH, but was '$Version'."
}
$OutputDirectory = Join-Path $PSScriptRoot 'Output'
$PipelineLogFile = Join-Path $OutputDirectory 'Publish-Release.log'
$UnityTestLogFile = Join-Path $OutputDirectory 'Unity-Tests.log'
$UnityTestResultsFile = Join-Path $OutputDirectory 'Unity-TestResults.xml'
$UnityBuildLogFile = Join-Path $OutputDirectory 'Unity-Build.log'

$ReleasesRepositoryUrl = 'https://github.com/squeezy102/SqueezySurvival-Releases'
$TokenVariableName = 'SQUEEZYSURVIVAL_RELEASES_TOKEN'
$PackageId = 'Squeezy102.SqueezySurvival'
$PackageTitle = 'SqueezySurvival'
$PackageAuthors = 'squeezy102'
$LauncherExecutableFileName = 'SqueezySurvivalLauncher.exe'
$PackageExcludePattern = '.*\.pdb'
$PackageRuntime = 'win-x64'
$ReleaseTag = "v$Version"

$LauncherSolution = Join-Path $RepositoryRoot 'Launcher\SqueezySurvival.Launcher.sln'
$LauncherProject = Join-Path $RepositoryRoot 'Launcher\SqueezySurvival.Launcher\SqueezySurvival.Launcher.csproj'
$StagingDirectory = Join-Path $RepositoryRoot "Builds\Release\$Version\Package"
$GameDirectory = Join-Path $StagingDirectory 'Game'
$ReleasesDirectory = Join-Path $RepositoryRoot 'Builds\Releases'

function Write-Step([string] $Name) {
    Write-Host ''
    Write-Host "== $Name"
}

# Runs a console tool and fails the release on a non-zero exit code.
function Invoke-NativeCommand([string] $StepName, [string] $FilePath, [string[]] $Arguments) {
    & $FilePath @Arguments | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "$StepName failed with exit code $LASTEXITCODE."
    }
}

# Unity.exe is a GUI application, so PowerShell does not wait for it unless started with -Wait.
function Invoke-UnityEditor([string] $StepName, [string] $UnityEditorPath, [string[]] $Arguments, [string] $LogFile) {
    $quotedArguments = $Arguments | ForEach-Object { if ($_ -match '\s') { '"' + $_ + '"' } else { $_ } }
    $process = Start-Process -FilePath $UnityEditorPath -ArgumentList $quotedArguments -Wait -PassThru -NoNewWindow
    if ($process.ExitCode -ne 0) {
        throw "$StepName failed with exit code $($process.ExitCode). See $LogFile."
    }
}

function Get-UnityEditorPath {
    if ($env:UNITY_EDITOR_PATH) {
        $editorPath = $env:UNITY_EDITOR_PATH
    }
    else {
        $versionLine = Get-Content (Join-Path $RepositoryRoot 'ProjectSettings\ProjectVersion.txt') |
            Where-Object { $_ -like 'm_EditorVersion:*' } |
            Select-Object -First 1
        $editorVersion = ($versionLine -split ':\s*', 2)[1].Trim()
        $editorPath = Join-Path $env:ProgramFiles "Unity\Hub\Editor\$editorVersion\Editor\Unity.exe"
    }

    if (-not (Test-Path -LiteralPath $editorPath)) {
        throw "Unity Editor not found at '$editorPath'. Set the UNITY_EDITOR_PATH environment variable to Unity.exe."
    }

    return $editorPath
}

function Assert-UnityEditorClosed {
    $lockFile = Join-Path $RepositoryRoot 'Temp\UnityLockfile'
    if (-not (Test-Path -LiteralPath $lockFile)) {
        return
    }

    try {
        $stream = [System.IO.File]::Open($lockFile, 'Open', 'ReadWrite', 'None')
        $stream.Close()
    }
    catch {
        throw 'The Unity Editor has this project open. Close it and run the release again.'
    }
}

New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
Start-Transcript -Path $PipelineLogFile -Force | Out-Null
Push-Location $RepositoryRoot
try {
    Write-Step "Release $ReleaseTag"

    Write-Step '1. Validate token'
    $token = [Environment]::GetEnvironmentVariable($TokenVariableName, 'User')
    if ([string]::IsNullOrWhiteSpace($token)) {
        throw "User environment variable $TokenVariableName is not set. See Docs/Engineering/ReleasePipeline.md#credentials."
    }

    Write-Step '2. Verify clean working tree'
    $uncommittedChanges = & git -C $RepositoryRoot status --porcelain
    if ($LASTEXITCODE -ne 0) {
        throw 'git status failed.'
    }
    if ($uncommittedChanges) {
        $uncommittedChanges | Out-Host
        throw 'The working tree has uncommitted changes. Commit them before releasing.'
    }

    Assert-UnityEditorClosed
    $unityEditorPath = Get-UnityEditorPath

    Write-Step '3. Build and test launcher'
    Invoke-NativeCommand 'Launcher build' 'dotnet' @('build', $LauncherSolution, '--configuration', 'Release')
    Invoke-NativeCommand 'Launcher tests' 'dotnet' @('test', $LauncherSolution, '--configuration', 'Release', '--no-build')

    Write-Step '4. Run Unity Edit Mode tests'
    Invoke-UnityEditor 'Unity Edit Mode tests' $unityEditorPath @(
        '-batchmode', '-projectPath', $RepositoryRoot,
        '-runTests', '-testPlatform', 'EditMode', '-testResults', $UnityTestResultsFile,
        '-logFile', $UnityTestLogFile) $UnityTestLogFile

    Write-Step '5. Publish launcher'
    if (Test-Path -LiteralPath $StagingDirectory) {
        Remove-Item -LiteralPath $StagingDirectory -Recurse -Force
    }
    Invoke-NativeCommand 'Launcher publish' 'dotnet' @(
        'publish', $LauncherProject, '--configuration', 'Release',
        '--runtime', 'win-x64', '--self-contained', 'true', '--output', $StagingDirectory)

    Write-Step '6. Build Unity Windows player'
    Invoke-UnityEditor 'Unity player build' $unityEditorPath @(
        '-batchmode', '-quit', '-projectPath', $RepositoryRoot,
        '-executeMethod', 'SqueezySurvival.Editor.Build.WindowsPlayerReleaseBuilder.BuildFromCommandLine',
        '-releaseVersion', $Version, '-outputPath', $GameDirectory,
        '-logFile', $UnityBuildLogFile) $UnityBuildLogFile

    Write-Step '7. Restore vpk'
    Invoke-NativeCommand 'Tool restore' 'dotnet' @('tool', 'restore')

    $env:VPK_TOKEN = $token
    try {
        Write-Step '8. Download previous release'
        Invoke-NativeCommand 'vpk download' 'dotnet' @(
            'vpk', 'download', 'github', '--repoUrl', $ReleasesRepositoryUrl, '--outputDir', $ReleasesDirectory)

        Write-Step '9. Package'
        # Unity marks debug-only output folders *_DoNotShip; they must not reach players.
        Get-ChildItem -LiteralPath $GameDirectory -Directory -Filter '*_DoNotShip' | Remove-Item -Recurse -Force
        Invoke-NativeCommand 'vpk pack' 'dotnet' @(
            'vpk', 'pack',
            '--packId', $PackageId, '--packVersion', $Version, '--runtime', $PackageRuntime, '--packDir', $StagingDirectory,
            '--mainExe', $LauncherExecutableFileName, '--packTitle', $PackageTitle, '--packAuthors', $PackageAuthors,
            '--exclude', $PackageExcludePattern, '--outputDir', $ReleasesDirectory)

        Write-Step '10. Publish GitHub release'
        Invoke-NativeCommand 'vpk upload' 'dotnet' @(
            'vpk', 'upload', 'github', '--repoUrl', $ReleasesRepositoryUrl, '--outputDir', $ReleasesDirectory,
            '--publish', '--tag', $ReleaseTag, '--releaseName', "SqueezySurvival $ReleaseTag")
    }
    finally {
        Remove-Item Env:\VPK_TOKEN -ErrorAction SilentlyContinue
    }

    Write-Step "Published $ReleaseTag"
    Write-Host "$ReleasesRepositoryUrl/releases/tag/$ReleaseTag"
}
finally {
    Pop-Location
    Stop-Transcript | Out-Null
}
