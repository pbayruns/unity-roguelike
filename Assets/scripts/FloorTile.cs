using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloorTile {

    public const int Grass = 1;
    public const int DarkGrass = 2;
    public const int RedGrass = 3;
    public const int PurpleGrass = 4;
    public const int RedFlowers = 5;
    public const int WhiteFlowers = 6;
    public const int BlueFlowers = 7;
    public const int Dirt = 8;
    public const int Gravel = 9;
    public const int Sand = 10;
    public const int TanTile = 11;
    public const int GreyTile = 12;
    public const int WhiteTile = 13;
    public const int BrownTile = 14;
    public const int Wood = 15;
    public const int Cobble = 16;
    public const int TanCobble = 17;
    public const int Bricks1 = 18;
    public const int Bricks2 = 19;
    public const int Bricks3 = 20;
    public const int Bricks4 = 21;
    public const int Bricks5 = 22;
    public const int Bricks6 = 23;
    public const int Bricks7 = 24;
    public const int Bricks8 = 25;
    public const int Bricks9 = 26;
    public const int Bricks10 = 27;

    public const int MinTile = 1;
    private static int[] Tiles = new int[] {Grass, DarkGrass, RedGrass, PurpleGrass,
        RedFlowers, WhiteFlowers, BlueFlowers,
        Dirt, Gravel, Sand,
        TanTile, GreyTile, WhiteTile, BrownTile,
        Wood, Cobble, TanCobble,
        Bricks1, Bricks2, Bricks3, Bricks4, Bricks5,
        Bricks6, Bricks7, Bricks8, Bricks9, Bricks10 };

    public static int GetRandomTile()
    {
        return UnityEngine.Random.Range(MinTile, Tiles.Length);
    }
}
