$content = Get-Content -Path '.\database\script_20_09_2026.sql'
$tables = @('AdmissionApplications_AA', 'Students_S')

foreach ($t in $tables) {
    Write-Host "================ $t ================"
    $recording = $false
    foreach ($line in $content) {
        if ($line -match "CREATE TABLE \[dbo\]\.\[$t\]") {
            $recording = $true
        }
        if ($recording) {
            Write-Host $line
            if ($line -match "CONSTRAINT \[PK_") {
                $recording = $false
                break
            }
        }
    }
}
