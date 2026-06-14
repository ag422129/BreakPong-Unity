using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] BlockPrefab;
    public GameObject SpawnPos;
    public GameObject DisplayPos;

    private GameObject currentBlock;
    private GameObject NextBlock;
    private GameObject NextBlockPrefab;

    public FangKuai_UI Lost;
    private void Start()
    {
        PickNextBlock();
        SpawnBlock();
    }
    private void Update()
    {
        if (!Lost.GameLost)
        {
            if (currentBlock == null)
            {
                SpawnBlock();
            }
            else
            {
                fangKuaiMove moveScript = currentBlock.GetComponent<fangKuaiMove>();
                if (moveScript != null && moveScript.isLocked)
                {
                    currentBlock = null;
                }
            }
        }
    }

    void SpawnBlock()
    {
        currentBlock = Instantiate(NextBlock, SpawnPos.transform.position, Quaternion.identity);
        PickNextBlock();
    }

    void PickNextBlock()
    {
        NextBlock = BlockPrefab[Random.Range(0, BlockPrefab.Length)];

        if(NextBlockPrefab != null)
        {
            Destroy(NextBlockPrefab);
        }

        NextBlockPrefab = Instantiate(NextBlock, DisplayPos.transform.position, Quaternion.identity);

        DisableBlockLogic(NextBlockPrefab);
    }

    void DisableBlockLogic(GameObject Block)
    {
        fangKuaiMove move = Block.GetComponent<fangKuaiMove>();
        if (move != null)
            Destroy(move);

    }
}

