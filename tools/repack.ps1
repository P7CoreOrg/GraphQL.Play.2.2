#powershell -NoProfile -ExecutionPolicy RemoteSigned -file $(SolutionDir)..\tools\repack.ps1 -rootDir $(SolutionDir)..\ -configJson $(ProjectDir)repackConfig.json  -targetDir $(TargetDir)


param (
    [string]$configJson = "config.json",
    [string]$targetDir = "targetDir",
    [string]$rootDir = "rootDir",
    [string]$projectDir = "projectDir"
 )

write-output $configJson
write-output $targetDir
write-output $rootDir
write-output $projectDir

$ILRepackJson = $projectDir+"ILRepack.json"

$jsonObj = (Get-Content $ILRepackJson) -join "`n" | ConvertFrom-Json
$outAssembly = $jsonObj.outputAssembly
$outTarget = "$targetDir$outAssembly"
$repackTarget = $targetDir+$jsonObj.outputAssembly


$execFile = $rootDir+"tools\ILRepack 2.0.16\tools\ILRepack.exe"
$listParams = New-Object System.Collections.Generic.List[System.Object]
$listParams.Add("/lib:$targetDir")
$listParams.Add("/internalize")
#$listParams.Add("/ndebug")
$listParams.Add("/out:"+$repackTarget)

foreach($assembly in $jsonObj.assembliesToMerge){
    $listParams.Add($targetDir+$assembly)
}
$params = $listParams.ToArray()

# Wait until the started process has finished
& $execFile $params
if (-not $?)
{
    # Show error message
}
#Remove-Item $outTarget
#Copy-Item $repackTarget -Destination $targetDir


# "$(SolutionDir)..\tools\ILRepack 2.0.16\tools\ILRepack.exe" /lib:$(TargetDir)   /internalize /ndebug /out:$(TargetDir)\repack\$(TargetFileName) $(TargetPath) $(TargetDir)P7Core.Burner.dll
# del $(TargetPath)
# copy $(TargetDir)repack\$(TargetFileName) $(TargetPath)
