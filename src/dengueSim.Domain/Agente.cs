using System;
using System.Collections.Generic;

namespace dengueSim.Domain
{
    /// <summary>
    /// Base class for agents
    /// </summary>
    public abstract class Agente
    {
        private Ambiente ambiente;
        private Celula posicaoAtual;
        private Random rd = new Random((int)DateTime.Now.Ticks);

        public abstract void Executar();
        public abstract int TamanhoCampoDeVisao { get; }

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

        public void Mover(Direcoes p)
        {
            int x = 0;
            int y = 0;
            switch (p)
            {
                case Direcoes.N: x = 0; y = -1;
                    break;
                case Direcoes.NE: x = 1; y = -1;
                    break;
                case Direcoes.E: x = 1; y = 0;
                    break;
                case Direcoes.SE: x = 1; y = 1;
                    break;
                case Direcoes.S: x = 0; y = 1;
                    break;
                case Direcoes.SO: x = -1; y = 1;
                    break;
                case Direcoes.O: x = -1; y = 0;
                    break;
                case Direcoes.NO: x = -1; y = -1;
                    break;
            }

            int proxX = Posicao.Coluna + x;
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

        protected List<Direcoes> MovimentosPossiveis(Celula posicaoAtual)
        {
            List<Direcoes> posicoesPossiveis = new List<Direcoes>() { Direcoes.E, Direcoes.N,
                                                                        Direcoes.NE, Direcoes.NO,
                                                                        Direcoes.O, Direcoes.S,
                                                                        Direcoes.SE, Direcoes.SO };
            // Verifica limite superior e inferior
            if (posicaoAtual.Linha == ambiente.Tamanho - 1)
            {
                posicoesPossiveis.RemoveAll(p => p == Direcoes.S || p == Direcoes.SO || p == Direcoes.SE);
            }
            else if (posicaoAtual.Linha == 0)
            {
                posicoesPossiveis.RemoveAll(p => p == Direcoes.N || p == Direcoes.NO || p == Direcoes.NE);
            }
            // Verifica limites laterais
            if (posicaoAtual.Coluna == 0)
            {
                posicoesPossiveis.RemoveAll(p => p == Direcoes.O || p == Direcoes.SO || p == Direcoes.NO);
            }
            else if (posicaoAtual.Coluna == ambiente.Tamanho - 1)
            {
                posicoesPossiveis.RemoveAll(p => p == Direcoes.E || p == Direcoes.SE || p == Direcoes.NE);
            }

            return posicoesPossiveis;
        }

        public Celula[,] GetCampoVisao()
        {
            int tamCampo = this.TamanhoCampoDeVisao * 2 + 1;
            Celula[,] campo = new Celula[tamCampo, tamCampo];

            int tamanho = this.TamanhoCampoDeVisao;

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