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
        //----- o jogador precisa de atributos ----//
        private int vida = 3;
        private int pontuacao = 0;
        private PictureBox foto = null;
        private Projetil tiroAtual = null;

        //------ construtor -----//
        public NaveJogador(int LarguraTela, int AlturaTela)
        {
            //------ quando cria a nave precisa definir a foto/vida/pontuacao do player ---------//
            Set_Foto(LarguraTela, AlturaTela);
            Set_Vida(3);
            Set_Pontuacao(0);
        }

        //----------  getters & setters -------------//
        public void Set_Vida(int vida)
        {
            //---- vida precisa ser positiva ---//
            if (vida > 0) this.vida = vida;
        }
        public void Set_Pontuacao(int pontos) => this.pontuacao = pontos;
        public void Set_Foto(int LarguraTela, int AlturaTela)
        {
            //------ atributos da foto: cria uma picture box, define tamanho, visibilidade, deixa o fundo transparente -----//
            //------ define a localizaçao, define a foto (q vem do repositorio), e a imagem esta com, stretch ------//
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

        //-------- quando toma dano diminui 1 de vida --------//
        public void Tomar_Dano() => Set_Vida(Get_Vida() - 1);

        //----------- metodos -------------//
        public void Movimentar(KeyEventArgs Tecla, int Borda)
        {
            //----- define a velocidade, que é quanto a imagem mexe pro lado ------//
            int Velocidade = 10;


            //------- usa switch case para movimento do player, que só se mexe na horizontal ------//
            switch (Tecla.KeyCode)
            {
                case Keys.Left:
                    //------ enquanto estiver na tela, ele muda a imagem pra 10 pra esquerda -----//
                    if (Get_Foto().Location.X >= 0)
                        Get_Foto().Location = new Point(Get_Foto().Location.X - Velocidade, Get_Foto().Location.Y);
                    break;

                case Keys.Right:
                    //------ enquanto estiver na tela, ele muda a imagem pra 10 pra direita -----//
                    if (Get_Foto().Location.X <= Borda)
                        Get_Foto().Location = new Point(Get_Foto().Location.X + Velocidade, Get_Foto().Location.Y);
                    break;

                case Keys.Space:
                    //-------- tambem define o tiro do espaço --------//

                    if (this.tiroAtual == null)
                    {
                        //------ cria um novo projetil, mandando a velocidade e de onde a nave ta atirando --------//
                        Projetil proj = new Projetil(40, Get_Foto().Location.X, Get_Foto().Location.Y);
                        Set_Tiro(proj);
                    }
                    break;
            }
        }
    }
}
