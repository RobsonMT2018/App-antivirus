@echo off
cls
echo ======================================================
echo           INSTALADOR E EXECUTOR - ANTIVIRUS ROBSON
echo                EDICAO UBUNTU SECURITY (C#)
echo ======================================================
echo.

:: Verifica se o .NET SDK está instalado
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERRO] .NET SDK nao encontrado! Por favor, instale o .NET para rodar o projeto.
    pause
    exit
)

echo [*] Restaurando dependencias e compilando o projeto visual...
cd MeuAntivirusGUI
dotnet build
if %errorlevel% neq 0 (
    echo [ERRO] Falha na compilacao. Verifique o codigo C#.
    pause
    exit
)

echo.
echo [OK] Compilacao concluida com sucesso!
echo [*] Iniciando o Painel de Controle estilo Ubuntu...
echo.

:: Executa o projeto de interface gráfica
dotnet run --project MeuAntivirusGUI.csproj

pause