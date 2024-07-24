DEL ..\..\DatabaseGenerator\bin\publish\*.* /Q
DEL ..\..\DatabaseGenerator\bin\publish\win-x64\*.* /Q
DEL ..\..\DatabaseGenerator\bin\publish\linux-x64\*.* /Q
DEL ..\..\DatabaseGenerator\bin\publish\osx-x64\*.* /Q
DEL ..\..\DatabaseGenerator\bin\publish\osx-arm64\*.* /Q

dotnet  build    ../../ContosoDGV2.sln --no-incremental --configuration Release
dotnet  publish  ../../ContosoDGV2.sln -p:PublishProfile=WinX64
dotnet  publish  ../../ContosoDGV2.sln -p:PublishProfile=LinuxX64
dotnet  publish  ../../ContosoDGV2.sln -p:PublishProfile=osx-x64
dotnet  publish  ../../ContosoDGV2.sln -p:PublishProfile=osx-arm64

COPY config.json ..\..\DatabaseGenerator\bin\publish\win-x64   /Y
COPY data.xlsx   ..\..\DatabaseGenerator\bin\publish\win-x64   /Y
COPY readme.txt  ..\..\DatabaseGenerator\bin\publish\win-x64   /Y

COPY config.json ..\..\DatabaseGenerator\bin\publish\linux-x64 /Y
COPY data.xlsx   ..\..\DatabaseGenerator\bin\publish\linux-x64 /Y
COPY readme.txt  ..\..\DatabaseGenerator\bin\publish\linux-x64 /Y

COPY config.json ..\..\DatabaseGenerator\bin\publish\osx-x64   /Y
COPY data.xlsx   ..\..\DatabaseGenerator\bin\publish\osx-x64   /Y
COPY readme.txt  ..\..\DatabaseGenerator\bin\publish\osx-x64   /Y

COPY config.json ..\..\DatabaseGenerator\bin\publish\osx-arm64 /Y
COPY data.xlsx   ..\..\DatabaseGenerator\bin\publish\osx-arm64 /Y
COPY readme.txt  ..\..\DatabaseGenerator\bin\publish\osx-arm64 /Y


cd ..\..\DatabaseGenerator\bin\publish\win-x64 
TAR.exe  -a -c -f DatabaseGenerator.winx64.zip   readme.txt config.json data.xlsx DatabaseGenerator.exe 
COPY DatabaseGenerator.winx64.zip ..\DatabaseGenerator.winx64.zip

cd ..\linux-x64
TAR.exe  -a -c -f DatabaseGenerator.linuxx64.zip   readme.txt config.json data.xlsx DatabaseGenerator
COPY DatabaseGenerator.linuxx64.zip ..\DatabaseGenerator.linuxx64.zip

cd ..\osx-x64
TAR.exe  -a -c -f DatabaseGenerator.osx-x64.zip   readme.txt config.json data.xlsx DatabaseGenerator
COPY DatabaseGenerator.osx-x64.zip ..\DatabaseGenerator.osx-x64.zip

cd ..\osx-arm64
TAR.exe  -a -c -f DatabaseGenerator.osx-arm64.zip   readme.txt config.json data.xlsx DatabaseGenerator
COPY DatabaseGenerator.osx-arm64.zip ..\DatabaseGenerator.osx-arm64.zip 


PAUSE