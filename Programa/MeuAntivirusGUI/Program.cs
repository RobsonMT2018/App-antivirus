using System;
using System.Windows.Forms;

namespace MeuAntivirusGUI // Verifique se este nome é IGUAL ao que está no seu Form1.cs
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Aqui ele diz ao Windows para abrir o seu formulário estilizado
            Application.Run(new Form1()); 
        }
    }
}