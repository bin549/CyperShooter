using UnityEngine;
using Photon.Pun;


public class EnvironmentItem : MonoBehaviour
{
    protected EnvironmentManager environmentManager;

    protected virtual void Start()
    {
        environmentManager = FindObjectOfType<EnvironmentManager>();

        if (!environmentManager.environments.Contains(this))
        {
            environmentManager.environments.Add(this);
        }
    }

    public void Clear()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    public virtual void TakeDamage()
    {

    }

    protected void OnDestroy()
    {
        if (environmentManager)
        {
            environmentManager.environments.Remove(this);
        }
    }
}
