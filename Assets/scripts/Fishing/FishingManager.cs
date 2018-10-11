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
        if(Input.GetKeyDown(KeyCode.Alpha7)){
            this.transform.position = Player.GetPosition() + new Vector3(1.0f, 0.5f, 0f);
        }
        if (Input.GetKey(KeyCode.Space)){
            hookBody.AddForce(new Vector2(0, 300f));
        }
        
	}
    
}
