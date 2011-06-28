using System;
using System.Collections.Generic;
using System.Drawing;
namespace dengueSim.Domain
{
    public class Ambiente:IEnumerable<Agente>
    {
        private int tamanho;
        private List<Agente> agentes;
        private Random rd = new Random();
        private List<Agente> agentesAdicionados;
        private List<Agente> agentesRemovidos;

        public int Tamanho
        {
            get { return tamanho; }
            set { tamanho = value; }
        }
        int ciclos =0;
        int qtdFocos = 0;
        public Ambiente(int tamanho, int numeroMosquitos, int numeroPessoas, int numeroAgentes, int numeroFocos)
        {
            //if (numeroMosquitos < 8)
            //{
            //    throw new ArgumentException("Numero de mosquitos deve ser igual ou superior a 8");
            //}
            //if (numeroPessoas < 10)
            //{
            //    throw new ArgumentException("Numero de pessoas deve ser igual ou superior a 10");
            //}
            //if (numeroAgentes < 2)
            //{
            //    throw new ArgumentException("Numero de agentes deve ser igual ou superior a 2");
            //}
            //if (numeroFocos < 2)
            //{
            //    throw new ArgumentException("Numero de focos deve ser igual ou superior a 2");
            //}

            agentes = new List<Agente>();
            agentesAdicionados = new List<Agente>();
            agentesRemovidos = new List<Agente>();

            //Inicializa as celulas
            this.tamanho = tamanho;

            qtdFocos = Convert.ToInt32((tamanho * tamanho) * 0.1);

            int numeroCelula = 0;
            celulas = new Celula[tamanho, tamanho];
            for (int i = 0; i < tamanho; i++)
            {
                for (int j = 0; j < tamanho; j++)
                {
                    celulas[i, j] = new Celula(i, j,numeroCelula++,this);
                }
            }
            //Cria os focos
            CriarFocos();
            
            // criar as Pessoas
           CriarPessoas();
            //TODO criar os mosquitos
           CriarMosquitos();
            //TODO criar os agentesDeSaude
            CriarAgentesDeSaude();

        }
        public int QtdMosquitosMortos
        {
            get;
            private set;
        }



        private void CriarMosquitos()
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < 4; i++)
            {
                 x =rd.Next(0, tamanho);
                 y = rd.Next(0,tamanho);
                MosquitoMacho mm = new MosquitoMacho(this, celulas[x,y]);
                celulas[x, y].AdicionarAgente(mm);
                mm.Morreu += new EventHandler(mf_Morreu);
                this.agentes.Add(mm);
            } 

