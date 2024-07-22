@ECHO OFF
ECHO START

CALL :buildandzip  csv-10k    
CALL :buildandzip  csv-100k    
CALL :buildandzip  csv-1m
CALL :buildandzip  csv-10m
CALL :buildandzip  csv-100m      -v500m
CALL :buildandzip  delta-10k  
CALL :buildandzip  delta-100k  
CALL :buildandzip  delta-1m
CALL :buildandzip  delta-10m
CALL :buildandzip  delta-100m    -v500m
CALL :buildandzip  parquet-10k 
CALL :buildandzip  parquet-100k 
CALL :buildandzip  parquet-1m
CALL :buildandzip  parquet-10m
CALL :buildandzip  parquet-100m  -v500m
GOTO :EOF

:buildandzip
ECHO BUILD DATA : %~1 %~2
IF EXIST "out\%~1.7z.*" DEL out\%~1.7z.*
CALL build_single.cmd %~1
bin\7za.exe  a  out\%~1.7z  out\%~1\.  -x!_log.log  %~2 
EXIT /B 0