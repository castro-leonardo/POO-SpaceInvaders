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


    internal class Jogo
    {
        private NaveJogador nave;
        private List<Alien> aliens;
        private bool jogoAtivo = false;
        private List<PictureBox> vidas;
        

        public Jogo()
        {

        }

        // setters and getters

        public bool Get_JogoAtivo() => this.jogoAtivo;
        public void Set_JogoAtivo(bool jogoAtivo) => this.jogoAtivo = jogoAtivo;

        public NaveJogador Get_Nave() => this.nave;
        public void Set_Nave(NaveJogador nave) => this.nave = nave;
        public List<Alien> Get_Aliens() => this.aliens;
        public void Set_Aliens(List<Alien> aliens) => this.aliens = aliens;
        public List<PictureBox> Get_Vidas() => this.vidas;

        // metodos

        public void Comecar_Jogo(int LarguraTela, int AlturaTela)
        {
            jogoAtivo = true;
            aliens = new List<Alien>();
            vidas = new List<PictureBox>();
            Criar_Aliens(LarguraTela);
            Setar_Vidas(LarguraTela, AlturaTela, vidas);

            this.nave = new NaveJogador(LarguraTela, AlturaTela);

        }

        public void Setar_Vidas(int LarguraTela, int AlturaTela, List<PictureBox> vidas)
        {
            int conta = 3;

            for(int j = 0; j < conta; j++)
            {

                PictureBox fotos = new PictureBox();
                fotos.Size = new Size(20, 20);
                fotos.Visible = true;
                fotos.BackColor = Color.Transparent;
                fotos.Location = new Point(LarguraTela - 60 - (30 * j), AlturaTela - 440);
                fotos.BackgroundImage = Properties.Resources.player;
                fotos.BackgroundImageLayout = ImageLayout.Stretch;

                vidas.Add(fotos);


            }
        }

        public void Criar_Aliens(int LarguraTela)
        {
            int colunas = 13;
            int linhas = 4;

            int gapX = 50;
            int gapY = 50;

            int larguraAliens = colunas * gapX;
            int margem = (LarguraTela - larguraAliens) / 2;


            for (int i = 0 ; i < linhas; i++)
            {
                string type = "";
                if ((i == 0)) type = "green";
                else if (i == 1) type = "yellow";
                else if ((i == 2) || (i == 3)) type = "red";

                for (int j = 0; j < colunas; j++)
                {

                    Alien alien = new Alien(type);
                    alien.Get_Foto().Location = new System.Drawing.Point(margem + (j * gapX), gapY + (i * gapY));
                    aliens.Add(alien);

                }
            }
        }

        public void Verificar_Aliens()
        {
            if(aliens.Count == 0)
            {
                jogoAtivo = false;
            }
        }

        public bool Status_Jogo()
        {
            return this.jogoAtivo;
        }


    }
}
