using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallTile
{

    public const int ShrubGreen = 1;
    public const int ShrubOrange = 2;
    public const int ShrubDarkGreen = 3;
    public const int WallTan = 4;
    public const int WallGrey = 5;
    public const int WallWhite = 6;
    public const int WallBrown = 7;

    public const int MinTile = 1;
    private static int[] Tiles = new int[] {
        ShrubGreen,
        ShrubOrange,
        ShrubDarkGreen,
        WallTan,
        WallGrey,
        WallWhite,
        WallBrown
    };

    public static int GetRandomTile()
    {
        return UnityEngine.Random.Range(MinTile, Tiles.Length);
    }
}
