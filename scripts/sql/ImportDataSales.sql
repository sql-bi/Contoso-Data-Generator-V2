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
SELECT       '[CurrencyExchanges]', COUNT(1) FROM [CurrencyExchanges]
UNION SELECT '[Customers]',         COUNT(1) FROM [Customers]
UNION SELECT '[Dates]',             COUNT(1) FROM [Dates]
UNION SELECT '[Products]',          COUNT(1) FROM [Products]
UNION SELECT '[Stores]',            COUNT(1) FROM [Stores]
UNION SELECT '[Sales]',             COUNT(1) FROM [Sales]

PRINT @LOGLINE + 'DB space' 
EXEC sp_spaceused  
