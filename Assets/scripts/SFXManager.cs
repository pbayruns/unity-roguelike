using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX_TYPE
{
    SWORD_ATTACK, STAIRS_DOWN
};

public class SFXManager : MonoBehaviour {

    public AudioSource stairs_down;
    public AudioSource[] sword_sounds;
    public AudioSource[] music;
    private static AudioSource lastSong = null;
    public static SFXManager instance = null; //singleton

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public static void PlaySFX(SFX_TYPE sfx)
    {
        switch (sfx)
        {
            case SFX_TYPE.STAIRS_DOWN:
                instance.stairs_down.Play();
                break;
            case SFX_TYPE.SWORD_ATTACK:
                int i = Random.Range(0, instance.sword_sounds.Length);
                instance.sword_sounds[i].Play();
                break;
        }
    }

    public static void PlayMusic()
    {
        if(lastSong) lastSong.Stop();
        lastSong = instance.music[Random.Range(0, instance.music.Length)];
        lastSong.Play();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
