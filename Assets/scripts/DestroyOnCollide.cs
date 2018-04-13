using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour {

    public string[] tagsToDestroyOn;
    public GameObject explosion;
    public float delay = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Array.IndexOf(tagsToDestroyOn, other.gameObject.tag) != -1)
        {
            Invoke("DestroySelf", delay);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
