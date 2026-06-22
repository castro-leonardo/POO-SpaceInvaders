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
    internal class Projetil
    {
        private PictureBox foto = null;
        private int Velocidade = 0;

        public Projetil(int Velocidade, int PosX, int PosY)
        {
            Set_Velocidade(Velocidade);
            Set_Tiro(PosX, PosY);
        }

        //getters e setters
        public void Set_Velocidade(int Velocidade) => this.Velocidade = Velocidade;


        public void Set_Tiro(int NaveX, int NaveY)
        {

            this.foto = new PictureBox();
            this.foto.Size = new Size(10, 20);
            this.foto.BackColor = Color.Yellow;


            this.foto.Location = new Point(NaveX + 10, NaveY - 10);

        }

        public void Set_Foto(PictureBox foto) => this.foto = foto;

        public int Get_Velocidade() => this.Velocidade;

        public PictureBox Get_Foto() => this.foto;

        public void Set_PosX(int X) => this.foto.Location = new Point(X, Get_PosY());

        public void Set_PosY(int Y) => this.foto.Location = new Point(Get_PosX(), Y);

        public int Get_PosX() => this.foto.Location.X;

        public int Get_PosY() => this.foto.Location.Y;

        //fim getters e setter

        //metodos
        public bool Colidiu_Com(PictureBox Objeto)
        {
            if (this.foto.Bounds.IntersectsWith(Objeto.Bounds) && Objeto.Visible == true)
            {
                Objeto.Visible = false;
                return true;
            }
            return false;
        }
      
        public void Atira()
        {
            while (Get_PosY() > 0)
            {
                Set_PosY(Get_PosY() + Get_Velocidade());
            }

            Get_Foto().Visible = false;
        }
    }
}