
CALL build_dataset.cmd %1

bin\7za.exe  a  out\%1.7z  out\%1\.  -x!_log.log  %2 
