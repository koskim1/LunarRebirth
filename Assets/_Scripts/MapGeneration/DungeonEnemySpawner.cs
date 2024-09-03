using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private int xPos;
    private int zPos;
    private int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        xPos = Random.Range(-28, 28);
        zPos = Random.Range(-30, 20);
        //Instantiate(enemyPrefab, new Vector3(xPos, 1, zPos), Quaternion.identity);
    }
}
