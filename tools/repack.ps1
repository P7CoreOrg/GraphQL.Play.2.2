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
$tempDir = $targetDir + "temp\"
If (!(test-path $tempDir)) {
    New-Item -ItemType Directory -Force -Path $tempDir
}
$symbolsDir = $rootDir + "symbols\"
If (!(test-path $symbolsDir)) {
    New-Item -ItemType Directory -Force -Path $symbolsDir
}
Add-Type -assembly "system.io.compression.filesystem"
# Download symbols if needed
foreach ($item in $jsonObj.assembliesToMerge) {
    $assembly = $item.assembly;
    $assemblyPath = $targetDir + $assembly + ".dll"
    if (!(Test-Path $assemblyPath) ) {
        Write-Error "$assemblyPath absent"
        $ExitCode = "-1"
        Exit $ExitCode
    }
    if (($item.symbols -eq $null) -or ($item.symbols.url -eq $null)) {
        continue
    }

    $version = Get-ChildItem $assemblyPath | Select-Object -ExpandProperty VersionInfo
    $symbolExtractPath = $symbolsDir + $assembly + "." + $version.ProductVersion + "\";
    if (!(Test-Path $symbolExtractPath) ) {
        New-Item -ItemType Directory -Force -Path $symbolExtractPath
        $url = $item.symbols.url
        $output = "$symbolExtractPath\symbols.nupkg"
        $start_time = Get-Date

        Invoke-WebRequest -Uri $url -OutFile $output
        Write-Output "Time taken: $((Get-Date).Subtract($start_time).Seconds) second(s)"

        [io.compression.zipfile]::ExtractToDirectory($output, $symbolExtractPath)

    }
}

# ensure that all pdb's are copied over
foreach ($item in $jsonObj.assembliesToMerge) {
    $assembly = $item.assembly;
    $assemblyPath = $targetDir + $assembly + ".dll"
    $version = Get-ChildItem $assemblyPath | Select-Object -ExpandProperty VersionInfo
    $pdbPath = $targetDir + $assembly + ".pdb"
    if (!(Test-Path $pdbPath) ) {
        # might be delivered via symbols
        $version = Get-ChildItem $assemblyPath | Select-Object -ExpandProperty VersionInfo
        $symbolExtractPath = $symbolsDir + $assembly + "." + $version.ProductVersion + "\";  
        if ((Test-Path $symbolExtractPath) ) {
            # we have symbols
            $libPath = $symbolExtractPath + "lib\*.pdb"
            $sourcefiles = Get-ChildItem -Path $libPath -Recurse
            foreach ($file in $sourcefiles) {
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
 