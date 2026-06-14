using UnityEngine;

public class fangKuaiMove : MonoBehaviour
{
    public static int width = 13;
    public static int height = 25;
    public static Transform[,] grid = new Transform[width, height];

    public float fallTime = 1f;
    private float lastFallTime;

    public bool isLocked = false;

    public static bool GameStart = false;
    public FangKuai_UI Scores;

    private void Start()
    {
        Scores = FindObjectOfType<FangKuai_UI>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameStart = true;
        }
        if (GameStart)
        {
            if (isLocked)
                return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.position += Vector3.left;
                if (!ValidMove())
                {
                    transform.position += Vector3.right;
                }
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.position += Vector3.right;
                if (!ValidMove())
                {
                    transform.position += Vector3.left;
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                transform.Rotate(0, 0, 90);
                if (!ValidMove())
                {
                    transform.Rotate(0, 0, -90);
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                fallTime = 0.05f;
            }
            else
            {
                fallTime = 1f;
            }
            if (Time.time - lastFallTime > fallTime)
            {
                transform.position += Vector3.down;
                if (!ValidMove())
                {
                    transform.position += Vector3.up;
                    AddToGrid();
                    CheckLines();
                    isLocked = true;
                }

                lastFallTime = Time.time;
            }
        }
}

    bool ValidMove()
    {
        foreach (Transform T in transform)
        {
            Vector2 Pos = Round(T.position);

            if (Pos.x < 0 || Pos.x >= width || Pos.y < 0)
                return false;

            if (grid[(int)Pos.x, (int)Pos.y] != null)
                return false;
        }
        return true;
    }

    void AddToGrid()
    {
        foreach (Transform T in transform)
        {
            Vector2 pos = Round(T.position);
            grid[(int)pos.x, (int)pos.y] = T;
            if(pos.y >= 21)
            {
                Scores.Lost();
            }
        }
    }

    Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    void DeleteLine(int y)
    {
        for(int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    bool IsLineFulll(int y)
    {
        for(int x = 0; x < width; x++)
        {
            if(grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    void MoveDownLines(int PreviousY)
    {
        for(int y = PreviousY + 1; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if(grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;

                    grid[x, y - 1].position += Vector3.down;
                }
            }
        }
    }
    void CheckLines()
    {
        for(int y = 0; y < height; y++)
        {
            if (IsLineFulll(y))
            {
                DeleteLine(y);
                Scores.Score += 10;
                MoveDownLines(y);
                y--;
            }
        }
    }

}
