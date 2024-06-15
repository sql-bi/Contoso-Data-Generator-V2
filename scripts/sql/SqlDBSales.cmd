CLS

sqlcmd -S (LocalDb)\MSSQLLocalDB -d ContosoDGV2Test -i CreateTablesCommon.sql -i CreateTablesSales.sql

sqlcmd -S (LocalDb)\MSSQLLocalDB -d ContosoDGV2Test -i ImportDataCommon.sql   -i ImportDataSales.sql    -v varCD="%CD%"

PAUSE
