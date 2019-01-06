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
Write-Output "=============== Ensure Symbols ======================"

write-output "-rootDir:$rootDir"
write-output "-projectDir:$projectDir"
write-output "-targetDir:$targetDir"

Write-Output "--- Load ILRepack.json ---"
$ILRepackJson = $projectDir + "ILRepack.json"
$jsonObj = (Get-Content $ILRepackJson) -join "`n" | ConvertFrom-Json
$jsonObj | ConvertTo-Json

$symbolsDir = $rootDir + "symbols\"
Write-Output "--- Ensure Symbols folder: $symbolsDir ---"
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
   
    Write-Output $item
    $version = Get-ChildItem $assemblyPath | Select-Object -ExpandProperty VersionInfo
    $symbolExtractPath = $symbolsDir + $assembly + "." + $version.ProductVersion + "\";
    if ((Test-Path $symbolExtractPath) ) {
        Write-Output "Already Exists: $symbolExtractPath"
        continue
    }
     
    New-Item -ItemType Directory -Force -Path $symbolExtractPath
    $url = $item.symbols.url -replace "{{VERSION}}", $version.ProductVersion
    Write-Output "Processing "  $url

    Write-OutPut "Fetching symbols: $url"
    $output = "$symbolExtractPath\symbols.nupkg"
    $start_time = Get-Date

    Invoke-WebRequest -Uri $url -OutFile $output
    Write-Output "Time taken: $((Get-Date).Subtract($start_time).Seconds) second(s)"

    Write-OutPut "Extracting Symbols Package"
    [io.compression.zipfile]::ExtractToDirectory($output, $symbolExtractPath)
     
}
 
 