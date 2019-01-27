using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RogueSharp.Random;

namespace HHRL {
    public class MapGenerator {
        private readonly int _width;
        private readonly int _height;

        private readonly HouseFloor _map;

        // Constructing a new MapGenerator requires the dimensions of the maps it will create
        public MapGenerator(int width, int height) {
            _width = width;
            _height = height;
            _map = new HouseFloor();


        }

        // Generate a new map that is a simple open floor with walls around the outside
        //public HouseFloor CreateMap() {
        //    // Initialize every cell in the map by
        //    // setting walkable, transparency, and explored to true
        //    _map.Initialize(_width, _height);
        //    _map.tiles = new TileTypes[_width, _height];

        //    foreach (Cell cell in _map.GetAllCells()) {
        //        _map.SetCellProperties(cell.X, cell.Y, true, true, false);
        //        _map.tiles[cell.X, cell.Y] = TileTypes.STONEFLOOR;
        //    }

        //    // Set the first and last rows in the map to not be transparent or walkable
        //    foreach (Cell cell in _map.GetCellsInRows(0, _height - 1)) {
        //        _map.SetCellProperties(cell.X, cell.Y, false, false, false);
        //        _map.tiles[cell.X, cell.Y] = TileTypes.STONEWALL;
        //    }

        //    // Set the first and last columns in the map to not be transparent or walkable
        //    foreach (Cell cell in _map.GetCellsInColumns(0, _width - 1)) {
        //        _map.SetCellProperties(cell.X, cell.Y, false, false, false);
        //        _map.tiles[cell.X, cell.Y] = TileTypes.STONEWALL;
        //    }

        //    return _map;
        //}


