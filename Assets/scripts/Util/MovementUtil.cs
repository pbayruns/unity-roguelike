using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUtil : MonoBehaviour {

    public static Vector2 GetRandomDirection()
    {
        int direction = Random.Range(0, 3);
        switch (direction)
        {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.down;
            case 2:
                return Vector2.left;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }
}
