using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{

    //------ classe jogador -----//
    internal class NaveJogador
    {
        //----- o jogador precisa de definiçoes ----//
        private int vida = 3;
        private int pontuacao = 0;
        private PictureBox foto = null;
        private Projetil tiroAtual = null;

        public NaveJogador(int LarguraTela, int AlturaTela)
        {
            Set_Foto(LarguraTela, AlturaTela);
            Set_Vida(3);
            Set_Pontuacao(0);
        }

        // getters & setters
        public void Set_Vida(int vida)
        {
            if (vida > 0) this.vida = vida;
        }

        public void Set_Pontuacao(int pontos) => this.pontuacao = pontos;

        public void Set_Foto(int LarguraTela, int AlturaTela)
        {

            this.foto = new PictureBox();
            this.foto.Size = new Size(30, 30);
            this.foto.Visible = true;
            this.foto.BackColor = Color.Transparent;
            this.foto.Location = new Point((LarguraTela - this.foto.Size.Width) / 2, AlturaTela - this.foto.Size.Height - 10);
            this.foto.BackgroundImage = Properties.Resources.player;
            this.foto.BackgroundImageLayout = ImageLayout.Stretch;
        }

        
        public void Set_Tiro(Projetil tiro) => this.tiroAtual = tiro;
        public PictureBox Get_Foto() => this.foto;

        public int Get_Pontuacao() => this.pontuacao;

        public int Get_Vida() => this.vida;

        public Projetil Get_Tiro() => this.tiroAtual;

        public void Tomar_Dano() => Set_Vida(Get_Vida() - 1);

        public void Movimentar(KeyEventArgs Tecla, int Borda)
        {
            int Velocidade = 10;

            switch (Tecla.KeyCode)
            {
                case Keys.Left:
                    if (Get_Foto().Location.X >= 0)
                        Get_Foto().Location = new Point(Get_Foto().Location.X - Velocidade, Get_Foto().Location.Y);
                    Console.WriteLine("PENIS");
                    break;

                case Keys.Right:
                    if (Get_Foto().Location.X <= Borda)
                        Get_Foto().Location = new Point(Get_Foto().Location.X + Velocidade, Get_Foto().Location.Y);
                    Console.WriteLine("SINEP");
                    break;

                case Keys.Space:

                    if (this.tiroAtual == null)
                    {
                        Projetil proj = new Projetil(40, Get_Foto().Location.X, Get_Foto().Location.Y);
                        Set_Tiro(proj);
                    }
                    break;
            }
        }
    }
}
