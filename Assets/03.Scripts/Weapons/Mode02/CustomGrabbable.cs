using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGrabbable : MonoBehaviour
{
    [SerializeField] protected Transform m_snapOffset;
    protected Rigidbody m_rigidBody;
    [SerializeField] protected OVRInput.Controller m_controller;

    public OVRInput.Controller controller
    { get => m_controller; set => m_controller = value; }

    public Rigidbody rigidBody
    { get => m_rigidBody; set => m_rigidBody = value; }

    public Transform snapOffset
    { get => m_snapOffset; set => m_snapOffset = value; }

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public virtual void GrabBegin(OVRInput.Controller controller)
    {
        this.controller = controller;
        VibrationManager.Instance.VibrateController(0.1f, 0.4f, 1.5f, controller);
    }

    public void GrabEnd()
    {
        VibrationManager.Instance.VibrateController(0.1f, 0.4f, 1.5f, controller);
        controller = OVRInput.Controller.None;
    }
}
