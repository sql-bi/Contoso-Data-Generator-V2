CLS

sqlcmd -S (LocalDb)\MSSQLLocalDB -d ContosoDGV2Test -i CreateTablesCommon.sql -i CreateTablesOrders.sql  

sqlcmd -S (LocalDb)\MSSQLLocalDB -d ContosoDGV2Test -i ImportDataCommon.sql   -i ImportDataOrders.sql    -v varCD="%CD%"

PAUSE
