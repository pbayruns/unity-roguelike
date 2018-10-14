using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class SpriteFlashUtil
{

    // public static void DamageFlash(SpriteRenderer sprite, Color flashColor, Color normalColor)
    // {
    //     float flashTime = 0.05f;
    //     sprite.color = flashColor;
    //     StartCoroutine(SetColor(sprite, normalColor, flashTime));
    //     StartCoroutine(SetColor(sprite, flashColor, flashTime * 2));
    //     StartCoroutine(SetColor(sprite, normalColor, flashTime * 3));
    //     StartCoroutine(SetColor(sprite, flashColor, flashTime * 4));
    //     StartCoroutine(SetColor(sprite, normalColor, flashTime * 10));
    // }

    // public static IEnumerator SetColor(SpriteRenderer sprite, Color color, float delayTime = 0.2f)
    // {
    //     yield return new WaitForSeconds(delayTime);
    //     if(sprite != null) sprite.color = color;
    // }
}