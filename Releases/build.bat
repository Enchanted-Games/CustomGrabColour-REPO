dotnet build ..\

REM Prep releaseFiles directory by making sure its empty
del /s /q .\releaseFiles\*
rmdir /s /q .\releaseFiles\
mkdir .\releaseFiles\

REM Copy metadata files to the releaseFiles directory
copy /y ..\Art\icon.png .\releaseFiles\
copy /y ..\Art\manifest.json .\releaseFiles\
copy /y ..\README.md .\releaseFiles\
copy /y ..\CHANGELOG.md .\releaseFiles\

REM Copy the built dll to the releaseFiles directory
xcopy /s /y /q ..\ExploitFixes\bin\Debug\netstandard2.1\ExploitFixes.dll .\releaseFiles\

REM Create a zip file named ExploitFixes.zip containing all files (except build.bat) in the current directory
"C:\Program Files\7-Zip\7z.exe" a ExploitFixes.zip .\releaseFiles\* -x!build.bat -x!ExploitFixes.zip -x!\releaseFiles\
