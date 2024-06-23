 
CREATE TABLE [dbo].[Sales](
    [OrderKey]      [bigint]       NOT NULL,
    [LineNumber]    [int]          NOT NULL,
    [OrderDate]     [date]         NOT NULL,
    [DeliveryDate]  [date]         NOT NULL,
    [CustomerKey]   [int]          NOT NULL,
    [StoreKey]      [int]          NOT NULL,
    [ProductKey]    [int]          NOT NULL,
    [Quantity]      [float]        NOT NULL,
    [UnitPrice]     [float]        NOT NULL,
    [NetPrice]      [float]        NOT NULL,
    [UnitCost]      [float]        NOT NULL,
    [CurrencyCode]  [nvarchar](5)  NOT NULL,
    [ExchangeRate]  [float]        NOT NULL,
    CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED  ( [OrderKey] ASC, [LineNumber] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[Sales] WITH CHECK ADD CONSTRAINT [FK_Sales_Customers] FOREIGN KEY([CustomerKey]) REFERENCES [dbo].[Customer] ([CustomerKey])
ALTER TABLE [dbo].[Sales] WITH CHECK ADD CONSTRAINT [FK_Sales_Products]  FOREIGN KEY([ProductKey])  REFERENCES [dbo].[Product]  ([ProductKey])
ALTER TABLE [dbo].[Sales] WITH CHECK ADD CONSTRAINT [FK_Sales_Stores]    FOREIGN KEY([StoreKey])    REFERENCES [dbo].[Store]    ([StoreKey])

ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Customers]
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Products]
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Stores]

CREATE NONCLUSTERED INDEX [IX_Sales_CustomerKey] ON [dbo].[Sales] ( [CustomerKey] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Sales_ProductKey]  ON [dbo].[Sales] ( [ProductKey] ASC )  WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Sales_StoreKey]    ON [dbo].[Sales] ( [StoreKey] ASC )    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
