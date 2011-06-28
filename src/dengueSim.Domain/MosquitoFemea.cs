using System;
using System.Collections.Generic;

namespace dengueSim.Domain
{
    public class MosquitoFemea : Mosquito
    {
        private bool jaPicou;
        private Random rd;
        private int qtdAcasalamento;
        private bool depositouOvos;
        public int numeroFemea{get;set;}
        public bool Acasalou
        {
            get;
            private set;
        }

        public MosquitoFemea(Ambiente amb, Celula posInicial)
            : base(EstagioMosquito.Adulto,posInicial,amb)
        
        {
            rd = RandomExtensions.GetInstance();
            qtdAcasalamento = 0;
            Acasalou = false;
            depositouOvos = false;
        }
        int ciclosEspera = 0;
        public override void Executar()
        {
            if (EstaVivo)
            {
                base.Executar();
                if (Estagio == EstagioMosquito.Adulto)
                {
                    //List<Posicoes> posicoesPossiveis = MovimentosPossiveis(this.Posicao);
                    List<Posicoes> desejos = new List<Posicoes>();

                    //int tamanho = TAMANHO_CAMPO_DE_VISAO * 2 + 1;
                    Celula[,] campoVisao = this.GetCampoVisao();
                    VerificaSePodePicar(campoVisao);
                    if (!jaPicou)
                    {
                        desejos = VerificaSePessoaNoCampoDeVisao(campoVisao);
                        if (desejos.Count == 0)
                            desejos.AddRange(MovimentosPossiveis(this.Posicao));

                    }
                    else if (!Acasalou)
                    {
                        desejos = VerificaSeMachoNoCampoDeVisao(campoVisao);

                        if (Posicao.Agentes.Find(ag => ag is MosquitoMacho) == null)
                        {
                            desejos.AddRange(MovimentosPossiveis(this.Posicao));
                        }
                    }
                    else if (Acasalou && !(Posicao is Foco) && !depositouOvos)
                    {
                        desejos = VerificaFocoNoCampoDeVisao(campoVisao);
                        if (desejos.Count == 0)
                            desejos.AddRange(MovimentosPossiveis(this.Posicao));

                    }

                    else if (Acasalou && (Posicao is Foco) && !depositouOvos)
                    {
                        Mosquito filhote;
                        int sexo;
                        for (int i = 0; i < 10; i++)
                        {
                            sexo = rd.Next(0, 100);
                            if (sexo < 50)
                            {
                                filhote = new MosquitoMacho(this.Ambiente, this.Posicao) { Estagio = EstagioMosquito.Ovo, TipoDengue = this.TipoDengue };
                            }
                            else
                            {
                                filhote = new MosquitoFemea(this.Ambiente, this.Posicao)
                                {
                                    Estagio = EstagioMosquito.Ovo,
                                    TipoDengue = this.TipoDengue
                                };
                            }
                            Ambiente.AdicionarAgente(filhote, Posicao);
                        }
                        depositouOvos = true;

                    }

                    else if (depositouOvos && ciclosEspera < 3)
                    {
                        ciclosEspera++;
                        desejos.AddRange(MovimentosPossiveis(this.Posicao));

                    }
                    else if (depositouOvos && ciclosEspera >= 3)
                    {
                        jaPicou = false;
                        depositouOvos = false;
                        Acasalou = true;
                        desejos.AddRange(MovimentosPossiveis(this.Posicao));
                    }
                    //Se nao tiver nenhuma pessoa perto,
                    //movimenta aleatoriamente


                    if (desejos.Count != 0)
                    {
                        Posicoes posicaoEscolhida = desejos[rd.Next(0, desejos.Count)];
                        Mover(posicaoEscolhida);
                    }
                }
            }
        }


