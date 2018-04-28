
$fileNames = Get-ChildItem "cliente\src\app\logica-apis\api\*.ts" -Recurse | Select-Object -expand fullname

Write-Output "Corrigida url base do cliente da API no seguintes arquivos: "

foreach ($filename in $filenames) 
{
    $simpleFileName = $fileName.split('\')[-1]
    Write-Output " - $($simpleFileName)"
    (Get-Content $fileName) -replace 'https://localhost', '' | Set-Content $fileName
}