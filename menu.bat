@echo off
cls
:MENU
ECHO.
ECHO ...............................................
ECHO PRESS 1, 2 selecting your task, or 3 to EXIT.
ECHO ...............................................
ECHO.
ECHO 1 - Open the Painter 
ECHO 2 - Open the Calculator
ECHO 3 - EXIT
ECHO.
SET /P M=Input 1, 2, 3 then press ENTER:
IF %M%==1 GOTO PAINT
IF %M%==2 GOTO CALC
IF %M%==3 GOTO EOF
:PAINT
start mspaint.exe
GOTO MENU
:CALC
start calc.exe
:EOF
exit /b
GOTO MENU
