using Microsoft.VisualBasic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace JogoGourmet
{
    public partial class JogoGourmet : Form
    {

        private static List<PerguntaPrato> Perguntas { get; set; } = [];
        private static int IndexPerguntaAtual {  get; set; } = 0;

        private static DialogResult CaixaDialogo = new();
        private static readonly string MensagemAcerto = "Acertei de novo!";
        private static readonly string PerguntaPrato = "Qual prato você pensou?";
        private static readonly string PerguntaPadrao = "O prato que você pensou é ";
        private static bool FimJogo = false;

        public JogoGourmet()
        {
            IniciarLayout();
            IniciarPerguntas();
            InitializeComponent();
        }

        private void IniciarLayout()
        {
            TableLayoutPanel tableLayoutPanel = new();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel.Controls.Add(LabelInicial());
            tableLayoutPanel.Controls.Add(BotaoOk());

            this.Controls.Add(tableLayoutPanel);
        }

        public static Label LabelInicial()
        {
            Label label = new()
            {
                Text = "Pense em um prato que gosta",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            return label;
        }

        public static Button BotaoOk()
        {
            Button botaoOk = new()
            {
                Parent = LabelInicial(),
                Text = "OK",
                ForeColor = Color.Black,
                BackColor = Color.CadetBlue,
                Margin = new Padding(-10),
                Anchor = AnchorStyles.None,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            botaoOk.Click += OkClick;

            return botaoOk;
        }

        public static void OkClick(object sender, EventArgs e)
        {
            FimJogo = false;

            while (!FimJogo && IndexPerguntaAtual < Perguntas.Count)
            {
                CaixaDialogo = MessageBox.Show(Perguntas[IndexPerguntaAtual].Pergunta, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                DeterminaProximaPergunta(CaixaDialogo);
                }
        }

        private static void DeterminaProximaPergunta(DialogResult resposta)
        {
            if (resposta == DialogResult.Yes)
            {                 
                if (Perguntas[IndexPerguntaAtual].ProxPratoSim is null && Perguntas[IndexPerguntaAtual].PerguntaEhFinal)
                {
                    FinalizaJogo(true);
                    return;
                }
                
                IndexPerguntaAtual = Perguntas.FindIndex(prato => prato.Prato == Perguntas[IndexPerguntaAtual].ProxPratoSim);          
            }
            else
            {
                if (Perguntas[IndexPerguntaAtual].PerguntaEhFinal)
                {
                    PerguntaQualOPrato();
                    return;
                }

                IndexPerguntaAtual = Perguntas.FindIndex(prato => prato.Prato == Perguntas[IndexPerguntaAtual].ProxPratoNao);
            }
        }

        private static void PerguntaQualOPrato()
        {
            string prato = Interaction.InputBox(PerguntaPrato, "Desisto", default, -1, -1);
            string caracteristica = Interaction.InputBox($"{prato} é _______ mas {Perguntas[IndexPerguntaAtual].Prato} não.", "Complete", default, -1, -1);
            var pratoProximaPerguntaNao = Perguntas.Where(prato => prato.ProxPratoNao == Perguntas[IndexPerguntaAtual].Prato).FirstOrDefault();
            
            if(pratoProximaPerguntaNao is not null)
                pratoProximaPerguntaNao.ProxPratoNao = caracteristica;
            else
                Perguntas[IndexPerguntaAtual - 1].ProxPratoSim = caracteristica;
            
            var respostaNao = IndexPerguntaAtual < Perguntas.Count ? Perguntas[IndexPerguntaAtual].Prato : null;
            
            Perguntas.Insert(IndexPerguntaAtual, new PerguntaPrato(string.Concat(PerguntaPadrao, caracteristica, "?"), caracteristica, false, prato, respostaNao));      
            
            Perguntas.Insert(IndexPerguntaAtual + 1, new PerguntaPrato(string.Concat(PerguntaPadrao, prato, "?"), prato, true, null, null));

            FinalizaJogo(false);
        }

        private static void IniciarPerguntas()
        {
            Perguntas.Add(new PerguntaPrato("O prato que você pensou é massa?", "massa", false, "Lasanha", "Bolo de Chocolate"));
            Perguntas.Add(new PerguntaPrato("O prato que você pensou é Lasanha?", "Lasanha", true, null, null));
            Perguntas.Add(new PerguntaPrato("O prato que você pensou é Bolo de Chocolate?", "Bolo de Chocolate", true, null, null));
        }

        public static void ResetarPerguntas() => IndexPerguntaAtual = 0;

        private static void MostrarMensagemBoxAcerto()
        {
            CaixaDialogo = MessageBox.Show(MensagemAcerto, "Jogo Gourmet", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private static void FinalizaJogo(bool venceu)
        {
            if (venceu)
                MostrarMensagemBoxAcerto();
            
            ResetarPerguntas();
            FimJogo = true;
        }
    }
}