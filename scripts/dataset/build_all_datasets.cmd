
CALL build_and_zip.cmd csv-100k    
CALL build_and_zip.cmd delta-100k  
CALL build_and_zip.cmd parquet-100k 

CALL build_and_zip.cmd csv-1m
CALL build_and_zip.cmd delta-1m
CALL build_and_zip.cmd parquet-1m

CALL build_and_zip.cmd csv-10m
CALL build_and_zip.cmd delta-10m
CALL build_and_zip.cmd parquet-10m

CALL build_and_zip.cmd csv-100m      -v500m
CALL build_and_zip.cmd delta-100m    -v500m
CALL build_and_zip.cmd parquet-100m  -v500m

PAUSE
