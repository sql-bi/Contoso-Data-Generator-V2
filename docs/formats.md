# Output formats and related parameters

** DRAFT **

## CSV

| Parameter | Values |  Notes |
| -- | -- | -- |
| OutputFormat | "CSV" | "OutputFormat": "CSV" |
| CsvMaxOrdersPerFile | -1 or a number >1 |  Maximum number of Orders per file |
| CsvGzCompression | 0 or 1 | Apply GZ compression to output CSV files |

For creating a single big CSV file:
```
"OutputFormat": "CSV"
"CsvMaxOrdersPerFile": -1
"CsvGzCompression": 0
```

For creating multiple CSV files:
```
"OutputFormat": "CSV"
"CsvMaxOrdersPerFile": 50000
"CsvGzCompression": 0
```

For creating multiple CSV.GZ files:
```
"OutputFormat": "CSV"
"CsvMaxOrdersPerFile": 50000
"CsvGzCompression": 1
```

## Parquet

"PARQUET" . . . . 

## Delta Table

"DELTATABLE"  . . . . 

