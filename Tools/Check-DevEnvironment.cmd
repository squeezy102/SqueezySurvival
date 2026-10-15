@echo off
rem Reports installed development tool versions to Tools\Output\DevEnvironment.log.
setlocal
set "OUTPUT_DIR=%~dp0Output"
set "LOG_FILE=%OUTPUT_DIR%\DevEnvironment.log"
if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"
(
  echo == Date
  echo %DATE% %TIME%
  echo == Windows
  ver
  echo == .NET SDKs
  dotnet --list-sdks
  echo == Git
  git --version
  echo == Git LFS
  git lfs version
  echo == winget
  winget --version
) > "%LOG_FILE%" 2>&1
type "%LOG_FILE%"
echo.
echo Log written to %LOG_FILE%
pause
