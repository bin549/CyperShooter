using UnityEngine;

public class LightWaker : MonoBehaviour
{
    [SerializeField] private GameObject[] LightObj;

    private void Awake()
    {
        foreach (GameObject obj in LightObj)
        {
            obj.SetActive(false);
        }
    }

    public void Light()
    {
        foreach (GameObject obj in LightObj)
        {
            obj.SetActive(true);
        }
    }
}
