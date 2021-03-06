﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tile
{
    //Floor Tiles
    GRASS_DARK, GRASS_DARK_1, GRASS_DARK_2, FLOWERS_BLUE,

    //Overlay Tiles
    MUSHROOM_1, MUSHROOM_2, MUSHROOM_3, MUSHROOM_4,
    PATH_HORIZ_DIRT,
    PATH_VERT_DIRT,
    PATH_END_NORTH_DIRT,
    PATH_END_SOUTH_DIRT,
    PATH_END_EAST_DIRT,
    PATH_END_WEST_DIRT,
    PATH_SINGLE_DIRT,

    //Wall Tiles
    SHRUB_DARK_GREEN, SHRUB_PINE_DARK_GREEN,

    //Objects
    STUMP_1, STUMP_2, STUMP_3,
    CHEST_CLOSED, CHEST_OPEN,

    NOT_SET
}

public enum Enemy
{
    SLIME_RED,
    KNIGHT_DEFAULT,
    ORC_DEFAULT,
    SORCERER_DEFAULT,
    NONE,
}

public enum DungeonObject
{
    CHEST
}

public class BoardCreator : MonoBehaviour
{
    public static BoardCreator instance = null;

    private Dictionary<Enemy, Object> EnemyPool = new Dictionary<Enemy, Object>();
    private Dictionary<Tile, Object> TilePool = new Dictionary<Tile, Object>();

    public int columns = 100;                                 // The number of columns on the board (how wide it will be).
    public int rows = 100;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(15, 20);         // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange(3, 10);         // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange(3, 10);        // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange(6, 10);    // The range of lengths corridors between rooms can have.
    public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
    public GameObject stairs;

    private float stairsDepth; // the percentage deep into the room sequence to spawn the stairs
    public float minStairDepth = 0.3f;
    public float maxStairDepth = 0.8f;

    private float spawnDepth; // the percentage deep into the room sequence to spawn the player
    public float minSpawnDepth = 0.2f;
    public float maxSpawnDepth = 1f;

