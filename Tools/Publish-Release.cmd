@echo off
rem Prompts for a release version and runs Publish-Release.ps1. See Docs\Engineering\ReleasePipeline.md.
setlocal
set /p "RELEASE_VERSION=Release version (MAJOR.MINOR.PATCH): "
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Publish-Release.ps1" -Version "%RELEASE_VERSION%"
echo.
echo Log written to %~dp0Output\Publish-Release.log
pause
