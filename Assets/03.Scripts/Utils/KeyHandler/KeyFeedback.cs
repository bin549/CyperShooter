using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyFeedback : MonoBehaviour
{
    public bool keyHit = false;
    public bool keyCanBeHitAgain = false;
    private float originalYPosition;

    private void Start()
    {
        originalYPosition = transform.position.y;
    }

    private void Update()
    {
        if (keyHit)
        {
            //  AudioManager.Instance.PlayKeyClick();
            keyCanBeHitAgain = false;
            keyHit = false;
            transform.position += new Vector3(0, -0.03f, 0);
        }

        if (transform.position.y < originalYPosition)
        {
            transform.position += new Vector3(0, 0.005f, 0);
        }
        else
        {
            keyCanBeHitAgain = true;
        }
    }
}
