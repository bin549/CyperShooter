using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDC_Robots_Demo : MonoBehaviour
{
	List <Animator> anims = new List<Animator>();

	void Start()
    {
		foreach (Animator other in GameObject.FindObjectsOfType<Animator>()) {
			anims.Add (other);
		}    
    }

	void Update(){
		if (Input.GetMouseButton (0)) {
			foreach (Animator other in anims) {
				other.transform.Rotate (new Vector3 (0, Input.GetAxis ("Mouse X")*3, 0));
			}
		}
	}

	public void Play(string animationName){
		foreach (Animator other in anims) {
			if (animationName == "Jump") {
				if (other.name == "Robot 5") {
					continue;
				}
			}
			if (animationName == "Air") {
				if (other.name == "Robot 5") {
					continue;
				}
			}
			if (animationName == "Land") {
				if (other.name == "Robot 5") {
					continue;
				}
			}
			if (animationName == "Walk") {
				if (other.name == "Robot 5" || other.name == "Robot 6") {
					continue;
				}
			}
			other.CrossFadeInFixedTime (animationName,0f);
		}
	}
}
