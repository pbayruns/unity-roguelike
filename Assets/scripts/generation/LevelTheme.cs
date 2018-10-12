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
        NORMAL_FOREST_POND, NORMAL_FOREST_GARDEN, NORMAL_FOREST_TREASURE
    }

    public struct RoomInfo
    {
        public Tile[][] tiles;
        public Tile[][] overlay_tiles;
        public Tile[][] objects;

        public Enemy[][] enemies;
    };

    public struct CorridorInfo
    {
        public Tile south_end;
        public Tile north_end;
        public Tile east_end;
        public Tile west_end;
        public Tile eastWest;
        public Tile northSouth;
        public Tile bottom_tile;
    }

    public const int THEME_GRAY = 1;

    //What is a Theme?
    //A way of generating rooms
    // Having different treasure and enemy structure
    // Different wall tiles
    // Different corridor tiles
    // Different floor tiles
    // Different room objects

    public Theme theme;
    public Tile wallTile;
    public Tile[] midWalls;
    public Tile midFloor;

    public int level;

    public LevelTheme(int level)
    {
        this.level = level;
        theme = Theme.NORMAL_FOREST;
        wallTile = Tile.SHRUB_DARK_GREEN;
        midFloor = Tile.GRASS_DARK;
        midWalls = new Tile[] { Tile.SHRUB_DARK_GREEN, Tile.SHRUB_PINE_DARK_GREEN };
    }

    public Tile GetMidWall()
    { 
        return midWalls[Random.Range(0, midWalls.Length)];
    }

    public Tile GetBaseTile()
    {
        Tile[] floor = new Tile[] { Tile.GRASS_DARK, Tile.GRASS_DARK, Tile.GRASS_DARK, Tile.GRASS_DARK_1, Tile.GRASS_DARK_2 };
        return floor[Random.Range(0, floor.Length)];
    }

    private int GetMaxEnemies(int width, int height)
    {
        float squares = width * height;
        float enemiesPerSquare = GetEnemiesPerSquare(width, height);
        int max = (int) Mathf.Ceil(squares * enemiesPerSquare);
        return (max >= 1) ? max : 1;
    }

    private float GetEnemiesPerSquare(int width, int height)
    {
        return Mathf.Log(level) / 50;
    }

    private RoomInfo GetForestDefaultRoom(int width, int height)
    {
        Enemy[] baddies = new Enemy[] { Enemy.ORC_DEFAULT, Enemy.SORCERER_DEFAULT, Enemy.SLIME_RED, Enemy.KNIGHT_DEFAULT };
        Tile[] specialTiles = new Tile[] { Tile.FLOWERS_BLUE };
        return GetGenericRoomTiles(width: width, height: height, specialTiles: specialTiles, baddies: baddies);
    }

    private RoomInfo GetForestMushroomRoom(int width, int height)
    {
        Tile[] overlayTiles = new Tile[] { Tile.MUSHROOM_1, Tile.MUSHROOM_2, Tile.MUSHROOM_3, Tile.MUSHROOM_4 };
        Enemy[] baddies = new Enemy[] { Enemy.SORCERER_DEFAULT };
        return GetGenericRoomTiles(width, height, overlayTiles: overlayTiles, baddies: baddies);
    }

    private RoomInfo GetForestLoggingRoom(int width, int height)
    {
        Tile[] objx = new Tile[] { Tile.STUMP_1, Tile.STUMP_2, Tile.STUMP_3 };
        Enemy[] baddies = new Enemy[] { Enemy.KNIGHT_DEFAULT };
        return GetGenericRoomTiles(width, height, objx: objx, baddies: baddies);
    }

    private RoomInfo GetForestTreasureRoom(int width, int height)
    {
        RoomInfo info = GetGenericRoomTiles(width, height);
        info.objects[width/2][height/2] = Tile.CHEST_CLOSED;
        return info;
    }

    public RoomInfo GetGenericRoomTiles(int width, int height, 
        Tile[] objx = null, float objChance = 0.05f, 
        Enemy[] baddies = null, float enemyChance = 0.08f,
        Tile[] specialTiles = null, float tileChance = 0.1f,
        Tile[] overlayTiles = null, float overlayChance = 0.1f
    ){
        Tile[][] tiles = GetTilesArray(width, height);
        Tile[][] objects = GetTilesArray(width, height);
        Enemy[][] enemies = GetEnemiesArray(width, height);
        Tile[][] overL = GetTilesArray(width, height);

        int enemyCount = 0;
        int maxEnemies = GetMaxEnemies(width, height);

        for (int x = 0; x < tiles.Length; x++)
        {
            Tile[] row = tiles[x];
            for (int y = 0; y < row.Length; y++)
            {
                Tile nextTile = GetBaseTile();
                Enemy nextEnemy = Enemy.NONE;
                Tile nextObject = Tile.NOT_SET;
                Tile nextOverlayTile = Tile.NOT_SET;

                if(specialTiles != null && Random.Range(0f, 1f) < tileChance){
                    nextTile = specialTiles[Random.Range(0, specialTiles.Length)];
                } else if(overlayTiles != null && Random.Range(0f, 1f) < overlayChance){
                    nextOverlayTile = overlayTiles[Random.Range(0, overlayTiles.Length)];
                }

                if (objx != null && Random.Range(0f, 1f) < objChance) {
                    nextObject = objx[Random.Range(0, objx.Length)];
                } else if (enemyCount < maxEnemies && baddies != null && Random.Range(0f, 1f) < enemyChance) {
                    nextEnemy = baddies[Random.Range(0, baddies.Length)];
                    enemyCount++;
                }
                tiles[x][y] = nextTile;
                objects[x][y] = nextObject;
                enemies[x][y] = nextEnemy;
                overL[x][y] = nextOverlayTile;
            }
        }
        RoomInfo info = new RoomInfo();
        info.objects = objects;
        info.enemies = enemies;
        info.tiles = tiles;
        info.overlay_tiles = overL;
        return info;
    }

    public RoomInfo GetForestRoomTiles(int width, int height, RoomType type)
    {
                return GetForestTreasureRoom(width, height);

        switch (type)
        {
            case RoomType.NORMAL_FOREST_DEFAULT:
                return GetForestDefaultRoom(width, height);
            case RoomType.NORMAL_FOREST_MUSHROOM:
                return GetForestMushroomRoom(width, height);
            case RoomType.NORMAL_FOREST_LOGGING:
                return GetForestLoggingRoom(width, height);
            case RoomType.NORMAL_FOREST_TREASURE:
                return GetForestTreasureRoom(width, height);
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

    public CorridorInfo GetCorridorInfo()
    {
        CorridorInfo info = new CorridorInfo();
        info.eastWest = Tile.PATH_HORIZ_DIRT;
        info.northSouth = Tile.PATH_VERT_DIRT;
        info.north_end = Tile.PATH_END_NORTH_DIRT;
        info.south_end = Tile.PATH_END_SOUTH_DIRT;
        info.east_end = Tile.PATH_END_EAST_DIRT;
        info.west_end = Tile.PATH_END_WEST_DIRT;
        info.bottom_tile = Tile.GRASS_DARK;

        return info;
    }

    private Tile[][] GetTilesArray(int width, int height)
    {
        // Create the tiles array with the right rows and columns
        Tile[][] tiles = new Tile[width][];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new Tile[height];
            for (int x = 0; x < tiles[i].Length; x++)
            {
                tiles[i][x] = Tile.NOT_SET;
            }
        }
        return tiles;
    }

    private Enemy[][] GetEnemiesArray(int width, int height)
    {
        // Create the enemies array with the right rows and columns
        Enemy[][] enemies = new Enemy[width][];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = new Enemy[height];
            for (int x = 0; x < enemies[i].Length; x++)
            {
                enemies[i][x] = Enemy.NONE;
            }
        }
        return enemies;
    }
}
