using System;
using System.Drawing;
using System.Windows.Forms;
using dengueSim.Domain;

namespace dengueSim.App
{
    public partial class World : Form
    {
        private Ambiente amb;
        private Random rd = new Random();

        public World()
        {
            InitializeComponent();
            amb = new Ambiente(App.Default.TamanhoMundo, 0, 0, 0, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            for (int i = 0; i < amb.Tamanho; i++)
            {
                dataGridView1.Columns.Add(i.ToString(), i.ToString());
                dataGridView1.Columns[i].Width = 30;
            }

            dataGridView1.Rows.Add(amb.Tamanho);
            Draw();
        }

        public void Draw()
        {
            string agentesNoLocal = String.Empty;
            for (int i = 0; i < amb.Tamanho; i++)
            {
                for (int j = 0; j < amb.Tamanho; j++)
                {
                    dataGridView1[j, i].Value = null;
                    dataGridView1[j, i].Style.BackColor = Color.White;
                    //Pinta os focos
                    if (amb[i, j] is Foco)
                    {
                        dataGridView1[j, i].Style.BackColor = Color.Yellow;
                    }
                    foreach (Agente ag in amb[i, j].Agentes)
                    {
                        agentesNoLocal += ag.Draw();
                        if (ag is Pessoa)
                        {
                            dataGridView1[j, i].Style.ForeColor = Color.Black;
                        }

                        if (ag is AgenteDeSaude)
                        {
                            dataGridView1[j, i].Style.BackColor = Color.Brown;
                            dataGridView1[j, i].Style.ForeColor = Color.White;
                        }
                    }
                    dataGridView1[j, i].Value = agentesNoLocal;
                    agentesNoLocal = String.Empty;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            amb.Executar();
            amb.Atualizar();
            this.Draw();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                timer1.Enabled = !timer1.Enabled;
            }
            if (e.KeyCode == Keys.S)
            {
                if (timer1.Enabled)
                {
                    timer1.Enabled = false;
                }
                timer1_Tick(this, EventArgs.Empty);
            }
        }
    }
}