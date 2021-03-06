using System;
using System.Linq;

namespace dengueSim.Domain
{
    public class Foco : Celula, IEquatable<Foco>
    {
        public Foco(int i, int j, int numeroCelula, Ambiente ambiente) : base(i, j, numeroCelula, ambiente) { }

        public bool PossuiOvos
        {
            get { return Agentes.Any(a => a is Mosquito && ((Mosquito)a).Estagio == EstagioMosquito.Ovo); }
        }

        public bool PossuiLarvas
        {
            get { return Agentes.Any(a => a is Mosquito && ((Mosquito)a).Estagio == EstagioMosquito.Larva); }
        }

        public bool PossuiPupas
        {
            get { return Agentes.Any(a => a is Mosquito && ((Mosquito)a).Estagio == EstagioMosquito.Pupa); }
        }

        public bool PossuiMosquitos
        {
            get { return Agentes.Any(a => a is Mosquito && ((Mosquito)a).Estagio == EstagioMosquito.Adulto); }
        }

        public int CalcularQtdOvos()
        {
            throw new System.NotImplementedException();
        }

        public int CalcularQtdPupas()
        {
            throw new System.NotImplementedException();
        }

        public int CalcularQtdLarvas()
        {
            throw new System.NotImplementedException();
        }

        public int CalcularQtdMosquitos()
        {
            throw new System.NotImplementedException();
        }

        #region IEquatable<Foco> Members

        public bool Equals(Foco other)
        {
            return this.Coluna == other.Coluna && this.Linha == other.Linha;
        }

        #endregion

        public void Limpar()
        {

        }

        internal void Exterminar()
        {
            Celula c = new Celula(this.Linha, this.Coluna, this.Numero, this.Ambiente);
            foreach (Agente ag in this.Agentes)
            {
                c.AdicionarAgente(ag);
            }
            Ambiente[this.Numero] = c;
        }
    }
}
