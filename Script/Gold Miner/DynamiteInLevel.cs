using System.Collections;
using UnityEngine;

public class DynamiteInLevel : MonoBehaviour
{
    private NormalControl control;
    private Hook hook;
    private float fallSpeed = 20f;

    public GameObject dynamitePrefab;
    public GameObject dynamiteExplode;
    private Vector3 dynamiteExplodePos;

    public float explosionRadius = 2.5f;
    public LayerMask hookableLayer;

    void Start()
    {
        control = FindObjectOfType<NormalControl>();
        hook = FindObjectOfType<Hook>();
    }

    void Update()
    {
        if (control.UsingDynamite)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        }
    }
    void ExplodeDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius,
            hookableLayer
        );

        foreach (Collider2D hit in hits)
        {
            TNT otherTNT = hit.GetComponent<TNT>();
            if (otherTNT != null)
            {
                otherTNT.Explode(0.4f);
            }

            Hookable hookable = hit.GetComponent<Hookable>();
            if (hookable != null && !hookable.CompareTag("TNT"))
            {
                Destroy(hit.gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Hookable hookable = other.GetComponent<Hookable>();
        if (hookable != null)
        {
            StartCoroutine(Explode());
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    IEnumerator Explode()
    {
        dynamiteExplodePos = transform.position;
        dynamitePrefab.SetActive(false);
        dynamiteExplode.transform.SetParent(null);
        dynamiteExplode.transform.position = dynamiteExplodePos;
        dynamiteExplode.SetActive(true);
        hook.pullSpeed = 5f;
        hook.hasGrabbed = false;
        ExplodeDamage();
        SoundManager.Instance.explodeSound.Play();
        control.UsingDynamite = false;
        yield return new WaitForSeconds(1f);
        dynamiteExplode.SetActive(false);
        Destroy(gameObject);
    }
}
