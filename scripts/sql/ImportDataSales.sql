DECLARE @LOGLINE NVARCHAR(10) = Char(10) + Char(10) + '#### '

DECLARE @csvSales      NVARCHAR(500)
DECLARE @sqlTxt        VARCHAR(max)

PRINT @LOGLINE + 'CD: ' + '$(varCD)'

SET @csvSales      = '$(varCD)' + '\inputcsv\sales.csv'


PRINT @LOGLINE + 'Load data'
PRINT @LOGLINE + @csvSales;      EXEC ('BULK INSERT [Data].[Sales]             FROM ''' + @csvSales +     ''' WITH (CODEPAGE = ''65001'', TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');

PRINT @LOGLINE + 'Shrink database'
DBCC SHRINKDATABASE(0)

PRINT @LOGLINE + 'DB space' 
EXEC sp_spaceused  

PRINT @LOGLINE + 'Count records' 
SELECT       '[CurrencyExchange]', COUNT(1) FROM [Data].[CurrencyExchange]
UNION SELECT '[Customer]',         COUNT(1) FROM [Data].[Customer]
UNION SELECT '[Date]',             COUNT(1) FROM [Data].[Date]
UNION SELECT '[Product]',          COUNT(1) FROM [Data].[Product]
UNION SELECT '[Store]',            COUNT(1) FROM [Data].[Store]
UNION SELECT '[Sales]',            COUNT(1) FROM [Data].[Sales]

