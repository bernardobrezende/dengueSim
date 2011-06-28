namespace dengueSim.Domain
{
    public class Mosquito : Agente
    {
        private int ciclosVivo;

        public dengueSim.Domain.TipoDengue? TipoDengue { get; set; }
        public override int TamanhoCampoDeVisao
        {
            get { return 2; }
        }
        public EstagioMosquito Estagio
        {
            get;
            set;
        }

        public bool EstaVivo
        {
            get;
            private set;
        }

        public Mosquito(EstagioMosquito e, Celula posicaoInicial, Ambiente amb)
            : base(amb, posicaoInicial)
        {
            this.Estagio = e;
            this.EstaVivo = true;
        }

        public override void Executar()
        {
            ciclosVivo++;
            if (ciclosVivo == 1)
            {
                Estagio = EstagioMosquito.Larva;
            }
            else if (ciclosVivo == 2)
            {
                Estagio = EstagioMosquito.Pupa;
            }
            else if (ciclosVivo == 3)
            {
                Estagio = EstagioMosquito.Adulto;
            }
            else if (ciclosVivo >= 50)
            {
                EstaVivo = false;
                OnMorreu();
            }
        }

        public override string Draw()
        {
            return "O";
        }
    }
}