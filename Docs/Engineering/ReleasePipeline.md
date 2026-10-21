# Release Pipeline

**Status:** Approved

How a player build is produced and published. Decisions: [ADR-0004](../Decisions/ADR-0004-Distribution-And-Self-Update.md), [ADR-0005](../Decisions/ADR-0005-Launcher-Technology.md).

## Usage

1. Set the new version in the `VERSION` file at the repository root and commit it. `VERSION` is the single source of truth for the release version.
2. Close the Unity Editor. Unity cannot build a project from the command line while the editor has it open.
3. Confirm the working tree is clean. The pipeline refuses to publish uncommitted changes.
4. Double-click `Tools/Publish-Release.cmd`. It takes no input.
5. Read `Tools/Output/Publish-Release.log`.

## Steps

| # | Step | Tool | Fails the release when |
|---|---|---|---|
| 1 | Read `VERSION`, validate it (`MAJOR.MINOR.PATCH`) and the token | PowerShell | Version malformed or token missing |
| 2 | Verify the working tree is clean | `git status` | Uncommitted changes |
| 3 | Build and test the launcher | `dotnet build`, `dotnet test` | Any warning, error or failing test |
| 4 | Run Unity Edit Mode tests | Unity `-runTests` | Any failing test |
| 5 | Publish the launcher, self-contained `win-x64` | `dotnet publish` | Non-zero exit |
| 6 | Build the Unity Windows player into `Game/` | Unity `-executeMethod` | Non-zero exit |
| 7 | Restore `vpk` at the pinned version | `dotnet tool restore` | Non-zero exit |
| 8 | Download the latest published release for delta generation | `vpk download github` | Non-zero exit. No previous release is not a failure. |
| 9 | Remove `*_DoNotShip` folders from `Game/`, then package | `vpk pack` | Non-zero exit |
| 10 | Create and publish the GitHub release | `vpk upload github` | Non-zero exit, or the tag already exists |
| 11 | Tag the source commit `v{version}` and push tags | `git tag`, `git push --tags` | Non-zero exit |


## Package layout

```
SqueezySurvivalLauncher.exe     Velopack main executable
...launcher runtime files
Game/
  SqueezySurvival.exe           Unity player
  ...player files
```

## Fixed values

| Setting | Value | Reason |
|---|---|---|
| Velopack package id | `Squeezy102.SqueezySurvival` | Velopack recommends `<Company>.<App>`. Never changes; installs and updates are keyed on it. |
| Main executable | `SqueezySurvivalLauncher.exe` | The launcher handles Velopack hooks. |
| Title | `SqueezySurvival` | Shortcut and installer display name. |
| Channel | `win` (Velopack default) | One channel until other platforms ship. |
| Runtime | `win-x64` | Without `--runtime`, `vpk pack` defaults the package to x86. |
| Release tag | `v{version}` | Matches the source repository tag. |
| Excluded from package | `*.pdb` | Debug symbols. Unity `*_DoNotShip` folders (Burst debug information) are deleted from the staging folder before packaging, because a file exclude leaves an empty directory entry. |

## Credentials

Publishing needs a GitHub fine-grained personal access token:

| Setting | Value |
|---|---|
| Repository access | Only `squeezy102/SqueezySurvival-Releases` |
| Permissions | Contents: Read and write ([GitHub REST: create a release](https://docs.github.com/en/rest/releases/releases#create-a-release)) |
| Storage | Windows user environment variable `SQUEEZYSURVIVAL_RELEASES_TOKEN`. Never committed. |
| Setup | Run `Tools/Set-ReleaseToken.cmd` and paste the token at the hidden prompt. |

The script passes the token to `vpk` through the `VPK_TOKEN` environment variable of its own process, so it never appears on a command line or in logs. `vpk` reads any option from a `VPK_`-prefixed environment variable (Velopack source, `BaseCommand.cs`).

## Known warnings

Every warning in a release log is either fixed or listed here with the reason it cannot be.

| Warning | Source | Reason it remains |
|---|---|---|
| `No signing parameters provided, N file(s) will not be signed.` | `vpk pack` | Signing requires a paid code-signing certificate. Consequence documented in [ADR-0004](../Decisions/ADR-0004-Distribution-And-Self-Update.md#consequences). |
| `No releases found` / `No full / applicable release was found to download.` | `vpk download` | Expected only for the first release, which has no predecessor for delta generation. |
| `File '...-full.nupkg' already exists on disk. Verifying checksum...` | `vpk download` | The previous release's package is already in `Builds/Releases` from an earlier run; `vpk` checks it matches instead of downloading it again. |

## Open issues

| Issue | Status |
|---|---|
| A batch-mode player build sets `m_Enabled: 1` in `ProjectSettings/UnityConnectSettings.asset`; opening the editor sets it back to `0`. All individual services (analytics, crash reporting, ads, purchasing, performance reporting) remain disabled. The build log shows the editor then trying to reach Unity Cloud hosts. Matches Unity issue "Unity Services are not disabled and cause longer build times when the project is not connected to Unity Cloud" ([tracker](https://issuetracker.unity.com/issues/6472)). Root cause not yet confirmed. | Under investigation. Until resolved, the change is reverted after each release (and after local builds) so the next release passes the clean-tree check. |
| A local editor build (`Build And Run Locally`) left the Input System's project-wide actions asset (`Assets/InputSystem_Actions.inputactions`) in `preloadedAssets` in `ProjectSettings/ProjectSettings.asset`. Root cause not yet confirmed. | Reverted after local builds so the tree stays clean. |

## Files written by the Unity build

URP and the engine save these files during every player build. Their post-build state is committed so builds leave the working tree clean.

| File | Writer (verified in package source) |
|---|---|
| `Assets/Settings/UniversalRP.asset` | URP `ShaderBuildPreprocessor` saves shader prefiltering settings. |
| `Assets/UniversalRenderPipelineGlobalSettings.asset` | Core RP `RenderPipelineGraphicsSettingsContainer` fills runtime settings only while building a player. |
| `Assets/DefaultVolumeProfile.asset` | URP `URPPreprocessBuild` adds missing default overrides. |
| `ProjectSettings/ProjectSettings.asset` | Engine writes the Standalone batching default. |

Changing render features or post-processing changes these files again; commit the result of the next build. Precedent: [Unity Discussions](https://discussions.unity.com/t/urp-build-issue-universalrenderpipelineglobalsettings-asset-gets-modified-every-build/1552440).

## Output

| Path | Contents | Versioned |
|---|---|---|
| `Builds/Release/{version}/Package/` | Staged launcher and game | No |
| `Builds/Releases/` | Velopack packages, installer and feed | No |
| `Tools/Output/Publish-Release.log` | Pipeline log | No |
| `Tools/Output/Unity-*.log` | Unity test and build logs | No |
