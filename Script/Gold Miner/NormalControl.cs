using UnityEngine;

public class NormalControl : MonoBehaviour
{
    public float rotateSpeed = 60f;     // 左右摆动速度
    public float maxAngle = 80f;        // 最大摆角

    public Hook hook;

    public bool allowSwing = true;

    public GameObject UseDynamite;
    public bool UsingDynamite = false;

    public Sprite normalHookSprite;
    public Sprite goldenHookSprite;
    private SpriteRenderer hookSR;
    void Start()
    {
        hook = GetComponentInChildren<Hook>();
        hookSR = hook.GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (!hook.isShooting && !hook.isPulling && allowSwing && !GM_GameManager.Instance.StartTransit)
        {
            if (ItemManager.Instance.GoldenHook)
            {
                hookSR.sprite = goldenHookSprite;
                FollowMouse();
            }
            else
            {
                hookSR.sprite = normalHookSprite;
                Swing();
            }
        }

        if (ItemManager.Instance.bombCount >= 1 && hook.hasGrabbed && Input.GetKeyDown(KeyCode.Space) && !UsingDynamite)
        {

            UsingDynamite = true;
            Instantiate(UseDynamite, new Vector3(0, 3.7f, 0), hook.transform.rotation);
            ItemManager.Instance.bombCount -= 1;
            GoldMiner_UI.Instance.dynamites[ItemManager.Instance.bombCount].SetActive(false);
        }

    }
    void Swing()
    {
        float angle = Mathf.Sin(Time.time * rotateSpeed * Mathf.Deg2Rad) * maxAngle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FollowMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = transform.position.z;

        Vector2 dir = worldPos - transform.position;

        transform.up = -dir;
    }

}