            for (int i = 0; i < 4; i++)
            {

                x = rd.Next(0, tamanho);
                y = rd.Next(0, tamanho);

                MosquitoFemea mf = new MosquitoFemea(this, celulas[x,y]) { numeroFemea = i };
                mf.TipoDengue = (TipoDengue)rd.Next(0, 4);
                mf.Morreu += new EventHandler(mf_Morreu);
                celulas[x, y].AdicionarAgente(mf);
                this.agentes.Add(mf);
                
            }
                 
        }

        private List<Agente> agentesMortos = new List<Agente>();

        public void RemoverAgente(Agente ag)
        {
            agentesRemovidos.Add(ag);
        }

        public void Executar()
        {
            
            foreach (Agente ag in agentesAdicionados)
            {
                agentes.Add(ag);
            }
            agentesAdicionados.Clear();

            foreach (Agente ag in agentesRemovidos)
            {
                agentes.Remove(ag);
            }
            agentesRemovidos.Clear();

            foreach (Agente ag in agentes)
            {
                ag.Executar();
            }
            ciclos++;
            if (ciclos == 10)
            {
                ciclos = 0;
                int x = rd.Next(0, tamanho);
                int y = rd.Next(0, tamanho);

                if (celulas[x, y].Agentes.Count == 0)
                {
                   Celula c =  celulas[x,y];
                   celulas[x, y] = new Foco(c.Linha, c.Coluna, c.Numero, c.Ambiente);
                }
            }
        }

        public void Atualizar()
        {
            foreach (Agente ag in agentesMortos)
            {
                ag.Posicao.RemoverAgente(ag);
                this.agentes.Remove(ag);
            }
            agentesMortos.Clear();
        }
        
        void mf_Morreu(object sender, EventArgs e)
        {
            agentesMortos.Add(sender as Mosquito);
            QtdMosquitosMortos++;
        }
        public IEnumerable<Agente> Agentes
        {
            get { return this.agentes; }
        }
        public void AdicionarAgente(Agente ag, Celula posicao)
        {
            agentesAdicionados.Add(ag);
            posicao.AdicionarAgente(ag);
        }

        private void CriarAgentesDeSaude()
        {
            // TODO: parametrizar este número
            double numAgentes = 5;//App.Default.QtdAgentesSaude;
            int intervalo = (int)Math.Round(Math.Pow(Tamanho, 2) / numAgentes);
            
            int valor;
            Celula celulaInicial, celulaFinal;

            for (int i = 0; i < numAgentes ; i++)
            {
                valor = i * intervalo;
                celulaInicial = this[valor];
                celulaFinal = this[valor + intervalo -1];

                AdicionarAgente(new AgenteDeSaude(this,celulaInicial,celulaFinal),celulaInicial);
            }

        }
        private void CriarPessoas()
        {
            // TODO: Parametrizar esta informação
            int qtdePessoas = 4;
            List<Point> locais = new List<Point>();
            Point aux;
            int x, y;
            for (int i = 0; i < qtdePessoas; i++)
            {
                x = rd.Next(0, this.Tamanho - 1);
                y = rd.Next(0, this.Tamanho - 1);
                aux = new Point(x, y);

                //verifica se existe uma pessoa
                while (locais.Contains(aux))
                {
                    x = rd.Next(0, this.Tamanho - 1);
                    y = rd.Next(0, this.Tamanho - 1);
                    aux = new Point(x, y);
                }

                this.AdicionarAgente(new Pessoa(this, this[x, y]), this[x, y]);
            }
                
        }
        private void CriarFocos()
        {
            // TODO: Parametrizar esta informação
            int qtdeFocos = 12;
            List<Foco> focos = new List<Foco>();           
            Foco aux;
            int x;
            int y;
            for (int i = 0; i < qtdeFocos; i++)
            {
                
                x = rd.Next(0, this.Tamanho - 1);
                y = rd.Next(0, this.Tamanho - 1);
                aux = new Foco(x, y,celulas[x,y].Numero,this);
                
                
                //Garantir que não sejam gerados dois focos no mesmo local
                while(focos.Contains(aux))
                {
                    x = rd.Next(0, this.Tamanho - 1);
                    y = rd.Next(0, this.Tamanho - 1);
                    aux = new Foco(x, y,celulas[x, y].Numero,this);
                }
                this.celulas[x, y] = aux;
            }
        }

        private Celula[,] celulas;

        public Celula this[int i, int j]
        {
            get { return celulas[i,j]; }
            set { celulas[i, j] = value; }
        }


        public Celula this[int numeroCelula]
        {
            get
            {
                int x = numeroCelula / Tamanho;
                int y = numeroCelula % Tamanho;
                return this[x, y];
            }
            set
            {
                int x = numeroCelula / Tamanho;
                int y = numeroCelula % Tamanho;
                this[x, y] = value;
            }
        }
        //public Celula[,] GetCampoVisao(Agente a)
        //{
        //    int tamCampo = a.TamanhoCampoDeVisao*2 + 1;
        //    Celula[,] campo = new Celula[tamCampo, tamCampo];

        //    int tamanho = a.TamanhoCampoDeVisao;

            
        //    for (int i = 0 ; i <tamCampo; i++)
        //    {
        //        for (int j = 0; j < tamCampo ; j++)
        //        {
        //            int celulaI = a.Posicao.Linha-2 + i;
        //            int celulaJ = a.Posicao.Coluna-2 + j;
        //            if ((celulaI  < 0 || celulaI  >= this.Tamanho) || (celulaJ < 0 || celulaJ >= this.Tamanho))
        //            {
        //                campo[i,j] = null;
        //            }
        //            else
        //            {
        //                campo[i ,j] = celulas[celulaI,celulaJ];
        //            }
               
        //        }
               
        //    }
        //    return campo;           
        //}

        #region IEnumerable<Agente> Members

        public IEnumerator<Agente> GetEnumerator()
        {
            foreach (Agente ag in agentes)
            {
                yield return ag;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (Agente ag in agentes)
            {
                yield return ag;
            }
        }

        #endregion
    }
}
