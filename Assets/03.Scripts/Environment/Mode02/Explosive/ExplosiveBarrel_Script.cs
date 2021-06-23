using UnityEngine;

public class ExplosiveBarrel_Script : Explosion_Script
{
    public Transform AfterDestroyedPrefab;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnDie()
    {
        Instantiate(AfterDestroyedPrefab, transform.position, transform.rotation);
        base.OnDie();
    }
}
