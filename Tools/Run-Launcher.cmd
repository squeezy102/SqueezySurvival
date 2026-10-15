@echo off
rem Runs the launcher from source for visual checks. Not installed, so update actions are disabled.
setlocal
pushd "%~dp0..\Launcher"
dotnet run --project SqueezySurvival.Launcher\SqueezySurvival.Launcher.csproj --configuration Release
popd
