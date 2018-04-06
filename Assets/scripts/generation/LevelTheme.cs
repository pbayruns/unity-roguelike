using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTheme
{

    public enum Theme
    {
        NORMAL_FOREST
    }

    public enum RoomType
    {
        NORMAL_FOREST_DEFAULT, NORMAL_FOREST_MUSHROOM, NORMAL_FOREST_LOGGING, NORMAL_FOREST_CAMP,
        NORMAL_FOREST_POND, NORMAL_FOREST_GARDEN
    }

    public struct RoomInfo
    {
        public Tile[][] tiles;
        public Tile[][] overlay_tiles;
        public Tile[][] objects;  
    };

    public const int THEME_GRAY = 1;
    private Tile[] gray_theme_tiles = new Tile[]
    {
        Tile.DIRT,
        Tile.WALL_GREY,
        Tile.TILE_GREY
    };
    //What is a Theme?
    //A way of generating rooms
    // Having different treasure and enemy structure
    // Different wall tiles
    // Different corridor tiles
    // Different floor tiles
    // Different room objects

    public Theme theme;
    public Tile wallTile;
    public Tile midWall;
    public Tile midFloor;

    public LevelTheme()
    {
        //theme = UnityEngine.Random.Range(MIN_THEME, MAX_THEME);
        theme = Theme.NORMAL_FOREST;
        wallTile = Tile.SHRUB_DARK_GREEN;
        midFloor = Tile.GRASS_DARK;
        midWall = Tile.SHRUB_DARK_GREEN;
    }

    private RoomInfo GetForestDefaultRoom(int width, int height)
    {
        Tile[][] tiles = GetTilesArray(width, height);
        for (int x = 0; x < tiles.Length; x++)
        {
            Tile[] row = tiles[x];
            for (int y = 0; y < row.Length; y++)
            {
                Tile nextTile = Tile.GRASS_DARK;
                if (Random.Range(0f, 1f) < 0.1f)
                {
                    nextTile = Tile.GRASS_DARK;
                }
                else if (Random.Range(0f, 1f) < 0.1f)
                {
                    nextTile = Tile.FLOWERS_BLUE;
                }
                tiles[x][y] = nextTile;
            }
        }
        RoomInfo info = new RoomInfo();
        info.tiles = tiles;
        return info;
    }

    private RoomInfo GetForestMushroomRoom(int width, int height)
    {
        Tile[][] tiles = GetTilesArray(width, height);
        Tile[][] overlayTiles = GetTilesArray(width, height);
        for (int x = 0; x < tiles.Length; x++)
        {
            Tile[] row = tiles[x];
            for (int y = 0; y < row.Length; y++)
            {
                Tile nextTile = Tile.GRASS_DARK;
                if (Random.Range(0f, 1f) < 0.1f)
                {
                    Tile[] overL = new Tile[] { Tile.MUSHROOM_1, Tile.MUSHROOM_2, Tile.MUSHROOM_3, Tile.MUSHROOM_4 };
                    overlayTiles[x][y] = overL[Random.Range(0, overL.Length)];
                }
                tiles[x][y] = nextTile;
            }
        }
        RoomInfo info = new RoomInfo();
        info.tiles = tiles;
        info.overlay_tiles = overlayTiles;
        return info;
    }

    private RoomInfo GetForestLoggingRoom(int width, int height)
    {
        Tile[][] tiles = GetTilesArray(width, height);
        Tile[][] objects = GetTilesArray(width, height);
        for (int x = 0; x < tiles.Length; x++)
        {
            Tile[] row = tiles[x];
            for (int y = 0; y < row.Length; y++)
            {
                tiles[x][y] = Tile.GRASS_NORMAL;
                if (Random.Range(0f, 1f) < 0.08f)
                {
                    Tile[] objx = new Tile[] { Tile.STUMP_1, Tile.STUMP_2, Tile.STUMP_3 };
                    objects[x][y] = objx[Random.Range(0, objx.Length)];
                }
            }
        }
        RoomInfo info = new RoomInfo();
        info.tiles = tiles;
        info.objects = objects;
        return info;
    }

    public RoomInfo GetForestRoomTiles(int width, int height, RoomType type)
    {

        switch (type)
        {
            case RoomType.NORMAL_FOREST_DEFAULT:
                return GetForestDefaultRoom(width, height);
            case RoomType.NORMAL_FOREST_MUSHROOM:
                return GetForestMushroomRoom(width, height);
            case RoomType.NORMAL_FOREST_LOGGING:
                return GetForestLoggingRoom(width, height);
            default:
                return GetForestDefaultRoom(width, height);
                //case RoomType.NORMAL_FOREST_CAMP:
                //    break;
                //case RoomType.NORMAL_FOREST_POND:
                //    break;
                //case RoomType.NORMAL_FOREST_GARDEN:
                //    break;
        }
    }

    public RoomInfo GetRoomTiles(int width, int height)
    {
        System.Array values = System.Enum.GetValues(typeof(RoomType));
        RoomType roomType = (RoomType)values.GetValue(Random.Range(0, values.Length));
        switch (theme)
        {
            case Theme.NORMAL_FOREST:
                return GetForestRoomTiles(width, height, roomType);
            default:
                return GetForestRoomTiles(width, height, roomType);
        }
    }

    public Tile GetCorridorTile()
    {
        return Tile.DIRT;
    }

    private Tile[][] GetTilesArray(int width, int height)
    {
        // Create the tiles array with the right rows and columns
        Tile[][] tiles = new Tile[width][];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new Tile[height];
        }
        return tiles;
    }
}
