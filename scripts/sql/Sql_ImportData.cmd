ECHO OFF

CALL Sql_Config.cmd

ECHO.
SET /P "RUNMODE=Enter run mode (sales/orders/both): "
ECHO Mode: %RUNMODE%

REM --- Create tables ---
                           sqlcmd -S %SqlServerName% -d %DatabaseName%  -i CreateTablesCommon.sql
IF "%RUNMODE%" == "sales"  sqlcmd -S %SqlServerName% -d %DatabaseName%  -i CreateTablesSales.sql 
IF "%RUNMODE%" == "orders" sqlcmd -S %SqlServerName% -d %DatabaseName%  -i CreateTablesOrders.sql 
IF "%RUNMODE%" == "both"   sqlcmd -S %SqlServerName% -d %DatabaseName%  -i CreateTablesSales.sql  -i CreateTablesOrders.sql 

REM --- Import data ---
                           sqlcmd -S %SqlServerName% -d %DatabaseName%  -i ImportDataCommon.sql                            -v varCD="%CD%"
IF "%RUNMODE%" == "sales"  sqlcmd -S %SqlServerName% -d %DatabaseName%  -i ImportDataSales.sql                             -v varCD="%CD%"
IF "%RUNMODE%" == "orders" sqlcmd -S %SqlServerName% -d %DatabaseName%  -i ImportDataOrders.sql                            -v varCD="%CD%"
IF "%RUNMODE%" == "both"   sqlcmd -S %SqlServerName% -d %DatabaseName%  -i ImportDataSales.sql    -i ImportDataOrders.sql  -v varCD="%CD%"


ECHO.
ECHO ### The end ###
ECHO.
PAUSE