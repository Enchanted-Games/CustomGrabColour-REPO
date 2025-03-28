REM Prep releaseFiles directory by making sure its empty
del /s /q .\releaseFiles\*
rmdir /s /q .\releaseFiles\
mkdir .\releaseFiles\

REM Copy ../Art/icon.png to the current directory
copy /y ..\Art\icon.png .\releaseFiles\
REM Copy ../Art/manifest.json to the current directory
copy /y ..\Art\manifest.json .\releaseFiles\
REM Copy ../README.md to the current directory
copy /y ..\README.md .\releaseFiles\
REM Copy ../CHANGELOG.md to the current directory
copy /y ..\CHANGELOG.md .\releaseFiles\

REM Copy all files from ../Template/build/bin/Debug to the current directory
xcopy /s /y /q ..\Template\build\bin\Debug\* .\releaseFiles\

REM Create a zip file named CustomGrabColour.zip containing all files (except build.bat) in the current directory
"C:\Program Files\7-Zip\7z.exe" a CustomGrabColour.zip .\releaseFiles\* -x!build.bat -x!CustomGrabColour.zip -x!\releaseFiles\
