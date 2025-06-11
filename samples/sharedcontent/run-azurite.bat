@echo off

REM Instructions to run this script:
REM 1. Make sure you have Azurite installed. You can install it using npm:
REM    npm install -g azurite
REM 2. Make sure you have the .NET SDK installed and the `dotnet` command available in your PATH.
REM 3. Run the script by double-clicking it or executing it from the command line.

echo Starting Azurite...
start /B azurite -s -l c:\azurite -d c:\azurite\debug.log
pause
