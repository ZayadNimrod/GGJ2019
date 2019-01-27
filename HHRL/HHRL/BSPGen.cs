using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp.Random;


namespace HHRL {
    static class BSPGen {

        public static List<Room> GenStructure(int w, int h, IRandom rng, int depth, float crazyness) {
            BSPBlock topLevel = new BSPBlock(0, 0, w, h, rng, depth, crazyness, true);
            List<Room> ret = topLevel.GetRooms();

            return ret;
        }



    }

    class BSPBlock {
        public int x, y, width, height;

        public BSPBlock left = null;
        public BSPBlock right = null;

        Room.RoomType myType;

        public BSPBlock(int x, int y, int w, int h, IRandom rng, int depth, float equality, bool lengthWise) {
            this.x = x;
            this.y = y;
            this.width = w;
            this.height = h;

            myType = new Room.RoomType[] { Room.RoomType.CELLAR, Room.RoomType.CELLAR, Room.RoomType.HABIT, Room.RoomType.HABIT, Room.RoomType.WOODEN , Room.RoomType.HABITFANCY, Room.RoomType.GRAND }[rng.Next(0,6)];

            if (depth != 0) {
                if (lengthWise) {
                    int splitPoint = rng.Next((int)Math.Round(height * equality), (int)Math.Round(height - height * equality));
                    left = new BSPBlock(x, y, width, splitPoint, rng, depth - 1, equality, !lengthWise);
                    right = new BSPBlock(x, y + splitPoint + 1, width, height - splitPoint - 1, rng, depth - 1, equality, !lengthWise);
                } else {
                    int splitPoint = rng.Next((int)Math.Round(width * equality), (int)Math.Round(width - width * equality));
                    left = new BSPBlock(x, y, splitPoint, height, rng, depth - 1, equality, !lengthWise);
                    right = new BSPBlock(x + splitPoint + 1, y, width - splitPoint - 1, height, rng, depth - 1, equality, !lengthWise);
                }
            }




        }

        public List<Room> GetRooms() {
            if (left != null) {
                List<Room> r = left.GetRooms();
                r.AddRange(right.GetRooms());
                return r;

            }


            return new List<Room>() {
                new Room(this.x, this.y, this.width, this.height,myType)
            };
        }

    }


}
