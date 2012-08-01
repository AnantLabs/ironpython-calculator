@echo off
cd bin\release
del *.pdb
del *.vshost.exe.manifest
del *.vshost.exe.config
del *.vshost.exe

if exist "%PROGRAMFILES%\IronPython 2.7.1" (
	copy "%PROGRAMFILES%\IronPython 2.7.1\*.exe"
	if not exist Lib md Lib
	cd Lib
	xcopy /s "%PROGRAMFILES%\IronPython 2.7.1\lib\*.*"
	cd ..
)

if exist "%PROGRAMFILES(X86)%\IronPython 2.7.1" (
	copy "%PROGRAMFILES(X86)%\IronPython 2.7.1\*.exe"
	if not exist Lib md Lib
	cd Lib
	xcopy /s "%PROGRAMFILES(X86)%\IronPython 2.7.1\lib\*.*"
	cd ..
)

cd ..
cd ..