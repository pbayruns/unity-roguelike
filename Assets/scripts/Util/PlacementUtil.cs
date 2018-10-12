using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PlacementUtil {


    public static Vector3 getRandomPosition(Vector3 center, float minRadius, float maxRadius = -1f){

//TODO: this shit's broke idk need to think about it
        if(maxRadius == -1f){
            maxRadius = minRadius;
        }
        // We want to spawn not on the center, 
        // O O O O O
        // O O O O O
        // O O X O O 
        // O O O O O
        // O O O O O

        // whether we'll be going up, down
        int xDir = Random.Range(0,3);
        int yDir = Random.Range(0,3);

        // y values we want: 1, 0, 1
        // x values we want: -1, 0, 1
        // -1, 0, or 1
        int xMult = Random.Range(0,3) - 1;
        int yMult = Random.Range(0,3) - 1;

        float xOffset = Random.Range(minRadius, maxRadius) * xMult;
        float yOffset = Random.Range(minRadius, maxRadius) * yMult;
        float x = xOffset  + center.x;
        float y = yOffset + center.y;
        Vector3 result = new Vector3(x, y, 0f);
        return result;
    }

    public static Vector3 PosAtZ(float z, GameObject gameObject)
    {
        Vector3 pos = gameObject.transform.position;
        pos.z = z;
        return pos;
    }
}