using System;
using System.Collections.Generic;
using System.Text;

namespace dengueSim.Domain
{
    public class AgenteDeSaude:Agente
    {
        
        private Celula alvo;
        private bool indoParaDireita;
        private int ciclosLimpando;
        private int quantidadeDeCiclosNecessarias;

        public bool EstaDetetizando{get; private set;}
        public Celula InicioCobertura { get; private set; }
        public Celula FimCobertura { get; private set; }

        public AgenteDeSaude(Ambiente amb, Celula inicioCobertura,Celula fimCobertura)
            :base(amb, inicioCobertura)
        {
            InicioCobertura = inicioCobertura;
            FimCobertura = fimCobertura;
            alvo = FimCobertura;
            indoParaDireita = true;
            EstaDetetizando = false;
            ciclosLimpando = 0;
            quantidadeDeCiclosNecessarias = 0;
        }

        private void AnalizarFoco(Foco f)
        {
            //Quantidade = 1+ para nao sair após limpar
            if (f.PossuiOvos)
                quantidadeDeCiclosNecessarias = 1;
            if (f.PossuiLarvas || f.PossuiPupas)
                quantidadeDeCiclosNecessarias = 2;
            if (f.PossuiMosquitos)
                quantidadeDeCiclosNecessarias = 3;
        }

   
        public override void Executar()
        {
            if (Posicao is Foco)
            {
                Foco foco = Posicao as Foco;
                if (!EstaDetetizando)
                {
                    //Verifica qual a complexidade do foco
                    AnalizarFoco(foco);
                    EstaDetetizando = true;
                }
                if(EstaDetetizando)
                {
                    //Verifica quantos ciclos ja estou no foco
                    if (ciclosLimpando < quantidadeDeCiclosNecessarias)
                    {
                        ciclosLimpando++;
                        foco.Limpar();
                    }
                    else
                    {
                        EstaDetetizando = false;
                        quantidadeDeCiclosNecessarias = 0;
                        ciclosLimpando = 0;
                        foco.Exterminar();
                        Posicao = Ambiente[this.Posicao.Numero];
                    }
                }
            }
            if (!EstaDetetizando)
            {
                    //posso me movimentar?
                    Celula proxCelula = CalcularProximoPasso();
                    if (proxCelula != null)
                        Mover(proxCelula);
            }
        }

        private Celula CalcularProximoPasso()
        {
            Celula proxCelula;

            //Verifica se esta indo do inicio para o fim da area
            if (alvo.Equals(FimCobertura))
            {
                proxCelula = CalculaDirecaoFim();
            }
            else
            {
                proxCelula = CalculaDirecaoInicio();
            }
            
            //TODO verificar se posso ir para a celula
            if (proxCelula.Agentes.Find(ag => ag is Pessoa) != null)
            {
                proxCelula = null;
            }
            return proxCelula;
        }

        private Celula CalculaDirecaoInicio()
        {
            Celula proxCelula;
            if (this.Posicao.Equals(InicioCobertura))
            {
                alvo = FimCobertura;
                proxCelula = Ambiente[Posicao.Linha, Posicao.Coluna + 1];
                indoParaDireita = true;
            }
            //Se estiver nas bordas da matriz, deverá subir
            else if (Posicao.Coluna == Ambiente.Tamanho - 1 && indoParaDireita)
            {
                proxCelula = Ambiente[Posicao.Linha - 1, Posicao.Coluna];
                indoParaDireita = false;
            }
            else if (Posicao.Coluna == 0 && !indoParaDireita)
            {
                proxCelula = Ambiente[Posicao.Linha - 1, Posicao.Coluna];
                indoParaDireita = true;
            }
            // senao continua no movimento que estava fazendo
            else
            {
                if (indoParaDireita)
                    proxCelula = Ambiente[Posicao.Numero + 1];
                else
                    proxCelula = Ambiente[Posicao.Numero - 1];
            }
            return proxCelula;
        }

        private Celula CalculaDirecaoFim()
        {
            Celula proxCelula;
            if (this.Posicao.Equals(InicioCobertura))
            {
                alvo = FimCobertura;
                proxCelula = Ambiente[Posicao.Linha, Posicao.Coluna + 1];
            }
            else if (this.Posicao.Equals(FimCobertura))
            {
                alvo = InicioCobertura;
                int extensao = FimCobertura.Linha + 1 - InicioCobertura.Linha;
                if (extensao % 2 == 0 && FimCobertura.Coluna != Ambiente.Tamanho - 1)
                {
                    proxCelula = Ambiente[Posicao.Linha, Posicao.Coluna + 1];
                    indoParaDireita = true;
                }
                else
                {
                    proxCelula = Ambiente[Posicao.Linha, Posicao.Coluna - 1];
                    indoParaDireita = false;
                }
            }
            // Se estiver nas bordas da matriz, deverá descer
            else if (Posicao.Coluna == Ambiente.Tamanho - 1 && indoParaDireita)
            {
                proxCelula = Ambiente[Posicao.Linha + 1, Posicao.Coluna];
                indoParaDireita = false;
            }
            else if (Posicao.Coluna == 0 && !indoParaDireita)
            {
                proxCelula = Ambiente[Posicao.Linha + 1, Posicao.Coluna];
                indoParaDireita = true;
            }
            //Senao continua movimentando-se 
            else
            {
                if (indoParaDireita)
                    proxCelula = Ambiente[Posicao.Numero + 1];
                else
                    proxCelula = Ambiente[Posicao.Numero - 1];
            }
            return proxCelula;
        }

        public override int TamanhoCampoDeVisao
        {
            get { return 1; }
        }

        public override string Draw()
        {
            return "A";
        }
    }
}
