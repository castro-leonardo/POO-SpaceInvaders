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
        // ------------ atributos ----------------//
        private Jogo NovoJogo;

        //--------- otimização ---------//

        //--------- para que o jogo rode mais limpo, foi adicionado isso ----------------//
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

        // ---------------------------------------------------------------//

        //-------- metodos---------//
        public void Iniciar_Jogo()
        {
            //------------ inicializa um novo jogo, com os parametros necessarios, cria o espaço do jogo ---------//
            NovoJogo = new Jogo();
            NovoJogo.Comecar_Jogo(this.ClientSize.Width, this.ClientSize.Height);
            PointsTxtBox.Text = "0";
            
            foreach (Alien alien in NovoJogo.Get_Aliens()) this.Controls.Add(alien.Get_Foto());

            foreach (PictureBox iconeVida in NovoJogo.Get_Vidas()) this.Controls.Add(iconeVida);

            this.Controls.Add(NovoJogo.Get_Nave().Get_Foto());

            //------- chama o metodo que movimenta --------//
            Movimento();
        }

        public void Clean()
        {
            //---------- limpa a tela, usado para quando reiniciamos o jogo ----------//
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                if (this.Controls[i] is PictureBox)
                {
                    this.Controls.RemoveAt(i);
                }
            }

        }

        public void Pontuar(string TipoAlien)
        {
            //----------- trabalha com a pontuação ---------//
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
            //---------- metodo que cuida do movimento, para facilitar o uso das threads declaramos uma var jogoAtual, que recebe o NovoJogo --------//
            Jogo jogoAtual = this.NovoJogo;

            //-------------- essa thread cuida dos movimentos dos aliens ------------------//
            Thread t = new Thread(() => {

                int gapX = 20;
                int gapY = 20;

                //------------ enquanto o jogo estiver ativo ---------//
                while (jogoAtual.Get_JogoAtivo() == true)
                {
                    //---------- bool para conferir se nao bateu na borda ----------//
                    bool borda = false;

                    //---------- usamos o invoke porque essa é uma thread secundaria, e para mover as imagens precisamos do invoke ---------//
                    this.Invoke(new Action(() =>
                    {
                        // ---------------- MOVIMENTO DOS ALIENS ---------------- //

                        if (jogoAtual.Get_JogoAtivo() == false) return; //ve se o jogo ta ativo, se nao tiver nao move

                    //---------- move toda a lista de aliens ----------//
                        foreach (Alien alien in jogoAtual.Get_Aliens()) 
                        {
                            if ((alien.Get_Foto().Location.X + alien.Get_Foto().Size.Width >= this.ClientSize.Width && gapX > 0) ||
                                (alien.Get_Foto().Location.X <= 0 && gapX < 0))
                            {
                                //------ se bater na borda sai do foreach -------//
                                borda = true;
                                break;
                            }
                        }

                        //-------- se bateu na borda tem que começar a mover pro lado oposto e ir uma fileira pra baixo -----------//
                        if (borda == true)
                        {
                            gapX = -gapX;

                            foreach (Alien alien in jogoAtual.Get_Aliens())
                            {
                                alien.Get_Foto().Location = new System.Drawing.Point(alien.Get_Foto().Location.X + gapX, alien.Get_Foto().Location.Y + gapY);

                                //--------- se quando mover pra baixo bater na altura da nave, o jogador perde o jogo -----------//
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

                    //--------- sleep da velocidade de movimentação dos aliens -------------//
                    Thread.Sleep(800);
                }
            });

            //---------------- essa thread cuida da movimentação dos tiros -------------//
            Thread u = new Thread(() => {

                //------------- como os tiros dos aliens sao aleatorios precisamos de um random, alem de uma lista de tiros ---------//
                Random sorteio = new Random();
                List<Projetil> tirosAliens = new List<Projetil>();

                while (jogoAtual.Get_JogoAtivo() == true)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (jogoAtual.Get_JogoAtivo() == false) return; //ver se o jogo ta ativo

                        //-------------------------- TIROS INIMIGOS -------------------------//

                        if (sorteio.Next(0, 170) < 10 && jogoAtual.Get_Aliens().Count > 0)
                        {
                            //--------- qual alien vai ter atirado ------------//
                            int indiceAtirador = sorteio.Next(0, jogoAtual.Get_Aliens().Count);
                            Alien atirador = jogoAtual.Get_Aliens()[indiceAtirador];

                            //---------- cria o projetil de acordo onde o alien que atirou ta -----------//
                            Projetil tiroInimigo = new Projetil(-20, atirador.Get_Foto().Location.X, atirador.Get_Foto().Location.Y);
                            tiroInimigo.Get_Foto().BackColor = Color.Red;

                            //----------- adiciona o projetil na lista e bota ele "na frente" das outras imagens --------------//
                            tirosAliens.Add(tiroInimigo);
                            this.Controls.Add(tiroInimigo.Get_Foto());
                            tiroInimigo.Get_Foto().BringToFront();
                        }

                        //------------ enquanto tiver projetil na tela ------------------//
                        for (int i = tirosAliens.Count - 1; i >= 0; i--)
                        {
                            //------------- move o projetil pra direçao do jogador -----------//
                            Projetil tiroMal = tirosAliens[i];
                            tiroMal.Set_PosY(tiroMal.Get_PosY() - tiroMal.Get_Velocidade());

                            //-------------- confere se bateu no jogador ----------//
                            if (tiroMal.Get_Foto().Bounds.IntersectsWith(jogoAtual.Get_Nave().Get_Foto().Bounds))
                            {
                                //---------- se sim, o tiro some e o jogador toma dano ---------//
                                this.Controls.Remove(tiroMal.Get_Foto());
                                tirosAliens.RemoveAt(i);

                                jogoAtual.Get_Nave().Tomar_Dano();
                                List<PictureBox> listaVidas = jogoAtual.Get_Vidas();

                                //--------- se o jogador tiver vidas, tira uma vida do display -------//
                                if (listaVidas.Count > 0)
                                {
                                    PictureBox vidaPerdida = listaVidas[listaVidas.Count - 1];

                                    this.Controls.Remove(vidaPerdida);

                                    listaVidas.Remove(vidaPerdida);
                                }

                                //---------- se nao acabou o jogo -----------//
                                if (listaVidas.Count <= 0)
                                {
                                    jogoAtual.Set_JogoAtivo(false);
                                    this.Game_Over();
                                    return;
                                }
                                continue;
                            }

                            //-------------- se o tiro passar reto, so remove ele --------------//
                            if (tiroMal.Get_PosY() > this.ClientSize.Height)
                            {
                                this.Controls.Remove(tiroMal.Get_Foto());
                                tirosAliens.RemoveAt(i);
                            }
                        }


                    //--------------- TIRO DO JOGADOR ----------------//

                    Projetil tiro = jogoAtual.Get_Nave().Get_Tiro();

                        if (tiro != null)
                        {
                            tiro.Set_PosY(tiro.Get_PosY() - tiro.Get_Velocidade()); //move o tiro em direçao aos aliens

                            //------------ confere se colidiu com algum alien ----------//
                            foreach (Alien alien in jogoAtual.Get_Aliens())
                            {
                                if (tiro.Colidiu_Com(alien.Get_Foto()) == true)
                                {
                                    //----- se sim "mata" o alien e aumenta a pontuação de acordo com o tipo ---------//
                                    alien.Tomar_Dano();
                                    this.Controls.Remove(tiro.Get_Foto());
                                    jogoAtual.Get_Nave().Set_Tiro(null);

                                    Pontuar(alien.Get_type());

                                    jogoAtual.Get_Aliens().Remove(alien);

                                    break;
                                }
                            }

                            //--------- se acabou os aliens, ganha o jogo ----------//
                            if (jogoAtual.Get_Aliens().Count <= 0)
                            {
                                jogoAtual.Set_JogoAtivo(false);
                                this.Game_Win();
                                return;
                            }

                            //------ se o tiro passa reto so some ----------//
                            if (tiro != null && tiro.Get_PosY() < 0)
                            {
                                this.Controls.Remove(tiro.Get_Foto());
                                jogoAtual.Get_Nave().Set_Tiro(null);
                            }
                        }


                    }));

                    //------------ se o jogo acabou, sai do loop ------------//
                    if (jogoAtual.Get_JogoAtivo() == false)
                    {
                        break;
                    }

                    //---------- movimentacao dos tiros ------------//
                    Thread.Sleep(70);

                }
            });

            t.IsBackground = true;
            u.IsBackground = true;

            t.Start();
            u.Start();
        }

        //----------- metodo pra caso o jogador perca, limpa e reinicia ------------//
        public void Game_Over()
        {
            DialogResult Resultado = MessageBox.Show("Game Over!");
            if (Resultado == DialogResult.OK)
            {
                Clean();
                this.Iniciar_Jogo();
                
            }
        }

        //-------------- metodo para caso o jogador ganhe, limpa e reinicia ------------//
        public void Game_Win()
        {
            DialogResult Resultado = MessageBox.Show("Você venceu!");
            if(Resultado == DialogResult.OK)
            {
                Clean();
                this.Iniciar_Jogo();
            }
        }

        //----------- qnd o form carrega mostra os controles e inicia o jogo --------------//
        private void Form1_Load(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("@@ SPACE INVADERS @@\n\nAndar: < >\nAtirar: SPACE\nSair: Q");
            Iniciar_Jogo();

        }

        //-------------- verifica as teclas pra registrar os movimentos -----------//
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
