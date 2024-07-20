@ECHO OFF
CLS

SET SqlServerName=.\K22

@ECHO.
ECHO SqlServerName: %SqlServerName%
@ECHO.
PAUSE


CALL :BUILDBATABASE  "csv-100k"  "Contoso V2 100k"  "ContosoV2100k.bak"
CALL :BUILDBATABASE  "csv-1m"    "Contoso V2 1M"    "ContosoV21M.bak"    
REM CALL :BUILDBATABASE  "csv-10m"   "Contoso V2 10M"   "ContosoV210M.bak"   
REM CALL :BUILDBATABASE  "csv-100m"  "Contoso V2 100M"  "ContosoV2100M.bak"  "-v500m"

ECHO.
ECHO ######### The end #########
ECHO.
PAUSE
GOTO :EOF




:BUILDBATABASE
ECHO.
ECHO BUILDBATABASE params: - data: %~1  db-name: %~2  backup-file: %~3  7zip-param: %~4

SET DatabaseName=%~2

@ECHO.
ECHO SqlServerName: %SqlServerName%
ECHO DatabaseName : %DatabaseName%
@ECHO.

IF EXIST "dump\fullbackup.bak"   DEL dump\fullbackup.bak
IF EXIST "dump\%~3"              DEL dump\%~3
IF EXIST "dump\%~3.7z"           DEL dump\%~3.7z
IF EXIST "dump\%~3.7z.*"         DEL dump\%~3.7z.*

ECHO --- Copy CSV files
ECHO --------------------------------------------------------------------------------
IF EXIST "inputcsv\*.csv"  DEL inputcsv\*.csv
COPY ..\build_data\out\%~1\*.csv inputcsv

ECHO --- Fill database
ECHO --------------------------------------------------------------------------------
CALL Sql_ImportData.cmd both

ECHO --- Backup database
ECHO --------------------------------------------------------------------------------
SQLCMD -S "%SqlServerName%" -d "%DatabaseName%" -Q "select '$(varBackupFile)'; select '$(varDatabaseName)'; BACKUP DATABASE [$(varDatabaseName)] TO  DISK = '$(varBackupFile)'  WITH  COPY_ONLY, NOFORMAT, INIT, NAME = N'ContosoDGV2 full database backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10;" -v varBackupFile="%CD%\dump\fullbackup.bak" -v varDatabaseName="%DatabaseName%"
RENAME dump\fullbackup.bak  %~3
..\build_data\bin\7za.exe  a  dump\%~3.7z  dump\%~3  %~4

ECHO.
EXIT /B 0
