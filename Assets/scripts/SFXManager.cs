using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX_TYPE
{
    SWORD_ATTACK, STAIRS_DOWN,
    PLAYER_HURT, LEVEL_UP, ENEMY_HURT,
    DEATH_GRUNT, DEATH_EXPLOSION,
    ATTACK_FAILED
};

public class SFXManager : MonoBehaviour {

    public AudioSource stairs_down;
    public AudioSource[] sword_sounds;
    public AudioSource[] music;
    public AudioSource level_up;
    public AudioSource[] enemy_hurt;
    public AudioSource[] player_hurt;
    public AudioSource death_grunt;
    public AudioSource death_explosion;

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
                int swFX = Random.Range(0, instance.sword_sounds.Length);
                instance.sword_sounds[swFX].Play();
                break;
            case SFX_TYPE.ENEMY_HURT:
                int hitFX = Random.Range(0, instance.enemy_hurt.Length);
                instance.enemy_hurt[hitFX].Play();
                break;
            case SFX_TYPE.PLAYER_HURT:
                int pHitFX = Random.Range(0, instance.player_hurt.Length);
                instance.player_hurt[pHitFX].Play();
                break;
            case SFX_TYPE.LEVEL_UP:
                instance.level_up.Play();
                break;
            case SFX_TYPE.DEATH_GRUNT:
                instance.death_grunt.Play();
                break;
            case SFX_TYPE.DEATH_EXPLOSION:
                instance.death_explosion.Play();
                break;
        }
    }

    public static void PauseMusic()
    {
        if (lastSong) lastSong.Stop();
    }

    public static void PauseSFX()
    {

    }

    public static void Pause()
    {
        lastSong.Pause();
    }

    public static void Resume()
    {
        lastSong.Play();
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
