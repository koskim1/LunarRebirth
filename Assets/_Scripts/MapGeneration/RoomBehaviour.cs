using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls; // 0 - Up, 1 - Down, 2 - Right, 3 - Left
    public GameObject[] doors;
    public GameObject[] woodDoors = new GameObject[4];
    public GameObject bossPrefab;
    public List<GameObject> enemies; // ���� �濡 �ִ� �� ����.

    // 0-Up, 1-Down, 2-Right, 3-Left
    public RoomBehaviour[] neighborRooms = new RoomBehaviour[4];

    private bool doorsLocked = false;

    public void UpdateRoom(bool[] status)
    {
        // status�迭�� true�� �� ������ door�������ְ� wall�� �����ֱ�.
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
                // Door�� ���������϶��� ���
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

        // �̿� ���� �ݴ��� ���� ����.
        /*  ������ �ݴ������
            0 - Up    => 1
            1 - Down  => 0
            2 - Right => 3
            3 - Left  => 2
        */
        if (neighborRooms[0] != null) neighborRooms[0].woodDoors[1].SetActive(false); // ���� ���� �Ʒ� ��
        if (neighborRooms[1] != null) neighborRooms[1].woodDoors[0].SetActive(false); // �Ʒ��� ���� �� ��
        if (neighborRooms[2] != null) neighborRooms[2].woodDoors[3].SetActive(false); // ������ ���� ���� ��
        if (neighborRooms[3] != null) neighborRooms[3].woodDoors[2].SetActive(false); // ���� ���� ������ ��

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
