@ECHO OFF
ECHO.

IF "%1" == ""  SET SqlServerName=Demo
IF "%1" == ""  SET DatabaseName=ContosoV2


ECHO.
IF "%1" == "" (
    SET /P "RUNMODE=Enter run mode (sales/orders/both): "
) ELSE (
	SET RUNMODE=%1
)

IF "%2" == "" (
    ECHO No Trim
) ELSE (
	SET TRIMTABLES=%2
)


@ECHO.
ECHO RunMode:       %RUNMODE%
ECHO SqlServerName: %SqlServerName%
ECHO DatabaseName : %DatabaseName%
ECHO TrimTables   : %TRIMTABLES%

@ECHO.
IF "%1" == "" ( PAUSE )


REM --- Create tables ---
                           sqlcmd -S "%SqlServerName%" -d "%DatabaseName%"  -i CreateTablesCommon.sql
IF "%RUNMODE%" == "sales"  sqlcmd -S "%SqlServerName%" -d "%DatabaseName%"  -i CreateTablesSales.sql 
IF "%RUNMODE%" == "orders" sqlcmd -S "%SqlServerName%" -d "%DatabaseName%"  -i CreateTablesOrders.sql 
IF "%RUNMODE%" == "both"   sqlcmd -S "%SqlServerName%" -d "%DatabaseName%"  -i CreateTablesSales.sql  -i CreateTablesOrders.sql 

REM --- Import data ---
                           sqlcmd -S "%SqlServerName%" -d "%DatabaseName%"  -i ImportDataCommon.sql                            -v varCD="%CD%"
IF "%RUNMODE%" == "sales"  sqlcmd -S "%SqlServerName%" -d "%DatabaseName%"  -i ImportDataSales.sql                             -v varCD="%CD%"
IF "%RUNMODE%" == "orders" sqlcmd -S "%SqlServerName%" -d "%DatabaseName%"  -i ImportDataOrders.sql                            -v varCD="%CD%"
IF "%RUNMODE%" == "both"   sqlcmd -S "%SqlServerName%" -d "%DatabaseName%"  -i ImportDataSales.sql    -i ImportDataOrders.sql  -v varCD="%CD%"


ECHO 
ECHO
IF "%TRIMTABLES%" =="Trim" sqlcmd -S "%SqlServerName%" -d "%DatabaseName%"  -i TrimTables.sql -v varCD="%CD%"

ECHO.
ECHO ### The end ###
ECHO.

IF "%1" == "" ( PAUSE )
