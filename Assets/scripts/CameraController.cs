using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //public GameObject followTarget;
    private GameObject followTarget;
    private Vector3 targetPos;
    public float speed;

    private static bool cameraExists;

    public BoxCollider2D boundBox;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private Camera cam;
    private float halfHeight;
    private float halfWidth;

    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(transform.gameObject);
        //if (!cameraExists)
        //{
        //    cameraExists = true;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        cam = GetComponent<Camera>();
        followTarget = gameObject.transform.parent.gameObject;
        //if (boundBox == null)
        //{
        //    boundBox = FindObjectOfType<Bounds>().GetComponent<BoxCollider2D>();
        //    minBounds = boundBox.bounds.min;
        //    maxBounds = boundBox.bounds.max;
        //}

        //minBounds = boundBox.bounds.min;
        //maxBounds = boundBox.bounds.max;

        //halfHeight = cam.orthographicSize;
        //halfWidth = halfHeight * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {

        targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

        //if (boundBox == null)
        //{
        //    boundBox = FindObjectOfType<Bounds>().GetComponent<BoxCollider2D>();
        //    minBounds = boundBox.bounds.min;
        //    maxBounds = boundBox.bounds.max;
        //}
        //float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        //float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
        //const float ratio = .0625f;
        //float roundedX = (int)(clampedX / ratio) * ratio;
        //float roundedY = (int)(clampedY / ratio) * ratio;
        //transform.position = new Vector3(roundedX, roundedY, transform.position.z);
    }

    //public void SetBounds(BoxCollider2D newBounds)
    //{
    //    boundBox = newBounds;
    //    minBounds = boundBox.bounds.min;
    //    maxBounds = boundBox.bounds.max;
    //}
}