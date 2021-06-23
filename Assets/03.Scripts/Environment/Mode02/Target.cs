using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float Speed;

    private Rigidbody rigidbody;
    public Transform leftPoint, rightPoint;
    private float leftx, rightx;
    private bool toRight = true;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        leftx = leftPoint.position.x;
        rightx = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (toRight)
        {
            rigidbody.velocity = new Vector2(Speed, rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = new Vector2(-Speed, rigidbody.velocity.y);
        }

        if (transform.position.x > rightx)
            toRight = false;
        else if (transform.position.x < leftx)
            toRight = true;
    }
}
