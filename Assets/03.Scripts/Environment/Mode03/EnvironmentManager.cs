using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public List<EnvironmentItem> environments;

    private void Awake()
    {
        environments = new List<EnvironmentItem>();

    }


    public virtual void TakeDamage() { }

}
 