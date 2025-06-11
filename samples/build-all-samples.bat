@echo off

REM Set the error level to 0
set ERRORLEVEL=0
REM Get the directory of the script
set SCRIPT_DIR=%~dp0

REM .NET CORE
echo BUILD NETCore - Console - WithoutDependencyInjections
dotnet build "%SCRIPT_DIR%dotnetcore\console\WithoutDependencyInjections\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)

echo.
echo BUILD NETCore - Console - DependencyInjections - minimal-configuration
dotnet build "%SCRIPT_DIR%dotnetcore\console\DependencyInjections\minimal-configuration\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)

echo.
echo BUILD NETCore - Console - DependencyInjections - connectionstring-configuration
dotnet build "%SCRIPT_DIR%dotnetcore\console\DependencyInjections\connectionstring-configuration\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)

echo.
echo BUILD NETCore - Console - DependencyInjections - configuration
dotnet build "%SCRIPT_DIR%dotnetcore\console\DependencyInjections\configuration\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)

echo.
echo.
echo.
echo BUILD NETCore - AZ Function - WithoutDependencyInjections
dotnet build "%SCRIPT_DIR%dotnetcore\az-function\WithoutDependencyInjections\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)

echo.
echo BUILD NETCore - AZ Function - DependencyInjections - minimal-configuration
dotnet build "%SCRIPT_DIR%dotnetcore\az-function\DependencyInjections\minimal-configuration\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)

echo.
echo BUILD NETCore - AZ Function - DependencyInjections - connectionstring-configuration
dotnet build "%SCRIPT_DIR%dotnetcore\az-function\DependencyInjections\connectionstring-configuration\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)

echo.
echo BUILD NETCore - AZ Function - DependencyInjections - configuration
dotnet build "%SCRIPT_DIR%dotnetcore\az-function\DependencyInjections\configuration\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)


REM .NET FRAMEWORK
echo.
echo.
echo.
echo BUILD NET FW - Console - WithoutDependencyInjections
dotnet build "%SCRIPT_DIR%dotnetfw\console\WithoutDependencyInjections\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)

echo.
echo BUILD NET FW - Console - DependencyInjections - minimal-configuration
dotnet build "%SCRIPT_DIR%dotnetfw\console\DependencyInjections\minimal-configuration\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)

echo.
echo BUILD NET FW - Console - DependencyInjections - connectionstring-configuration
dotnet build "%SCRIPT_DIR%dotnetfw\console\DependencyInjections\connectionstring-configuration\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)

echo.
echo BUILD NET FW - Console - DependencyInjections - configuration
dotnet build "%SCRIPT_DIR%dotnetfw\console\DependencyInjections\configuration\samples.sln"
if %ERRORLEVEL% neq 0 (
    echo Application failed!
    exit /b %ERRORLEVEL%
)


REM If all projects ran successfully
echo All projects ran successfully!
pause
exit /b 0

