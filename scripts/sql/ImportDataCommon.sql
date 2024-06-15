DECLARE @LOGLINE NVARCHAR(10) = Char(10) + Char(10) + '#### '

DECLARE @csvCurrExch   NVARCHAR(500)
DECLARE @csvCustomers  NVARCHAR(500)
DECLARE @csvDates      NVARCHAR(500)
DECLARE @csvProducts   NVARCHAR(500)
DECLARE @csvStores     NVARCHAR(500)
DECLARE @sqlTxt        VARCHAR(max)

PRINT @LOGLINE + 'CD: ' + '$(varCD)'

SET @csvCurrExch   = '$(varCD)' + '\inputcsv\currencyexchanges.csv'
SET @csvCustomers  = '$(varCD)' + '\inputcsv\customers.csv'
SET @csvDates      = '$(varCD)' + '\inputcsv\dates.csv'
SET @csvProducts   = '$(varCD)' + '\inputcsv\products.csv'
SET @csvStores     = '$(varCD)' + '\inputcsv\stores.csv'


PRINT @LOGLINE + 'Load data'
PRINT @LOGLINE + @csvCurrExch;   EXEC ('BULK INSERT [CurrencyExchanges] FROM ''' + @csvCurrExch +  ''' WITH ( TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
PRINT @LOGLINE + @csvCustomers;  EXEC ('BULK INSERT [Customers]         FROM ''' + @csvCustomers + ''' WITH ( TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
PRINT @LOGLINE + @csvDates;      EXEC ('BULK INSERT [Dates]             FROM ''' + @csvDates +     ''' WITH ( TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
PRINT @LOGLINE + @csvProducts;   EXEC ('BULK INSERT [Products]          FROM ''' + @csvProducts +  ''' WITH ( TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
PRINT @LOGLINE + @csvStores;     EXEC ('BULK INSERT [Stores]            FROM ''' + @csvStores +    ''' WITH ( TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
