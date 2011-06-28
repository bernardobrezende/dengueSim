using System;
using System.Collections.Generic;
using System.Text;

namespace dengueSim.Domain
{
    public abstract class Agente
    {
        private Ambiente ambiente;
        private Celula posicaoAtual;

        public abstract void Executar();
        public abstract int TamanhoCampoDeVisao{get;}
        Random rd = new Random((int)DateTime.Now.Ticks);
        public Celula Posicao
        {
            get { return posicaoAtual; }
            set { posicaoAtual = value; }
        }
        public event EventHandler Morreu;

        public Ambiente Ambiente
        {
            get { return this.ambiente; }
        }

        public Agente(Ambiente amb, Celula posInicial)
        {
            this.ambiente = amb;
            this.posicaoAtual = posInicial;
        }

        protected virtual void OnMorreu()
        {
            if (Morreu != null)
            {
                Morreu(this, EventArgs.Empty);
            }
        }
        public void Mover(Posicoes p)
        {
            int x =0;
            int y = 0;
            switch (p)
            {
                case Posicoes.N: x = 0; y = -1;
                    break;
                case Posicoes.NE: x = 1; y = -1;
                    break;
                case Posicoes.E: x = 1; y = 0;
                    break;
                case Posicoes.SE: x = 1; y = 1;
                    break;
                case Posicoes.S: x = 0; y = 1;
                    break;
                case Posicoes.SO: x = -1; y = 1;
                    break;
                case Posicoes.O: x = -1; y = 0;
                    break;
                case Posicoes.NO: x = -1; y = -1;
                    break;
            }         

            int proxX  = Posicao.Coluna + x;
            int proxY = Posicao.Linha + y;

            Mover(ambiente[proxY, proxX]);
            
        }
        public void Mover(Celula c)
        {
            Posicao.RemoverAgente(this);
            posicaoAtual = c;
            Posicao.AdicionarAgente(this);
        }
        public abstract string Draw();

        protected List<Posicoes> MovimentosPossiveis(Celula posicaoAtual)
        {
            List<Posicoes> posicoesPossiveis = new List<Posicoes>() { Posicoes.E, Posicoes.N,
                                                                        Posicoes.NE, Posicoes.NO,
                                                                        Posicoes.O, Posicoes.S,
                                                                        Posicoes.SE, Posicoes.SO };
            //Verifica limite superior e inferior
            if (posicaoAtual.Linha == ambiente.Tamanho - 1)
            {
                posicoesPossiveis.RemoveAll(p => p == Posicoes.S || p == Posicoes.SO || p == Posicoes.SE);
            }
            else if (posicaoAtual.Linha == 0)
            {
                posicoesPossiveis.RemoveAll(p => p == Posicoes.N || p == Posicoes.NO || p == Posicoes.NE);
            }

            //verifica limites laterais
            if (posicaoAtual.Coluna == 0)
            {
                posicoesPossiveis.RemoveAll(p => p == Posicoes.O || p == Posicoes.SO || p == Posicoes.NO);
            }

            else if (posicaoAtual.Coluna == ambiente.Tamanho - 1)
            {
                posicoesPossiveis.RemoveAll(p => p == Posicoes.E || p == Posicoes.SE || p == Posicoes.NE);
            }
            return posicoesPossiveis;
        }

        public Celula[,] GetCampoVisao()
        {
            int tamCampo = this.TamanhoCampoDeVisao * 2 + 1;
            Celula[,] campo = new Celula[tamCampo, tamCampo];

            int tamanho =this.TamanhoCampoDeVisao;


            for (int i = 0; i < tamCampo; i++)
            {
                for (int j = 0; j < tamCampo; j++)
                {
                    int celulaI = this.Posicao.Linha - 2 + i;
                    int celulaJ = this.Posicao.Coluna - 2 + j;
                    if ((celulaI < 0 || celulaI >= Ambiente.Tamanho) || (celulaJ < 0 || celulaJ >= Ambiente.Tamanho))
                    {
                        campo[i, j] = null;
                    }
                    else
                    {
                        campo[i, j] = Ambiente[celulaI, celulaJ];
                    }
                }
            }
            return campo;
        }

    }
}