        public HouseFloor CreateMap(IRandom rng, int depth, int hallCycles) {
            // Initialize every cell in the map by
            // setting walkable, transparency, and explored to true
            _map.Initialize(_width, _height);
            _map.tiles = new TileTypes[_width, _height];
            _map.light = new int[_width, _height];

            List<Room> rooms = BSPGen.GenStructure(_width, _height, rng, 4, 0.3f);

            foreach (Cell cell in _map.GetAllCells()) {
                _map.SetCellProperties(cell.X, cell.Y, false, false, false);
                _map.tiles[cell.X, cell.Y] = TileTypes.STONEWALL;

                _map.light[cell.X, cell.Y] = 0;
            }

            //carve out rooms 
            foreach (Room i in rooms) {

                switch (i.type) {
                    case Room.RoomType.WOODEN:
                        for (int x = 1; x < i.width - 1; x++) {
                            for (int y = 1; y < i.height - 1; y++) {
                                _map.SetCellProperties(x + i.x, y + i.y, true, true, false);
                                _map.tiles[x + i.x, y + i.y] = TileTypes.WOODFLOOR;
                            }
                        }

                        for (int x = 0; x < i.width; x++) {
                            _map.SetCellProperties(x + i.x, i.y + 1, false, false, false);
                            _map.SetCellProperties(x + i.x, i.y + i.height - 1, false, false, false);
                            _map.tiles[x + i.x, i.y + 1] = TileTypes.WOODWALL;
                            _map.tiles[x + i.x, i.y + i.height - 1] = TileTypes.WOODWALL;
                        }
                        for (int y = 0; y < i.height; y++) {

                            _map.SetCellProperties(i.x + 1, y + i.y, false, false, false);
                            _map.SetCellProperties(i.x + i.width - 1, y + i.y, false, false, false);
                            _map.tiles[i.x + 1, y + i.y] = TileTypes.WOODWALL;
                            _map.tiles[i.x + i.width - 1, y + i.y] = TileTypes.WOODWALL;

                        }
                        break;
                    case Room.RoomType.CELLAR:
                        for (int x = 1; x < i.width - 1; x++) {
                            for (int y = 1; y < i.height - 1; y++) {
                                _map.SetCellProperties(x + i.x, y + i.y, true, true, false);
                                _map.tiles[x + i.x, y + i.y] = TileTypes.STONEFLOOR;
                            }
                        }

                        for (int x = 0; x < i.width; x++) {

                            _map.SetCellProperties(x + i.x, i.y + 1, false, false, false);
                            _map.SetCellProperties(x + i.x, i.y + i.height - 1, false, false, false);
                            _map.tiles[x + i.x, i.y + 1] = TileTypes.STONEWALL;
                            _map.tiles[x + i.x, i.y + i.height - 1] = TileTypes.STONEWALL;
                        }
                        for (int y = 0; y < i.height; y++) {

                            _map.SetCellProperties(i.x + 1, y + i.y, false, false, false);
                            _map.SetCellProperties(i.x + i.width - 1, y + i.y, false, false, false);
                            _map.tiles[i.x + 1, y + i.y] = TileTypes.STONEWALL;
                            _map.tiles[i.x + i.width - 1, y + i.y] = TileTypes.STONEWALL;

                        }
                        break;
                    case Room.RoomType.HABIT:
                        for (int x = 1; x < i.width - 1; x++) {
                            for (int y = 1; y < i.height - 1; y++) {
                                _map.SetCellProperties(x + i.x, y + i.y, true, true, false);
                                _map.tiles[x + i.x, y + i.y] = TileTypes.WOODFLOOR;
                            }
                        }

                        for (int x = 0; x < i.width; x++) {

                            _map.SetCellProperties(x + i.x, i.y + 1, false, false, false);
                            _map.SetCellProperties(x + i.x, i.y + i.height - 1, false, false, false);
                            _map.tiles[x + i.x, i.y + 1] = TileTypes.STONEWALL;
                            _map.tiles[x + i.x, i.y + i.height - 1] = TileTypes.STONEWALL;
                        }
                        for (int y = 0; y < i.height; y++) {

                            _map.SetCellProperties(i.x + 1, y + i.y, false, false, false);
                            _map.SetCellProperties(i.x + i.width - 1, y + i.y, false, false, false);
                            _map.tiles[i.x + 1, y + i.y] = TileTypes.WOODWALL;
                            _map.tiles[i.x + i.width - 1, y + i.y] = TileTypes.WOODWALL;

                        }
                        break;
                    case Room.RoomType.HABITFANCY:

                        for (int x = 3; x < i.width - 3; x++) {
                            for (int y = 3; y < i.height - 3; y++) {
                                _map.SetCellProperties(x + i.x, y + i.y, true, true, false);
                                _map.tiles[x + i.x, y + i.y] = TileTypes.CARPETFLOOR;
                            }
                        }
                        for (int x = 0; x < i.width; x++) {
                            _map.tiles[x + i.x, i.y + 1] = TileTypes.STONEWALL;
                            _map.tiles[x + i.x, i.y + 2] = TileTypes.WOODFLOOR;
                            _map.tiles[x + i.x, i.y + 3] = TileTypes.WOODFLOOR;
                            _map.tiles[x + i.x, i.y + i.height - 1] = TileTypes.STONEWALL;
                            _map.tiles[x + i.x, i.y + i.height - 2] = TileTypes.WOODFLOOR;
                            _map.tiles[x + i.x, i.y + i.height - 3] = TileTypes.WOODFLOOR;

                            _map.SetCellProperties(x + i.x, i.y + 2, true, true, false);
                            _map.SetCellProperties(x + i.x, i.y + 3, true, true, false);
                            _map.SetCellProperties(x + i.x, i.y + i.height - 2, true, true, false);
                            _map.SetCellProperties(x + i.x, i.y + i.height - 3, true, true, false);

                        }
                        for (int y = 0; y < i.height; y++) {
                            _map.tiles[i.x + 1, y + i.y] = TileTypes.STONEWALL;
                            _map.tiles[i.x + 2, y + i.y] = TileTypes.WOODFLOOR;
                            _map.tiles[i.x + 3, y + i.y] = TileTypes.WOODFLOOR;
                            _map.tiles[i.x + i.width - 1, y + i.y] = TileTypes.STONEWALL;
                            _map.tiles[i.x + i.width - 2, y + i.y] = TileTypes.WOODFLOOR;
                            _map.tiles[i.x + i.width - 3, y + i.y] = TileTypes.WOODFLOOR;

                            _map.SetCellProperties(i.x + 2, y + i.y, true, true, false);
                            _map.SetCellProperties(i.x + 3, y + i.y, true, true, false);
                            _map.SetCellProperties(i.x + i.width - 2, y + i.y, true, true, false);
                            _map.SetCellProperties(i.x + i.width - 3, y + i.y, true, true, false);
                        }

                        break;
                    case Room.RoomType.GRAND:
                        for (int x = 3; x < i.width - 3; x++) {
                            for (int y = 3; y < i.height - 3; y++) {
                                _map.SetCellProperties(x + i.x, y + i.y, true, true, false);
                                _map.tiles[x + i.x, y + i.y] = TileTypes.CARPETFLOOR;
                            }
                        }
                        for (int x = 0; x < i.width; x++) {
                            _map.tiles[x + i.x, i.y + 1] = TileTypes.STONEWALL;
                            _map.tiles[x + i.x, i.y + 2] = TileTypes.STONEFLOOR;
                            _map.tiles[x + i.x, i.y + 3] = TileTypes.STONEFLOOR;
                            _map.tiles[x + i.x, i.y + i.height - 1] = TileTypes.STONEWALL;
                            _map.tiles[x + i.x, i.y + i.height - 2] = TileTypes.STONEFLOOR;
                            _map.tiles[x + i.x, i.y + i.height - 3] = TileTypes.STONEFLOOR;


                            _map.SetCellProperties(x + i.x, i.y + 2, true, true, false);
                            _map.SetCellProperties(x + i.x, i.y + 3, true, true, false);
                            _map.SetCellProperties(x + i.x, i.y + i.height - 2, true, true, false);
                            _map.SetCellProperties(x + i.x, i.y + i.height - 3, true, true, false);
                        }
                        for (int y = 0; y < i.height; y++) {
                            _map.tiles[i.x + 1, y + i.y] = TileTypes.STONEWALL;
                            _map.tiles[i.x + 2, y + i.y] = TileTypes.STONEFLOOR;
                            _map.tiles[i.x + 3, y + i.y] = TileTypes.STONEFLOOR;
                            _map.tiles[i.x + i.width - 1, y + i.y] = TileTypes.STONEWALL;
                            _map.tiles[i.x + i.width - 2, y + i.y] = TileTypes.STONEFLOOR;
                            _map.tiles[i.x + i.width - 3, y + i.y] = TileTypes.STONEFLOOR;

                            _map.SetCellProperties(i.x + 2, y + i.y, true, true, false);
                            _map.SetCellProperties(i.x + 3, y + i.y, true, true, false);
                            _map.SetCellProperties(i.x + i.width - 2, y + i.y, true, true, false);
                            _map.SetCellProperties(i.x + i.width - 3, y + i.y, true, true, false);
                        }
                        break;

                }



            }


            //carve out doorways

            for (int round = 0; round < hallCycles; round++) {
                foreach (Room i in rooms) {
                    //select other room
                    Room room2 = rooms[rng.Next(0, rooms.Count() - 1)];
                    //select endpoints for corridor
                    int x1, x2, y1, y2 = 0;

                    x1 = rng.Next(i.x + 2, i.x + i.width - 2);
                    y1 = rng.Next(i.y + 2, i.y + i.height - 2);
                    x2 = rng.Next(room2.x + 2, room2.x + room2.width - 3);
                    y2 = rng.Next(room2.y + 2, room2.y + room2.height - 3);


                    //carve out hall

                    for (int x = x1; x < x2; x++) {

                        _map.SetCellProperties(x, y1, true, true, false);
                        if (_map.tiles[x, y1] == TileTypes.STONEWALL) { _map.tiles[x, y1] = TileTypes.STONEFLOOR; }
                        if (_map.tiles[x, y1] == TileTypes.WOODWALL) { _map.tiles[x, y1] = TileTypes.WOODFLOOR; }

                    }

                    for (int y = y1; y < y2; y++) {
                        _map.SetCellProperties(x2, y, true, true, false);
                        if (_map.tiles[x2, y] == TileTypes.STONEWALL) { _map.tiles[x2, y] = TileTypes.STONEFLOOR; }
                        if (_map.tiles[x2, y] == TileTypes.WOODWALL) { _map.tiles[x2, y] = TileTypes.WOODFLOOR; }
                    }


                }
            }


            //add lamps
            foreach (Room i in rooms) {
                float avBraziers = 0;
                float avLamps = 0;
                float avTorches = 0;
                float avCandles = 0;

                switch (i.type) {
                    case Room.RoomType.WOODEN:
                        avCandles = rng.Next(1, 3) / 3f;
                        avLamps = (rng.Next(0, 2)) / 2f;
                        break;
                    case Room.RoomType.CELLAR:
                        avCandles = rng.Next(1, 4) / 3f;
                        avTorches = rng.Next(0, 2) / 2f;
                        break;
                    case Room.RoomType.HABIT:
                        avLamps = rng.Next(0, 2) / 3f;
                        avCandles = rng.Next(1, 3) / 3f;
                        break;
                    case Room.RoomType.HABITFANCY:
                        avLamps = rng.Next(0, 2) / 3f;
                        avBraziers = rng.Next(0, 1) / 3f;
                        break;
                    case Room.RoomType.GRAND:
                        avCandles = rng.Next(1, 3) / 3f;
                        avBraziers = rng.Next(0, 2) / 3f;
                        break;

                }

                int area = i.width * i.height;

                int braziers = (int)(avBraziers * area / 50);
                int lamps = (int)(avLamps * area / 50);
                int torches = (int)(avTorches * area / 50);
                int candles = (int)(avCandles * area / 50);

                for (int j = 0; j < braziers; j++) {
                    bool ok = false;
                    int newX = 0;
                    int newY = 0;
                    while (!ok) {
                        ok = true;
                        newX = rng.Next(i.x + 1, i.x + i.width - 2);
                        newY = rng.Next(i.y + 1, i.y + i.height - 2);

                        foreach (LightSource l in _map.lamps) {
                            if (l.X() == i.x && l.Y() == i.y) {
                                ok = false;
                                break;
                            }
                        }
                    }

                    LampGeneric n = new Brazier(newX, newY);
                    n.Unlight();
                    _map.lamps.Add(n);
                }


                for (int j = 0; j < lamps; j++) {
                    bool ok = false;
                    int newX = 0;
                    int newY = 0;
                    while (!ok) {
                        ok = true;
                        newX = rng.Next(i.x + 1, i.x + i.width - 2);
                        newY = rng.Next(i.y + 1, i.y + i.height - 2);

                        foreach (LightSource l in _map.lamps) {
                            if (l.X() == i.x && l.Y() == i.y) {
                                ok = false;
                                break;
                            }
                        }
                    }

                    LampGeneric n = new Lamp(newX, newY);
                    n.Unlight();
                    _map.lamps.Add(n);
                }

                for (int j = 0; j < torches; j++) {
                    bool ok = false;
                    int newX = 0;
                    int newY = 0;
                    while (!ok) {
                        ok = true;
                        newX = rng.Next(i.x + 1, i.x + i.width - 2);
                        newY = rng.Next(i.y + 1, i.y + i.height - 2);

                        foreach (LightSource l in _map.lamps) {
                            if (l.X() == i.x && l.Y() == i.y) {
                                ok = false;
                                break;
                            }
                        }
                    }

                    LampGeneric n = new Torch(newX, newY);
                    n.Unlight();
                    _map.lamps.Add(n);
                }

                for (int j = 0; j < candles; j++) {
                    bool ok = false;
                    int newX = 0;
                    int newY = 0;
                    while (!ok) {
                        ok = true;
                        newX = rng.Next(i.x + 1, i.x + i.width - 2);
                        newY = rng.Next(i.y + 1, i.y + i.height - 2);

                        foreach (LightSource l in _map.lamps) {
                            if (l.X() == i.x && l.Y() == i.y) {
                                ok = false;
                                break;
                            }
                        }
                    }

                    LampGeneric n = new Candle(newX, newY);
                    n.Light();
                    _map.lamps.Add(n);
                }



                //add props

                switch (i.type) {
                    case Room.RoomType.WOODEN:
                        break;
                    case Room.RoomType.CELLAR:
                        break;
                    case Room.RoomType.HABIT:
                        break;
                    case Room.RoomType.HABITFANCY:
                        break;
                    case Room.RoomType.GRAND:
                        break;
                }

                //add tables
                int tables = area / 120;
                for (int j = 0; j < tables; j++) {
                    bool ok = false;
                    int newX = 0;
                    int newY = 0;
                    while (!ok) {
                        ok = true;
                        newX = rng.Next(i.x + 1, i.x + i.width - 2);
                        newY = rng.Next(i.y + 1, i.y + i.height - 2);

                        if (_map.tiles[newX, newY] != TileTypes.TABLE) {
                            ok = false;
                            break;
                        }

                    }

                    _map.tiles[newX, newY] = TileTypes.TABLE;
                }

                int chairs = area / 80;
                for (int j = 0; j < chairs; j++) {
                    bool ok = false;
                    int newX = 0;
                    int newY = 0;
                    while (!ok) {
                        ok = true;
                        newX = rng.Next(i.x + 1, i.x + i.width - 2);
                        newY = rng.Next(i.y + 1, i.y + i.height - 2);

                        if (_map.tiles[newX, newY] != TileTypes.CHAIR) {
                            ok = false;
                            break;
                        }

                    }

                    _map.tiles[newX, newY] = TileTypes.CHAIR;
                }

            }




            return _map;
        }


    }


    public class Room {

        public enum RoomType {
            WOODEN, //wooden walls and floors
            CELLAR, //stone walls and floors
            HABIT, // stone walls, wood floors
            HABITFANCY, // wood walls, wood floors, carpet
            GRAND // stone walls and floor, carped
        }



        public int x, y, width, height;
        public RoomType type;


        public Room(int x, int y, int w, int h, RoomType type) {
            this.x = x;
            this.y = y;
            this.width = w;
            this.height = h;
            this.type = type;


        }

    }
}
