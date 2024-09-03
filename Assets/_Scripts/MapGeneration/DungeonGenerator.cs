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

    List<Cell> board;

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    void GenerateDungeon()
    {
        for(int i = 0; i < size.x; i++)
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
                        // 일반 방에 적 생성
                        SpawnEnemies(newRoom, currentCell.distanceFromStart);
                    }

                    newRoom.name = room.name + " " + i + "-" + j;
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

        // Keep Track where we at.
        int k = 0;

        while (k < 1000)
        {
            k++;

            board[currentCell].visited = true;
            board[currentCell].distanceFromStart = path.Count;

            // Last Cell of our board
            if(currentCell == board.Count - 1)
            {
                break;
            }

            //Check the cell's neighbors
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
                    //down or right
                    if(newCell - 1 == currentCell)
                    {
                        // right
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        // down
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //up or left
                    if (newCell + 1 == currentCell)
                    {
                        // left
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        // up
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

        // 깊이에 따라 적 선택
        int enemyIndex = Mathf.Min(distanceFromStart / 2, enemyPrefab.Length - 1);

        // 랜덤으로 적 수 선택
        int enemyCount = Random.Range(3, 6);

        for(int i = 0; i < enemyCount; i++)
        {
            var enemy = Instantiate(enemyPrefab[enemyIndex], room.transform.position
                + new Vector3(Random.Range(-28, 28), 0, Random.Range(-30, 20)), Quaternion.identity, room.transform);
            room.enemies.Add(enemy);
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
