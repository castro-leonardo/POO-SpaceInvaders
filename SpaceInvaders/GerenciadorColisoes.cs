using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class GerenciadorColisoes
    {
        private bool colisao = false;
        public GerenciadorColisoes() { }

        public bool Get_Colisao() => this.colisao;
        public void Set_Colisao(bool colisao) => this.colisao = colisao;

        //Possiveis colisoes

        public bool Verificar_Colisao(List<Alien> aliens, NaveJogador nave)
        {
            foreach (Alien alien in aliens)
            {
                if (alien.Get_Foto().Bounds.IntersectsWith(nave.Get_Foto().Bounds))
                {
                    Set_Colisao(true);
                    nave.Tomar_Dano();
                    return true;
                }

            }

            Set_Colisao(false);
            return false;
        }


    }
}
