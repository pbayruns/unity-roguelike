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
    public Tile midWall;
    public Tile midFloor;

    public int level;

    public LevelTheme(int level)
    {
        //theme = UnityEngine.Random.Range(MIN_THEME, MAX_THEME);
        this.level = level;
        theme = Theme.NORMAL_FOREST;
        wallTile = Tile.SHRUB_DARK_GREEN;
        midFloor = Tile.GRASS_DARK;
        midWall = Tile.SHRUB_DARK_GREEN;
    }
    private int GetMaxEnemies(int width, int height)
    {
        float squares = width * height;
        float enemiesPerSquare = Mathf.Log(level) / 25;
        int max = (int) Mathf.Ceil(squares * enemiesPerSquare);
        return (max >= 1) ? max : 1;
    }
    private RoomInfo GetForestDefaultRoom(int width, int height)
    {
        Tile[][] tiles = GetTilesArray(width, height);
        Tile[][] overlayTiles = GetTilesArray(width, height);
        Enemy[][] enemies = GetEnemiesArray(width, height);
        int enemyCount = 0;
        int maxEnemies = GetMaxEnemies(width, height);
        for (int x = 0; x < tiles.Length; x++)
        {
            Tile[] row = tiles[x];
            for (int y = 0; y < row.Length; y++)
            {
                Tile nextTile = Tile.GRASS_DARK;
                 if (Random.Range(0f, 1f) < 0.1f)
                {
                    nextTile = Tile.FLOWERS_BLUE;
                }
                tiles[x][y] = nextTile;

                Enemy nextEnemy = Enemy.NONE;
                if (enemyCount < maxEnemies && Random.Range(0f, 1f) < 0.05f)
                {
                    Enemy[] enemy = new Enemy[] { Enemy.ORC_DEFAULT, Enemy.SLIME_RED, Enemy.KNIGHT_DEFAULT };
                    nextEnemy = enemy[Random.Range(0, enemy.Length)];
                    enemyCount++;
                }
                enemies[x][y] = nextEnemy;
            }
        }
        RoomInfo info = new RoomInfo();
        info.enemies = enemies;
        info.tiles = tiles;
        info.overlay_tiles = overlayTiles;
        return info;
    }

    private RoomInfo GetForestMushroomRoom(int width, int height)
    {
        Tile[][] tiles = GetTilesArray(width, height);
        Tile[][] overlayTiles = GetTilesArray(width, height);
        Enemy[][] enemies = GetEnemiesArray(width, height);
        int enemyCount = 0;
        int maxEnemies = GetMaxEnemies(width, height);
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

                Enemy nextEnemy = Enemy.NONE;
                if(Random.Range(0f, 1f) < 0.05f && enemyCount < maxEnemies)
                {
                    Enemy[] enemy = new Enemy[] { Enemy.SORCERER_DEFAULT };
                    nextEnemy = enemy[Random.Range(0, enemy.Length)];
                    enemyCount++;
                }
                enemies[x][y] = nextEnemy;
            }
        }
        RoomInfo info = new RoomInfo();
        info.enemies = enemies;
        info.tiles = tiles;
        info.overlay_tiles = overlayTiles;
        return info;
    }

    private RoomInfo GetForestLoggingRoom(int width, int height)
    {
        Tile[][] tiles = GetTilesArray(width, height);
        Tile[][] objects = GetTilesArray(width, height);
        Enemy[][] enemies = GetEnemiesArray(width, height);

        for (int x = 0; x < tiles.Length; x++)
        {
            Tile[] row = tiles[x];
            for (int y = 0; y < row.Length; y++)
            {
                tiles[x][y] = Tile.GRASS_DARK;
                if (Random.Range(0f, 1f) < 0.05f)
                {
                    Tile[] objx = new Tile[] { Tile.STUMP_1, Tile.STUMP_2, Tile.STUMP_3 };
                    objects[x][y] = objx[Random.Range(0, objx.Length)];
                }

                Enemy nextEnemy = Enemy.NONE;
                if (Random.Range(0f, 1f) < 0.03f)
                {
                    Enemy[] enemy = new Enemy[] { Enemy.KNIGHT_DEFAULT };
                    nextEnemy = enemy[Random.Range(0, enemy.Length)];
                }
                enemies[x][y] = nextEnemy;
            }
        }
        RoomInfo info = new RoomInfo();
        info.objects = objects;
        info.enemies = enemies;
        info.tiles = tiles;
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
