using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Obstacles {
    public GameObject obstacle;
    [Range(1, 100)]
    public int rarity;
    public bool isPowerup;
}

public class Spawner : MonoBehaviour {

    public float spawnRate = 2f;
    [Header("Higher rarity is more common")]
    public List<Obstacles> obstacles = new List<Obstacles>();
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    private Transform floorTile;

    private OddmentTable<GameObject> obstacleTable = new OddmentTable<GameObject>();

	// Use this for initialization
	void Start () {
        
        foreach(Obstacles obj in obstacles) {
            if (obj.rarity != 0)
                obstacleTable.Add(obj.obstacle, obj.rarity);
            else
                obstacleTable.Add(obj.obstacle, 1);
            obj.obstacle.CreatePool();
        }

        InvokeRepeating("Spawn", 1, spawnRate);
    }

    // Update is called once per frame
    void Update () {
        if (spawnedObstacles.Count > 0) {
            for (int i = 0; i < spawnedObstacles.Count; i++) {
                if (spawnedObstacles[i] != null && spawnedObstacles[i].transform.position.z < -10) {
                    spawnedObstacles[i].Recycle();
                    spawnedObstacles.Remove(spawnedObstacles[i]);
                }
            }
        }
    }

    void OnValidate()
    {
        if (obstacleTable.Count > 0) {
            foreach (Obstacles obj in obstacles) {
                obstacleTable.Remove(obj.obstacle);
                obstacleTable.Add(obj.obstacle, obj.rarity);
            }
        }
    }

    void Spawn()
    {
        GameObject result = obstacleTable.Pick();
        GameObject spawnedObstacle;

        spawnedObstacle = result.Spawn(new Vector3(Random.Range(-4.5f, 4.5f), result.transform.position.y, transform.position.z), result.transform.rotation);
        spawnedObstacle.name = result.name;
        spawnedObstacles.Add(spawnedObstacle);

        RaycastHit hit;
        if (Physics.Raycast(spawnedObstacle.transform.position, Vector3.down, out hit, 20)) {
            spawnedObstacle.transform.parent = hit.transform;
        }

    }
}
