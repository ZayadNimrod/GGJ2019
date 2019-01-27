using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHRL {
    public class Player:LightSource {
        public int x;
        public int y;

        public int hp;


        public Player(int x, int y) {
            this.x = x;
            this.y = y;
            this.hp = 3;
        }


        public int GetStrength() {
            return 3;
        }

        public int X() {
            return x;
        }

        public int Y() {
            return y;
        }
    }
}
