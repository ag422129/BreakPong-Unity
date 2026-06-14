using UnityEngine;

public class Hook : MonoBehaviour
{
    public float shootSpeed = 8f; 
    public float pullSpeed = 5f; 

    private Vector3 startPos;

    public bool isShooting = false;
    public bool isPulling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Hookable grabbObject;
    public bool hasGrabbed = false;
    public NormalControl control;

    public AudioSource RopeScrech;
    public AudioSource ShootHook;
    public AudioSource HookedSomething;
    public AudioSource GainedMoney;
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {


        if (!isShooting && !isPulling && control.allowSwing && !GM_GameManager.Instance.StartTransit)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isShooting = true;
                ShootHook.Play();
            }
        }
        if (isShooting)
        {
            transform.Translate(Vector3.down * shootSpeed * Time.deltaTime);
        }

        if (isPulling)
        {
            if (ItemManager.Instance.Power)
            {
                pullSpeed = 5;
            }
           transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                startPos,
                pullSpeed * Time.deltaTime
            );
            if (!RopeScrech.isPlaying)
            {
                RopeScrech.Play();
            }
            if (Vector3.Distance(transform.localPosition, startPos) < 0.1f)
            {
                if (grabbObject != null)
                {
                    grabbObject.OnCollected();
                }
                ResetHook();
            }
        }
        else
        {
            if (RopeScrech.isPlaying)
            {
                RopeScrech.Stop();
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (hasGrabbed)
        {
            return;
        }
        if(isShooting && other.CompareTag("Wall"))
        {
            isShooting = false;
            isPulling = true;
        }

        Hookable hookable = other.GetComponent<Hookable>();
        if (hookable != null)
        {
            if(hookable.type == Hookable.HookableType.Bag)
            {
                hookable.weight = Random.Range(1f, 4.5f);
            }
            if (hookable.type == Hookable.HookableType.TNT)
            {
                TNT tnt = other.GetComponent<TNT>();
                tnt.Explode();
            }

            grabbObject = hookable;
            hasGrabbed = true;
            HookedSomething.Play();
            pullSpeed -= hookable.weight;

            hookable.AttachToHook(transform);

            isShooting = false;
            isPulling = true;
        }
    }

    public void ResetHook()
    {
        isShooting = false;
        isPulling = false;
        grabbObject = null;
        hasGrabbed = false;
        pullSpeed = 5;
        transform.localPosition = startPos;
    }

}
