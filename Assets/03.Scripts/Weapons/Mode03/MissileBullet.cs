using UnityEngine;

public class MissileBullet : ShootBullet
{
    [SerializeField] private Transform target;

    public void Seek(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null)
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        }
        else
        {
            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.LookAt(target);
        }
    }
}
