using UnityEngine;
using System.Collections;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class Car : EnvironmentItem
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionMinRadius = 5f;
    [SerializeField] private float explosionMaxRadius = 5f;

    private int wavepointIndex = 0;
    private Transform target;
    [SerializeField] private Waypoints waypoint;
    private Rigidbody rigidbody;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Animator tailAnimator;
    [SerializeField] private bool isRotate;
    [SerializeField] private bool isLand;
    private int groundLayer;
    public int currentFlyTime;
    public PhotonView photonView;
    [SerializeField] private Waypoints[] waypoints;
    [Range(0, 50.0f)] [SerializeField] private int duration = 30;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        waypoints = FindObjectsOfType<Waypoints>();
    }

    protected override void Start()
    {
        base.Start();
        SetWaypoint();
        groundLayer = LayerMask.NameToLayer("Teleportable");
    }

    public void SetWaypoint()
    {
        waypoint = waypoints[Random.Range(0, waypoints.Length)];
        target = waypoint.points[wavepointIndex];
    }

    private void Update()
    {
        if (isLand)
            return;
        if (target == null)
            return;
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
            //   isRotate = true;
        }

        //  Debug.Log(Vector3.Angle(transform.position, target.position));
        // tailAnimator.SetBool("isHide", false);

        // StartCoroutine(Rotate());
    }

    /*
    private IEnumerator Rotate()
    {
      if (Vector3.Angle(transform.position, target.position) < 90.0f &&  !isLand)
      {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
      //  Debug.Log(Vector3.Angle(transform.position, target.position));
        tailAnimator.SetBool("isHide", false);
        yield return new WaitForSeconds(1.1f);
      }
      isRotate = false;
    }
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(100.0f);
        }
        if (collision.gameObject.CompareTag("Car"))
        {
            collision.gameObject.GetComponent<Car>().TakeDamage();
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject.transform);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(10.0f);

        }
        if (collision.gameObject.layer == groundLayer)
        {
            photonView.RPC("SpawnExplosionEffect", RpcTarget.All);
        }
    }

    [PunRPC]
    private void SpawnExplosionEffect()
    {
        GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 2f);
    }

    private void Destroy(Transform obstacle)
    {
        Barrel barrel = obstacle.gameObject.GetComponent<Barrel>();
        if (barrel != null)
        {
            barrel.TakeDamage();
        }
        Destructible destructible = obstacle.gameObject.GetComponent<Destructible>();
        if (destructible != null)
        {
            destructible.TakeDamage();
        }
    }

    private void GetNextWaypoint()
    {
        //tailAnimator.SetBool("isHide", true);
        if (wavepointIndex >= waypoint.points.Length - 1)
        {
            currentFlyTime++;
            if (currentFlyTime >= waypoint.flyTime)
            {
                waypoint.End();
                PhotonNetwork.Destroy(this.gameObject);
            }
            wavepointIndex = 0;
            return;
        }
        wavepointIndex++;
        target = waypoint.points[wavepointIndex];
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionMinRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionMaxRadius);
    }

    public override void TakeDamage()
    {
        Land();
    }

    public void LaserDamage()
    {
        photonView.RPC("LaserDamageRPC", RpcTarget.All);

    }

    [PunRPC]
    public void LaserDamageRPC()
    {
        duration--;
        if (duration <= 0)
        {
            Land();
        }
    }

    public void Land()
    {
        isLand = true;
        rigidbody.isKinematic = false;
        photonView.RPC("Explosion", RpcTarget.All);
    }

    [PunRPC]
    public void Explosion()
    {
        float rand_Radius = Random.Range(explosionMinRadius, explosionMaxRadius);
        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        var explosion = GameObject.Instantiate(explosionEffect, randDir, transform.rotation);
        Destroy(explosion, 3.0f);

        Destroy(this.gameObject, 5.0f);
        Invoke("SetHide", 2.0f);
    }

    public void SetHide()
    {
        tailAnimator.SetBool("isHide", true);
    }
}
