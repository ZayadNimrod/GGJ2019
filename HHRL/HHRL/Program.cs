using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp.Random;


namespace HHRL {

    enum PlayState {
        INMENU,
        PLAYING,
        VICTORY
    }


    class Program {
        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;

        private static RLRootConsole _rootConsole;

        private static PlayState playState = PlayState.INMENU;

        private static Game currentGame;

        // Singleton of IRandom used throughout the game when generating random numbers
        private static IRandom rng;


        public static void Main() {
            // This must be the exact name of the bitmap font file we are using or it will error.
            string fontFileName = "terminal8x8.png";
            // The title will appear at the top of the console window
            string consoleTitle = "The House";
            // Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);
            rng = new DotNetRandom();

            playState = PlayState.INMENU;



            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;
            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;
            // Begin RLNET's game loop
            _rootConsole.Run();
        }

        // Event handler for RLNET's Update event
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e) {




            if (playState == PlayState.INMENU) {
                if (_rootConsole.Keyboard.GetKeyPress() != null) {
                    currentGame = new Game(rng);
                    playState = PlayState.PLAYING;
                }
            } else if (playState == PlayState.PLAYING) {
                playState = currentGame.GameUpdate(_rootConsole);
            } else if (playState == PlayState.VICTORY) {
                if (_rootConsole.Keyboard.GetKeyPress() != null) {
                    playState = PlayState.INMENU;
                }
            }


        }

        // Event handler for RLNET's Render event
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e) {
            // Tell RLNET to draw the console that we set
            if (playState == PlayState.INMENU) {
                RenderMenu();
            } else if (playState == PlayState.PLAYING) {
                currentGame.Render(_rootConsole);
            } else if (playState == PlayState.VICTORY) {
                RenderWinScreen();
            }

            _rootConsole.Draw();
        }

        private static void RenderWinScreen() {
            _rootConsole.Clear(' ', Colors.black, Colors.black);
            int leftOffset = 23;
            int upOffset = 5;


            //V
            _rootConsole.Set(leftOffset + 1, upOffset, 2, 5, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset, 2, 5, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset + 5, 4, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 3, upOffset + 6, 2, 1, Colors.red, Colors.red, ' ');


            //I
            leftOffset += 8;
            _rootConsole.Set(leftOffset + 2, upOffset, 4, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset + 6, 4, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 3, upOffset, 2, 6, Colors.red, Colors.red, ' ');

            //C
            leftOffset += 8;
            _rootConsole.Set(leftOffset, upOffset + 2, 2, 3, Colors.red, Colors.red, ' ');

            _rootConsole.Set(leftOffset + 1, upOffset + 5, 2, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset + 6, 4, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset + 5, 2, 1, Colors.red, Colors.red, ' ');

            _rootConsole.Set(leftOffset + 1, upOffset + 1, 2, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset, 4, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset + 1, 2, 1, Colors.red, Colors.red, ' ');


            //T
            leftOffset += 8;
            _rootConsole.Set(leftOffset + 1, upOffset, 6, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset + 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 6, upOffset + 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 3, upOffset, 2, 6, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset + 6, 4, 1, Colors.red, Colors.red, ' ');

            //O
            leftOffset += 8;
            _rootConsole.Set(leftOffset, upOffset + 2, 2, 3, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset + 2, 2, 3, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset, 3, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset + 6, 3, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset + 1, 2, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 4, upOffset + 1, 2, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset + 5, 2, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 4, upOffset + 5, 2, 1, Colors.red, Colors.red, ' ');

            //R
            leftOffset += 8;
            _rootConsole.Set(leftOffset, upOffset, 6, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset, 2, 7, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset, upOffset + 6, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset + 1, 2, 2, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 3, upOffset + 3, 2, 2, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset, upOffset + 6, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 6, upOffset + 6, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset + 6, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 4, upOffset + 5, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset + 5, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 4, upOffset + 3, Colors.red, Colors.red, ' ');


            //Y
            leftOffset += 8;

            _rootConsole.Set(leftOffset + 1, upOffset, 2, 3, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset, 2, 3, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset + 3, 4, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 3, upOffset + 3, 2, 3, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset + 6, 4, 1, Colors.red, Colors.red, ' ');


            _rootConsole.Print(22, 30, "You have rid the house of the ghosts and reclaimed your home!", RLColor.Gray);

            _rootConsole.Print(38, 40, "Press any key to return...", RLColor.White);
        }

        private static void RenderMenu() {
            _rootConsole.Clear(' ', Colors.black, Colors.black);


            int leftOffset = 17;
            int upOffset = 5;

            //T
            _rootConsole.Set(leftOffset + 1, upOffset, 6, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset + 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 6, upOffset + 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 3, upOffset, 2, 6, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset + 6, 4, 1, Colors.red, Colors.red, ' ');

            //H
            leftOffset += 8;
            _rootConsole.Set(leftOffset + 1, upOffset + 3, 6, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset, 2, 7, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset, 2, 7, Colors.red, Colors.red, ' ');


            //E
            leftOffset += 8;
            _rootConsole.Set(leftOffset, upOffset, 7, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset, upOffset + 6, 7, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset, 2, 7, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 4, upOffset + 2, 1, 3, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 6, upOffset + 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 6, upOffset + 5, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 3, upOffset + 3, Colors.red, Colors.red, ' ');

            leftOffset += 8;


            leftOffset += 8;
            //H
            _rootConsole.Set(leftOffset + 1, upOffset + 3, 6, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset, 2, 7, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset, 2, 7, Colors.red, Colors.red, ' ');


            leftOffset += 8;
            //O
            _rootConsole.Set(leftOffset, upOffset + 2, 2, 3, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset + 2, 2, 3, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset, 3, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset + 6, 3, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset + 1, 2, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 4, upOffset + 1, 2, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset + 5, 2, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 4, upOffset + 5, 2, 1, Colors.red, Colors.red, ' ');



            leftOffset += 8;
            //U
            _rootConsole.Set(leftOffset + 1, upOffset, 2, 7, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 5, upOffset, 2, 7, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset + 6, 6, 1, Colors.red, Colors.red, ' ');



            leftOffset += 8;
            //S
            _rootConsole.Set(leftOffset + 1, upOffset, 4, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset + 6, 4, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset, upOffset + 1, 2, 2, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 4, upOffset + 1, 2, 1, Colors.red, Colors.red, ' ');

            _rootConsole.Set(leftOffset + 4, upOffset + 4, 2, 2, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset, upOffset + 5, 2, 1, Colors.red, Colors.red, ' ');

            _rootConsole.Set(leftOffset + 3, upOffset + 3, 2, 2, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 2, upOffset + 2, 1, 2, Colors.red, Colors.red, ' ');

            leftOffset += 8;
            //E
            _rootConsole.Set(leftOffset, upOffset, 7, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset, upOffset + 6, 7, 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 1, upOffset, 2, 7, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 4, upOffset + 2, 1, 3, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 6, upOffset + 1, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 6, upOffset + 5, Colors.red, Colors.red, ' ');
            _rootConsole.Set(leftOffset + 3, upOffset + 3, Colors.red, Colors.red, ' ');


            _rootConsole.Print(38, 40, "Press any key to enter...", RLColor.White);
        }
    }
}
