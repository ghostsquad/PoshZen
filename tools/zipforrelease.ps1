param
(
	[string]$zipExe = $(throw "Please provide path to 7z.exe"),
	[string]$releaseDir = $(throw "Please provide path to release folder"),
	[string]$projectDir = $(throw "Please provide path to project root folder")
)

trap
{
	EXIT 1
}

$ErrorActionPreference = "Stop"

$datetime = [DateTime]::Now.ToUniversalTime().ToString("yyyy-MM-ddTHH-mm-ss") 
$zipfilename = $datetime + ".zip"
$zipfilepath = Join-Path $releaseDir $zipfilename

remove-item $(Join-Path $releaseDir "*")

& $zipExe a $zipfilepath $sourceLocation *.*

EXIT 0