using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    public int maxAmountOfEnemies;

    public float spawnTimer = 2;

    public List<GameObject> enemies;

    [Header ("Spawn Radius")]
    public List<WaypointDistance> inRangePoints;

    WaypointDistance[] waypoints;

    public float maxSpawnDistance = 30;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new WaypointDistance[temp.Length];

        for (int i = 0; i < temp.Length; i++)
        {
            waypoints[i] = temp[i].GetComponent<WaypointDistance>();
        }
        inRangePoints = new List<WaypointDistance>();

        GameObject[] temp2 = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = new List<GameObject>();
        for (int i = 0; i < temp2.Length; i++)
        {
            enemies.Add(temp2[i]);
        }
    }

    void Update()
    {
        SpawnEnemies();
    }

    void LateUpdate()
    {
        UpdateWaypointList();      
    }

    void UpdateWaypointList()
    {
        foreach (WaypointDistance waypoint in waypoints)
        {
            if (waypoint.distanceFromPlayer <= maxSpawnDistance)
            {
                if (!inRangePoints.Contains(waypoint))
                {
                    inRangePoints.Add(waypoint);
                }
            }
            else
            {
                if (inRangePoints.Contains(waypoint))
                {
                    inRangePoints.Remove(waypoint);
                }
            }
        }
    }

    void SpawnEnemies()
    {
        if (enemies.Count >= 0)
        {
            if (enemies.Count >= maxAmountOfEnemies)
            {
                timer = 0;
                return;
            }

            timer += Time.deltaTime;

            if (timer >= spawnTimer && inRangePoints.Count > 0)
            {
                GameObject enemy = Instantiate(enemyPrefab, inRangePoints[Random.Range(0, inRangePoints.Count)].transform);
                enemies.Add(enemy);

                timer = 0;
            }
        }
    }

    public void RemoveFromList(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    
}
