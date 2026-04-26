@echo off
cls
echo ======================================================
echo          GERADOR DE ASSINATURAS - ANTIVIRUS ROBSON
echo ======================================================
echo.

set /p arquivo="Arraste o arquivo aqui e aperte Enter: "

:: Remove as aspas caso o usuario tenha arrastado o arquivo
set arquivo=%arquivo:"=%

echo.
echo [*] Gerando Hash SHA-256 para o arquivo...

:: Comando corrigido para ignorar erros de espaco no caminho
for /f "skip=1 tokens=* delims=" %%a in ('certutil -hashfile "%arquivo%" SHA256') do (
    if not "%%a"=="CertUtil: -hashfile comando concluido com exito." (
        set hash=%%a
        goto :mostrar
    )
)

:mostrar
:: Remove os espaços que o certutil coloca entre os caracteres do hash
set hash=%hash: =%

echo [OK] Hash Gerado: %hash%
echo.
echo [!] Copie a linha abaixo e cole no seu arquivo signatures.json:
echo.
echo       { "name": "Spy-Keylogger", "type": "Spyware", "hash": "%hash%", "severity": "High" },
echo.
echo ======================================================
pause