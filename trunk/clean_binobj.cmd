@echo off
rem Bin & Obj folder cleanup
rem Written by Webmaster442

for /D %%d in (*) do if exist "%%d\bin" rmdir /q /s "%%d\bin"
for /D %%d in (*) do if exist "%%d\obj" rmdir /q /s "%%d\obj"

cd bin
rmdir /q /s release
rmdir /q /s debug
rmdir /q /s setup
cd ..