    private Tile[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Tile[][] overlay;
    private Tile[][] objects;
    private Enemy[][] enemies;

    private DungeonObject[] dungeonObjects;
    private Room[] rooms;              // All the rooms that are created for this board.
    private Corridor[] corridors;      // All the corridors that connect the rooms.
    public GameObject boardHolder;     // GameObject that acts as a container for all other tiles.
    public LevelTheme theme;
    private BoxCollider2D boundsBox;

    //Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetupBoard(int level)
    {
        stairsDepth = Random.Range(minStairDepth, maxStairDepth);
        spawnDepth = Random.Range(minSpawnDepth, maxSpawnDepth);

        GameObject bounds = GameObject.Find("Bounds");
        if (bounds == null)
        {
            bounds = new GameObject("Bounds");
        }
        else
        {
            Destroy(bounds.gameObject);
            bounds = new GameObject("Bounds");
        }
        boundsBox = bounds.AddComponent<BoxCollider2D>() as BoxCollider2D;
        float mapWidth = columns + 1;
        float mapHeight = rows + 2;
        boundsBox.size = new Vector2(mapWidth, mapHeight);
        boundsBox.gameObject.transform.position = new Vector3((mapWidth / 2f) - 1f, (mapHeight / 2f) - 1f, 0f);
        boundsBox.isTrigger = true;
        Player.GetCamera().SetBounds(boundsBox);

        // Create the board holder.
        GameObject holder = GameObject.Find("BoardHolder");
        if (holder == null)
        {
            boardHolder = new GameObject("BoardHolder");
            boardHolder.transform.position = Vector3.zero;
        }
        else
        {
            foreach (Transform child in holder.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            boardHolder = holder;
        }

        // Get the theme for this level
        theme = new LevelTheme(level);

        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
        InstantiateOuterWalls();
    }


    private void SetupTilesArray()
    {
        // Create the tiles array with the right rows and columns
        tiles = new Tile[columns][];
        overlay = new Tile[columns][];
        objects = new Tile[columns][];
        enemies = new Enemy[columns][];
        for (int col = 0; col < tiles.Length; col++)
        {
            tiles[col] = new Tile[rows];
            overlay[col] = new Tile[rows];
            objects[col] = new Tile[rows];
            enemies[col] = new Enemy[rows];
            for (int row = 0; row < tiles[col].Length; row++)
            {
                tiles[col][row] = Tile.NOT_SET;
                overlay[col][row] = Tile.NOT_SET;
                objects[col][row] = Tile.NOT_SET;
                enemies[col][row] = Enemy.NONE;
            }
        }
    }


    private void CreateRoomsAndCorridors()
    {
        // Create rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // One less corridor than rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            // Create a room.
            rooms[i] = new Room();

            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length)
            {
                // ... create a corridor.
                corridors[i] = new Corridor();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }


            if (i == (int)(rooms.Length * spawnDepth))
            {
                int offsetY = Random.Range(0, rooms[i].roomHeight);
                int offsetX = Random.Range(0, rooms[i].roomWidth);
                Vector3 playerPos = new Vector3(rooms[i].xPos + offsetX, rooms[i].yPos + offsetY, 0f);
                Player.Move(playerPos);
            }
            if (i == (int)(rooms.Length * stairsDepth))
            {
                int offsetY = Random.Range(0, rooms[i].roomHeight);
                int offsetX = Random.Range(0, rooms[i].roomWidth);
                Vector3 stairsPos = new Vector3(rooms[i].xPos + offsetX, rooms[i].yPos + offsetY, 0);
                GameObject stairsInstance = Instantiate(stairs, stairsPos, Quaternion.identity);
                stairsInstance.transform.parent = boardHolder.transform;
            }
        }
    }

