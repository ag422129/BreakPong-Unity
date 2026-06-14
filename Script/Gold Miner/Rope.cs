using UnityEngine;

public class RopeStretch : MonoBehaviour 
{ 
    private float speed = 8f; 
    private float pullbackSpeed = 20f; 
    public float maxLength = 20f; 
    private float currentLength = 1f; 
    public Hook hook; 
    private void Start() 
    { 

    } 
    void Update() 
    {
        speed = hook.shootSpeed * 2.9f;
        pullbackSpeed = hook.pullSpeed * 4.35f;
        if (hook.isShooting) 
        { 
            currentLength += speed * Time.deltaTime; 
        } 
        else 
        { 
            currentLength -= pullbackSpeed * Time.deltaTime; 
        } 
        
        currentLength = Mathf.Clamp(currentLength, 1f, maxLength); 
        transform.localScale = new Vector3(4f, currentLength, 1f); 
    } 
}