using UnityEngine;

public class Actor : MonoBehaviour
{
    public int affiliation;
    public Transform aimPoint;
    private ActorsManager actorsManager;

    private void Start()
    {
        actorsManager = FindObjectOfType<ActorsManager>();

        if (!actorsManager.actors.Contains(this))
        {
            actorsManager.actors.Add(this);
        }
    }

    private void OnDestroy()
    {
        if (actorsManager)
        {
            actorsManager.actors.Remove(this);
        }
    }
}
