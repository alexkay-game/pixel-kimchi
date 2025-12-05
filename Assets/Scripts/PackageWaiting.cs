using UnityEngine;

public class PackageWaiting : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {        
        if (!collision.CompareTag("Player"))     //Pokud gameObject NENÍ player, vrať se zpátky na začátek. Pokud gameObject JE player, podmínka projde dál.
            return;
        
        if (collision.TryGetComponent(out PackageHandler handler))
        {
            if (!handler.hasPackage)
            {
                handler.ActivatePackage();
                Destroy(gameObject);
            }

            else    Debug.Log("Player has a package");
        }
        
        else    Debug.Log("Player nemá PackageHandler script.");
        
    }

}
