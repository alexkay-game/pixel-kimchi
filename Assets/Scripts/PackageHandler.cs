using UnityEngine;

public class PackageHandler : MonoBehaviour
{
    public bool hasPackage;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.enabled = false;
        hasPackage = false;
    }

    public void ActivatePackage()
    {
        sr.enabled = true;
        hasPackage = true;
    }

    public void DeactivatePackage()
    {
        sr.enabled = false;
        hasPackage = false;
    }
}
