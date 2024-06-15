
CREATE TABLE [dbo].[Orders](
     [OrderKey]      [bigint]       NOT NULL,
     [CustomerKey]   [int]          NOT NULL,
     [StoreKey]      [int]          NOT NULL,
     [OrderDate]     [date]         NOT NULL,
     [DeliveryDate]  [date]         NOT NULL,
     [CurrencyCode]  [nvarchar](5)  NOT NULL
     CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED  ( [OrderKey] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[OrderRows](
     [OrderKey]   [bigint] NOT NULL,
     [LineNumber] [int]    NOT NULL,
     [ProductKey] [int]    NOT NULL,
     [Quantity]   [float]  NOT NULL,
     [UnitPrice]  [float]  NOT NULL,
     [NetPrice]   [float]  NOT NULL,
     [UnitCost]   [float]  NOT NULL    
     CONSTRAINT [PK_OrderRows] PRIMARY KEY CLUSTERED  ( [OrderKey] ASC, [LineNumber] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[OrderRows] WITH CHECK ADD  CONSTRAINT [FK_OrderRows_Orders]   FOREIGN KEY([OrderKey])   REFERENCES [dbo].[Orders]   ([OrderKey])
ALTER TABLE [dbo].[OrderRows] WITH CHECK ADD  CONSTRAINT [FK_OrderRows_Products] FOREIGN KEY([ProductKey]) REFERENCES [dbo].[Products] ([ProductKey])
ALTER TABLE [dbo].[OrderRows] CHECK CONSTRAINT [FK_OrderRows_Orders]
ALTER TABLE [dbo].[OrderRows] CHECK CONSTRAINT [FK_OrderRows_Products]

ALTER TABLE [dbo].[Orders] WITH CHECK ADD CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerKey]) REFERENCES [dbo].[Customers] ([CustomerKey])
ALTER TABLE [dbo].[Orders] WITH CHECK ADD CONSTRAINT [FK_Orders_Stores]    FOREIGN KEY([StoreKey])    REFERENCES [dbo].[Stores]    ([StoreKey])
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers]
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Stores]

CREATE NONCLUSTERED INDEX [IX_OrderRows_OrderKey]    ON [dbo].[OrderRows] ( [OrderKey] ASC )    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_OrderRows_ProductKey]  ON [dbo].[OrderRows] ( [ProductKey] ASC )  WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Orders_CustomerKey]    ON [dbo].[Orders]    ( [CustomerKey] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Orders_StoreKey]       ON [dbo].[Orders]    ( [StoreKey] ASC )    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
