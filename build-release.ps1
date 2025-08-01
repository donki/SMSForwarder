# Script para compilar, firmar y exportar APK/AAB con clave pública
# Requiere: dotnet, jarsigner, pepk.jar en la raíz del proyecto

$keystore = "smsforwarder.keystore"
$keyalias = "smsforwarder"
$keypass = "07052012"
$publickey = "encryption_public_key.pem"
$pepkjar = "pepk.jar"
$version = "1.8.3"

# 1. Compilar APK y AAB en modo Release
Write-Host "Compilando APK en modo Release..."
dotnet build -c Release -f net9.0-android /p:AndroidPackageFormat=apk

Write-Host "Compilando AAB en modo Release..."
dotnet build -c Release -f net9.0-android /p:AndroidPackageFormat=aab

# 2. Firmar APK
$apk = Get-ChildItem -Path ".\bin\Release\net9.0-android" -Filter "*.apk" | Select-Object -First 1
if ($apk) {
    Write-Host "Firmando APK: $($apk.Name)"
    jarsigner -verbose -sigalg SHA256withRSA -digestalg SHA-256 -keystore $keystore -storepass $keypass $apk.FullName $keyalias
    Copy-Item $apk.FullName ".\Release\SMSForwarder-v$version-release.apk" -Force
    Write-Host "APK firmado copiado a .\Release\SMSForwarder-v$version-release.apk"
} else {
    Write-Host "No se encontró APK para firmar."
}

# 3. Firmar AAB y exportar clave pública con pepk.jar para Google Play
$aab = Get-ChildItem -Path ".\bin\Release\net9.0-android" -Filter "*.aab" | Select-Object -First 1
if ($aab) {
    Write-Host "Firmando AAB: $($aab.Name)"
    jarsigner -verbose -sigalg SHA256withRSA -digestalg SHA-256 -keystore $keystore -storepass $keypass $aab.FullName $keyalias
    
    Write-Host "Exportando clave pública con pepk.jar para AAB: $($aab.Name)"
    java -jar $pepkjar --keystore=$keystore --alias=$keyalias --storepass=$keypass --output=$publickey --encryptionkey=eb10fe8f7c7c9df715022017b00c6471f8ba8170b13049a11e6c09ffe3056a104a3bbe4ac5a955f4ba4fe93fc8cef27558a3eb9d2a529a2092761fb833b656cd48b9de6a
    
    Copy-Item $aab.FullName ".\Release\SMSForwarder-v$version-release.aab" -Force
    Write-Host "AAB firmado copiado a .\Release\SMSForwarder-v$version-release.aab"
} else {
    Write-Host "No se encontró AAB para firmar/exportar."
}

Write-Host "Proceso completado. Archivos firmados y copiados en la carpeta Release."
