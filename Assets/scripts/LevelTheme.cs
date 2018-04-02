using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTheme {

    public const int THEME_GRAY = 1;
    private Tile[] gray_theme_tiles = new Tile[]
    {
        Tile.DIRT,
        Tile.WALL_GREY,
        Tile.TILE_GREY
    };

    public const int GRASS = 2;
    public const int GRASS_FLOWERS = 3;

    public const int MIN_THEME = 1;
    public const int MAX_THEME = 3;

    public int theme;

    public LevelTheme()
    {
        //theme = UnityEngine.Random.Range(MIN_THEME, MAX_THEME);
        theme = 1;
    }

    public Tile[][] GetRoomTiles(int width, int height)
    {
        Tile[][] tiles = GetTilesArray(width, height);
        for(int x = 0; x < tiles.Length; x++)
        {
            Tile[] row = tiles[x];
            for(int y = 0; y < row.Length; y++)
            {
                tiles[x][y] = Tile.GRASS_NORMAL;
            }
        }
        return tiles;
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
