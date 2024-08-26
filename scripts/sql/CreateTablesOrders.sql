PRINT 'SQL > Creating [Data].[Orders] and [Data].[OrderRows]'

CREATE TABLE [Data].[Orders](
     [OrderKey]      [bigint]       NOT NULL,
     [CustomerKey]   [int]          NOT NULL,
     [StoreKey]      [int]          NOT NULL,
     [OrderDate]     [date]         NOT NULL,
     [DeliveryDate]  [date]         NOT NULL,
     [CurrencyCode]  [nvarchar](5)  NOT NULL
     CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED  ( [OrderKey] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [Data].[OrderRows](
     [OrderKey]   [bigint] NOT NULL,
     [LineNumber] [int]    NOT NULL,
     [ProductKey] [int]    NOT NULL,
     [Quantity]   [int]  NOT NULL,
     [UnitPrice]  [money]  NOT NULL,
     [NetPrice]   [money]  NOT NULL,
     [UnitCost]   [money]  NOT NULL    
     CONSTRAINT [PK_OrderRows] PRIMARY KEY CLUSTERED  ( [OrderKey] ASC, [LineNumber] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [Data].[OrderRows] WITH CHECK ADD  CONSTRAINT [FK_OrderRows_Orders]   FOREIGN KEY([OrderKey])   REFERENCES [Data].[Orders]  ([OrderKey])
ALTER TABLE [Data].[OrderRows] WITH CHECK ADD  CONSTRAINT [FK_OrderRows_Products] FOREIGN KEY([ProductKey]) REFERENCES [Data].[Product] ([ProductKey])
ALTER TABLE [Data].[OrderRows] CHECK CONSTRAINT [FK_OrderRows_Orders]
ALTER TABLE [Data].[OrderRows] CHECK CONSTRAINT [FK_OrderRows_Products]

ALTER TABLE [Data].[Orders] WITH CHECK ADD CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerKey]) REFERENCES [Data].[Customer] ([CustomerKey])
ALTER TABLE [Data].[Orders] WITH CHECK ADD CONSTRAINT [FK_Orders_Stores]    FOREIGN KEY([StoreKey])    REFERENCES [Data].[Store]    ([StoreKey])
ALTER TABLE [Data].[Orders] CHECK CONSTRAINT [FK_Orders_Customers]
ALTER TABLE [Data].[Orders] CHECK CONSTRAINT [FK_Orders_Stores]

CREATE NONCLUSTERED INDEX [IX_OrderRows_OrderKey]    ON [Data].[OrderRows] ( [OrderKey] ASC )    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_OrderRows_ProductKey]  ON [Data].[OrderRows] ( [ProductKey] ASC )  WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Orders_CustomerKey]    ON [Data].[Orders]    ( [CustomerKey] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Orders_StoreKey]       ON [Data].[Orders]    ( [StoreKey] ASC )    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
