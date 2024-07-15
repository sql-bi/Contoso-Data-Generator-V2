DECLARE @BackupFile  NVARCHAR(500)

PRINT 'CD: ' + '$(varCD)'

SET @BackupFile = '$(varCD)' + '\dump\fullbackup.bak'

PRINT @BackupFile 

BACKUP DATABASE [ContosoDGV2Test] TO  DISK = @BackupFile  WITH  COPY_ONLY, NOFORMAT, INIT, NAME = N'ContosoDGV2 full database backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10

