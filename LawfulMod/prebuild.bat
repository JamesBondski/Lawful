md www\assets
del www\assets\*.* /Y
cd ..
cd lawful
call npm run build
copy dist\*.* ..\LawfulMod\www /Y
copy dist\assets\*.* ..\LawfulMod\www\assets /Y
