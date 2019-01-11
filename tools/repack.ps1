# $(SolutionDir)..\tools\Pdb2Pdb\tools\PDB2PDB.exe $(TargetPath) /pdb $(TargetDir)$(TargetName).pdb 
# del $(TargetDir)$(TargetName).pdb 
# copy $(TargetDir)$(TargetName).pdb2 $(TargetDir)$(TargetName).pdb 

# powershell -NoProfile -ExecutionPolicy RemoteSigned -file $(SolutionDir)..\tools\repack.ps1 -rootDir $(SolutionDir)..\  -targetDir $(TargetDir) -projectDir $(ProjectDir)

param (
    [Parameter( Mandatory = $true)]
    [string]$rootDir = "rootDir",
    [Parameter( Mandatory = $true)]
    [string]$projectDir = "projectDir",
    [Parameter( Mandatory = $true)]
    [string]$targetDir = "targetDir"
)
write-output "-rootDir:$rootDir"
write-output "-projectDir:$projectDir"
write-output "-targetDir:$targetDir"

& ((split-path -parent $MyInvocation.MyCommand.Definition) + "\ensureSymbols.ps1") -rootDir $rootDir -projectDir $projectDir -targetDir $targetDir
if (-not $?) {
    write-output "Something went wrong with ensureSymbols"
    $ExitCode = "-1"
    Exit $ExitCode
}
& ((split-path -parent $MyInvocation.MyCommand.Definition) + "\ensurePortablePDB.ps1") -rootDir $rootDir -projectDir $projectDir -targetDir $targetDir
if (-not $?) {
    write-output "Something went wrong with ensureSymbols"
    $ExitCode = "-1"
    Exit $ExitCode
}



$ILRepackJson = $projectDir + "ILRepack.json"
$jsonObj = (Get-Content $ILRepackJson) -join "`n" | ConvertFrom-Json

$targetPath = $targetDir + $jsonObj.outputAssembly + ".dll"

$ILRepackCLI = $rootDir + "tools\ILRepack.MSBuild.Task.2.0.0\tools\ILRepack.exe"


$listParams = New-Object System.Collections.Generic.List[System.Object]
$listParams.Add("/lib:$targetDir")
$listParams.Add("/internalize")
#$listParams.Add("/ndebug")
$listParams.Add("/out:" + $targetPath)

foreach ($item in $jsonObj.assembliesToMerge) {
    $assembly = $item.assembly;
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
 