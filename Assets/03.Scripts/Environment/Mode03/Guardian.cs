using UnityEngine;
using Photon.Pun;

public class Guardian : MonoBehaviour
{
    public float damage = 99999.9f;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Damage(collision.gameObject.transform);
        }
    }

    protected void Damage(Transform enemy)
    {
        EnemyHealth e = enemy.gameObject.GetComponent<EnemyHealth>();
        if (e != null)
        {
            e.TakeDamage(damage);

        }
    }

}
 