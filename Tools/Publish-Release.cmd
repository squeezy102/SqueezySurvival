@echo off
rem Publishes the version in the repository's VERSION file. See Docs\Engineering\ReleasePipeline.md.
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Publish-Release.ps1"
echo.
echo Log written to %~dp0Output\Publish-Release.log
pause
