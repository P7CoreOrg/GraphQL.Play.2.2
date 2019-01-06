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

$symbolsDir = $rootDir + "symbols\"
$ILRepackJson = $projectDir + "ILRepack.json"
$jsonObj = (Get-Content $ILRepackJson) -join "`n" | ConvertFrom-Json

$pdb2PDBCLI = $rootDir + "tools\Pdb2Pdb\tools\Pdb2Pdb.exe"

write-output "----------PDB2PDB---------------------"
# this works, but I can't get it to work with launching it &
# $myarg = """$targetPath"" /pdb ""$pdbPath"" /out ""$portablePDBPath"""
# Start-Process $pdb2PDBCLI -ArgumentList $myarg

# ensure that all pdb's are copied over
foreach ($item in $jsonObj.assembliesToMerge) {
    $assembly = $item.assembly;
    $assemblyPath = $targetDir + $assembly + ".dll"
    $version = Get-ChildItem $assemblyPath | Select-Object -ExpandProperty VersionInfo
    $pdbPath = $targetDir + $assembly + ".pdb"
    if (!(Test-Path $pdbPath) ) {
        Write-Output "Does Not Exist: $pdbPath" 
        # might be delivered via symbols
        $version = Get-ChildItem $assemblyPath | Select-Object -ExpandProperty VersionInfo
        $symbolExtractPath = $symbolsDir + $assembly + "." + $version.ProductVersion + "\";  
        if ((Test-Path $symbolExtractPath) ) {
            Write-Output "Exits: $symbolExtractPath" 
            # we have symbols
            $libPath = $symbolExtractPath + "lib\*.pdb"
            $sourcefiles = Get-ChildItem -Path $libPath -Recurse
            foreach ($file in $sourcefiles) {
                Write-Output "Copying Symbols PDB: $file"
                Copy-Item -Path $file  -Destination $targetDir
                break
            }
        } 
    }
}

foreach ($item in $jsonObj.assembliesToMerge) {
    $assembly = $item.assembly;
    $assemblyPath = $targetDir + $assembly + ".dll"
    $version = Get-ChildItem $assemblyPath | Select-Object -ExpandProperty VersionInfo
  
    $pdbPath = $targetDir + $assembly + ".pdb"
  
    if ((Test-Path $pdbPath) ) {

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

 
 