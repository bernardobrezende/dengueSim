using System;
using System.Collections.Generic;
using System.Text;

namespace dengueSim.Domain
{
    public class Celula
    {
        private List<Agente> agentes;
        private int i,j;
        private int numero;
        private Ambiente amb;

        public Celula(int i,int j, int numero, Ambiente amb)
        {
            agentes = new List<Agente>();
            this.i = i;
            this.j = j;
            this.numero = numero;
            this.amb = amb;
        }


        public Ambiente Ambiente
        {
            get { return amb; }
        }
        public List<Agente> Agentes
        {
            get { return agentes; }
        }
        public int Numero
        {
            get { return numero; }
        }

        public int Linha
        {
            get
            {
                return i;
            }
        }

        public int Coluna
        {
            get { return j; }
        }

        public virtual void AdicionarAgente(Agente ag)
        {
            agentes.Add(ag);
        }

        public virtual void RemoverAgente(Agente ag)
        {
            agentes.Remove(ag);
        }
    }
}
