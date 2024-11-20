@ECHO OFF
ECHO --- Start

IF "%1" == "" (
    SET /P "id=Enter ID: "
) ELSE (
	SET id=%1
)

ECHO OPTION:: %id%

IF "%id%" == "csv-10k"       CALL :do_build  %id%  CSV         10000      0.05  2015-01-01  10  2021-05-18  2024-04-20  ""
IF "%id%" == "delta-10k"     CALL :do_build  %id%  DELTATABLE  10000      0.05  2015-01-01  10  2021-05-18  2024-04-20  2000
IF "%id%" == "parquet-10k"   CALL :do_build  %id%  PARQUET     10000      0.05  2015-01-01  10  2021-05-18  2024-04-20  ""
IF "%id%" == "csv-100k"      CALL :do_build  %id%  CSV         100000     0.05  2015-01-01  10  2014-05-18  2024-04-20  ""
IF "%id%" == "delta-100k"    CALL :do_build  %id%  DELTATABLE  100000     0.05  2015-01-01  10  2014-05-18  2024-04-20  20000
IF "%id%" == "parquet-100k"  CALL :do_build  %id%  PARQUET     100000     0.05  2015-01-01  10  2014-05-18  2024-04-20  ""
IF "%id%" == "csv-1m"        CALL :do_build  %id%  CSV         1000000    0.05  2015-01-01  10  2014-05-18  2024-04-20  ""
IF "%id%" == "delta-1m"      CALL :do_build  %id%  DELTATABLE  1000000    0.05  2015-01-01  10  2014-05-18  2024-04-20  200000
IF "%id%" == "parquet-1m"    CALL :do_build  %id%  PARQUET     1000000    0.05  2015-01-01  10  2014-05-18  2024-04-20  ""
IF "%id%" == "csv-10m"       CALL :do_build  %id%  CSV         10000000   0.80  2015-01-01  10  2014-05-18  2024-04-20  ""
IF "%id%" == "delta-10m"     CALL :do_build  %id%  DELTATABLE  10000000   0.80  2015-01-01  10  2014-05-18  2024-04-20  2000000
IF "%id%" == "parquet-10m"   CALL :do_build  %id%  PARQUET     10000000   0.80  2015-01-01  10  2014-05-18  2024-04-20  ""
IF "%id%" == "csv-100m"      CALL :do_build  %id%  CSV         100000000  1.00  2015-01-01  10  2014-05-18  2024-04-20  ""
IF "%id%" == "delta-100m"    CALL :do_build  %id%  DELTATABLE  100000000  1.00  2015-01-01  10  2014-05-18  2024-04-20  20000000
IF "%id%" == "parquet-100m"  CALL :do_build  %id%  PARQUET     100000000  1.00  2015-01-01  10  2014-05-18  2024-04-20  ""

ECHO --- End
GOTO :eof


:do_build
ECHO DO BUILD : %1 %2 %3 %4 %5 %6 %7 %8 %9
..\..\DatabaseGenerator\bin\Release\net8.0\DatabaseGenerator.exe config.json  data.xlsx  out\%1  cache  param:OutputFormat=%2  param:OrdersCount=%3  param:CustomerPercentage=%4  param:StartDT=%5 param:YearsCount=%6 param:CutDateBefore=%7  param:CutDateAfter=%8 param:DeltaTableOrdersPerFile=%9

