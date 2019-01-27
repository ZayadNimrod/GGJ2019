using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RogueSharp.Random;

namespace HHRL {
    interface Enemy {
        int X();
        int Y();
        int Stealth();

        void ResolveTurn(IRandom rng, Player player, HouseFloor floor);

    }

    public class Ghost : Enemy {
        private int x, y;
        const int blockingLight = 6;
        const int damagingLight = 3;
        int hp = 400;
        private int targX = 0, targY = 0;
        public Ghost(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public int X() { return x; }
        public int Y() { return y; }

        public int Stealth() { return 3; }

        public void ResolveTurn(IRandom rng, Player player, HouseFloor floor) {

            if (rng.Next(0, 4) == 0) {
                //only acts every 10 moves or so
                //can we see the player?
                floor.ComputeFov(x, y, 100, false);
                if (floor.IsInFov(player.x, player.y)) {
                    targX = player.X();
                    targY = player.Y();
                } else {
                    //find new target

                    if (targX == X() && targY == Y() || targX == 0 && targY == 0) {
                        //get new target
                        bool okPos = false;
                        while (!okPos) {
                            okPos = true;
                            targX = rng.Next(0, floor.Width - 1);
                            targY = rng.Next(0, floor.Height - 1);

                            if (floor.GetCell(targX, targY).IsWalkable == false) { okPos = false; }

                        }
                    }

                }
                //before we get a path, we must set tiles of light 6 or above as impassable
                List<Tuple<int, int>> bright = new List<Tuple<int, int>>();
                for (int i = 0; i < floor.Width; i++) {
                    for (int j = 0; j < floor.Height; j++) {
                        if (floor.light[i, j] >= blockingLight && floor.GetCell(i, j).IsWalkable) {
                            floor.SetCellProperties(i, j, floor.GetCell(i, j).IsTransparent, false);
                            bright.Add(new Tuple<int, int>(i, j));
                        }
                    }
                }


                PathFinder pathFinder = new PathFinder(floor);
                Path path = null;


                try {
                    path = pathFinder.ShortestPath(floor.GetCell(x, y), floor.GetCell(targX, targY));





                    if (path != null) {
                        try {
                            if (path.Steps.Count() > 1) {
                                x = path.Steps.ElementAt(1).X;
                                y = path.Steps.ElementAt(1).Y;
                            }
                        } catch (NoMoreStepsException) {

                        }
                    }

                } catch (PathNotFoundException) {
                    //break off of path
                    if (floor.light[X(), Y()] >= blockingLight) {
                        //first, if we are in a high-light tile, move to a lower light tile

                        int lightHere = floor.light[X(), Y()];
                        List<ICell> valid = new List<ICell>();
                        if (floor.light[X() + 1, Y()] <= lightHere) { valid.Add(floor.GetCell(X() + 1, Y())); }
                        if (floor.light[X() - 1, Y()] <= lightHere) { valid.Add(floor.GetCell(X() - 1, Y())); }
                        if (floor.light[X(), Y() + 1] <= lightHere) { valid.Add(floor.GetCell(X(), Y() + 1)); }
                        if (floor.light[X(), Y() - 1] <= lightHere) { valid.Add(floor.GetCell(X(), Y() - 1)); }
                        ICell goingTo = valid[rng.Next(0, valid.Count() - 1)];
                        x = goingTo.X;
                        y = goingTo.Y;

                    } else {


                        //if we are not in a high light tile anymore, find a new place to go
                        bool okPos = false;
                        while (!okPos) {
                            okPos = true;
                            targX = rng.Next(0, floor.Width - 1);
                            targY = rng.Next(0, floor.Height - 1);

                            if (floor.GetCell(targX, targY).IsWalkable == false) { okPos = false; }

                        }
                    }


                }

                //undo the blocking
                foreach (Tuple<int, int> c in bright) {
                    floor.SetCellProperties(c.Item1, c.Item2, floor.GetCell(c.Item1, c.Item2).IsTransparent, true);
                }






            }
            if (floor.light[X(), Y()] >= damagingLight) {
                hp--;
                
            }




        }

        internal bool Dead() {
            return hp <= 0;
        }
    }


    public class Poltergiest : Enemy {
        private int x, y;
        private int targX = 0, targY = 0;
        public Poltergiest(int x, int y) {
            this.x = x;
            this.y = y;
        }

        private int turnTimer = 0;

        public int X() { return x; }
        public int Y() { return y; }

        public int Stealth() { return 4; }

        public void ResolveTurn(IRandom rng, Player player, HouseFloor floor) {
            if (turnTimer == 0) {
                if (rng.Next(0, 4) == 0) {
                    floor.ComputeFov(x, y, 100, false);

                    //find new target

                    if (targX == X() && targY == Y() || targX == 0 && targY == 0) {
                        //get new target
                        bool okPos = false;
                        while (!okPos) {
                            okPos = true;
                            targX = rng.Next(0, floor.Width - 1);
                            targY = rng.Next(0, floor.Height - 1);

                            if (floor.GetCell(targX, targY).IsWalkable == false) { okPos = false; }

                        }
                    }






                    PathFinder pathFinder = new PathFinder(floor);
                    Path path = null;


                    try {
                        path = pathFinder.ShortestPath(floor.GetCell(x, y), floor.GetCell(targX, targY));



                        if (path != null) {
                            try {
                                if (path.Steps.Count() > 1) {
                                    //dont walk into the player willingly
                                    if (!(player.X() == path.Steps.ElementAt(1).X && player.Y() == path.Steps.ElementAt(1).Y)) {

                                        x = path.Steps.ElementAt(1).X;
                                        y = path.Steps.ElementAt(1).Y;
                                    }
                                }
                            } catch (NoMoreStepsException) {

                            }
                        }

                    } catch (PathNotFoundException) {
                        //break off of path
                        bool okPos = false;
                        while (!okPos) {
                            okPos = true;
                            targX = rng.Next(0, floor.Width - 1);
                            targY = rng.Next(0, floor.Height - 1);

                            if (floor.GetCell(targX, targY).IsWalkable == false) { okPos = false; }

                        }


                    }

                }
            } else { turnTimer--; }
            //snuff out lights
            foreach (LightSource k in floor.lamps) {
                if (k is LampGeneric l) {
                    if (l.X() == X()) {
                        if (Math.Abs(l.Y() - Y()) == 1) {
                            l.Unlight();
                        }
                    } else if (Math.Abs(l.X() - X()) == 1) {
                        if (l.Y() == Y()) {
                            l.Unlight();
                        }
                    }
                }
            }









        }

        internal void RandomTeleport(IRandom rng, HouseFloor houseFloor) {
            x = 0;
            y = 0;
            while (!houseFloor.GetCell(x, y).IsWalkable) {
                x = rng.Next(0, houseFloor.Width - 1);
                y = rng.Next(0, houseFloor.Height - 1);
            }
        }

        internal void TeleportToGhost(List<Enemy> enemies) {
            foreach (Enemy e in enemies) {
                if (e is Ghost g) {
                    x = g.X();
                    y = g.Y();
                }
            }
        }
    }



}


