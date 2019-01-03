# $(SolutionDir)..\tools\Pdb2Pdb\tools\PDB2PDB.exe $(TargetPath) /pdb $(TargetDir)$(TargetName).pdb 
# del $(TargetDir)$(TargetName).pdb 
# copy $(TargetDir)$(TargetName).pdb2 $(TargetDir)$(TargetName).pdb 

# powershell -NoProfile -ExecutionPolicy RemoteSigned -file $(SolutionDir)..\tools\repack.ps1 -rootDir $(SolutionDir)..\  -targetDir $(TargetDir) -projectDir $(ProjectDir)

param (
    [string]$rootDir = "rootDir",
    [string]$projectDir = "projectDir",
    [string]$targetDir = "targetDir"
)

write-output $rootDir
write-output $projectDir
write-output $targetDir


$ILRepackJson = $projectDir + "ILRepack.json"
$jsonObj = (Get-Content $ILRepackJson) -join "`n" | ConvertFrom-Json

$targetPath = $targetDir + $jsonObj.outputAssembly + ".dll"
$pdbPath = $targetDir + $jsonObj.outputAssembly + ".pdb"
$portablePDBPath = $pdbPath + ".portable"

$ILRepackCLI = $rootDir + "tools\ILRepack 2.0.16\tools\ILRepack.exe"
$pdb2PDBCLI = $rootDir + "tools\Pdb2Pdb\tools\Pdb2Pdb.exe"

write-output "----------PDB2PDB---------------------"
# this works, but I can't get it to work with launching it &
# $myarg = """$targetPath"" /pdb ""$pdbPath"" /out ""$portablePDBPath"""
# Start-Process $pdb2PDBCLI -ArgumentList $myarg

foreach ($assembly in $jsonObj.assembliesToMerge) {
    $assemblyPath = $targetDir + $assembly + ".dll"
    $pdbPath = $targetDir + $assembly + ".pdb"

    if (!(Test-Path $assemblyPath) ) {
        Write-Error "$assemblyPath absent"
        $ExitCode = "-1"
        Exit $ExitCode
    }
    if (!(Test-Path $pdbPath) ) {
        Write-Warning "$pdbPath absent not a problem if you are merging a release package"
    }
    else {
        & $pdb2PDBCLI $assemblyPath
        if (-not $?) {
            write-output "Something went wrong with pdb2pdb"
            $ExitCode = "-1"
            Exit $ExitCode
        }
        $portablePDBPath = $pdbPath + "2" #appends a 2
        Remove-Item $pdbPath
        Rename-Item -Path $portablePDBPath -NewName $pdbPath

    }
   
}


$listParams = New-Object System.Collections.Generic.List[System.Object]
$listParams.Add("/lib:$targetDir")
$listParams.Add("/internalize")
#$listParams.Add("/ndebug")
$listParams.Add("/out:" + $targetPath)

foreach ($assembly in $jsonObj.assembliesToMerge) {
    $assemblyPath = $targetDir + $assembly + ".dll"
    $listParams.Add($assemblyPath)
}
$params = $listParams.ToArray()

# Wait until the started process has finished
& $ILRepackCLI $params
if (-not $?) {
    # Show error message
    $ExitCode = "-1"
    Exit $ExitCode
}
 