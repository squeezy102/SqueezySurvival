#Requires -Version 5.1
<#
.SYNOPSIS
    Stores the GitHub release token in the current Windows user's environment.
.DESCRIPTION
    Prompts with hidden input so the token never appears on screen, in a command line or in shell history.
    See Docs/Engineering/ReleasePipeline.md#credentials.
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$TokenVariableName = 'SQUEEZYSURVIVAL_RELEASES_TOKEN'

$secureToken = Read-Host -Prompt 'Paste the GitHub token, then press Enter' -AsSecureString
$tokenPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureToken)
try {
    $token = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($tokenPointer)
}
finally {
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($tokenPointer)
}

if ([string]::IsNullOrWhiteSpace($token)) {
    throw 'No token entered. Nothing was stored.'
}

[Environment]::SetEnvironmentVariable($TokenVariableName, $token.Trim(), 'User')
Write-Host "Stored in user environment variable $TokenVariableName."
