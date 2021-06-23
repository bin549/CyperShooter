using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explosion))]
public class ExplosiveBarrel : MonoBehaviour
{
    //TODO：油桶煤气罐：改成换爆炸后物体,不销毁
    [Header("爆炸有物体生成模块")]
    public Transform AfterDestroyedPrefab;
    public bool isHit;
    private bool notSecondExplod;
    private Explosion explosion;

    private void Start()
    {
        notSecondExplod = true;
        explosion = GetComponent<Explosion>();
    }

    private void Update()
    {
        if (isHit)
        {
            if (notSecondExplod)
            {
                Instantiate(AfterDestroyedPrefab, transform.position,
                                     transform.rotation);
                explosion.explod = true;
                notSecondExplod = false;
            }
        }
    }
}
