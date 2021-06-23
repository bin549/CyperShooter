using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    //public Transform ExplodePrefab;
    //public float minTime = 0.05f;
    //public float maxTime = 0.25f;
    //public float explosionRadius = 12.5f;
    //public float explosionForce = 4000.0f;
    [Header("爆炸模块")]
    public ConfigurationScript ConfigurationScript;//载入配置脚本
    public bool explod = false;

    private bool notSecondExplod;
    private float randomTime;

    private void Start()
    {
        notSecondExplod = true;
        randomTime = Random.Range(ConfigurationScript.minTime, ConfigurationScript.maxTime);
    }

    private void Update()
    {
        //TODO:如果被爆炸条件被触发了
        if (explod)
        {
            if (notSecondExplod == true)
            {
                StartCoroutine(Explode());
                notSecondExplod = false;
            }
        }
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(randomTime);
        Vector3 explosionPos = transform.position;
        //TODO：生成换爆炸特效，过时间销毁        
        RaycastHit checkGround;
        if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
        {
            Instantiate(ConfigurationScript.ExplodePrefab, checkGround.point,
                Quaternion.FromToRotation(Vector3.forward, checkGround.normal));
        }
        //爆炸范围
        Collider[] colliders = Physics.OverlapSphere(explosionPos, ConfigurationScript.explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            //有刚体的加力
            if (rb != null)
                rb.AddExplosionForce(ConfigurationScript.explosionForce * 50, explosionPos, ConfigurationScript.explosionRadius);
            //TODO对应物体对应动作

            //if (hit.transform.tag != null)
            //{
            //    hit.transform.gameObject.GetComponent(hit.transform.tag).isHit = true;
            //}
            //if   hit.transform.gameObject.GetComponent(hit.transform.tag).hasMethod("ishit")
            if (hit.transform.tag == "GasTank")
            {
                hit.transform.gameObject.GetComponent<GasTank>().isHit = true;
            }
            if (hit.transform.tag == "ExplosiveBarrel")
            {
                hit.transform.gameObject.GetComponent<ExplosiveBarrel>().isHit = true;
            }
            if (hit.transform.tag == "Target")
            {
                hit.transform.gameObject.GetComponent<TargetScript>().isHit = true;
            }
            //Destroy(explosionEffect, 2f);//关特效写在特效里        
            Destroy(gameObject);
        }
    }
}
