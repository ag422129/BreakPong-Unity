using System.Collections;
using UnityEngine;

public class TNT : MonoBehaviour
{

    public GameObject TNTBarrel;
    public GameObject Explotion;

    public float explosionRadius = 2.5f;
    public LayerMask hookableLayer;

    private bool hasExploded = false;
    public void Explode(float delay = 0f)
    {
        if (hasExploded) return;
        hasExploded = true;

        StartCoroutine(ExplodeDelayed(delay));
    }

    IEnumerator BOOM()
    {
        Explotion.SetActive(true);
        SoundManager.Instance.explodeSound.Play();
        ExplodeDamage();
        yield return new WaitForSeconds(1f);
        Explotion.SetActive(false);
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
            if (otherTNT != null && otherTNT != this)
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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    IEnumerator ExplodeDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        TNTBarrel.SetActive(false);
        StartCoroutine(BOOM());
    }
}
