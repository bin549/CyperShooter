using UnityEngine;

public class DropoutGrabber : WeaponController
{
    [SerializeField] private Dropout dropout;
    private Transform dropoutGrabberParent;

    private void Start()
    {
        dropout = null;
        dropoutGrabberParent = transform.parent;
    }

    private void Update()
    {
        if (dropout != null)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                dropout.BePickUp(dropoutGrabberParent, controller);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.root.gameObject.CompareTag("Dropout"))
        {
            dropout = collision.GetComponentInParent<Dropout>();
            dropout.HighLight(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.root.gameObject.CompareTag("Dropout") && dropout != null)
        {
            dropout.HighLight(false);
            dropout = null;
        }
    }
}
