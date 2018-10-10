using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour {

    public int HookSize = 150;
    public GameObject Hook;
    public GameObject Bar;
    private Rigidbody2D hookBody;

    // Use this for initializationss
    void Start () {
        hookBody = Hook.gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space)){
            hookBody.AddForce(new Vector2(0, 6000f));
        }
	}
    
}
