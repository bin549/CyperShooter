using UnityEngine;
using System.Linq;

public class SnapOffsets : MonoBehaviour
{
    public Transform[] snapOffsets;

    private void Start()
    {
        var transforms = GetComponentsInChildren<Transform>().ToList();
        snapOffsets = transforms.FindAll(tf => tf.gameObject.name.Contains("SnapPosition")).ToArray();
    }
}
 