#
# Instructions
#
# This script only creates the SQL Server databases that are populated by SQLBI_ALL_DB.cmd
#
param(
    # Instance of SQL Server to process the database
    [String]$SqlServerInstance='Demo', 

    # LOCAL Folder for the SQL Server scripts
    [String]$sqlScriptsFolder=".\", 

    # Temporary folder to store MDF/LDF/BAK/ZIP files during processing
    # MUST BE a physical folder accessible by SQL Server, don't use user folders!
    [String]$SqlDataFilesFolder='C:\Temp'
)


# Include the list of rows/database name for the database to generate
$databases = @()
$databases += [System.Tuple]::Create( 'Contoso V2 10K' )
$databases += [System.Tuple]::Create( 'Contoso V2 100K' )
$databases += [System.Tuple]::Create( 'Contoso V2 1M' )
$databases += [System.Tuple]::Create( 'Contoso V2 10M DimRatio' )
$databases += [System.Tuple]::Create( 'Contoso V2 10M' )
$databases += [System.Tuple]::Create( 'Contoso V2 100M' )
# $databases += [System.Tuple]::Create( 'Contoso V2 1G' )


$databases | ForEach-Object { 
    Write-Host *****************************************************************

    $DatabaseName = $_.Item1

    Write-Host 'Executing '$_
    $Filename = $sqlScriptsFolder + '\CreateDB.sql'
    $Original = Get-Content $Filename -raw
    $SqlCommand = $Original `
        -replace ('#DATABASE_NAME#',$DatabaseName) `
        -replace ('#SQLDATA_FOLDER#',$SqlDataFilesFolder) `
        -replace ('#SQLBACKUP_FOLDER#',$SqlDataFilesFolder) 
	
    Invoke-Sqlcmd -ServerInstance $SqlServerInstance -Query $SqlCommand -QueryTimeout 60000 -ConnectionTimeout 60000

}