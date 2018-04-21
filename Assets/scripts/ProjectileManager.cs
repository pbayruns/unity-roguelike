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
    void Awake()
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
        if(instance == null || instance.projectiles == null || instance.projectiles.Count == 0)
        {
            return;
        }
		for(int i = 0; i < instance.projectiles.Count; i++)
        {
            ProjectileInfo info = instance.projectiles[i];
            if(info.body != null)
            {
                info.body.velocity = info.velocity;
            }
            else
            {
                instance.projectiles.RemoveAt(i);
                i--;
            }
        }
	}

    public static void CreateProjectile(GameObject prefab, Vector3 position, Vector2 velocity, Quaternion rotation)
    {
        Vector3 predictedPos = new Vector2(position.x, position.y) + velocity * 0.5f;
        GameObject projectileObj = Instantiate(prefab, predictedPos, rotation);
        Rigidbody2D projectile = projectileObj.GetComponent<Rigidbody2D>();
        projectileObj.transform.rotation = rotation;
        ProjectileInfo info = new ProjectileInfo();
        info.body = projectile;
        info.velocity = velocity;
        instance.projectiles.Add(info);
    }
}
