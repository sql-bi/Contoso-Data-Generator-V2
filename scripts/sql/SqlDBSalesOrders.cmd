CLS

sqlcmd -S (LocalDb)\MSSQLLocalDB -d ContosoDGV2Test -i CreateTablesCommon.sql -i CreateTablesSales.sql -i CreateTablesOrders.sql 

sqlcmd -S (LocalDb)\MSSQLLocalDB -d ContosoDGV2Test -i ImportDataCommon.sql   -i ImportDataSales.sql   -i ImportDataOrders.sql    -v varCD="%CD%"

PAUSE
