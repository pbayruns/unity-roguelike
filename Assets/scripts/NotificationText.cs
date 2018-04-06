using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationText : MonoBehaviour
{

    private Text display;
    private Outline outline;
    private DestroyOverTime timer;
    private float moveSpeed;

    // Use this for initialization
    void Start()
    {
        display = GetComponentInChildren<Text>();
        outline = GetComponentInChildren<Outline>();
        timer = GetComponent<DestroyOverTime>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,
    transform.position.y + (moveSpeed * Time.deltaTime),
    transform.position.z);
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetText(string message)
    {
        display = GetComponentInChildren<Text>();
        display.text = message;
    }

    public void SetColor(Color color)
    {
        outline = GetComponentInChildren<Outline>();
        outline.effectColor = color;
    }

    public void SetTime(float time)
    {
        timer = GetComponent<DestroyOverTime>();
        timer.timeToDestroy = time;
    }
}
