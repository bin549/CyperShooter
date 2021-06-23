using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyHealth> enemies;
    private void Awake()
    {
        enemies = new List<EnemyHealth>();
    }
}
