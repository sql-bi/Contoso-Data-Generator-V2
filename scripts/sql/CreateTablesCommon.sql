
DROP TABLE IF EXISTS [dbo].[OrderRows];
DROP TABLE IF EXISTS [dbo].[Orders];
DROP TABLE IF EXISTS [dbo].[Sales];
DROP TABLE IF EXISTS [dbo].[Customer];
DROP TABLE IF EXISTS [dbo].[Product];
DROP TABLE IF EXISTS [dbo].[Store];
DROP TABLE IF EXISTS [dbo].[CurrencyExchange];
DROP TABLE IF EXISTS [dbo].[Date];


CREATE TABLE [dbo].[CurrencyExchange](
    [Date]          [date]          NOT NULL,
    [FromCurrency]  [nvarchar](10)  NOT NULL,
    [ToCurrency]    [nvarchar](10)  NOT NULL,
    [Exchange]      [float]         NOT NULL,
    CONSTRAINT [PK_CurrencyExchange] PRIMARY KEY CLUSTERED ( [Date] ASC, [FromCurrency] ASC, [ToCurrency] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[Customer](
    [CustomerKey]    [int]            NOT NULL,
    [GeoAreaKey]     [int]            NOT NULL,
    [StartDT]        [Date]           NULL,
    [EndDT]          [Date]           NULL,
    [Continent]      [nvarchar](50)   NULL,
    [Gender]         [nvarchar](50)   NULL,
    [Title]          [nvarchar](50)   NULL,
    [GivenName]      [nvarchar](50)   NULL,
    [MiddleInitial]  [nvarchar](50)   NULL,
    [Surname]        [nvarchar](50)   NULL,
    [StreetAddress]  [nvarchar](50)   NULL,
    [City]           [nvarchar](50)   NULL,
    [State]          [nvarchar](50)   NULL,
    [StateFull]      [nvarchar](50)   NULL,
    [ZipCode]        [nvarchar](50)   NULL,
    [Country]        [nvarchar](50)   NULL,
    [CountryFull]    [nvarchar](50)   NULL,
    [Birthday]       [Date]           NOT NULL,
    [Age]            [int]            NOT NULL,
    [Occupation]     [nvarchar](200)  NULL,
    [Company]        [nvarchar](50)   NULL,
    [Vehicle]        [nvarchar](50)   NULL,
    [Latitude]       [float]          NOT NULL,
    [Longitude]      [float]          NOT NULL,    
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ( [CustomerKey] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

 
CREATE TABLE [dbo].[Date](
    [Date]               [Date]          NOT NULL ,
    [DateKey]            [nvarchar](50)  NOT NULL,
    [Year]               [int]           NOT NULL ,
    [YearQuarter]        [nvarchar](50)  NOT NULL,
    [YearQuarterNumber]  [nvarchar](50)  NOT NULL,
    [Quarter]            [nvarchar](50)  NOT NULL,
    [YearMonth]          [nvarchar](50)  NOT NULL,
    [YearMonthShort]     [nvarchar](50)  NOT NULL,
    [YearMonthNumber]    [nvarchar](50)  NOT NULL,
    [Month]              [nvarchar](50)  NOT NULL,
    [MonthShort]         [nvarchar](50)  NOT NULL,
    [MonthNumber]        [int]           NOT NULL ,
    [DayofWeek]          [nvarchar](50)  NOT NULL,
    [DayofWeekShort]     [nvarchar](50)  NOT NULL,
    [DayofWeekNumber]    [int]           NOT NULL,
    [WorkingDay]         [int]           NOT NULL ,
    [WorkingDayNumber]   [int]           NOT NULL,
    CONSTRAINT [PK_Date] PRIMARY KEY CLUSTERED ( [DateKey] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[Product](
    [ProductKey]       [int]            NOT NULL,
    [ProductCode]      [nvarchar](200)  NOT NULL,
    [ProductName]      [nvarchar](200)  NULL,
    [Manufacturer]     [nvarchar](50)   NULL,
    [Brand]            [nvarchar](50)   NULL,
    [Color]            [nvarchar](50)   NULL,
    [WeightUnit]       [nvarchar](50)   NULL,
    [Weight]           [float]          NULL,
    [Cost]             [nvarchar](50)   NOT NULL,
    [Price]            [nvarchar](50)   NOT NULL,
    [CategoryKey]      [int]            NOT NULL,
    [CategoryName]     [nchar](50)      NULL,
    [SubCategoryKey]   [int]            NOT NULL,
    [SubCategoryName]  [nvarchar](50)   NULL,
    CONSTRAINT [PK_TestProduct] PRIMARY KEY CLUSTERED ( [ProductKey] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] 
 ) ON [PRIMARY]


CREATE TABLE [dbo].[Store] (
    [StoreKey]       [int]             NOT NULL,
    [StoreCode]      [int]             NOT NULL,
    [GeoAreaKey]     [int]             NOT NULL,
    [CountryCode]    [nvarchar](100)   NULL,
    [CountryName]    [nvarchar](100)   NULL,
    [State]          [nvarchar](100)   NULL,
    [OpenDate]       [Date]            NULL,
    [CloseDate]      [Date]            NULL,
    [Description]    [nvarchar](100)   NULL,
    [SquareMeters]   [int]             NULL,
    [Status]         [nvarchar](50)    NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED ( [StoreKey] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

