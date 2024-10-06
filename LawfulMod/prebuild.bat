md www\assets
del www\assets\*.* /Q
cd ..
cd lawful
call npm run build
copy dist\*.* ..\LawfulMod\www /Y
copy dist\assets\*.* ..\LawfulMod\www\assets /Y
ping /n 1 /w 500 localhost >nul 