        private List<Posicoes> VerificaSeMachoNoCampoDeVisao(Celula[,] campoVisao)
        {
            List<Posicoes> desejos = new List<Posicoes>();

            // Noroeste
            // 0,0 0,1 1,0             
            if ((campoVisao[0, 0] != null && campoVisao[0, 0].Agentes.Find(p => p is MosquitoMacho &&((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[0, 1] != null && campoVisao[0, 1].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[1, 0] != null && campoVisao[1, 0].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[1, 1] != null && campoVisao[1, 1].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Posicoes.NO);
            }

            // Norte
            if ((campoVisao[0, 2] != null && campoVisao[0, 2].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[0, 1] != null && campoVisao[0, 1].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Posicoes.N);
            }

            // Nordeste
            // 0,3 0,4 1,4
            if ((campoVisao[0, 3] != null && campoVisao[0, 3].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[0, 4] != null && campoVisao[0, 4].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[1, 4] != null && campoVisao[1, 4].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[1, 3] != null && campoVisao[1, 3].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Posicoes.NE);
            }

            // Leste
            if ((campoVisao[2, 4] != null && campoVisao[2, 4].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[2, 3] != null && campoVisao[2, 3].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Posicoes.E);
            }

            // Sudeste
            // 3,4 4,3 4,4
            if ((campoVisao[3, 4] != null && campoVisao[3, 4].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[4, 3] != null && campoVisao[4, 3].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[4, 4] != null && campoVisao[4, 4].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[3, 3] != null && campoVisao[3, 3].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Posicoes.SE);
            }

            // Sul
            if ((campoVisao[4, 2] != null && campoVisao[4, 2].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[3, 2] != null && campoVisao[3, 2].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Posicoes.S);
            }

            // Sudoeste            
            // 3,0 4,0 4,1
            if ((campoVisao[3, 0] != null && campoVisao[3, 0].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[4, 0] != null && campoVisao[4, 0].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[4, 1] != null && campoVisao[4, 1].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[3, 1] != null && campoVisao[3, 1].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Posicoes.SO);
            }

            // Oeste
            if ((campoVisao[2, 0] != null && campoVisao[2, 0].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[2, 1] != null && campoVisao[2, 1].Agentes.Find(p => p is MosquitoMacho && ((MosquitoMacho)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Posicoes.O);
            }

            return desejos;
        }


        private List<Posicoes> VerificaSePessoaNoCampoDeVisao(Celula[,] campoVisao)
        {
            List<Posicoes> desejos = new List<Posicoes>();

            // Noroeste
            // 0,0 0,1 1,0             
            if ((campoVisao[0, 0] != null && campoVisao[0,0].Agentes.Find(p => p is Pessoa) != null) ||
                 (campoVisao[0, 1] != null && campoVisao[0,1].Agentes.Find(p => p is Pessoa) != null) ||
                (campoVisao[1,0] != null && campoVisao[1,0].Agentes.Find(p => p is Pessoa) != null))
            {
                desejos.Add(Posicoes.NO);
            }

            // Norte
            if (campoVisao[0, 2] != null && campoVisao[0, 2].Agentes.Find(p => p is Pessoa) != null)
            {
                desejos.Add(Posicoes.N);
            }

            // Nordeste
            // 0,3 0,4 1,4
            if ((campoVisao[0, 3] != null && campoVisao[0, 3].Agentes.Find(p => p is Pessoa) != null) ||
                 (campoVisao[0, 4] != null && campoVisao[0, 4].Agentes.Find(p => p is Pessoa) != null) ||
                (campoVisao[1, 4] != null && campoVisao[1, 4].Agentes.Find(p => p is Pessoa) != null))
            {
                desejos.Add(Posicoes.NE);
            }

            // Leste
            if (campoVisao[2, 4] != null && campoVisao[2, 4].Agentes.Find(p => p is Pessoa) != null)
            {
                desejos.Add(Posicoes.E);
            }

            // Sudeste
            // 3,4 4,3 4,4
            if ((campoVisao[3, 4] != null && campoVisao[3, 4].Agentes.Find(p => p is Pessoa) != null) ||
                 (campoVisao[4, 3] != null && campoVisao[4, 3].Agentes.Find(p => p is Pessoa) != null) ||
                (campoVisao[4, 4] != null && campoVisao[4, 4].Agentes.Find(p => p is Pessoa) != null))
            {
                desejos.Add(Posicoes.SE);
            }

            // Sul
            if (campoVisao[4, 2] != null && campoVisao[4, 2].Agentes.Find(p => p is Pessoa) != null)
            {
                desejos.Add(Posicoes.S);
            }

            // Sudoeste            
            // 3,0 4,0 4,1
            if ((campoVisao[3, 0] != null && campoVisao[3, 0].Agentes.Find(p => p is Pessoa) != null) ||
                 (campoVisao[4, 0] != null && campoVisao[4, 0].Agentes.Find(p => p is Pessoa) != null) ||
                (campoVisao[4, 1] != null && campoVisao[4, 1].Agentes.Find(p => p is Pessoa) != null))
            {
                desejos.Add(Posicoes.SO);
            }

            // Oeste
            if (campoVisao[2, 0] != null && campoVisao[2, 0].Agentes.Find(p => p is Pessoa) != null)
            {
                desejos.Add(Posicoes.O);
            }

            return desejos;
        }

        private List<Posicoes> VerificaFocoNoCampoDeVisao(Celula[,] campoVisao)
        {
            List<Posicoes> desejos = new List<Posicoes>();

            // Noroeste
            // 0,0 0,1 1,0             
            if ((campoVisao[0, 0] != null && (campoVisao[0, 0] is Foco)) ||
                (campoVisao[0, 1] != null && (campoVisao[0, 1] is Foco)) ||
                (campoVisao[1, 0] != null && (campoVisao[1, 0] is Foco)) ||
                (campoVisao[1, 1] != null && (campoVisao[1, 1] is Foco)))
            {
                desejos.Add(Posicoes.NO);
            }

            // Norte
            if ((campoVisao[0, 2] != null && (campoVisao[0, 2]  is Foco)) ||
                (campoVisao[0, 1] != null && (campoVisao[0, 1]  is Foco)))
            {
                desejos.Add(Posicoes.N);
            }

            // Nordeste
            // 0,3 0,4 1,4
            if ((campoVisao[0, 3] != null && (campoVisao[0, 3] is Foco)) ||
                (campoVisao[0, 4] != null && (campoVisao[0, 4] is Foco)) ||
                (campoVisao[1, 4] != null && (campoVisao[1, 4] is Foco)) ||
                (campoVisao[1, 3] != null && (campoVisao[1, 3] is Foco)))
            {
                desejos.Add(Posicoes.NE);
            }

            // Leste
            if ((campoVisao[2, 4] != null && (campoVisao[2, 4]  is Foco)) ||
                (campoVisao[2, 3] != null && (campoVisao[2, 3]  is Foco)))
            {
                desejos.Add(Posicoes.E);
            }

            // Sudeste
            // 3,4 4,3 4,4
            if ((campoVisao[3, 4] != null && (campoVisao[3, 4] is Foco)) ||
                (campoVisao[4, 3] != null && (campoVisao[4, 3] is Foco)) ||
                (campoVisao[4, 4] != null && (campoVisao[4, 4] is Foco)) ||
                (campoVisao[3, 3] != null && (campoVisao[3, 3] is Foco)))
            {
                desejos.Add(Posicoes.SE);
            }

            // Sul
            if ((campoVisao[4, 2] != null && (campoVisao[4, 2] is Foco))||
                (campoVisao[3, 2] != null && (campoVisao[3, 2] is Foco)))
            {
                desejos.Add(Posicoes.S);
            }

            // Sudoeste            
            // 3,0 4,0 4,1
            if ((campoVisao[3, 0] != null && (campoVisao[3, 0] is Foco)) ||
                (campoVisao[4, 0] != null && (campoVisao[4, 0] is Foco)) ||
                (campoVisao[4, 1] != null && (campoVisao[4, 1] is Foco)) ||
                (campoVisao[3, 1] != null && (campoVisao[3, 1] is Foco)))
            {
                desejos.Add(Posicoes.SO);
            }

            // Oeste
            if ((campoVisao[2, 0] != null && (campoVisao[2, 0] is Foco)) ||
                (campoVisao[2, 1] != null && (campoVisao[2, 1] is Foco)))
            {
                desejos.Add(Posicoes.O);
            }

            return desejos;
        }

        private void VerificaSePodePicar(Celula[,] campoVisao)
        {
            Pessoa aux;
            // verifica noroeste
            aux = campoVisao[1, 1] != null ? campoVisao[1, 1].Agentes.Find(p => p is Pessoa) as Pessoa : null;
            if (aux != null)
            {
                this.Picar(aux);
                return;
            }

            //verifica se ha alguem no norte
            aux = campoVisao[1, 2] != null ? campoVisao[1,2].Agentes.Find(p => p is Pessoa) as Pessoa : null;
            if (aux != null)
            {
                this.Picar(aux);
                return;
                //desejos.Add(Posicoes.N);
            }
            //verifca se ha alguem no nordeste
            aux = campoVisao[1, 3] != null ? campoVisao[1,3].Agentes.Find(p => p is Pessoa) as Pessoa : null;
            if (aux != null)
            {
                this.Picar(aux);
                return;
                //desejos.Add(Posicoes.NE);
            }

            // verifica oeste
            aux = campoVisao[2, 1] != null ? campoVisao[2,1].Agentes.Find(p => p is Pessoa) as Pessoa : null;
            if (aux != null)
            {
                this.Picar(aux);
                return;
                //desejos.Add(Posicoes.O);
            }

            //verifica leste
            aux = campoVisao[2,3] != null ? campoVisao[2,3].Agentes.Find(p => p is Pessoa) as Pessoa : null;
            if (aux != null)
            {
                this.Picar(aux);
                return;
                //desejos.Add(Posicoes.E);
            }

            //verifica sudoeste
            aux = campoVisao[3, 1] != null ? campoVisao[3,1].Agentes.Find(p => p is Pessoa) as Pessoa : null;
            if (aux != null)
            {
                this.Picar(aux);
                return;
                //desejos.Add(Posicoes.SO);
            }

            //verifica o sul tche
            aux = campoVisao[3, 2] != null ? campoVisao[3,2].Agentes.Find(p => p is Pessoa) as Pessoa : null;
            if (aux != null)
            {
                this.Picar(aux);
                return;
                //desejos.Add(Posicoes.S);
            }

            //verifica sudeste
            aux = campoVisao[3, 3] != null ? campoVisao[3,3].Agentes.Find(p => p is Pessoa) as Pessoa : null;
            if (aux != null)
            {
                this.Picar(aux);
                return;
                //desejos.Add(Posicoes.SE);
            }
        }

        private void Picar(Pessoa aux)
        {
            if (this.TipoDengue.HasValue)
            {
                aux.SofrerPicada(TipoDengue.Value);
            }
            else
            {
                if (aux.ContaminadoPor.HasValue)
                {
                    this.TipoDengue = aux.ContaminadoPor.Value;
                }
            }
            jaPicou = true;
        }

        public override string Draw()
        {
            if (Estagio == EstagioMosquito.Ovo)
            {
                return "O";
            }
            if (Estagio == EstagioMosquito.Larva)
            {
                return "L";
            }
            if (Estagio == EstagioMosquito.Pupa)
            {
                return "P";
            }
            return "MF";
        }

        public bool QuerAcasalar()
        {
            if (qtdAcasalamento < 4)
            {
                if (jaPicou &&!Acasalou)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal void AcasalarCom(MosquitoMacho mosquitoMacho)
        {
           // MessageBox.Show(numeroFemea + " fudeuuu");
            Acasalou = true;          
            qtdAcasalamento++;
        }
    }
}
