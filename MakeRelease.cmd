@echo off

if not exist bin md bin
cd bin
if not exist release md release
cd release

del *.pdb
del *.vshost.exe.manifest
del *.vshost.exe.config
del *.vshost.exe

if exist "%PROGRAMFILES%\IronPython 2.7.1" goto x86

if exist "%PROGRAMFILES(X86)%\IronPython 2.7.1" goto x64

:x86
copy "%PROGRAMFILES%\IronPython 2.7.1\*.exe"
if not exist Lib md Lib
cd Lib
xcopy /s "%PROGRAMFILES%\IronPython 2.7.1\lib\*.*"
cd ..
goto gnuplot

:x64
copy "%PROGRAMFILES(X86)%\IronPython 2.7.1\*.exe"
if not exist Lib md Lib
cd Lib
xcopy /s "%PROGRAMFILES(X86)%\IronPython 2.7.1\lib\*.*"
cd ..
gotot gnuplot


:gnuplot
cd ..
cd ..

cd InstallerSource
7zr x -o"..\bin\release" gnuplot.7z
cd .. 