using UnityEngine;

public class Potion : MonoBehaviour
{
    public float healPoint = 20f;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Mouth"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().Heal(healPoint);
            Destroy(gameObject);
        }
    }
}
