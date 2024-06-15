# Configuration file (config.json)

This file contains the main configuration of the data generator.
- **OrdersCount**: (int) total number of orders to be generated.

- **StartDT**: (datetime) date of the first order.

- **YearsCount**: (int) total number of years generated. Orders are distributed over the years.

- **CutDateBefore**, **CutDateAfter**: (datetime optional parameters) the 2 parameters allow to create data starting from a day different from January 1st  and ending on a date different from December 31st. Data before CutDateBefore and after CutDateAfter is removed

- **CustomerFakeGenerator**: (int) number of fake customers. If > 0, customers.rpt file is ignored and a number of fake customers are generated.

- **DaysWeight** (section)

    - **DaysWeightConstant**: (bool) if set to true, the configuration about days is ignored.

    - **DaysWeightPoints**, **DaysWeighValues**: (double[]) points for interpolating the curve of distribution of orders over time. It covers the entire YearsCount period.

    - **DaysWeightAddSpikes**: (bool) if set to false, annual spikes are ignored.

    - **WeekDaysFactor**: (double[] – length 7) weight multiplication factor for each day of the week. The first day is Sunday.

    - **DayRandomness**: (double) percentage of randomness add to days, to avoid having a too-perfect curve over time.

- **OrderRowsWeights**: (double[]) distribution of the number of rows per order. Each element is a weight. The first element is the weight of orders with one row, the second is the weight of orders with two rows. and so on

- **OrderQuantityWeights**: (double[]) distribution of the quantity applied to each order row. Each element is a weight. The first element is the weight of rows with quantity=1, the second element is the weight of rows with quantity=2, and so on.

- **DiscountWeights**: (double[]) distribution of the discounts applied to order rows. Each element is a weight. The first element is the weight of rows with a discount of 0%, the second element is the weight of rows with a discount of 1%, and so on.

- **OnlinePerCent**: (double[]) distribution of the percentage of orders sold online, over the orders total. 

- **DeliveryDateLambdaWeights**: (double[]) distribution of the days for delivery. The delivery date is computed by adding one day plus a random number generated using the distribution built from this parameter.

- **CountryCurrency**: table mapping Country to Currency

- **AnnualSpikes** : set of periods where orders show a spike. For each spike, you define the start day, the end day, and the multiplication factor.

- **OneTimeSpikes**: set of spikes with a fixed start and end date. For each spike, you define the start end, the end date, and the multiplication factor.

- **CustomerActivity** : contains the configuration for customer start/end date

    - **StartDateWeightPoints**, **StartDarteWeightValues**: configuration for the spline of customer start date

    - **EndDateWeightPoints**, **EndDateWeightValues**: configuration for the spline of customer end dates
