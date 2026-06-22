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
        //--------- atributos --------------//
        //-------- para o jogo precisamos de uma nave e uma lista de aliens, alem de algo pra ter track se o jogo ta ativo ou nao ------------//
        //-------- nessa classe tambem controlamos a picturebox das vidas, como sao 3, é uma lista ---------//
        private NaveJogador nave;
        private List<Alien> aliens;
        private bool jogoAtivo = false;
        private List<PictureBox> vidas;

        //--------- construtor vazio ----------//
        public Jogo(){ }

        //----------- setters e getters -------------//

        public bool Get_JogoAtivo() => this.jogoAtivo;
        public void Set_JogoAtivo(bool jogoAtivo) => this.jogoAtivo = jogoAtivo;
        public NaveJogador Get_Nave() => this.nave;
        public void Set_Nave(NaveJogador nave) => this.nave = nave;
        public List<Alien> Get_Aliens() => this.aliens;
        public void Set_Aliens(List<Alien> aliens) => this.aliens = aliens;
        public List<PictureBox> Get_Vidas() => this.vidas;

        //------------ metodos -------------------//

        public void Comecar_Jogo(int LarguraTela, int AlturaTela)
        {
            //------ qnd comeca o jogo precisa do bool do jogo indicando que ta ativo -----------//
            //------ alem de inicializar os atributos que foram definidos ----------------//
            //------ precisamos tambem colocar os aliens/vidas na tela
            Set_JogoAtivo(true);
            this.aliens = new List<Alien>();
            this.vidas = new List<PictureBox>();
            this.nave = new NaveJogador(LarguraTela, AlturaTela);
            Criar_Aliens(LarguraTela);
            Setar_Vidas(LarguraTela, AlturaTela, vidas);

        }

        public void Setar_Vidas(int LarguraTela, int AlturaTela, List<PictureBox> vidas)
        {
            //----------- começamos com 3 vidas, entao para cada vida "desenhamos" uma picture box ----------//
            //----------- com a representacao de vidas ---------//

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
            //-------- para criar os aliens, definimos um numero inicial de 13 aliens por linha, temos 4 linhas, seguindo os jogos classicos --------//
            //-------- tb foi definido o espaço entre eles e a largura total, alem de precisar de uma margem --------//
            int colunas = 13;
            int linhas = 4;
            int gapX = 50;
            int gapY = 50;
            int larguraAliens = colunas * gapX;
            int margem = (LarguraTela - larguraAliens) / 2;

            //--------- adiciona os aliens na lista e "desenha" eles na tela para cada linha/coluna que temos, considerando tipo -----------//
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

        //---------- metodo apenas para verificar se ainda tem aliens, enqt tiver, ainda tem jogo ----------//
        public void Verificar_Aliens()
        {
            if(aliens.Count == 0)
            {
                jogoAtivo = false;
            }
        }

    }
}
