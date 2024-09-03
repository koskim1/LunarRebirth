using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls; // 0 - Up, 1 - Down, 2 - Right, 3 - Left
    public GameObject[] doors;
    public GameObject[] upWoodDoors;
    public GameObject[] downWoodDoors;
    public GameObject[] rightWoodDoors;
    public GameObject[] leftWoodDoors;

    public GameObject bossPrefab;
    public List<GameObject> enemies; // 현재 방에 있는 적 관리.

    private bool doorsLocked = false;

    public void UpdateRoom(bool[] status)
    {
        // status배열이 true면 그 방향은 door설정해주고 wall은 없애주기.
        for(int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }

        LockDoors();
    }

    public void SpawnBoss()
    {
        Instantiate(bossPrefab, transform.position, Quaternion.identity, transform);
    }

    public void LockDoors()
    {
        LockOrUnlockDirectionDoors(upWoodDoors, true);
        LockOrUnlockDirectionDoors(downWoodDoors, true);
        LockOrUnlockDirectionDoors(rightWoodDoors, true);
        LockOrUnlockDirectionDoors(leftWoodDoors, true);

        doorsLocked = true;
    }

    public void UnlockDoors()
    {
        LockOrUnlockDirectionDoors(upWoodDoors, false);
        LockOrUnlockDirectionDoors(downWoodDoors, false);
        LockOrUnlockDirectionDoors(rightWoodDoors, false);
        LockOrUnlockDirectionDoors(leftWoodDoors, false);

        doorsLocked = false;
    }

    private void LockOrUnlockDirectionDoors(GameObject[] doors, bool lockStatus)
    {
        foreach (var door in doors)
        {
            if (door != null)
                door.SetActive(lockStatus);
        }
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
