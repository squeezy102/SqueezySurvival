@echo off
rem Builds and tests the launcher solution. Output: Tools\Output\Build-Launcher.log
setlocal enabledelayedexpansion
set "OUTPUT_DIR=%~dp0Output"
set "LOG_FILE=%OUTPUT_DIR%\Build-Launcher.log"
if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"
pushd "%~dp0..\Launcher"
(
  echo == Date
  echo %DATE% %TIME%
  echo == dotnet --version
  dotnet --version
  echo == Build
  dotnet build SqueezySurvival.Launcher.sln --configuration Release
  echo == Build exit code: !ERRORLEVEL!
  echo == Test
  dotnet test SqueezySurvival.Launcher.sln --configuration Release --no-build
  echo == Test exit code: !ERRORLEVEL!
) > "%LOG_FILE%" 2>&1
popd
type "%LOG_FILE%"
echo.
echo Log written to %LOG_FILE%
pause
