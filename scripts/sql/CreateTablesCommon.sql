PRINT 'SQL > Creating common tables'

--
-- Create Demo schema if it does not exist
--
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Data')
BEGIN
    -- The schema must be run in its own batch!
    EXEC( 'CREATE SCHEMA Data' );
END

DROP TABLE IF EXISTS [Data].[OrderRows];
DROP TABLE IF EXISTS [Data].[Orders];
DROP TABLE IF EXISTS [Data].[Sales];
DROP TABLE IF EXISTS [Data].[Customer];
DROP TABLE IF EXISTS [Data].[Product];
DROP TABLE IF EXISTS [Data].[Store];
DROP TABLE IF EXISTS [Data].[CurrencyExchange];
DROP TABLE IF EXISTS [Data].[Date];


CREATE TABLE [Data].[CurrencyExchange](
    [Date]          [date]          NOT NULL,
    [FromCurrency]  [nchar](3)      NOT NULL,
    [ToCurrency]    [nchar](3)      NOT NULL,
    [Exchange]      [float]         NOT NULL,
    CONSTRAINT [PK_CurrencyExchange] PRIMARY KEY CLUSTERED ( [Date] ASC, [FromCurrency] ASC, [ToCurrency] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [Data].[Customer](
    [CustomerKey]    [int]            NOT NULL,
    [GeoAreaKey]     [int]            NOT NULL,
    [StartDT]        [Date]           NULL,
    [EndDT]          [Date]           NULL,
    [Continent]      [nvarchar](50)   NULL,
    [Gender]         [nvarchar](10)   NULL,
    [Title]          [nvarchar](50)   NULL,
    [GivenName]      [nvarchar](150)  NULL,
    [MiddleInitial]  [nvarchar](150)  NULL,
    [Surname]        [nvarchar](150)  NULL,
    [StreetAddress]  [nvarchar](150)  NULL,
    [City]           [nvarchar](50)   NULL,
    [State]          [nvarchar](50)   NULL,
    [StateFull]      [nvarchar](50)   NULL,
    [ZipCode]        [nvarchar](50)   NULL,
    [Country]        [nvarchar](50)   NULL,
    [CountryFull]    [nvarchar](50)   NULL,
    [Birthday]       [Date]           NOT NULL,
    [Age]            [int]            NOT NULL,
    [Occupation]     [nvarchar](100)  NULL,
    [Company]        [nvarchar](50)   NULL,
    [Vehicle]        [nvarchar](50)   NULL,
    [Latitude]       [float]          NOT NULL,
    [Longitude]      [float]          NOT NULL,    
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ( [CustomerKey] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

 
CREATE TABLE [Data].[Date](
    [Date]               [Date]          NOT NULL ,
    [DateKey]            [nvarchar](50)  NOT NULL,
    [Year]               [int]           NOT NULL ,
    [YearQuarter]        [nvarchar](30)  NOT NULL,
    [YearQuarterNumber]  [int]           NOT NULL,
    [Quarter]            [nvarchar](2)   NOT NULL,
    [YearMonth]          [nvarchar](30)  NOT NULL,
    [YearMonthShort]     [nvarchar](30)  NOT NULL,
    [YearMonthNumber]    [int]           NOT NULL,
    [Month]              [nvarchar](30)  NOT NULL,
    [MonthShort]         [nvarchar](30)  NOT NULL,
    [MonthNumber]        [int]           NOT NULL ,
    [DayofWeek]          [nvarchar](30)  NOT NULL,
    [DayofWeekShort]     [nvarchar](30)  NOT NULL,
    [DayofWeekNumber]    [int]           NOT NULL,
    [WorkingDay]         [bit]           NOT NULL ,
    [WorkingDayNumber]   [int]           NOT NULL,
    CONSTRAINT [PK_Date] PRIMARY KEY CLUSTERED ( [DateKey] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [Data].[Product](
    [ProductKey]       [int]            NOT NULL,
    [ProductCode]      [nvarchar](255)  NOT NULL,
    [ProductName]      [nvarchar](500)  NULL,
    [Manufacturer]     [nvarchar](50)   NULL,
    [Brand]            [nvarchar](50)   NULL,
    [Color]            [nvarchar](20)   NULL,
    [WeightUnit]       [nvarchar](20)   NULL,
    [Weight]           [float]          NULL,
    [Cost]             [money]          NULL,
    [Price]            [money]          NULL,
    [CategoryKey]      [int]            NULL,
    [CategoryName]     [nvarchar](30)   NULL,
    [SubCategoryKey]   [int]            NULL,
    [SubCategoryName]  [nvarchar](50)   NULL,
    CONSTRAINT [PK_TestProduct] PRIMARY KEY CLUSTERED ( [ProductKey] ASC ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] 
 ) ON [PRIMARY]


CREATE TABLE [Data].[Store] (
    [StoreKey]       [int]             NOT NULL,
    [StoreCode]      [int]             NOT NULL,
    [GeoAreaKey]     [int]             NOT NULL,
    [CountryCode]    [nvarchar](50)    NULL,
    [CountryName]    [nvarchar](50)    NULL,
    [State]          [nvarchar](100)   NULL,
    [OpenDate]       [Date]            NULL,
    [CloseDate]      [Date]            NULL,
    [Description]    [nvarchar](100)   NULL,
    [SquareMeters]   [int]             NULL,
    [Status]         [nvarchar](50)    NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED ( [StoreKey] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

------------------------------------------------------------------------------------------
--
--	Views
--
------------------------------------------------------------------------------------------
CREATE OR ALTER VIEW dbo.Customer AS
	SELECT
		[CustomerKey],
		[Gender],
		[GivenName] + ' ' + [Surname] AS [Name],
		[StreetAddress] AS [Address],
		[City],
		[State] AS [State Code],
		StateFull AS [State],
		ZipCode AS [Zip Code],
		[Country] as [Country Code],
		[CountryFull] as [Country],
		[Continent],
		[Birthday],
		[Age] as [Age]
	FROM
		[Data].Customer
GO


CREATE OR ALTER VIEW dbo.Product AS
	SELECT
		ProductKey,
		ProductCode AS [Product Code],
		ProductName AS [Product Name],
		[Manufacturer],
		[Brand],
		[Color],
		WeightUnit AS [Weight Unit Measure],
		[Weight],
		Cost AS [Unit Cost],
		Price AS [Unit Price],
		SubcategoryKey AS [Subcategory Code],
		SubCategoryName AS [Subcategory],
		CategoryKey AS [Category Code],
		CategoryName AS [Category]
	FROM [Data].Product

GO

CREATE OR ALTER VIEW dbo.[Currency Exchange] AS
    SELECT
	    [Date],
	    [FromCurrency],
	    [ToCurrency],
	    [Exchange]
    FROM [Data].[CurrencyExchange]
GO


CREATE OR ALTER VIEW dbo.Store AS
SELECT 
    StoreKey,
    StoreCode AS [Store Code],
    CountryName AS [Country],
    [State],
    [Description] AS [Name],
    SquareMeters AS [Square Meters],
    OpenDate AS [Open Date],
    CloseDate AS [Close Date],
    [Status]
FROM
    [Data].Store
GO

CREATE OR ALTER VIEW dbo.Date AS
    SELECT 
      [Date], 
      -- [DateKey],  -- We do not import DateKey in the view
      [Year], 
      YearQuarter AS [Year Quarter], 
      YearQuarterNumber AS [Year Quarter Number], 
      [Quarter], 
      YearMonth AS [Year Month], 
      YearMonthShort AS [Year Month Short], 
      YearMonthNumber AS [Year Month Number], 
      [Month], 
      MonthShort AS [Month Short], 
      MonthNumber AS [Month Number], 
      [DayofWeek] AS [Day of Week], 
      DayofWeekShort AS [Day of Week Short], 
      DayofWeekNumber AS [Day of Week Number], 
      WorkingDay AS [Working Day], 
      WorkingDayNumber AS [Working Day Number] 
    FROM 
      [Data].[Date]
GO

