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

    public RoomBehaviour[,] roomGrid; // 2D 배열로 각 방을 저장

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    void GenerateDungeon()
    {
        roomGrid = new RoomBehaviour[(int)size.x, (int)size.y]; // roomGrid 초기화

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

                    // 마지막 방 체크
                    // 이렇게 해두면 난이도나 방사이즈가 달라져도 항상 마지막방에 보스.
                    if(i == size.x -1 && j == size.y -1)
                    {
                        newRoom.SpawnBoss();
                    }
                    else
                    {
                        if (i == 0 && j == 0) firstRoom = true;
                        else firstRoom = false;

                        // 일반 방에 적 생성
                        SpawnEnemies(newRoom, currentCell.distanceFromStart);                        
                    }

                    newRoom.name = room.name + " " + i + "-" + j;
                    roomGrid[i, j] = newRoom; // 방을 roomGrid에 저장

                    // 이웃 방 설정
                    if (i > 0 && roomGrid[i - 1, j] != null) // 왼쪽 방이 있을 때만 설정
                    {
                        newRoom.neighborRooms[3] = roomGrid[i - 1, j];
                        roomGrid[i - 1, j].neighborRooms[2] = newRoom;
                    }

                    if (j > 0 && roomGrid[i, j - 1] != null) // 위쪽 방이 있을 때만 설정
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

        // 현재 어딘지 계속 추적
        int k = 0;

        while (k < 1000)
        {
            k++;

            board[currentCell].visited = true;
            board[currentCell].distanceFromStart = path.Count;

            // 현재 보드의 마지막 셀
            if(currentCell == board.Count - 1)
            {
                break;
            }

            // 각 셀의 이웃들 체크하기.
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
                    // 오른쪽 or 아래
                    if(newCell - 1 == currentCell)
                    {
                        // 오른쪽
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        // 아래
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    // 왼쪽 or 위
                    if (newCell + 1 == currentCell)
                    {
                        // 왼쪽
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        // 위
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
        // 던전 길이에 따른 적 강도 설정

        // 출발 지점에서로 부터 깊이에 따라 적 선택
        int enemyIndex = Mathf.Min(distanceFromStart / 2, enemyPrefab.Length - 1);

        // 랜덤으로 적 수 선택
        int enemyCount = Random.Range(3, 6);

        for(int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab[enemyIndex], room.transform.position
                + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 15)), Quaternion.identity, room.transform);
            // 방에있는 적이 죽으면 Remove해줘야함

            EnemyAttributesManager currentEnemy = enemy.GetComponent<EnemyAttributesManager>();

            if(currentEnemy != null)
            {
                currentEnemy.currentRoom = room;
            }

            room.enemies.Add(enemy);

            // 프레임 및 메모리를 위해 처음 입장한 방 제외하고는 일단 비활성화
            // 이에 대한 관리는 RoomBehaviour.cs의 ActivateEnemies()에서 하고있음.
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
