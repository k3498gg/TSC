@echo off
@echo 客户端表格
tool\\Python27\\python.exe tool\\data_output.py ./client_pro_conf.xml
if not %errorlevel% == 0 GOTO ERROR_OUT

@echo 服务器表格
tool\\Python27\\python.exe tool\\data_output.py ./pro_conf.xml
if not %errorlevel% == 0 GOTO ERROR_OUT

Pause

:ERROR_OUT
Pause