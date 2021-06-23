using UnityEngine;

public class EnemyDropout : MonoBehaviour
{
    [SerializeField] private DropoutType dropoutType;
    [Range(0, 1.0f)] [SerializeField] private float dropoutPossibility;
    public DropoutType DropoutType { get => dropoutType; set => dropoutType = value; }
    public float DropoutPossibility { get => dropoutPossibility; set => dropoutPossibility = value; }
}
