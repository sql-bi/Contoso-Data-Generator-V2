PRINT 'SQL > Creating [Data].[Sales]'

CREATE TABLE [Data].[Sales](
    [OrderKey]      [bigint]       NOT NULL,
    [LineNumber]    [int]          NOT NULL,
    [OrderDate]     [date]         NOT NULL,
    [DeliveryDate]  [date]         NOT NULL,
    [CustomerKey]   [int]          NOT NULL,
    [StoreKey]      [int]          NOT NULL,
    [ProductKey]    [int]          NOT NULL,
    [Quantity]      [int]        NOT NULL,
    [UnitPrice]     [money]        NOT NULL,
    [NetPrice]      [money]        NOT NULL,
    [UnitCost]      [money]        NOT NULL,
    [CurrencyCode]  [nvarchar](5)  NOT NULL,
    [ExchangeRate]  [float]        NOT NULL,
    CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED  ( [OrderKey] ASC, [LineNumber] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [Data].[Sales] WITH CHECK ADD CONSTRAINT [FK_Sales_Customers] FOREIGN KEY([CustomerKey]) REFERENCES [Data].[Customer] ([CustomerKey])
ALTER TABLE [Data].[Sales] WITH CHECK ADD CONSTRAINT [FK_Sales_Products]  FOREIGN KEY([ProductKey])  REFERENCES [Data].[Product]  ([ProductKey])
ALTER TABLE [Data].[Sales] WITH CHECK ADD CONSTRAINT [FK_Sales_Stores]    FOREIGN KEY([StoreKey])    REFERENCES [Data].[Store]    ([StoreKey])

ALTER TABLE [Data].[Sales] CHECK CONSTRAINT [FK_Sales_Customers]
ALTER TABLE [Data].[Sales] CHECK CONSTRAINT [FK_Sales_Products]
ALTER TABLE [Data].[Sales] CHECK CONSTRAINT [FK_Sales_Stores]

CREATE NONCLUSTERED INDEX [IX_Sales_CustomerKey] ON [Data].[Sales] ( [CustomerKey] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Sales_ProductKey]  ON [Data].[Sales] ( [ProductKey] ASC )  WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Sales_StoreKey]    ON [Data].[Sales] ( [StoreKey] ASC )    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

GO


------------------------------------------------------------------------------------------
--
--	Views
--
------------------------------------------------------------------------------------------

CREATE OR ALTER VIEW dbo.Sales AS
SELECT 
        OrderKey AS [Order Number],
        [LineNumber] AS [Line Number],
        [OrderDate] AS [Order Date],
        [DeliveryDate] AS [Delivery Date],
        CustomerKey,
        StoreKey,
        ProductKey,
        Quantity,
        UnitPrice AS [Unit Price],
        NetPrice AS [Net Price],
        UnitCost AS [Unit Cost],
        CurrencyCode AS [Currency Code],
        ExchangeRate AS [Exchange Rate]
    FROM
        [Data].Sales  
                    
GO

