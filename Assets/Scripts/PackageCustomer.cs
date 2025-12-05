using Unity.VisualScripting;
using UnityEngine;

public class PackageCustomer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float packageDuration;
    
    [Header("Debug")]
    [SerializeField] float packageTimer;
    
    bool isVisible;
    
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.enabled = false;
    }

    void Update()
    {
        if (isVisible)
        {
            packageTimer -= Time.deltaTime;
            if (packageTimer <= 0)
            {
                sr.enabled = false;
                isVisible = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PackageHandler handler))
        {
            if (handler.hasPackage && !isVisible)
            {
                sr.enabled = true;
                handler.DeactivatePackage();
                StartCountDown();
            }
        }

        else Debug.Log("Player nemá PackageHandler script.");
    }

    void StartCountDown()
    {
        packageTimer = packageDuration;
        isVisible = true;
    }
}
