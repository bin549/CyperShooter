using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds = 2.0f;

    private void Start()
    {
        GameObject.Destroy(gameObject, seconds);
    }
}
