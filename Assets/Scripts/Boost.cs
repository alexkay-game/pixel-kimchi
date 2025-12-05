using UnityEngine;

public class Boost : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<MovingPlayer>().StartBoost();
            Destroy(gameObject);
        }
    }
}
