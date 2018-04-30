
$fileNames = Get-ChildItem "cliente2\src\app\logica-api\api\*.ts" -Recurse | Select-Object -expand fullname

Write-Output "Corrigida url base do cliente da API no seguintes arquivos: "

foreach ($filename in $filenames) 
{
    $simpleFileName = $fileName.split('\')[-1]
    Write-Output " - $($simpleFileName)"
    (Get-Content $fileName) -replace 'https://localhost', '' | Set-Content $fileName
}