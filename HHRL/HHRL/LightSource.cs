using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHRL {
    public interface LightSource {
        int X();
        int Y();

        int GetStrength();


    }

    abstract public class LampGeneric : LightSource {
        protected int str;
        protected bool lit;

        public int x, y;

        public char symbol;


        public int GetStrength() {
            if (lit) { return str; } else { return 0; }
        }

        public int X() { return x; }
        public int Y() { return y; }

        public void Light() { lit = true; }
        public void Unlight() { lit = false; }
    }


    public class Candle : LampGeneric {
        public Candle(int x, int y) {
            this.x = x;
            this.y = y;
            symbol = 'i';
            lit = false;
            str = 4;
        }
    }

    public class Torch : LampGeneric {
        public Torch(int x, int y) {
            this.x = x;
            this.y = y;
            symbol = '!';
            lit = false;
            str =6;
        }
    }

    public class Lamp : LampGeneric {
        public Lamp(int x, int y) {
            this.x = x;
            this.y = y;
            symbol = (char)140;
            lit = false;
            str = 8;
        }
    }
    public class Brazier : LampGeneric {
        public Brazier(int x, int y) {
            this.x = x;
            this.y = y;
            symbol = 'U';
            lit = false;
            str = 10;
        }
    }






}
