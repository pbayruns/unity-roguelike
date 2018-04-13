using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {

    public static ProjectileManager instance = null;
    private List<ProjectileInfo> projectiles;

    struct ProjectileInfo
    {
        public Rigidbody2D body;
        public Vector2 velocity;
    }

    //Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        projectiles = new List<ProjectileInfo>();
    }
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < projectiles.Count; i++)
        {
            ProjectileInfo info = projectiles[i];
            if(info.body != null)
            {
                info.body.velocity = info.velocity;
            }
            else
            {
                projectiles.RemoveAt(i);
                i--;
            }
        }
	}

    public static void CreateProjectile(GameObject prefab, Vector3 position, Vector3 velocity, Quaternion rotation)
    {
        GameObject projectileObj = Instantiate(prefab, position, rotation);
        Rigidbody2D projectile = projectileObj.GetComponent<Rigidbody2D>();
        projectile.transform.rotation = rotation;
        ProjectileInfo info = new ProjectileInfo();
        info.body = projectile;
        info.velocity = velocity;
        instance.projectiles.Add(info);
    }
}
