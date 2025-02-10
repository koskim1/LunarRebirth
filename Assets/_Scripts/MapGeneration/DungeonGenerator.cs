using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
        public int distanceFromStart;
    }

    public Vector2 size;
    public int startPos = 0;
    public GameObject room;
    public Vector2 offset;
    public GameObject bossPrefab;
    public GameObject[] enemyPrefab;

    private bool firstRoom;

    List<Cell> board;

    public RoomBehaviour[,] roomGrid; // 2D �迭�� �� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    void GenerateDungeon()
    {
        roomGrid = new RoomBehaviour[(int)size.x, (int)size.y]; // roomGrid �ʱ�ȭ

        for (int i = 0; i < size.x; i++)
        {
            for(int j=0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];

                if (currentCell.visited)
                {
                    var newRoom = Instantiate(room, new Vector3(i * offset.x, 0, -j * offset.y),
                    Quaternion.identity, transform).GetComponent<RoomBehaviour>();

                    newRoom.UpdateRoom(currentCell.status);

                    // ������ �� üũ
                    // �̷��� �صθ� ���̵��� ������ �޶����� �׻� �������濡 ����.
                    if(i == size.x -1 && j == size.y -1)
                    {
                        newRoom.SpawnBoss();
                    }
                    else
                    {
                        if (i == 0 && j == 0) firstRoom = true;
                        else firstRoom = false;

                        // �Ϲ� �濡 �� ����
                        SpawnEnemies(newRoom, currentCell.distanceFromStart);                        
                    }

                    newRoom.name = room.name + " " + i + "-" + j;
                    roomGrid[i, j] = newRoom; // ���� roomGrid�� ����

                    // �̿� �� ����
                    if (i > 0 && roomGrid[i - 1, j] != null) // ���� ���� ���� ���� ����
                    {
                        newRoom.neighborRooms[3] = roomGrid[i - 1, j];
                        roomGrid[i - 1, j].neighborRooms[2] = newRoom;
                    }

                    if (j > 0 && roomGrid[i, j - 1] != null) // ���� ���� ���� ���� ����
                    {
                        newRoom.neighborRooms[0] = roomGrid[i, j - 1];
                        roomGrid[i, j - 1].neighborRooms[1] = newRoom;
                    }
                }
                
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for(int i=0;i< size.x; i++)
        {
            for(int j=0; j<size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        // ���� ����� ��� ����
        int k = 0;

        while (k < 1000)
        {
            k++;

            board[currentCell].visited = true;
            board[currentCell].distanceFromStart = path.Count;

            // ���� ������ ������ ��
            if(currentCell == board.Count - 1)
            {
                break;
            }

            // �� ���� �̿��� üũ�ϱ�.
            List<int> neighbors = CheckNeighbors(currentCell);

            if(neighbors.Count == 0)
            {
                if(path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if(newCell > currentCell)
                {
                    // ������ or �Ʒ�
                    if(newCell - 1 == currentCell)
                    {
                        // ������
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        // �Ʒ�
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    // ���� or ��
                    if (newCell + 1 == currentCell)
                    {
                        // ����
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        // ��
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }


        GenerateDungeon();
    }


    void SpawnEnemies(RoomBehaviour room, int distanceFromStart)
    {
        // ���� ���̿� ���� �� ���� ����

        // ��� ���������� ���� ���̿� ���� �� ����
        int enemyIndex = Mathf.Min(distanceFromStart / 2, enemyPrefab.Length - 1);

        // �������� �� �� ����
        int enemyCount = Random.Range(3, 6);

        for(int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab[enemyIndex], room.transform.position
                + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 15)), Quaternion.identity, room.transform);
            // �濡�ִ� ���� ������ Remove�������

            EnemyAttributesManager currentEnemy = enemy.GetComponent<EnemyAttributesManager>();

            if(currentEnemy != null)
            {
                currentEnemy.currentRoom = room;
            }

            room.enemies.Add(enemy);

            // ������ �� �޸𸮸� ���� ó�� ������ �� �����ϰ�� �ϴ� ��Ȱ��ȭ
            // �̿� ���� ������ RoomBehaviour.cs�� ActivateEnemies()���� �ϰ�����.
            if (!firstRoom)  enemy.SetActive(false);
        }
    }


    List<int> CheckNeighbors(int currentCell)
    {
        List<int> neighbors = new List<int>();

        // Check up neighbor
        if(currentCell - size.x >= 0 && !board[Mathf.FloorToInt(currentCell - size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(currentCell - size.x));
        }
        // Check down neighbor
        if (currentCell + size.x < board.Count && !board[Mathf.FloorToInt(currentCell + size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(currentCell + size.x));
        }
        // Check right neighbor
        if ((currentCell+1) % size.x != 0 && !board[Mathf.FloorToInt(currentCell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(currentCell + 1));
        }
        // Check left neighbor
        if (currentCell % size.x != 0 && !board[Mathf.FloorToInt(currentCell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(currentCell - 1));
        }

        return neighbors;
    }

}
