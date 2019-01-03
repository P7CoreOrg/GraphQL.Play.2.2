#powershell -NoProfile -ExecutionPolicy RemoteSigned -file $(SolutionDir)..\tools\repack.ps1 -rootDir $(SolutionDir)..\ -configJson $(ProjectDir)repackConfig.json  -targetDir $(TargetDir)


param (
    [string]$targetDir = "targetDir",
    [string]$rootDir = "rootDir",
    [string]$projectDir = "projectDir"
)

 


write-output $rootDir
write-output $projectDir

$ILRepackJson = $projectDir + "ILRepack.json"

$jsonObj = (Get-Content $ILRepackJson) -join "`n" | ConvertFrom-Json

$assemblyPDB = $targetDir + $jsonObj.outputAssembly + ".pdb"
$portablePDB = $targetDir + "portable\" + $jsonObj.outputAssembly + ".pdb"
$repackTarget = $targetDir + $jsonObj.outputAssembly + ".dll"

$listParams = New-Object System.Collections.Generic.List[System.Object]
$ILRepackCLI = $rootDir + "tools\ILRepack 2.0.16\tools\ILRepack.exe"
$pdb2PDB = $rootDir + "tools\Pdb2Pdb\tools\Pdb2Pdb.exe"

$listParams.Add("$repackTarget ")
$listParams.Add("/pdb " + $assemblyPDB)
#$listParams.Add("/out " + $portablePDB)
$params = $listParams.ToArray()

write-output "----------PDB2PDB---------------------"
write-output $pdb2PDB $params

 

$listParams.Clear();
$listParams.Add("/lib:$targetDir")
$listParams.Add("/internalize")
#$listParams.Add("/ndebug")
$listParams.Add("/out:" + $repackTarget)

foreach ($assembly in $jsonObj.assembliesToMerge) {
    $listParams.Add($targetDir + $assembly + ".dll")
}
$params = $listParams.ToArray()

# Wait until the started process has finished
& $ILRepackCLI $params
if (-not $?) {
    # Show error message
}
#Remove-Item $outTarget
#Copy-Item $repackTarget -Destination $targetDir


# "$(SolutionDir)..\tools\ILRepack 2.0.16\tools\ILRepack.exe" /lib:$(TargetDir)   /internalize /ndebug /out:$(TargetDir)\repack\$(TargetFileName) $(TargetPath) $(TargetDir)P7Core.Burner.dll
# del $(TargetPath)
# copy $(TargetDir)repack\$(TargetFileName) $(TargetPath)
