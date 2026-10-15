@echo off
rem Stores the GitHub release token for the current Windows user. See Docs\Engineering\ReleasePipeline.md.
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Set-ReleaseToken.ps1"
pause
