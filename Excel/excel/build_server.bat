@echo off

tool\\Python27\\python.exe tool\\data_output.py ./pro_conf.xml
if not %errorlevel% == 0 GOTO ERROR_OUT

tool\\Python27\\python.exe tool\\code_output.py ./pro_conf.xml
if not %errorlevel% == 0 GOTO ERROR_OUT

Pause

:ERROR_OUT
Pause