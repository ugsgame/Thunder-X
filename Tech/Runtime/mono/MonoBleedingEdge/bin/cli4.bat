@echo off
SETLOCAL
set MONO_PREFIX=%~dp0..
set MONO=%MONO_PREFIX%\bin\mono
REM set MONO_PATH=%MONO_PREFIX%\lib\mono\4.5;%MONO_PREFIX%\lib\mono\4.0
set MONO_CFG_DIR=%MONO_PREFIX%\etc
"%MONO%" %*
ENDLOCAL
