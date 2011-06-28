using System;
using System.Collections.Generic;
using System.Text;

namespace dengueSim.Domain
{
    public class Pessoa : Agente
    {
        //Variáveis de controle
        private const int TAMANHO_CAMPO_DE_VISAO = 2;
        private Random rd;       
        private Stack<TipoDengue> historico;
        private int ciclosContaminado = 0;
        private bool vaiMorrer = false;


        //Propriedades
        public bool TemFebreHemorragica { get; private set; }
        public bool EstaDoente { get; private set; }
        public bool EstaContaminado { get; private set; }  
        public bool EstaVivo { get;private set; }
        
        public TipoDengue? ContaminadoPor
        {
            get
            {
                if (EstaContaminado)
                {
                    return historico.Peek();
                }
                return null;
            }
        }

        //Construtores
        public Pessoa(Ambiente amb, Celula posicaoInicial)
            : base(amb, posicaoInicial)
        {
            rd = RandomExtensions.GetInstance();
            historico = new Stack<TipoDengue>();
            
            EstaContaminado = false;
            EstaDoente = false;
            EstaVivo = true;
        }

        //Métodos
        public void SofrerPicada(TipoDengue tipo)
        {
            if(!historico.Contains(tipo))
            {
                if (tipo == TipoDengue.Tipo2)
                {
                    int prob = rd.Next(0, 100);
                    if (prob <=100)//== 1)
                    {
                        TemFebreHemorragica = true;
                        //~= 7.5 % de chances de possuir febre hemorragica
                        prob = rd.Next(0, 106);
                        if (prob <= 106)//8)
                        {
                            vaiMorrer = true;
                        }
                    }
                }
                historico.Push(tipo);
                EstaContaminado = true;
                ciclosContaminado = 0;
            }
        }
       
        //Overrides
        public override void Executar()
        {
            if (EstaVivo)
            {
                if (EstaContaminado)
                {
                    this.ciclosContaminado++;
                    if (ciclosContaminado == 2)
                    {
                        EstaDoente = true;
                        if (vaiMorrer)
                        {
                            EstaVivo = false;
                            OnMorreu();
                        }
                    }
                    else if (ciclosContaminado == 8 &&
                        ContaminadoPor != TipoDengue.Tipo2)
                    {
                        EstaDoente = false;
                        EstaContaminado = false;
                    }
                    else if (ciclosContaminado == 12 &&
                        ContaminadoPor == TipoDengue.Tipo2)
                    {
                        EstaDoente = false;
                        EstaContaminado = false;
                        TemFebreHemorragica = false;
                    }
                }

                List<Posicoes> posicoesPossiveis = MovimentosPossiveis(this.Posicao);
                


                //int tamanho = TAMANHO_CAMPO_DE_VISAO * 2 + 1;
                Celula[,] campoVisao = this.GetCampoVisao();

                // verifica se outras pessoas estão ao lado
                //for (int i = 1; i < tamanho-1; i++)
                //{
                //    for (int j = 1; j < tamanho-1; j++)
                //    {

                //    }
                //}
                 
                // verifica noroeste
                if (campoVisao[1, 1] != null && campoVisao[1, 1].Agentes.Find(p => p is Pessoa || p is AgenteDeSaude) != null)
                {
                    posicoesPossiveis.RemoveAll(p => p == Posicoes.NO);
                }
                //verifica se ha alguem no norte
                if (campoVisao[1, 2] != null && campoVisao[1, 2].Agentes.Find(p => p is Pessoa || p is AgenteDeSaude) != null)
                {
                    posicoesPossiveis.RemoveAll(p => p == Posicoes.N);
                }
                //verifca se ha alguem no nordeste
                if (campoVisao[1, 3] != null && campoVisao[1, 3].Agentes.Find(p => p is Pessoa || p is AgenteDeSaude) != null)
                {
                    posicoesPossiveis.RemoveAll(p => p == Posicoes.NE);
                }
                // verifica oeste
                if (campoVisao[2, 1] != null && campoVisao[2, 1].Agentes.Find(p => p is Pessoa || p is AgenteDeSaude) != null)
                {
                    posicoesPossiveis.RemoveAll(p => p == Posicoes.O);
                }
                //verifica este
                if (campoVisao[2, 3] != null && campoVisao[2, 3].Agentes.Find(p => p is Pessoa || p is AgenteDeSaude) != null)
                {
                    posicoesPossiveis.RemoveAll(p => p == Posicoes.E);
                }
                //verifica sudoeste
                if (campoVisao[3, 1] != null && campoVisao[3, 1].Agentes.Find(p => p is Pessoa || p is AgenteDeSaude) != null)
                {
                    posicoesPossiveis.RemoveAll(p => p == Posicoes.SO);
                }
                //verifica o sul tche
                if (campoVisao[3, 2] != null && campoVisao[3, 2].Agentes.Find(p => p is Pessoa || p is AgenteDeSaude) != null)
                {
                    posicoesPossiveis.RemoveAll(p => p == Posicoes.S);
                }
                //verifica sudeste
                if (campoVisao[3, 3] != null && campoVisao[3, 3].Agentes.Find(p => p is Pessoa || p is AgenteDeSaude) != null)
                {
                    posicoesPossiveis.RemoveAll(p => p == Posicoes.SE);
                }

                Posicoes posicaoEscolhida = posicoesPossiveis[rd.Next(0, posicoesPossiveis.Count)];
                Mover(posicaoEscolhida);
            }
        }

        public override int TamanhoCampoDeVisao
        {
            get { return TAMANHO_CAMPO_DE_VISAO; }
        }

        public override string Draw()
        {
            //TODO fugir do mosquito - opcional
            //TODO alterar representacoes de acordo com o estado da pessoa
            if (EstaContaminado)
            {
                return "C";
            }
            return "P";
        }
    }
}
