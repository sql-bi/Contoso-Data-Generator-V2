# Configuration data (data.xlsx)

The Excel configuration file contains both fixed data and parameters to control the distribution of random data. For example, from here you can decide the relative percentage of orders for categories and subcategories.

The file contains several sheets, further described here. Each sheet contains multiple columns. The software reads some of the columns recognizing them by name. Columns with names that do not follow the standard requirements of the software are ignored. Columns have been conveniently colored in yellow if they are used by the software. Any non-yellow color is considered a comment and it is useful only for human purposes. 

### Categories
From here you can configure sales of categories using two curves: W and PPC. “W” define the relative weight of each category in the set of all categories for different periods in the entire timeframe. “PPC” define the variation in the price of items of each category during the whole period (Price percent). Normally the last column is 100%.

### Subcategories
From here you can configure sales of subcategories using a weight curve with columns marked with W. The values are used to define the weight of a subcategory inside its category. Therefore, the numbers are summed by category and then used to weight subcategories inside the category.

### Subcatlinks
On this page, you can configure the likelihood that one product in a subcategory triggers the purchase of another product in another subcategory. The values are in percentage: <17, 18, 80%> means that if a product of subcategory 17 is added to an order, there is an 80% chance that a product of subcategory 18 will be added to the same order.

### Products
On this page, you configure, for each product, the initial price and the distribution of sales of the product over different periods. The weights identified in the W columns are relative to the subcategory to which the product belongs.

### CustomerClusters 
On this page, you define clusters of customers. Each cluster is defined by two columns: OW (OrderWeight) and CW (CustomerWeight). Order Weight defines the percentage of orders assigned to customers belonging to the cluster, whereas CustomerWeight defines the percentage of the total customers used to fill the cluster.

It is possible to define a large cluster of customers that generates a small number of orders. The number of clusters is free.

### GeoAreas
This page is intended to define geographical areas, each with a set of weights to change the activity of the area over time. Each area is independent of the other and the definition of geographical areas needs to be done at the leaf level: no grouping is provided.
For each geographic area, you define the W columns to provide the activity spline.

### Stores
On this page, you enumerate the stores. For each store, you provide its geographical area and the open and close date. A store is active only between the two dates.
You do not provide weight activity for the stores, as the behavior is dictated by the customer clusters. A special store marked -1 as StoreID defines the online store.
Each order is assigned to either the online store or to a local store depending on the country of the customer.