using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp.Random;

namespace HHRL {
    class Game {

        public enum Actions {
            MoveN,
            MoveNE,
            MoveE,
            MoveSE,
            MoveS,
            MoveSW,
            MoveW,
            MoveNW,

            Climb,
            Descend,

            Wait

        }



        private Player player;
        private IRandom rng;

        private HouseFloor[] floors;
        private int currentFloor;

        private List<Enemy> enemies = new List<Enemy>();
        private int losingTimer = 0;
        private int winTimer = 0;

        public static bool takingInput;
        public static RLKeyPress lastKey = null;


        public Game(IRandom rng) {
            takingInput = true;
            this.rng = rng;
            MapGenerator mapGen = new MapGenerator(100, 70);

            floors = new HouseFloor[1];
            floors[0] = mapGen.CreateMap(rng, 1, 2);
            currentFloor = 0;

            int x = 0;
            int y = 0;

            while (floors[currentFloor].GetCell(x, y).IsWalkable == false) {
                x = rng.Next(0, floors[currentFloor].Width-1);
                y = rng.Next(0, floors[currentFloor].Height-1);
            }
            player = new Player(x, y);

            floors[0].lamps.Add(player);


            x = 0;
            y = 0;
            while (floors[currentFloor].GetCell(x, y).IsWalkable == false) {
                x = rng.Next(0, floors[currentFloor].Width-1);
                y = rng.Next(0, floors[currentFloor].Height-1);
            }
            Ghost ghost = new Ghost(x, y);

            enemies.Add(ghost);

            for (int i = 0; i < 2; i++) {
                x = 0;
                y = 0;
                while (floors[currentFloor].GetCell(x, y).IsWalkable == false) {
                    x = rng.Next(0, floors[currentFloor].Width-1);
                    y = rng.Next(0, floors[currentFloor].Height-1);
                }
                Poltergiest p = new Poltergiest(x, y);

                enemies.Add(p);
            }


        }


