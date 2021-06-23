using UnityEngine;
using OVRTouchSample;

public class GunGrabber : MonoBehaviour
{
    public OVRInput.Controller controller;
    [SerializeField] private Transform deagleSnapPosition;
    [SerializeField] private Transform deagleMagazineSnapPosition;
    [SerializeField] private Transform uziSnapPosition;
    [SerializeField] private Transform uziMagazineSnapPosition;
    [SerializeField] private Animator animator;
    [SerializeField] private Hand handScript;
    private HandAnimationController handAnimationController;
    [SerializeField] private GameObject objectInHand;
    [SerializeField] private bool handIsFull = false;

    private void Start()
    {
        handAnimationController = transform.root.GetComponent<HandAnimationController>();
        handScript = GetComponent<Hand>();
    }

    private void Update()
    {
        DropItem();
    }

    private void DropItem()
    {
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controller) || Input.GetKeyUp(KeyCode.E))
        {
            if (objectInHand != null)
            {
                if (!objectInHand.CompareTag("UsedMagazine"))
                {
                    objectInHand.transform.parent = null;
                    var rigidBody = objectInHand.GetComponent<Rigidbody>();
                    rigidBody.isKinematic = false;
                }
                if (objectInHand.CompareTag("Uzi"))
                {
                    var uzi = objectInHand.GetComponent<Uzi>();
                    uzi.autoFiring = false;
                    uzi.stopAutoFiring = true;
                    if (controller == OVRInput.Controller.RTouch)
                    {
                        VibrationManager.Instance.TurnOffVibrate(OVRInput.Controller.RTouch);
                    }
                    else
                    {
                        VibrationManager.Instance.TurnOffVibrate(OVRInput.Controller.LTouch);
                    }
                }
                var canHolster = objectInHand.GetComponent<ICanHolster>();
                if (canHolster != null)
                {
                    HolsterWeapon(canHolster);
                }
                handIsFull = false;
                objectInHand = null;
                handAnimationController.SetHoldingUziMagazine(animator, false);
                handAnimationController.SetHoldingUzi(animator, false);
                handAnimationController.SetHoldingDeagle(animator, false);
                handAnimationController.SetHoldingDeagleMagazine(animator, false);

                // handScript..AllowThumbsUp = true;
                // handScript.BlockDefaultHandPose = false;
            }
        }
    }

    private void HolsterWeapon(ICanHolster canHolster)
    {
        var holsters = GameObject.FindGameObjectsWithTag("Holster");
        foreach (var holster in holsters)
        {
            var distanceToHolster = Vector3.Distance(objectInHand.transform.position, holster.transform.position);
            var childrenOfHolster = holster.GetComponentInChildren<ICanHolster>();
            if (childrenOfHolster == null && distanceToHolster < 0.25f)
            {
                switch (canHolster.SnapPosition)
                {
                    case 0:
                        objectInHand.transform.rotation = holster.transform.GetChild(0).transform.rotation;
                        objectInHand.transform.position = holster.transform.GetChild(0).transform.position;
                        break;
                    case 1:
                        objectInHand.transform.rotation = holster.transform.GetChild(1).transform.rotation;
                        objectInHand.transform.position = holster.transform.GetChild(1).transform.position;
                        break;
                    default:
                        objectInHand.transform.rotation = holster.transform.GetChild(0).transform.rotation;
                        objectInHand.transform.position = holster.transform.GetChild(0).transform.position;
                        break;
                }
                objectInHand.GetComponent<Rigidbody>().isKinematic = true;
                objectInHand.transform.parent = holster.transform;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.CompareTag("DesertEagle"))
        {
            //  handScript.BlockDefaultHandPose = true;
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller) || Input.GetKeyDown(KeyCode.E))
        {
            if (!handIsFull && other.transform.root.CompareTag("DesertEagle"))
            {
                if (other.transform.parent != null && other.transform.parent.name.Contains("Snap"))
                {
                    return;
                }
                else
                {
                    PlaceInHand(other, deagleSnapPosition);
                    handAnimationController.SetHoldingDeagle(animator, true);
                }
            }
            else if (!handIsFull && other.transform.root.CompareTag("DesertEagleMagazine"))
            {
                PlaceInHand(other, deagleMagazineSnapPosition);
                handAnimationController.SetHoldingDeagleMagazine(animator, true);
            }
            else if (!handIsFull && other.transform.root.CompareTag("Uzi") || !handIsFull && other.transform.CompareTag("Uzi"))
            {
                if (other.transform.parent != null && other.transform.parent.name.Contains("Snap"))
                {
                    return;
                }
                else
                {
                    PlaceInHand(other, uziSnapPosition);
                    handAnimationController.SetHoldingUzi(animator, true);
                }
            }
            else if (!handIsFull && other.transform.root.CompareTag("UziMagazine"))
            {
                PlaceInHand(other, uziMagazineSnapPosition);
                handAnimationController.SetHoldingUziMagazine(animator, true);
            }
        }
    }

    private void PlaceInHand(Collider other, Transform snapPosition)
    {
        if (other.transform.parent != null && !other.transform.parent.CompareTag("Holster"))
        {
            objectInHand = other.transform.parent.gameObject;
        }
        else
        {
            objectInHand = other.transform.gameObject;
        }
        var rigidBody = objectInHand.GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        objectInHand.transform.parent = snapPosition;
        objectInHand.transform.position = snapPosition.position;
        objectInHand.transform.rotation = snapPosition.rotation;
        //  handScript.AllowThumbsUp = false;
        handIsFull = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("DesertEagle"))
        {
            //    handScript.BlockDefaultHandPose = false;
        }
    }
}
