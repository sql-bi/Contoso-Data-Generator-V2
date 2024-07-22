DECLARE @LOGLINE NVARCHAR(10) = Char(10) + Char(10) + '#### '

PRINT @LOGLINE + 'Trim Customer'
DELETE FROM [Data].Customer 
WHERE CustomerKey NOT IN (SELECT DISTINCT CustomerKey FROM [Data].Sales )
AND CustomerKey NOT IN (SELECT DISTINCT CustomerKey FROM [Data].Orders )
GO
