using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class Form1 : Form
    {
        private int Points = 0;
        private Jogo NovoJogo;
        private GerenciadorColisoes colisoes;

        /// OTIMIZA AS IMAGENS PARA O JOGO PODER FICAR RODANDO MELHOR
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  
                return cp;
            }
        }
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true; //melhora o visual
        }

        /// -------------------------------------------

        public void Iniciar_Jogo()
        {

            NovoJogo = new Jogo();
            colisoes = new GerenciadorColisoes();
            NovoJogo.Comecar_Jogo(this.ClientSize.Width, this.ClientSize.Height);
            PointsTxtBox.Text = "0";
            
            foreach (Alien alien in NovoJogo.Get_Aliens()) this.Controls.Add(alien.Get_Foto());

            foreach (PictureBox iconeVida in NovoJogo.Get_Vidas()) this.Controls.Add(iconeVida);

            this.Controls.Add(NovoJogo.Get_Nave().Get_Foto());

            Movimento();
        }

        public void Clean()
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                if (this.Controls[i] is PictureBox)
                {
                    this.Controls.RemoveAt(i);
                }
            }

            this.Points = 0;
        }


        public void Pontuar(string TipoAlien)
        {
            int PontosGanho = 0;
            int PontosAtuais = Convert.ToInt32(PointsTxtBox.Text);
            int PontosTotais = 0;
            
            if (TipoAlien == "red")    PontosGanho = 100;
            if (TipoAlien == "yellow") PontosGanho = 500;
            if (TipoAlien == "green")  PontosGanho = 1000;

            PontosTotais = PontosAtuais + PontosGanho;
            PointsTxtBox.Text = PontosTotais.ToString();
        }

        public void Movimento()
        {
            Jogo jogoAtual = this.NovoJogo;

            Thread t = new Thread(() => {
                int gapX = 20;
                int gapY = 20;

                while (jogoAtual.Status_Jogo() == true)
                {
                    bool borda = false;

                    this.Invoke(new Action(() =>
                    {
                        // ---------------- MOVIMENTO DOS ALIENS ---

                        if (jogoAtual.Status_Jogo() == false) return;
                        foreach (Alien alien in jogoAtual.Get_Aliens())
                        {
                            if ((alien.Get_Foto().Location.X + alien.Get_Foto().Size.Width >= this.ClientSize.Width && gapX > 0) ||
                                (alien.Get_Foto().Location.X <= 0 && gapX < 0))
                            {
                                borda = true;
                                break;
                            }
                        }

                        if (borda == true)
                        {
                            gapX = -gapX;

                            foreach (Alien alien in jogoAtual.Get_Aliens())
                            {
                                alien.Get_Foto().Location = new System.Drawing.Point(alien.Get_Foto().Location.X + gapX, alien.Get_Foto().Location.Y + gapY);

                                if (alien.Get_Foto().Location.Y + alien.Get_Foto().Height >= jogoAtual.Get_Nave().Get_Foto().Location.Y)
                                {
                                    jogoAtual.Set_JogoAtivo(false);
                                    this.Game_Over();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            foreach (Alien a in jogoAtual.Get_Aliens())
                                a.Get_Foto().Location = new System.Drawing.Point(a.Get_Foto().Location.X + gapX, a.Get_Foto().Location.Y);
                        }
                    }));

                    Thread.Sleep(800);
                }
            });

            Thread u = new Thread(() => {
                Random sorteio = new Random();
                List<Projetil> tirosAliens = new List<Projetil>();

                while (jogoAtual.Status_Jogo() == true)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (jogoAtual.Status_Jogo() == false) return;

                        // -------------------------- TIROS INIMIGOS ---
                        if (sorteio.Next(0, 150) < 10 && jogoAtual.Get_Aliens().Count > 0)
                        {
                            int indiceAtirador = sorteio.Next(0, jogoAtual.Get_Aliens().Count);
                            Alien atirador = jogoAtual.Get_Aliens()[indiceAtirador];

                            Projetil tiroInimigo = new Projetil(-20, atirador.Get_Foto().Location.X, atirador.Get_Foto().Location.Y);
                            tiroInimigo.Get_Foto().BackColor = Color.Red;

                            tirosAliens.Add(tiroInimigo);
                            this.Controls.Add(tiroInimigo.Get_Foto());
                            tiroInimigo.Get_Foto().BringToFront();
                        }

                        for (int i = tirosAliens.Count - 1; i >= 0; i--)
                        {
                            Projetil tiroMal = tirosAliens[i];
                            tiroMal.Set_PosY(tiroMal.Get_PosY() - tiroMal.Get_Velocidade());

                            if (tiroMal.Get_Foto().Bounds.IntersectsWith(jogoAtual.Get_Nave().Get_Foto().Bounds))
                            {
                                this.Controls.Remove(tiroMal.Get_Foto());
                                tirosAliens.RemoveAt(i);

                                jogoAtual.Get_Nave().Tomar_Dano();
                                List<PictureBox> listaVidas = jogoAtual.Get_Vidas();

                                if (listaVidas.Count > 0)
                                {
                                    PictureBox vidaPerdida = listaVidas[listaVidas.Count - 1];

                                    this.Controls.Remove(vidaPerdida);

                                    listaVidas.Remove(vidaPerdida);
                                }

                                if (listaVidas.Count <= 0)
                                {
                                    jogoAtual.Set_JogoAtivo(false);
                                    this.Game_Over();
                                    return;
                                }
                                continue;
                            }

                            if (tiroMal.Get_PosY() > this.ClientSize.Height)
                            {
                                this.Controls.Remove(tiroMal.Get_Foto());
                                tirosAliens.RemoveAt(i);
                            }
                        }


                    // --------------- TIRO DO JOGADOR ----------------

                    Projetil tiro = jogoAtual.Get_Nave().Get_Tiro();
                        if (tiro != null)
                        {
                            tiro.Set_PosY(tiro.Get_PosY() - tiro.Get_Velocidade());

                            foreach (Alien alien in jogoAtual.Get_Aliens())
                            {
                                if (tiro.Colidiu_Com(alien.Get_Foto()) == true)
                                {
                                    alien.Tomar_Dano();
                                    this.Controls.Remove(tiro.Get_Foto());
                                    jogoAtual.Get_Nave().Set_Tiro(null);

                                    Pontuar(alien.Get_type());

                                    jogoAtual.Get_Aliens().Remove(alien);

                                    break;
                                }
                            }

                            if (jogoAtual.Get_Aliens().Count <= 0)
                            {
                                jogoAtual.Set_JogoAtivo(false);
                                this.Game_Win();
                                return;
                            }

                            if (tiro != null && tiro.Get_PosY() < 0)
                            {
                                this.Controls.Remove(tiro.Get_Foto());
                                jogoAtual.Get_Nave().Set_Tiro(null);
                            }
                        }


                    }));

                    if (jogoAtual.Status_Jogo() == false)
                    {
                        break;
                    }

                    Thread.Sleep(70);

                }
            });

            t.IsBackground = true;
            u.IsBackground = true;

            t.Start();
            u.Start();
        }

        public void Game_Over()
        {
            DialogResult Resultado = MessageBox.Show("Game Over!");
            if (Resultado == DialogResult.OK)
            {
                Clean();
                this.Iniciar_Jogo();
                
            }
        }

        public void Game_Win()
        {
            DialogResult Resultado = MessageBox.Show("Você venceu!");
            if(Resultado == DialogResult.OK)
            {
                Clean();
                this.Iniciar_Jogo();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("@@ SPACE INVADERS @@\n\nAndar: < >\nAtirar: SPACE\nSair: Q");
            Iniciar_Jogo();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            NovoJogo.Get_Nave().Movimentar(e, this.ClientSize.Width);

            if(e.KeyCode == Keys.Q) ///sai do jogo
            {
                Application.Exit();
            }

            if (e.KeyCode == Keys.Space)
            {
                Projetil tiro = NovoJogo.Get_Nave().Get_Tiro();

                if (tiro != null && !this.Controls.Contains(tiro.Get_Foto()))
                {
                    this.Controls.Add(tiro.Get_Foto());
                    tiro.Get_Foto().BringToFront();
                }
            }
        }

        private void PointsTxtBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
