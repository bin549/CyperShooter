using UnityEngine;
using VolumetricLines;

public class Lightsaber : MeleeWeapon
{
    [SerializeField] private bool activate = false;
    private GameObject laser;
    private Vector3 fullSize;
    [SerializeField] private Color[] colors;
    [SerializeField] private VolumetricLineBehavior volumetricLineBehavior;
    private int nextColorIndex = 0;
    [SerializeField] private Collider collider;

    private void Start()
    {
        laser = transform.Find("SingleLine-LightSaber").gameObject;
        fullSize = laser.transform.localScale;
        laser.transform.localScale = new Vector3(fullSize.x, 0, fullSize.z);
        volumetricLineBehavior = GetComponentInChildren<VolumetricLineBehavior>();
        collider = GetComponent<Collider>();
    }


    protected override void Update()
    {
        base.Update();
        GetInput();
        LaserControll();
    }

    private void GetInput()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, controller)|| Input.GetKeyDown(KeyCode.X))
        {
            activate = !activate;
        }
    }

    private void LaserControll()
    {
        if (activate && laser.transform.localScale.y < fullSize.y)
        {
            laser.SetActive(true);
            laser.transform.localScale += new Vector3(0, 0.001f, 0);
            collider.enabled = true;
        }
        else if (!activate && laser.transform.localScale.y > 0)
        {
            laser.transform.localScale += new Vector3(0, -0.001f, 0);
        }
        else if (!activate)
        {
            laser.SetActive(false);
            collider.enabled = false;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        VibrationManager.Instance.VibrateController(duration, frequency, amplitude, Controller);

    }

    public override void UpgradeWeapon()
    {
        base.UpgradeWeapon();
        if (colors[nextColorIndex] != null)
            volumetricLineBehavior.LineColor = colors[nextColorIndex];
        nextColorIndex++;
    }
}
