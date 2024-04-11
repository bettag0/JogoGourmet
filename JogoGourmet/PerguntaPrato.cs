namespace JogoGourmet
{
    public class PerguntaPrato(string pergunta, string prato, bool ehFinal, string? proxPratoSim, string? proxPratoNao)
    {
        public string Pergunta { get; set; } = pergunta;
        public bool PerguntaEhFinal { get; set; } = ehFinal;
        public string Prato { get; set; } = prato;
        public string? ProxPratoSim { get; set; } = proxPratoSim;
        public string? ProxPratoNao { get; set; } = proxPratoNao;
    }
}