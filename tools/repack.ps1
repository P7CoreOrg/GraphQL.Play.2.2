#powershell -NoProfile -ExecutionPolicy RemoteSigned -file $(SolutionDir)..\tools\repack.ps1 -projectDir $(SolutionDir)..\ -configJson $(ProjectDir)repackConfig.json  -outDir $(TargetDir)


param (
    [string]$configJson = "config.json",
    [string]$outDir = "outDir",
    [string]$projectDir = "projectDir"
 )

 write-output $configJson
 write-output $outDir
 write-output $projectDir

$jsonObj = (Get-Content $configJson) -join "`n" | ConvertFrom-Json
$outAssembly = $jsonObj.outputAssembly
$outTarget = "$outDir$outAssembly"
$repackTarget = $outDir+"repack\"+$jsonObj.outputAssembly

$arr = $jsonObj.assembliesToMerge | ForEach-Object { "$outDir$assembly" }


$execFile = $projectDir+"tools\ILRepack 2.0.16\tools\ILRepack.exe"
$listParams = New-Object System.Collections.Generic.List[System.Object]
$listParams.Add("/lib:$outDir")
$listParams.Add("/internalize")
$listParams.Add("/ndebug")
$listParams.Add("/out:"+$repackTarget)

foreach($assembly in $jsonObj.assembliesToMerge){
    $listParams.Add($outDir+$assembly)
}
$params = $listParams.ToArray()

# Wait until the started process has finished
& $execFile $params
if (-not $?)
{
    # Show error message
}
Remove-Item $outTarget
Copy-Item $repackTarget -Destination $outDir


# "$(SolutionDir)..\tools\ILRepack 2.0.16\tools\ILRepack.exe" /lib:$(TargetDir)   /internalize /ndebug /out:$(TargetDir)\repack\$(TargetFileName) $(TargetPath) $(TargetDir)P7Core.Burner.dll
# del $(TargetPath)
# copy $(TargetDir)repack\$(TargetFileName) $(TargetPath)
