@echo off
@echo �ͻ��˱��
tool\\Python27\\python.exe tool\\data_output.py ./client_pro_conf.xml
if not %errorlevel% == 0 GOTO ERROR_OUT

@echo ���������
tool\\Python27\\python.exe tool\\data_output.py ./pro_conf.xml
if not %errorlevel% == 0 GOTO ERROR_OUT

Pause

:ERROR_OUT
Pause