using SpaceInvaders.Properties;
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
    //----- classe que define os inimigos ----//
    internal class Alien
    {
        //----- atributos -----//
        //-- sao mortos com 1 tiro--//
        private int vida = 1;
        private string type = "";
        private PictureBox foto_ = null;

        //------ construtor --------//
        public Alien(string type)
        {
            Set_type(type);
            Set_Foto();
        }

        //--------- setters and getters ----------//

        public void Set_Vida(int vida)
        {
            //------ a vida precisa ser posivita ---------//
            if (vida >= 0) this.vida = vida;
        }
        public void Set_Foto()
        {
            //------------ bem similar a definicao de imagem do jogador ---------//
            this.foto_ = new PictureBox();
            this.foto_.Size = new Size(30, 30);
            this.foto_.Visible = true;
            this.foto_.Location = new Point(0, 0);
            this.foto_.BackColor = Color.Transparent;

            //------------ muda aqui, porque depende do tipo pra definir a imagem -----------//
            if (Get_type() == "red")
            {
                this.foto_.BackgroundImage = Properties.Resources.red;
            }
            else if (Get_type() == "green")
            {
                this.foto_.BackgroundImage = Properties.Resources.green;
            }
            else if (Get_type() == "yellow")
            {
                this.foto_.BackgroundImage = Properties.Resources.yellow;
            }

            //------ como a imagem aparece -----//
            this.foto_.BackgroundImageLayout = ImageLayout.Stretch;
        }
        public void Set_type(string type)
        {
            //------- tem 3 tipos de alien, aq define qual --------//
            if ((type == "yellow") || (type == "red") || (type == "green"))
            {
                this.type = type;
            }
        }
        public int Get_Vida() => this.vida;
        public string Get_type() => this.type;
        public PictureBox Get_Foto() => this.foto_;
        
        //-------------- metodos ----------//

        //--------- define o dano que o alien tomou pra tirar ele da tela -------//
        public void Tomar_Dano()
        {
            Set_Vida(Get_Vida() - 1);
            this.foto_.Visible = false;
        }



    }
}