        public PlayState GameUpdate(RLRootConsole rootConsole) {
            if (takingInput) {
                lastKey = rootConsole.Keyboard.GetKeyPress();

                takingInput = false;
            }
            

            if (losingTimer == 0 && winTimer == 0) {
                if (lastKey != null) {
                    //process player input
                    Actions action = Actions.Wait;

                    if (lastKey.Key == RLKey.Keypad8 || lastKey.Key == RLKey.Up) { action = Actions.MoveN; } else if (lastKey.Key == RLKey.Keypad9) { action = Actions.MoveNE; } else if (lastKey.Key == RLKey.Keypad6 || lastKey.Key == RLKey.Right) { action = Actions.MoveE; } else if (lastKey.Key == RLKey.Keypad3) { action = Actions.MoveSE; } else if (lastKey.Key == RLKey.Keypad2 || lastKey.Key == RLKey.Down) { action = Actions.MoveS; } else if (lastKey.Key == RLKey.Keypad1) { action = Actions.MoveSW; } else if (lastKey.Key == RLKey.Keypad4 || lastKey.Key == RLKey.Left) { action = Actions.MoveW; } else if (lastKey.Key == RLKey.Keypad7) { action = Actions.MoveNW; } else if (lastKey.Key == RLKey.Period || lastKey.Key == RLKey.Keypad5) { action = Actions.Wait; }


                    List<Enemy> markedForDeath = new List<Enemy>();
                    if (action == Actions.MoveN || action == Actions.MoveNE || action == Actions.MoveE || action == Actions.MoveSE || action == Actions.MoveS || action == Actions.MoveSW || action == Actions.MoveW || action == Actions.MoveNW) {
                        int xOff = 0;
                        int yOff = 0;
                        if (action == Actions.MoveN || action == Actions.MoveNE || action == Actions.MoveNW) {
                            yOff--;
                        } else if (action == Actions.MoveS || action == Actions.MoveSE || action == Actions.MoveSW) {
                            yOff++;
                        }

                        if (action == Actions.MoveW || action == Actions.MoveNW || action == Actions.MoveSW) {
                            xOff--;
                        } else if (action == Actions.MoveE || action == Actions.MoveSE || action == Actions.MoveNE) {
                            xOff++;
                        }

                        if (!(player.x + xOff < 0 || player.x + xOff >= floors[currentFloor].Width || player.y + yOff < 0 || player.y + yOff >= floors[currentFloor].Height)) {
                            //check if the player is about to walk into a Poltergiest


                            foreach (Enemy e in enemies) {
                                if (e is Poltergiest p) {
                                    if (e.X() == player.x + xOff && e.Y() == player.y + yOff) {
                                        //if player can see the poltergiest, kill it with no consequence
                                        if (floors[currentFloor].light[e.X(), e.Y()] >= e.Stealth()) {
                                            //this means nothing happens?
                                            //maybe a small animation is in order?

                                            Enemy ghost = null;
                                            foreach (Enemy f in enemies) {
                                                if (f is Ghost) { ghost = f; }
                                            }
                                            //if the ghost is alive, teleport to it and wait for a few turns before moving again
                                            if (ghost != null) {
                                                p.TeleportToGhost(enemies);
                                            } else {
                                                //else, it dissapears
                                                markedForDeath.Add(e);
                                            }



                                        } else {
                                            //otherwise, the player is damaged by the poltergiest
                                            player.hp--;

                                            //poltergiest then teleports to the ghost if it can, otherwise random

                                            Enemy ghost = null;
                                            foreach (Enemy f in enemies) {
                                                if (f is Ghost) { ghost = f; }
                                            }
                                            //if the ghost is alive, teleport to it and wait for a few turns before moving again
                                            if (ghost != null) {
                                                p.TeleportToGhost(enemies);
                                            } else {
                                                //else, it dissapears
                                                p.RandomTeleport(rng, floors[currentFloor]);
                                            }
                                        }


                                    }
                                }
                            }


                            if (floors[currentFloor].IsWalkable(player.x + xOff, player.y + yOff)) {
                                player.x += xOff;
                                player.y += yOff;
                            }

                        }
                    }

                    //light lamps
                    foreach (LightSource k in floors[currentFloor].lamps) {
                        if (k is LampGeneric l) {
                            if (l.X() == player.X()) {
                                if (Math.Abs(l.Y() - player.Y()) <= 1) {
                                    l.Light();
                                }
                            } else if (Math.Abs(l.X() - player.X()) <= 1) {
                                if (l.Y() == player.Y()) {
                                    l.Light();
                                }
                            }
                        }
                    }



                    Ghost theGhost = null;
                    //process enemies
                    foreach (Enemy e in enemies) {
                        e.ResolveTurn(rng, player, floors[currentFloor]);

                        if (e is Ghost g) {
                            if (g.X() == player.X() && g.Y() == player.Y()) {

                                player.hp = 0;


                            }
                            theGhost = g;



                        }
                    }


                    //if ghost is dead remove it
                    if (theGhost != null) {
                        if (theGhost.Dead()) {
                            enemies.Remove(theGhost);
                        }
                    }
                    foreach (Enemy enemyToKill in markedForDeath) {
                        enemies.Remove(enemyToKill);
                    }




                    //process traps
                    //looks like I wont have time for this
                    //shame


                    //process other effects
                    floors[currentFloor].CalculateLighting();

                    //process sight
                    floors[currentFloor].UpdatePlayerFieldOfView(player);

                    //check for death
                    if (player.hp <= 0) {
                        losingTimer = 1;
                    }


                    //check for victory

                    if (enemies.Count() == 0) {
                        //go to victory screen
                        winTimer = 1;
                    }




                }
            } else {

                if (losingTimer >= 255) {
                    return PlayState.INMENU;
                }
                if (winTimer >= 255) {
                    return PlayState.VICTORY;
                }

            }

            lastKey = null;
            takingInput = true;
            return PlayState.PLAYING;


        }


        public void Render(RLRootConsole console) {
            if (losingTimer == 0 && winTimer == 0) {
                floors[currentFloor].Draw(console);
                if (player.hp == 3) {
                    console.Set(player.x, player.y, Colors.white, Colors.black, '@');
                } else if (player.hp == 2) {
                    console.Set(player.x, player.y, (Colors.red + Colors.white) / 2, Colors.black, '@');
                } else {
                    console.Set(player.x, player.y, Colors.red, Colors.black, '@');
                }
                foreach (Enemy e in enemies) {
                    int lightHere = floors[currentFloor].light[e.X(), e.Y()];
                    if ((floors[currentFloor].IsInFov(e.X(), e.Y()) && lightHere >= e.Stealth()) || (e.X() == player.X() && e.Y() == player.Y())) {
                        RLColor darkenBy = new RLColor(8, 8, 8) * (10 - lightHere);
                        if (e is Ghost) {
                            console.Set(e.X(), e.Y(), Colors.blue - darkenBy, Colors.black, 'G');
                        } else if (e is Poltergiest) {
                            console.Set(e.X(), e.Y(), Colors.pink - darkenBy, Colors.black, 'P');
                        }
                    }

                    //if (e is Ghost) {
                    //    console.Set(e.X(), e.Y(), Colors.blue, Colors.black, 'G');
                    //}
                }
            } else {
                if (losingTimer > 0) {
                    losingTimer++;
                    for (int i = 0; i < losingTimer; i++) {
                        for (int j = 0; j + i < losingTimer; j++) {
                            if (i < console.Width || j < console.Height) {
                                console.Set(i, j, Colors.black, Colors.black, ' ');
                            }
                        }
                    }
                } else if (winTimer > 0) {
                    winTimer++;


                    console.Set(0, 0, console.Width, winTimer, Colors.black, Colors.black, ' ');




                }


            }
        }
    }
}
