DECLARE @LOGLINE NVARCHAR(10) = Char(10) + Char(10) + '#### '

DECLARE @csvOrders     NVARCHAR(500)
DECLARE @csvOrderRows  NVARCHAR(500)
DECLARE @sqlTxt        VARCHAR(max)

PRINT @LOGLINE + 'CD: ' + '$(varCD)'

SET @csvOrders     = '$(varCD)' + '\inputcsv\orders.csv'
SET @csvOrderRows  = '$(varCD)' + '\inputcsv\orderrows.csv'


PRINT @LOGLINE + 'Load data'
PRINT @LOGLINE + @csvOrders;     EXEC ('BULK INSERT [Orders]            FROM ''' + @csvOrders +    ''' WITH ( TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
PRINT @LOGLINE + @csvOrderRows;  EXEC ('BULK INSERT [OrderRows]         FROM ''' + @csvOrderRows + ''' WITH ( TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');

PRINT @LOGLINE + 'Shrink database'
DBCC SHRINKDATABASE(0)

PRINT @LOGLINE + 'Count records' 
SELECT       '[CurrencyExchanges]', COUNT(1) FROM [CurrencyExchanges]
UNION SELECT '[Customers]',         COUNT(1) FROM [Customers]
UNION SELECT '[Dates]',             COUNT(1) FROM [Dates]
UNION SELECT '[Products]',          COUNT(1) FROM [Products]
UNION SELECT '[Stores]',            COUNT(1) FROM [Stores]
UNION SELECT '[Orders]',            COUNT(1) FROM [Orders]
UNION SELECT '[OrderRows]',         COUNT(1) FROM [OrderRows]

PRINT @LOGLINE + 'DB space' 
EXEC sp_spaceused  
