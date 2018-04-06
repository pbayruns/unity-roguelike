using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingNumber : MonoBehaviour
{

    public float moveSpeed = 5;
    public int damage;
    public Text display;

    // Update is called once per frame
    void Update()
    {
        display.text = "-" + damage;
        transform.position = new Vector3(transform.position.x,
            transform.position.y + (moveSpeed * Time.deltaTime),
            transform.position.z);
    }
}
