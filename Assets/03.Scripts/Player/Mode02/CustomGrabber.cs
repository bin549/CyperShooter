using UnityEngine;
using OVRTouchSample;
using System.Linq;

public class CustomGrabber : MonoBehaviour
{
    [SerializeField] protected OVRInput.Controller m_controller;

    public OVRInput.Controller controller
    {
        get { return m_controller; }
    }

    [SerializeField] protected GameObject objectInHand;
    public Hand hand;
    public GameObject handVisual;
    protected HandAnimationController handAnimationController;
    [SerializeField] protected SnapOffsets snapOffsets;

    protected virtual void Start()
    {
        handAnimationController = transform.root.GetComponent<HandAnimationController>();
        hand = GetComponent<Hand>();
        handVisual = hand.animator.gameObject;
        snapOffsets = GetComponent<SnapOffsets>();
    }

    protected virtual void Update()
    {
        DropItem();
    }

    protected virtual void DropItem()
    {
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controller) || Input.GetKeyUp(KeyCode.E))
        {
            if (objectInHand != null)
            {
                objectInHand.transform.parent = null;
                var customGrabbable = objectInHand.GetComponent<CustomGrabbable>();
                customGrabbable.GrabEnd();
                var rigidBody = objectInHand.GetComponent<Rigidbody>();
                rigidBody.isKinematic = false;
                hand.isFull = false;
                objectInHand = null;
                handVisual.SetActive(true);
            }
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        var customGrabbable = other.transform.root.gameObject.GetComponent<CustomGrabbable>();
        if (customGrabbable != null)
            customGrabbable.snapOffset = snapOffsets.snapOffsets.ToList().Find(snapOffset => snapOffset.gameObject.name.Contains(other.gameObject.name.Split('(')[0]));
    }

    protected void PlaceInHand(Collider other, Transform snapPosition)
    {

        if (other.transform.parent != null && !other.transform.parent.CompareTag("Holster"))
        {
            objectInHand = other.transform.parent.gameObject;
        }
        else
        {
            objectInHand = other.transform.gameObject;
        }
        var customGrabbable = objectInHand.GetComponent<CustomGrabbable>();
        customGrabbable.GrabBegin(m_controller);
        var rigidBody = objectInHand.GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;

        objectInHand.transform.parent = snapPosition;
        objectInHand.transform.position = snapPosition.position;
        objectInHand.transform.rotation = snapPosition.rotation;

        hand.isFull = true;
        handVisual.SetActive(false);
    }
}
