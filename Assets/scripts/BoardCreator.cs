using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tile
{
    //Floor Tiles
    GRASS_NORMAL, GRASS_DARK, GRASS_RED, GRASS_PURPLE,
    FLOWERS_RED, FLOWERS_WHITE, FLOWERS_BLUE,
    DIRT, GRAVEL, SAND,
    TILE_TAN, TILE_GREY, TILE_WHITE, TILE_BROWN,
    WOOD, COBBLE, COBBLE_TAN, COBBLE_GRASS,
    BRICKS1, BRICKS2, BRICKS3, BRICKS4, BRICKS5,
    BRICKS6, BRICKS7, BRICKS8, BRICKS9, BRICKS10,

    //Wall Tiles
    SHRUB_GREEN, SHRUB_ORANGE, SHRUB_DARK_GREEN,
    WALL_TAN, WALL_GREY, WALL_WHITE, WALL_BROWN,


    NOT_SET
}

public enum DungeonObject
{
    CHEST
}

public class BoardCreator : MonoBehaviour
{

    public GameObject GRASS_NORMAL;
    public GameObject GRASS_DARK;
    public GameObject GRASS_RED;
    public GameObject GRASS_PURPLE;
    public GameObject FLOWERS_BLUE;
    public GameObject DIRT;
    public GameObject TILE_GREY;
    public GameObject WALL_GREY;
    public GameObject COBBLE;

    private Dictionary<Tile, GameObject> TileObjects;

    public int columns = 100;                                 // The number of columns on the board (how wide it will be).
    public int rows = 100;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(15, 20);         // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange(3, 10);         // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange(3, 10);        // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange(6, 10);    // The range of lengths corridors between rooms can have.
    public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
    public GameObject stairs;
    private Tile[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private DungeonObject[] dungeonObjects;
    private Room[] rooms;                                     // All the rooms that are created for this board.
    private Corridor[] corridors;                             // All the corridors that connect the rooms.
    public GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
    public LevelTheme theme;
    public static bool firstLevel = true;
    private float stairsDepth;
    private Vector3 playerPos;
    //Tiles is an array with the dimensions of the specified rows and columns
    private void Start()
    {
     }

    public void SetupBoard(int level)
    {
        stairsDepth = Random.Range(0.3f, 0.8f);
        TileObjects = new Dictionary<Tile, GameObject>()
        {
            {Tile.GRASS_NORMAL, GRASS_NORMAL},
            {Tile.GRASS_DARK, GRASS_DARK},
            {Tile.GRASS_RED, GRASS_RED},
            {Tile.GRASS_PURPLE, GRASS_PURPLE},
            {Tile.FLOWERS_BLUE, FLOWERS_BLUE },
            {Tile.DIRT, DIRT},
            {Tile.TILE_GREY, TILE_GREY},
            {Tile.WALL_GREY, WALL_GREY},
            {Tile.COBBLE, COBBLE}
        };

        // Create the board holder.
        GameObject holder = GameObject.Find("BoardHolder");
        if (holder == null)
        {
            boardHolder = new GameObject("BoardHolder");
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
        theme = new LevelTheme();

        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValuesForCorridors();
        SetTilesValuesForRooms();

        InstantiateTiles();
        InstantiateOuterWalls();
    }


    void SetupTilesArray()
    {
        // Create the tiles array with the right rows and columns
        tiles = new Tile[columns][];
        for (int col = 0; col < tiles.Length; col++)
        {
            tiles[col] = new Tile[rows];
            for (int row = 0; row < tiles[col].Length; row++)
            {
                tiles[col][row] = Tile.NOT_SET;
            }
        }
    }


    void CreateRoomsAndCorridors()
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


            if (true)//(i == rooms.Length * .5f)
            {
                playerPos = new Vector3(rooms[i].xPos, rooms[i].yPos, 0f);
                Player.Move(playerPos);
            }
            if(i == (int) (rooms.Length * stairsDepth))
            {
                int offsetY = Random.Range(0, rooms[i].roomHeight);
                int offsetX = Random.Range(0, rooms[i].roomWidth);
                Vector3 stairsPos = new Vector3(rooms[i].xPos + offsetX, rooms[i].yPos + offsetY, 0);
                GameObject stairsInstance = Instantiate(stairs, stairsPos, Quaternion.identity);
                stairsInstance.transform.parent = boardHolder.transform;
            }
        }
    }

    void SetTilesValuesForRooms()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];
            Tile[][] roomTiles = theme.GetRoomTiles(currentRoom.roomWidth, currentRoom.roomHeight);
            //DungeonObject[] roomObjects = theme.GetDungeonObjects(currentRoom.roomWidth, currentRoom.roomHeight);

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;
                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;
                    Tile tile = roomTiles[j][k];
                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = tile;
                }
            }
        }
    }

    void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
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
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                Tile tile = theme.GetCorridorTile();
                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = tile;
            }
        }
    }


    void InstantiateTiles()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                Tile tile = tiles[i][j];
                if (tile == Tile.NOT_SET) {

                    if(i > 0)
                    {
                        if(tiles[i - 1][j] != Tile.NOT_SET)
                        {
                            InstantiateTile(Tile.WALL_GREY, i, j);
                        }
                    }
                    if (j > 0)
                    {
                        if(tiles[i][j - 1] != Tile.NOT_SET)
                        {
                            InstantiateTile(Tile.WALL_GREY, i, j);
                        }
                    }

                    if(j < tiles[i].Length - 1)
                    {
                        if(tiles[i][j + 1] != Tile.NOT_SET)
                        {
                            InstantiateTile(Tile.WALL_GREY, i, j);
                        }
                    }
                    if (i < tiles.Length - 1)
                    {
                        if(tiles[i + 1][j] != Tile.NOT_SET)
                        {
                            InstantiateTile(Tile.WALL_GREY, i, j);
                        }
                    }
                }
                else if (tile != Tile.NOT_SET)
                {
                    InstantiateTile(tile, i, j);
                }
            }
        }
    }


    void InstantiateOuterWalls()
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


    void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
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


    void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
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


    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
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

    void InstantiateTile(Tile tile, float xCoord, float yCoord)
    {
        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Get the gameobject for this tile from the dictionary
        GameObject tileObject = TileObjects[tile];

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(tileObject, position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }
}