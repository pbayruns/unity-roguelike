using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Colorutil {

    public static Color GetColor(int r, int g, int b, float alpha = 1.0f){
        float red = r / 255f;
        float green = g / 255f;
        float blue = b / 255f;
        return new Color(red, green, blue, alpha);
    }
}