using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthDisplay: MonoBehaviour
{
    public Image[] hearts;

    public void UpdateHP(int currentHP, int maxHP)
    {
        float cur = (float) currentHP;
        float max = (float) maxHP;

        if(cur < 0f){
            cur = 0f;
        }
        int totalHearts = (int) Math.Floor(max/4);
        int fullHearts = (int) Math.Floor(cur/4);

        int remainder = currentHP % 4;

        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

        for(int i = 0; i < fullHearts; i++){
            var newHeart = Instantiate(hearts[4], transform.position, Quaternion.identity);
            newHeart.transform.SetParent(gameObject.transform, false);
        }
        if(fullHearts < totalHearts){
            // This means there are unfilled hearts, so find out what they are
            // and display them
            var partialHeart = Instantiate(hearts[remainder], transform.position, Quaternion.identity);
            partialHeart.transform.SetParent(gameObject.transform, false);
            fullHearts++;
        }
        while(fullHearts < totalHearts){
            var emptyHeart = Instantiate(hearts[0], transform.position, Quaternion.identity);
            emptyHeart.transform.SetParent(gameObject.transform, false);
            fullHearts++;
        }
    }
}