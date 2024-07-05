DECLARE @LOGLINE NVARCHAR(10) = Char(10) + Char(10) + '#### '

DECLARE @csvCurrExch  NVARCHAR(500)
DECLARE @csvCustomer  NVARCHAR(500)
DECLARE @csvDate      NVARCHAR(500)
DECLARE @csvProduct   NVARCHAR(500)
DECLARE @csvStore     NVARCHAR(500)
DECLARE @sqlTxt       VARCHAR(max)

PRINT @LOGLINE + 'CD: ' + '$(varCD)'

SET @csvCurrExch = '$(varCD)' + '\inputcsv\currencyexchange.csv'
SET @csvCustomer = '$(varCD)' + '\inputcsv\customer.csv'
SET @csvDate     = '$(varCD)' + '\inputcsv\date.csv'
SET @csvProduct  = '$(varCD)' + '\inputcsv\product.csv'
SET @csvStore    = '$(varCD)' + '\inputcsv\store.csv'


PRINT @LOGLINE + 'Load data'
PRINT @LOGLINE + @csvCurrExch;  EXEC ('BULK INSERT [Data].[CurrencyExchange] FROM ''' + @csvCurrExch + ''' WITH (CODEPAGE = ''65001'', TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
PRINT @LOGLINE + @csvCustomer;  EXEC ('BULK INSERT [Data].[Customer]         FROM ''' + @csvCustomer + ''' WITH (CODEPAGE = ''65001'', TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
PRINT @LOGLINE + @csvDate;      EXEC ('BULK INSERT [Data].[Date]             FROM ''' + @csvDate +     ''' WITH (CODEPAGE = ''65001'', TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
PRINT @LOGLINE + @csvProduct;   EXEC ('BULK INSERT [Data].[Product]          FROM ''' + @csvProduct +  ''' WITH (CODEPAGE = ''65001'', TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
PRINT @LOGLINE + @csvStore;     EXEC ('BULK INSERT [Data].[Store]            FROM ''' + @csvStore +    ''' WITH (CODEPAGE = ''65001'', TABLOCK, FORMAT=''CSV'', FIRSTROW=2, FIELDTERMINATOR ='','' )');