    private void SetTilesValuesForRooms()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];
            LevelTheme.RoomInfo info = theme.GetRoomTiles(currentRoom.roomWidth, currentRoom.roomHeight);
            //DungeonObject[] roomObjects = theme.GetDungeonObjects(currentRoom.roomWidth, currentRoom.roomHeight);

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;
                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;
                    Tile tile = Tile.NOT_SET;
                    Tile obj = Tile.NOT_SET;
                    Tile top = Tile.NOT_SET;
                    Enemy enemy = Enemy.NONE;
                    if (info.tiles != null) tile = info.tiles[j][k];
                    if (info.objects != null) obj = info.objects[j][k];
                    if (info.enemies != null) enemy = info.enemies[j][k];
                    if (info.overlay_tiles != null) top = info.overlay_tiles[j][k];

                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = tile;
                    overlay[xCoord][yCoord] = top;
                    objects[xCoord][yCoord] = obj;
                    enemies[xCoord][yCoord] = enemy;
                }
            }
        }
    }

    private void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];
            Tile tile = Tile.NOT_SET;
            LevelTheme.CorridorInfo info = theme.GetCorridorInfo();

            // and go through it's length.
            int length = currentCorridor.corridorLength;
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        if (j == length - 1) tile = info.north_end;
                        else if (j == 0) tile = info.south_end;
                        else tile = info.northSouth;
                        break;
                    case Direction.East:
                        xCoord += j;
                        if (j == length - 1) tile = info.east_end;
                        else if (j == 0) tile = info.west_end;
                        else tile = info.eastWest;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        if (j == length - 1) tile = info.south_end;
                        else if (j == 0) tile = info.north_end;
                        else tile = info.northSouth;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        if (j == length - 1) tile = info.west_end;
                        else if (j == 0) tile = info.east_end;
                        else tile = info.eastWest;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                overlay[xCoord][yCoord] = tile;
                tiles[xCoord][yCoord] = info.bottom_tile;
                objects[xCoord][yCoord] = Tile.NOT_SET;
            }
        }
    }


    private void InstantiateTiles()
    {
        // Go through all the tiles in the jagged array...
        Tile wall = theme.wallTile;
        for (int row = 0; row < tiles.Length; row++)
        {
            for (int col = 0; col < tiles[row].Length; col++)
            {
                Tile tile = tiles[row][col];
                Tile top = overlay[row][col];
                Tile obj = objects[row][col];
                Enemy enemy = enemies[row][col];

                if (tile == Tile.NOT_SET)
                {
                    // We want to put a wall if the tile above, below, left, or right is set
                    bool putWall = (row > 0 && tiles[row - 1][col] != Tile.NOT_SET);
                    putWall = putWall || (col > 0 && tiles[row][col - 1] != Tile.NOT_SET);
                    putWall = putWall || (col < tiles[row].Length - 1 && tiles[row][col + 1] != Tile.NOT_SET);
                    putWall = putWall || (row < tiles.Length - 1 && tiles[row + 1][col] != Tile.NOT_SET);
                    if (putWall)
                    {
                        //InstantiateTile(wall, i, j);
                        obj = theme.GetMidWall();
                        tile = theme.midFloor;
                    }
                    if (obj == Tile.NOT_SET)
                    {
                        obj = theme.GetMidWall();
                        tile = theme.midFloor;
                    }
                }
                // Create the tile, overlay, and dungeon objects
                if (tile != Tile.NOT_SET) { InstantiateTile(tile, row, col); }
                // Create only one of these types, in this order of precedence:
                // 1. any dungeon object for this tile (i.e. a chest)
                // 2. any overlay for this tile (i.e. flowers)
                // 3. any enemy for this tile
               if (obj != Tile.NOT_SET) {
                    InstantiateTile(obj, row, col);
                } else if (top != Tile.NOT_SET) { 
                    InstantiateTile(top, row, col); 
                } else if(enemy != Enemy.NONE) {
                    InstantiateEnemy(enemy, row, col);
                }
            }
        }
    }


    private void InstantiateOuterWalls()
    {
        // The outer walls are one unit left, right, up and down from the board.
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    private void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
    {
        // Start the loop at the starting value for Y.
        float currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
            InstantiateFromArray(outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }


    private void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
    {
        // Start the loop at the starting value for X.
        float currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
            InstantiateFromArray(outerWallTiles, currentX, yCoord);

            currentX++;
        }
    }


    private void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }

    private void InstantiateTile(Tile tile, float xCoord, float yCoord)
    {
        if (tile == Tile.NOT_SET) return;
        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        Object tileObject;
        // try to load the gameobject from our cache, if it doesn't load, load it from resources and cache it.
        if (TilePool.ContainsKey(tile))
        {
            tileObject = TilePool[tile];
        }
        else
        {
            string tileName = System.Enum.GetName(typeof(Tile), tile);
            tileObject = Resources.Load("tiles/" + tileName);
            TilePool[tile] = tileObject;
        }
       
        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(tileObject, position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }

    private void InstantiateEnemy(Enemy enemy, float xCoord, float yCoord)
    {
        if (enemy == Enemy.NONE) return;
        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        Object enemyObject;
        // try to load the gameobject from our cache, if it doesn't load, load it from resources and cache it.
        if (EnemyPool.ContainsKey(enemy))
        {
            enemyObject = EnemyPool[enemy];
        }
        else
        {
            string enemyName = System.Enum.GetName(typeof(Enemy), enemy);
            enemyObject = Resources.Load("enemies/" + enemyName);
            EnemyPool[enemy] = enemyObject;
        }

        // Create an instance of the prefab from the random index of the array.
        GameObject enemyInstance = Instantiate(enemyObject, position, Quaternion.identity) as GameObject;

        // Set the enemy's parent to the board holder.
        enemyInstance.transform.parent = boardHolder.transform;
    }

}