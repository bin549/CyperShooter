using UnityEngine;
using System.Collections;

public class SceneCanvas : MonoBehaviour
{
    protected GameObject centerEyeAnchor;

    protected virtual void Start()
    {
        centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
    }

    protected virtual void Update()
    {
        StartCoroutine(FollowEye());
    }

    protected IEnumerator FollowEye()
    {
        transform.position = centerEyeAnchor.transform.position + new Vector3(0f, 0f, -100f);
        yield return new WaitForSeconds(1f);
    }
}
