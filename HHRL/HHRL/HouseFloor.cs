using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;

namespace HHRL {
    public enum TileTypes {
        STONEFLOOR,
        STONEWALL,
        WOODFLOOR,
        WOODWALL,
        CARPETFLOOR,
        TABLE,
        CHAIR

    }


  



    public class HouseFloor : Map {


        public TileTypes[,] tiles;
        public int[,] light;


        public List<LightSource> lamps = new List<LightSource>();


        public HouseFloor() : base() {

        }




        // The Draw method will be called each time the map is updated
        // It will render all of the symbols/colors for each cell to the map sub console
        public void Draw(RLConsole mapConsole) {
            mapConsole.Clear();
            foreach (Cell cell in GetAllCells()) {
                SetConsoleSymbolForCell(mapConsole, cell);
            }

            //draw light sources

            foreach (LightSource l in lamps) {
                int lightHere = light[l.X(), l.Y()];
                if (IsInFov(l.X(), l.Y()) && lightHere >= 1) {
                    RLColor darkenBy = new RLColor(8, 8, 8) * (10 - lightHere);
                    if (l is LampGeneric g) {
                        if (g.GetStrength() > 0) {
                            mapConsole.Set(l.X(), l.Y(), Colors.yellow + Colors.yellow - darkenBy, Colors.black, g.symbol);
                        } else {

                            mapConsole.Set(l.X(), l.Y(), Colors.grey - darkenBy, Colors.black, g.symbol);
                        }
                    }
                }
            }



        }

        private void SetConsoleSymbolForCell(RLConsole console, Cell cell) {
            // When we haven't explored a cell yet, we don't want to draw anything
            if (!cell.IsExplored) {
                console.Set(cell.X, cell.Y, Colors.black, Colors.black, ' ');
                return;
            }

            char symbol = ' ';
            RLColor color = Colors.black;

            switch (tiles[cell.X, cell.Y]) {
                case TileTypes.STONEFLOOR:
                    symbol = '.';
                    color = Colors.grey;
                    break;
                case TileTypes.STONEWALL:
                    symbol = ' ';
                    color = Colors.grey;
                    break;
                case TileTypes.WOODFLOOR:
                    symbol = '.';
                    color = Colors.yellow;
                    break;
                case TileTypes.WOODWALL:
                    symbol = ' ';
                    color = Colors.yellow;
                    break;
                case TileTypes.CARPETFLOOR:
                    symbol = '#';
                    color = Colors.red;
                    break;
                case TileTypes.TABLE:
                    symbol = (char)194;
                    color = Colors.yellow;
                    break;
                case TileTypes.CHAIR:
                    symbol = (char)191;
                    color = Colors.yellow;
                    break;
            }

            int lightHere = light[cell.X, cell.Y];

            // When a cell is currently in the field-of-view it should be drawn with ligher colors
            if (IsInFov(cell.X, cell.Y) && lightHere >= 1) {
                // Choose the symbol to draw based on if the cell is walkable or not

                RLColor darkenBy = new RLColor(8, 8, 8) * (10 - lightHere);

                if (cell.IsWalkable) {
                    console.Set(cell.X, cell.Y, color - darkenBy, Colors.black, symbol);
                } else {
                    console.Set(cell.X, cell.Y, color - darkenBy, color - darkenBy, symbol);
                }
            }
            // When a cell is outside of the field of view draw it with darker colors
            else {
                if (cell.IsWalkable) {
                    console.Set(cell.X, cell.Y, color - new RLColor(96, 96, 96), Colors.black, symbol);
                } else {
                    console.Set(cell.X, cell.Y, color - new RLColor(96, 96, 96), color - new RLColor(96, 96, 96), symbol);
                }
            }






        }

        public void UpdatePlayerFieldOfView(Player player) {

            // Compute the field-of-view based on the player's location and awareness



            ComputeFov(player.x, player.y, 100, true);




            // Mark all cells in field-of-view as having been explored
            foreach (Cell cell in GetAllCells()) {
                if (IsInFov(cell.X, cell.Y) && light[cell.X, cell.Y] >= 1) {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        internal void CalculateLighting() {


            foreach (Cell cell in GetAllCells()) {

                light[cell.X, cell.Y] = 0;
            }
            foreach (LightSource l in lamps) {
                ReadOnlyCollection<ICell> cells = ComputeFov(l.X(), l.Y(), l.GetStrength() * l.GetStrength(), true);

                foreach (ICell i in cells) {
                    int lightLevel = l.GetStrength() - (int)Math.Round(Math.Sqrt(Math.Pow(i.X - l.X(), 2) + Math.Pow(i.Y - l.Y(), 2)));
                    //Console.WriteLine(lightLevel);
                    if (light[i.X, i.Y] < lightLevel) { light[i.X, i.Y] = lightLevel; }
                }



            }
        }
    }
}
