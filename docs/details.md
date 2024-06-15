# Details

## Dataset

Output elements:
 - Customers
 - Stores
 - Dates
 - CurrencyExchanges
 - Sales 
 - (optional) Orders & OrderRows
  
Customers dataset will be filled with fake customer data.  


## Pre-data-preparation: static data from SQLBI repository

The tool needs some files containing static data: fake customers, exchange rates, postal codes, etc. The files are cached under cache folder specified as a parameter on the command line. The files are downloaded from a specific SQLBI repository if not found in the cache folder. In normal usage, if you reuse the same cache folder, the files are downloaded only on the first run.
After downloading, some files are processed to create a consistent set of fake customers. The output file, customersall.csv, is placed under cache folder. If you delete it, it will be recreated on the following run.

https://github.com/sql-bi/Contoso-Data-Generator-V2-Data/releases/tag/static-files

