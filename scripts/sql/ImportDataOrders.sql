DECLARE @LOGLINE NVARCHAR(10) = Char(10) + Char(10) + '#### '

DECLARE @csvOrders     NVARCHAR(500)
DECLARE @csvOrderRows  NVARCHAR(500)
DECLARE @sqlTxt        VARCHAR(max)

PRINT @LOGLINE + 'CD: ' + '$(varCD)'

SET @csvOrders     = '$(varCD)' + '\inputcsv\orders.csv'
SET @csvOrderRows  = '$(varCD)' + '\inputcsv\orderrows.csv'


PRINT @LOGLINE + 'Load data'
PRINT @LOGLINE + @csvOrders;     EXEC ('BULK INSERT [Data].[Orders]            FROM ''' + @csvOrders +    ''' WITH ( TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
PRINT @LOGLINE + @csvOrderRows;  EXEC ('BULK INSERT [Data].[OrderRows]         FROM ''' + @csvOrderRows + ''' WITH ( TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');

PRINT @LOGLINE + 'Shrink database'
DBCC SHRINKDATABASE(0)

PRINT @LOGLINE + 'Count records' 
SELECT       '[Data].[CurrencyExchange]', COUNT(1) FROM [Data].[CurrencyExchange]
UNION SELECT '[Data].[Customer]',         COUNT(1) FROM [Data].[Customer]
UNION SELECT '[Data].[Date]',             COUNT(1) FROM [Data].[Date]
UNION SELECT '[Data].[Product]',          COUNT(1) FROM [Data].[Product]
UNION SELECT '[Data].[Store]',            COUNT(1) FROM [Data].[Store]
UNION SELECT '[Data].[Orders]',           COUNT(1) FROM [Data].[Orders]
UNION SELECT '[Data].[OrderRows]',        COUNT(1) FROM [Data].[OrderRows]

PRINT @LOGLINE + 'DB space' 
EXEC sp_spaceused  
