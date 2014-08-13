param
(
    [string]$zipExe = $(throw "Please provide path to 7z.exe"),
    [string]$releaseDir = $(throw "Please provide path to release folder"),
    [string]$projectDir = $(throw "Please provide path to project root folder")
    [string]$version = $(throw "please provide version")
)

trap
{
	throw $_
    EXIT 1
}

$ErrorActionPreference = "Stop"

$datetime = [DateTime]::Now.ToUniversalTime().ToString("yyyy-MM-ddTHH-mm-ss")
$zipfilename = $datetime + ".zip"
$zipfilepath = Join-Path $releaseDir $zipfilename
$sourceLocation = Join-Path $projectDir "bin\debug\*"

new-item $releaseDir -itemType Directory -force
gci $releaseDir | %{remove-item $_.fullname -recurse -force}

& $zipExe a $zipfilepath $sourceLocation

EXIT 0
