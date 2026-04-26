using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MeuAntivirusGUI
{
    public partial class Form1 : Form
    {
        // Variáveis de Controle
        private bool estaPausado = false;
        private string quarantineFolder = "Quarentena";
        private List<string> ameacasDetectadas = new List<string>();

        // Cores da Paleta "Modern Light"
        Color corFundo = Color.FromArgb(245, 247, 250);     // Branco Gelo
        Color corHeader = Color.FromArgb(45, 62, 80);     // Azul Marinho Profissional
        Color corAccent = Color.FromArgb(39, 174, 96);     // Verde Esmeralda (Sucesso/Scan)
        Color corTextoEscuro = Color.FromArgb(44, 62, 80); // Grafite para textos

        // Controles da Interface
        ProgressBar progressBar;
        Label lblArquivoAtual;
        Label lblPorcentagem;
        Button btnPausar;
        TextBox txtDiretorioFixado;

        Image imgLogo = Image.FromFile("icon.svg"); // Certifique-se de que a imagem está na pasta bin/Debug

        public Form1()
        {
            InitializeComponent();
            ConfigurarInterfaceUbuntuFinal();
        }

        private void ConfigurarInterfaceUbuntuFinal()
        {
            // --- JANELA PRINCIPAL ---
            this.Text = "NOSBOR Security - Antivírus";
            this.Size = new Size(680, 720);
            this.BackColor = corFundo;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- CABEÇALHO ---
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = corHeader};
            this.Controls.Add(pnlHeader);

            // Substitua os Labels lblLogoSimulado e lblLetraR por isso:
            PictureBox picLogo = new PictureBox {
                Image = imgLogo, // Certifique-se de que a imagem está na pasta bin/Debug
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(590, 0),
                Size = new Size(80, 80),
                BackColor = Color.Transparent
            };
            pnlHeader.Controls.Add(picLogo);

            // LOGO (Usando o escudo que criamos)
            Label lblTitulo = new Label { 
                Text = "NR - SECURITY SYSTEM", 
                ForeColor = Color.White, 
                Font = new Font("Segoe UI", 22, FontStyle.Bold), 
                TextAlign = ContentAlignment.MiddleCenter, 
                Location = new Point(0, 15), 
                Size = new Size(520, 50) 
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitulo });

            // --- BARRA DE BOTÕES (CINZA CLARO) ---
            Panel pnlBotoes = new Panel { Location = new Point(0, 80), Size = new Size(680, 60), BackColor = Color.FromArgb(230, 233, 237) };
            Font fntBtn = new Font("Segoe UI", 9, FontStyle.Bold);

            Button btnScanGeral = new Button { Text = "▶ SCAN", ForeColor = Color.White ,BackColor = Color.FromArgb(39, 174, 96),FlatStyle = FlatStyle.Flat, Location = new Point(45, 10), Size = new Size(130, 40), Font = fntBtn };
            btnScanGeral.Click += async (s, e) => await SelecionarEPesquisar();

            Button btnRelatorio = new Button { Text = "📋 REPORT", ForeColor = Color.White, BackColor = Color.FromArgb(75, 75, 75), FlatStyle = FlatStyle.Flat, Location = new Point(185, 10), Size = new Size(130, 40), Font = fntBtn };
            btnRelatorio.Click += (s, e) => GerarRelatorioTexto();

            Button btnLimpar = new Button { Text = "🧹 CLEAR LOG", ForeColor = Color.White, BackColor = Color.FromArgb(44, 62, 80), FlatStyle = FlatStyle.Flat, Location = new Point(325, 10), Size = new Size(130, 40), Font = fntBtn };
            btnLimpar.Click += (s, e) => { ((ListBox)this.Controls["lstLogs"]).Items.Clear(); };

            btnPausar = new Button { Text = "⏸ PAUSE", ForeColor = Color.White, BackColor = Color.FromArgb(255, 82, 82), FlatStyle = FlatStyle.Flat, Location = new Point(465, 10), Size = new Size(130, 40), Font = fntBtn };
            btnPausar.Click += (s, e) => { estaPausado = !estaPausado; btnPausar.Text = estaPausado ? "▶ RESUME" : "⏸ PAUSE"; };

            pnlBotoes.Controls.AddRange(new Control[] { btnScanGeral, btnRelatorio, btnLimpar, btnPausar });
            this.Controls.Add(pnlBotoes);

            // --- ENTRADA DE DIRETÓRIO ---
            Label lblTarget = new Label { Text = "DIRETÓRIO ALVO:", ForeColor = corTextoEscuro, Location = new Point(20, 155), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            txtDiretorioFixado = new TextBox { Text = @"C:\", Location = new Point(20, 175), Size = new Size(510, 25), BackColor = Color.White, ForeColor = corTextoEscuro, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10) };
            Button btnScanFixo = new Button { Text = "🔍 SCAN", ForeColor = Color.White, BackColor = corHeader, FlatStyle = FlatStyle.Flat, Location = new Point(540, 172), Size = new Size(110, 30), Font = fntBtn };
            btnScanFixo.Click += async (s, e) => await ExecutarVarredura(txtDiretorioFixado.Text);
            this.Controls.AddRange(new Control[] { lblTarget, txtDiretorioFixado, btnScanFixo });

            // --- PROGRESSO ---
            progressBar = new ProgressBar { Location = new Point(20, 240), Size = new Size(530, 20), Style = ProgressBarStyle.Continuous };
            lblPorcentagem = new Label { Text = "0%", ForeColor = corAccent, Location = new Point(560, 238), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            lblArquivoAtual = new Label { Text = "Pronto para iniciar.", ForeColor = Color.Gray, Location = new Point(20, 260), Size = new Size(620, 20), Font = new Font("Segoe UI", 9, FontStyle.Italic) };
            this.Controls.AddRange(new Control[] { progressBar, lblPorcentagem, lblArquivoAtual });

            // --- LOGS (FUNDO BRANCO) ---
            ListBox lstLogs = new ListBox { Name = "lstLogs", Location = new Point(30, 290), Size = new Size(600, 370), BackColor = Color.Black, ForeColor = corTextoEscuro, DrawMode = DrawMode.OwnerDrawFixed, HorizontalScrollbar = true, HorizontalExtent = 2000, Font = new Font("Consolas", 9), BorderStyle = BorderStyle.FixedSingle };
            lstLogs.DrawItem += LstLogs_DrawItem;
            this.Controls.Add(lstLogs);
        }

        private async Task SelecionarEPesquisar()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK) await ExecutarVarredura(fbd.SelectedPath);
            }
        }

        private async Task ExecutarVarredura(string diretorio)
        {
            ameacasDetectadas.Clear();
            progressBar.Value = 0;
            RegistrarLog($"[*] Scan started in: {diretorio}", Color.Cyan);

            // Lista para armazenar arquivos encontrados com segurança
            List<string> listaArquivos = new List<string>();
            

            try {
            // Busca arquivos ignorando pastas que dão erro de acesso
            foreach (string f in Directory.EnumerateFiles(diretorio, "*.*", SearchOption.AllDirectories)) {
                listaArquivos.Add(f);
            }
            }
            catch (UnauthorizedAccessException) {
                RegistrarLog("[!] Aviso: Algumas pastas do sistema foram puladas (Acesso Negado).", Color.Yellow);
            }
            catch (Exception ex) {
                RegistrarLog($"[!] Erro crítico: {ex.Message}", Color.Red);
            }

            

            SignatureDatabase db = null;
            try { if (File.Exists("signatures.json")) db = JsonSerializer.Deserialize<SignatureDatabase>(File.ReadAllText("signatures.json")); } 
            catch { RegistrarLog("[!] Error loading signatures.json"); }

            string[] arquivos = Directory.GetFiles(diretorio, "*.*", SearchOption.AllDirectories);

            for (int i = 0; i < arquivos.Length; i++)
            {
                while (estaPausado) { await Task.Delay(500); }

                string caminho = arquivos[i];
                string arq = Path.GetFileName(caminho);
                lblArquivoAtual.Text = $"Analyzing: {arq}";
                progressBar.Value = (int)(((double)(i + 1) / arquivos.Length) * 100);
                lblPorcentagem.Text = $"{progressBar.Value}%";

                try {
                    string hash = GerarHashSHA256(caminho);
                    var match = db?.malware_signatures?.Find(m => m.hash.Equals(hash, StringComparison.OrdinalIgnoreCase));

                    if (match != null || arq.ToLower().Contains("teste")) {
                        ameacasDetectadas.Add(caminho);
                        SystemSounds.Exclamation.Play();
                        string tipo = match != null ? match.type : "Heuristic.Test";
                        string severity = match != null ? match.severity : "Medium";
                        
                        Color corLog;
                        switch (severity.ToLower()) {
                            case "critical": case "high": corLog = Color.Red; break;
                            case "medium": corLog = Color.Orange; break;
                            default: corLog = Color.Yellow; break;
                        }
                        RegistrarLog($"[XXX] {arq} --- THREAT DETECTED: [{severity.ToUpper()}] | TYPE: {tipo} | FILE: {caminho}", corLog);
                    }
                } catch { }
                await Task.Delay(5);
            }
            RegistrarLog($"[OK] Scan finished. {ameacasDetectadas.Count} threats found.", Color.Lime);
            if (ameacasDetectadas.Count > 0) CriarJanelaDeDecisao();
        }

        private void CriarJanelaDeDecisao()
        {
            Form frm = new Form { Text = "Action Required", Size = new Size(600, 460), BackColor = Color.FromArgb(230, 233, 237), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
            CheckedListBox clb = new CheckedListBox { Location = new Point(20, 40), Size = new Size(540, 280), BackColor = Color.Black, ForeColor = Color.White, HorizontalScrollbar = true, HorizontalExtent = 1500, CheckOnClick = true };
            foreach (string a in ameacasDetectadas) clb.Items.Add(a, true);

            Button btnQ = new Button { Text = "🛡️ QUARENTENA", Location = new Point(20, 340), Size = new Size(260, 45), BackColor = Color.FromArgb(233, 84, 32), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnQ.Click += (s, e) => { foreach (string item in clb.CheckedItems) MoverParaQuarentena(item); frm.Close(); };

            Button btnE = new Button { Text = "🗑️ EXCLUIR TUDO", Location = new Point(300, 340), Size = new Size(260, 45), BackColor = Color.FromArgb(150, 0, 0), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnE.Click += (s, e) => {
                if (MessageBox.Show("Delete permanently?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    foreach (string item in clb.CheckedItems) try { File.Delete(item); } catch { }
                    frm.Close();
                }
            };

            frm.Controls.AddRange(new Control[] { clb, btnQ, btnE });
            frm.ShowDialog();
        }

        private void MoverParaQuarentena(string path) {
            if (!Directory.Exists(quarantineFolder)) Directory.CreateDirectory(quarantineFolder);
            try { File.Move(path, Path.Combine(quarantineFolder, Path.GetFileName(path) + ".quarentena"), true); } catch { }
        }

        private string GerarHashSHA256(string arquivo) {
            using (SHA256 sha = SHA256.Create()) {
                using (FileStream fs = File.OpenRead(arquivo)) {
                    byte[] hb = sha.ComputeHash(fs);
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hb) sb.Append(b.ToString("x2"));
                    return sb.ToString();
                }
            }
        }

        private void RegistrarLog(string msg, Color? cor = null) {
            var lst = (ListBox)this.Controls["lstLogs"];
            Color c = cor ?? Color.White;
            this.Invoke(new Action(() => {
                lst.Items.Add(new LogEntry { Texto = $"[{DateTime.Now:HH:mm:ss}] {msg}", Cor = c });
                lst.TopIndex = lst.Items.Count - 1;
            }));
        }

        private void LstLogs_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index < 0) return;
            var item = (LogEntry)((ListBox)sender).Items[e.Index];
            e.DrawBackground();
            using (SolidBrush b = new SolidBrush(item.Cor)) e.Graphics.DrawString(item.Texto, e.Font, b, e.Bounds);
        }

        private void GerarRelatorioTexto() {
            try { File.WriteAllLines($"Scan_Report_{DateTime.Now:yyyyMMdd}.txt", ameacasDetectadas); MessageBox.Show("Report saved!"); } catch { }
        }
    }

    public class Malware { public string name { get; set; } public string type { get; set; } public string hash { get; set; } public string severity { get; set; } }
    public class SignatureDatabase { public List<Malware> malware_signatures { get; set; } }
    public class LogEntry { public string Texto { get; set; } public Color Cor { get; set; } }
}