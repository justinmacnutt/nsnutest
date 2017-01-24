@echo off
rem COPYONEWEB.BAT transfers all files in all subdirectories of
rem the OneWeb4 directories to the destination
rem drive or directory (%1) 
rem Note that the web.config file will not be copied.

@SET FILESRC=.
@SET BINSRC=%FILESRC%\bin

SET /p SYSTEM="(L)ocal, or (S)taging?"
IF %SYSTEM%==L GOTO LOCAL
IF %SYSTEM%==l GOTO LOCAL
IF %SYSTEM%==S GOTO STAGING
IF %SYSTEM%==s GOTO STAGING
GOTO EXIT

:LOCAL
rem @SET APPFILEDEST=\Projects\Nova Scotia Nurses Union\webroot\site\applications\NSNUMembership
@SET BINDEST=\Projects\Nova Scotia Nurses Union\webroot\bin
GOTO COPYIT


:STAGING
rem @SET APPFILEDEST=\\CORUSCANT64\stagingroot\nsnu\webroot\site\applications\NSNUMembership
@SET BINDEST=\\CORUSCANT64\stagingroot\nsnu\webroot\bin
GOTO COPYIT


:COPYIT
rem xcopy "%FILESRC%\App\*.ascx" "%APPFILEDEST%" /D /I /S /R /Y
rem xcopy "%FILESRC%\App\*.js" "%APPFILEDEST%" /D /I /S /R /Y
rem xcopy "%FILESRC%\App\*.jpg" "%APPFILEDEST%" /D /I /S /R /Y
rem xcopy "%FILESRC%\Provider\*.xml" "%APPFILEDEST%" /D /I /S /R /Y

xcopy "%BINSRC%\NSNUMembership.dll" "%BINDEST%" /D /I /S /R /Y
xcopy "%BINSRC%\Nsnu.DataAccess.dll" "%BINDEST%" /D /I /S /R /Y
xcopy "%BINSRC%\Nsnu.MembershipServices.dll" "%BINDEST%" /D /I /S /R /Y

if errorlevel 4 goto lowmemory
if errorlevel 2 goto abort
if errorlevel 0 goto exit
:lowmemory
echo Insufficient memory to copy files or
echo invalid drive or command-line syntax.
goto exit
:abort
echo You pressed CTRL+C to end the copy operation.
goto exit
:exit

@pause