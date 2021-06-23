using UnityEngine;

public class ShieldLaucher
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
        }
    }
}