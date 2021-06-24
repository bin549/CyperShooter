using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frist : MonoBehaviour {
    private Transform camTrans;
    private Vector3 camAng;
    private float camHeight = 2.5f;
    // Use this for initialization
    void Start () {
        camTrans = Camera.main.transform;

        Vector3 startPos = transform.position;

        startPos.y += camHeight;

        camTrans.position = startPos;

        camTrans.rotation = transform.rotation;

        camAng = camTrans.eulerAngles;
    }
	
	// Update is called once per frame
	void Update () {
        Rot_move();
        if(Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * 20.0f);
        }

    }
    void Rot_move()
    {
        float y = Input.GetAxis("Mouse X");

        float x = Input.GetAxis("Mouse Y");

        camAng.x -= x;

        camAng.y += y;

        camTrans.eulerAngles = camAng;

        camTrans.position = new Vector3(this.transform.position.x, camTrans.position.y, this.transform.position.z);

        float camy = camAng.y;

        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, camy, this.transform.eulerAngles.z);

        Vector3 startPos = transform.position;

        startPos.y += camHeight;

        camTrans.position = startPos;
    }
}
