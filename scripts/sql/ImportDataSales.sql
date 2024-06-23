DECLARE @LOGLINE NVARCHAR(10) = Char(10) + Char(10) + '#### '

DECLARE @csvSales      NVARCHAR(500)
DECLARE @sqlTxt        VARCHAR(max)

PRINT @LOGLINE + 'CD: ' + '$(varCD)'

SET @csvSales      = '$(varCD)' + '\inputcsv\sales.csv'


PRINT @LOGLINE + 'Load data'
PRINT @LOGLINE + @csvSales;      EXEC ('BULK INSERT [Sales]             FROM ''' + @csvSales +     ''' WITH ( TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');

PRINT @LOGLINE + 'Shrink database'
DBCC SHRINKDATABASE(0)

PRINT @LOGLINE + 'Count records' 
SELECT       '[CurrencyExchange]', COUNT(1) FROM [CurrencyExchange]
UNION SELECT '[Customer]',         COUNT(1) FROM [Customer]
UNION SELECT '[Date]',             COUNT(1) FROM [Date]
UNION SELECT '[Product]',          COUNT(1) FROM [Product]
UNION SELECT '[Store]',            COUNT(1) FROM [Store]
UNION SELECT '[Sales]',            COUNT(1) FROM [Sales]

PRINT @LOGLINE + 'DB space' 
EXEC sp_spaceused  
