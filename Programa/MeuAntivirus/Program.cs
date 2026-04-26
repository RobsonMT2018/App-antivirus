using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Collections.Generic;

namespace AntivirusZero
{
    class Program
    {
        static string dbPath = "signatures.json";
        static string quarantineFolder = "Quarentena";

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("=== ANTIVÍRUS REAL-TIME ATIVADO ===");
            
            if (!Directory.Exists(quarantineFolder)) Directory.CreateDirectory(quarantineFolder);

            // DEFINA AQUI A PASTA QUE QUER VIGIAR (Ex: sua pasta de Downloads ou a pasta do projeto '.')
            string folderToWatch = "."; 

            using var watcher = new FileSystemWatcher(folderToWatch);

            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            
            // Evento disparado quando um novo arquivo é criado
            watcher.Created += OnChanged;
            
            watcher.Filter = "*.*";
            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"[*] Monitorando a pasta: {Path.GetFullPath(folderToWatch)}");
            Console.WriteLine("[*] Pressione 'q' para sair.");
            while (Console.Read() != 'q');
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            // Pequena pausa para garantir que o arquivo terminou de ser escrito no disco
            System.Threading.Thread.Sleep(500);

            Console.WriteLine($"\n[NOVO ARQUIVO DETECTADO]: {e.Name}");
            VerificarArquivo(e.FullPath);
        }

        static void VerificarArquivo(string filePath)
        {
            try {
                string jsonContent = File.ReadAllText(dbPath);
                var db = JsonSerializer.Deserialize<dynamic>(jsonContent); // Simplificado para o exemplo
                
                string fileHash = CalculateSHA256(filePath);
                
                // Lógica de busca no JSON (simplificada)
                if (jsonContent.Contains(fileHash))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[!!!] AMEAÇA IDENTIFICADA EM TEMPO REAL: {Path.GetFileName(filePath)}");
                    Console.ResetColor();
                    
                    MoverParaQuarentena(filePath);
                }
                else {
                    Console.WriteLine($"[OK] Arquivo {Path.GetFileName(filePath)} escaneado e seguro.");
                }
            }
            catch { /* Ignora se o arquivo estiver bloqueado temporariamente */ }
        }

        static void MoverParaQuarentena(string fileSource)
        {
            string fileName = Path.GetFileName(fileSource);
            string destFile = Path.Combine(quarantineFolder, fileName + ".quarentena");
            File.Move(fileSource, destFile, true);
            Console.WriteLine($"[>>>] Movido para quarentena.");
        }

        static string CalculateSHA256(string filePath)
        {
            using var sha256 = SHA256.Create();
            using var stream = File.OpenRead(filePath);
            var hash = sha256.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}