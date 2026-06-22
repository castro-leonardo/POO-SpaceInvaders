using SpaceInvaders.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class Alien
    {
        private int vida = 1;
        private string type = "";
        private PictureBox foto_ = null;

        public Alien(string type)
        {
            Set_type(type);
            Set_Foto();
        }

        //Setters and getters

        public void Set_Vida(int vida)
        {
            if (vida >= 0) this.vida = vida;
        }

        public int Get_Vida() => this.vida;

        public void Set_type(string type)
        {
            if ((type == "yellow") || (type == "red") || (type == "green"))
            {
                this.type = type;
            }
        }

        public string Get_type() => this.type;

        public void Set_Foto()
        {
            this.foto_ = new PictureBox();
            this.foto_.Size = new Size(30, 30);
            this.foto_.Visible = true;
            this.foto_.Location = new Point(0, 0);
            this.foto_.BackColor = Color.Transparent;

            if (Get_type() == "red") this.foto_.BackgroundImage = Properties.Resources.red;
            else if (Get_type() == "green") this.foto_.BackgroundImage = Properties.Resources.green;
            else if (Get_type() == "yellow") this.foto_.BackgroundImage = Properties.Resources.yellow;

            this.foto_.BackgroundImageLayout = ImageLayout.Stretch;
        }

        public PictureBox Get_Foto() => this.foto_;

        // metodos

        public void Tomar_Dano()
        {
            Set_Vida(Get_Vida() - 1);
            this.foto_.Visible = false;
        }



    }
}
