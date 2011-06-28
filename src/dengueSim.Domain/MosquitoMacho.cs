using System;
using System.Collections.Generic;

namespace dengueSim.Domain
{
    public class MosquitoMacho : Mosquito
    {
        private Random rd;
        private bool acasalou;
        private int ciclosEspera;

        public MosquitoMacho(Ambiente amb, Celula posInicial)
            : base(EstagioMosquito.Adulto, posInicial, amb)
        {
            rd = RandomExtensions.GetInstance();
            ciclosEspera = 0;
            acasalou = false;
        }

        public override void Executar()
        {
            if (EstaVivo)
            {
                base.Executar();
                if (Estagio == EstagioMosquito.Adulto)
                {
                    List<Direcoes> desejos = new List<Direcoes>();

                    if (acasalou && ciclosEspera < 3)
                    {
                        ciclosEspera++;
                        desejos.AddRange(MovimentosPossiveis(this.Posicao));
                    }
                    else
                    {
                        acasalou = false;
                        ciclosEspera = 0;
                        MosquitoFemea vitima = Posicao.Agentes.Find(mf => mf is MosquitoFemea) as MosquitoFemea;
                        if (vitima != null)
                        {
                            AcasalarCom(vitima);
                            desejos.AddRange(MovimentosPossiveis(this.Posicao)); //movimenta aleatoriamente
                        }

                        else
                        {
                            Celula[,] campoVisao = this.GetCampoVisao();
                            desejos = VerificaSeFemeaNoCampoDeVisao(campoVisao);

                            //Se nao tiver nenhuma pessoa perto, movimenta aleatoriamente
                            if (desejos.Count == 0)
                                desejos.AddRange(MovimentosPossiveis(this.Posicao));
                        }
                    }
                    Direcoes posicaoEscolhida = desejos[rd.Next(0, desejos.Count)];
                    Mover(posicaoEscolhida);
                }
            }
        }

        private void AcasalarCom(MosquitoFemea vitima)
        {
            if (vitima.QuerAcasalar())
            {
                //Se nao esta contaminado, ficara com o tipo de dengue da femea
                if (!this.TipoDengue.HasValue && vitima.TipoDengue.HasValue)
                {
                    this.TipoDengue = vitima.TipoDengue.Value;
                }
                vitima.AcasalarCom(this);
                acasalou = true;
            }
        }

        private List<Direcoes> VerificaSeFemeaNoCampoDeVisao(Celula[,] campoVisao)
        {
            List<Direcoes> desejos = new List<Direcoes>();

            // Noroeste
            // 0,0 0,1 1,0 1,1            
            if ((campoVisao[0, 0] != null && campoVisao[0, 0].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[0, 1] != null && campoVisao[0, 1].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[1, 0] != null && campoVisao[1, 0].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[1, 1] != null && campoVisao[1, 1].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Direcoes.NO);
            }

            // Norte
            if ((campoVisao[0, 2] != null && campoVisao[0, 2].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[1, 2] != null && campoVisao[1, 2].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Direcoes.N);
            }

            // Nordeste
            // 0,3 0,4 1,4 1,3
            if ((campoVisao[0, 3] != null && campoVisao[0, 3].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                 (campoVisao[0, 4] != null && campoVisao[0, 4].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[1, 4] != null && campoVisao[1, 4].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[1, 3] != null && campoVisao[1, 3].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Direcoes.NE);
            }

            // Leste
            if ((campoVisao[2, 4] != null && campoVisao[2, 4].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[2, 3] != null && campoVisao[2, 3].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Direcoes.E);
            }

            // Sudeste
            // 3,4 4,3 4,4 3,3
            if ((campoVisao[3, 4] != null && campoVisao[3, 4].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                 (campoVisao[4, 3] != null && campoVisao[4, 3].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[4, 4] != null && campoVisao[4, 4].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[3, 3] != null && campoVisao[3, 3].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Direcoes.SE);
            }

            // Sul
            if ((campoVisao[4, 2] != null && campoVisao[4, 2].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[3, 2] != null && campoVisao[3, 2].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Direcoes.S);
            }

            // Sudoeste            
            // 3,0 4,0 4,1 3,1
            if ((campoVisao[3, 0] != null && campoVisao[3, 0].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                 (campoVisao[4, 0] != null && campoVisao[4, 0].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[4, 1] != null && campoVisao[4, 1].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[3, 1] != null && campoVisao[3, 1].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Direcoes.SO);
            }

            // Oeste
            if ((campoVisao[2, 0] != null && campoVisao[2, 0].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null) ||
                (campoVisao[2, 1] != null && campoVisao[2, 1].Agentes.Find(p => p is MosquitoFemea && ((MosquitoFemea)p).Estagio == EstagioMosquito.Adulto) != null))
            {
                desejos.Add(Direcoes.O);
            }

            return desejos;
        }

        public override string Draw()
        {
            if (Estagio == EstagioMosquito.Ovo)
                return "0";
            if (Estagio == EstagioMosquito.Larva)
                return "L";
            if (Estagio == EstagioMosquito.Pupa)
                return "P";

            if (acasalou)
            {
                return "MM F";
            }
            return "MM";
        }
    }
}
