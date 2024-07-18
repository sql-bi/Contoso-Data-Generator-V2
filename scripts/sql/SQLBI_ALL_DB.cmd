@ECHO OFF
CLS
ECHO.

ECHO Configuration data from Sql_Config.cmd :
CALL Sql_Config.cmd
ECHO.
PAUSE


CALL :BUILDBATABASE  csv-100k  
CALL :BUILDBATABASE  csv-1m
REM CALL :BUILDBATABASE  csv-10m
REM CALL :BUILDBATABASE  csv-100m   -v500m


ECHO.
ECHO.
PAUSE
GOTO :EOF




:BUILDBATABASE
ECHO.
ECHO BUILDBATABASE___ : %~1 %~2
ECHO.

IF EXIST "dump\fullbackup.bak"    DEL dump\fullbackup.bak
IF EXIST "dump\%~1.bak"           DEL dump\%~1.bak
IF EXIST "dump\%~1.bak.7z"        DEL dump\%~1.bak.7z
IF EXIST "dump\%~1.bak.7z.*"      DEL dump\%~1.bak.7z.*

ECHO --- Copy CSV files
ECHO --------------------------------------------------------------------------------
IF EXIST "inputcsv\*.csv"  DEL inputcsv\*.csv
COPY ..\build_data\out\%~1\*.csv inputcsv


ECHO --- Fill database
ECHO --------------------------------------------------------------------------------
CALL Sql_ImportData.cmd both


ECHO --- Backup database
ECHO --------------------------------------------------------------------------------

SQLCMD -S %SqlServerName% -d %DatabaseName% -Q "select '$(varBackupFile)'; select '$(varDatabaseName)'; BACKUP DATABASE [$(varDatabaseName)] TO  DISK = '$(varBackupFile)'  WITH  COPY_ONLY, NOFORMAT, INIT, NAME = N'ContosoDGV2 full database backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10;" -v varBackupFile="%CD%\dump\fullbackup.bak" -v varDatabaseName="%DatabaseName%"

RENAME dump\fullbackup.bak  %~1.bak

..\build_data\bin\7za.exe  a  dump\%~1.bak.7z  dump\%~1.bak  %~2

ECHO.
ECHO.
EXIT /B 0
