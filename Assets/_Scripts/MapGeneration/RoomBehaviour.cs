using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls; // 0 - Up, 1 - Down, 2 - Right, 3 - Left
    public GameObject[] doors;
    public GameObject[] woodDoors = new GameObject[4];
    public GameObject bossPrefab;
    public List<GameObject> enemies; // 현재 방에 있는 적 관리.

    // 0-Up, 1-Down, 2-Right, 3-Left
    public RoomBehaviour[] neighborRooms = new RoomBehaviour[4];

    private bool doorsLocked = false;

    public void UpdateRoom(bool[] status)
    {
        // status배열이 true면 그 방향은 door설정해주고 wall은 없애주기.
        for(int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
            woodDoors[i].SetActive(status[i]);
        }

        LockDoors();
    }

    public void SpawnBoss()
    {
        Instantiate(bossPrefab, transform.position, Quaternion.identity, transform);
    }

    public void LockDoors()
    {
        for (int i = 0; i < woodDoors.Length; i++)
        {
            if (woodDoors[i] != null && doors[i].activeSelf)
            {
                // Door가 열린상태일때만 잠금
                woodDoors[i].SetActive(true);
            }
        }
        doorsLocked = true;
    }

    public void UnlockDoors()
    {
        for (int i = 0; i < woodDoors.Length; i++)
        {
            if (woodDoors[i] != null)
            {
                woodDoors[i].SetActive(false);
            }
        }

        // 이웃 방의 반대쪽 문도 연다.
        /*  각자의 반대방향은
            0 - Up    => 1
            1 - Down  => 0
            2 - Right => 3
            3 - Left  => 2
        */
        if (neighborRooms[0] != null) neighborRooms[0].woodDoors[1].SetActive(false); // 위쪽 방의 아래 문
        if (neighborRooms[1] != null) neighborRooms[1].woodDoors[0].SetActive(false); // 아래쪽 방의 위 문
        if (neighborRooms[2] != null) neighborRooms[2].woodDoors[3].SetActive(false); // 오른쪽 방의 왼쪽 문
        if (neighborRooms[3] != null) neighborRooms[3].woodDoors[2].SetActive(false); // 왼쪽 방의 오른쪽 문

        doorsLocked = false;
    }

    private void Update()
    {
        if(doorsLocked && enemies.Count == 0)
        {
            UnlockDoors();
        }
    }

    public void OnEnemyKilled(GameObject enemy)
    {
        enemies.Remove(enemy);

        if(enemies.Count == 0)
        {
            UnlockDoors();
        }
    }
}
