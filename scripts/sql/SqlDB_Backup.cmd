CLS

IF EXIST "dump\fullbackup.bak"    DEL dump\fullbackup.bak
IF EXIST "dump\fullbackup.bak.7z" DEL dump\fullbackup.bak.7z

sqlcmd -S (LocalDb)\MSSQLLocalDB -d ContosoDGV2Test -i BackupFull.sql -v varCD="%CD%"

..\build_data\bin\7za.exe  a  dump\fullbackup.bak.7z  dump\fullbackup.bak 

PAUSE
