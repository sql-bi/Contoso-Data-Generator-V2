# Contoso Data Generator V2


DataGenerator is a tool for generating sets of fake data, ready to be imported into PowerBI or Fabric OneLake for analysis. This is the V2 version, evolution of the [older one](https://github.com/sql-bi/Contoso-Data-Generator).

If you are just interested in **ready to use datasets** , [download them here.](https://github.com/sql-bi/Contoso-Data-Generator-V2-Data)

Supported [output formats](docs/formats.md):
 - Parquet
 - Delta Table (files)
 - CSV
 - CSV multi file
 - CSV multi file - gz compressed
 - Sql Server, via bulk insert script of the generated CSV files


Delta Table output can be directly used in Fabric LakeHouse without any conversion:

<img src="docs/imgs/fabric_01.png" width="700px"/><br/><br/>

<br/>
<br/>

Data schema (Sales version):

<br/>

![Schema Sales](docs/imgs/schema_sales.svg)

<br/>

Data schema (Orders/OrderRows version):

<br/>

![Schema Sales](docs/imgs/schema_orders.svg)

<br/>


## Usage

### Command line details

DataGenerator requires four mandatory elements to run:
 - a configuration file (json)
 - a data file (excel)
 - an output folder
 - a cache folder
 - [optional parameters]

```
databasegenerator.exe  configfile  datafile  outputfolder  cachefolder   [param:AAAAA=nnnn] [param:BBBBB=mmmm]
```
Example:

```
databasegenerator.exe  c:\temp\config.json  c:\temp\data.xlsx  c:\temp\OUT\  c:\temp\CACHE\
```

**Note**: the tool needs some files containing static data: fake customers, exchange rates, postal codes, etc. The files are cached after been downloaded over the Internet from a specific SQLBI repository. [More details](docs/details.md).

### Ready to use run scripts

Under `script/dataset`, there are 3 ready to use scripts:
 - `make_tool.cmd` : compiles the tool in release mode, using dotnet from the command line.
 - `build_all_datasets.cmd` : creates the sets of data published on the ready-to-use repository.
 - `build_dataset.cmd` : create a single dataset.

Steps:
 - run `build_tool.cmd`
 - run `build_dataset.cmd`. When asked, enter the code of the dataset you want to create. E.g.: `csv-100k`, `delta-1m`, `parquet-10m`, etc.



## Next steps

 - [Transfer data to Fabric LakeHouse](docs/lakehouse.md)
 - Import to PowerBI Desktop
 - [Import to Sql Server database](docs/importsql.md)

