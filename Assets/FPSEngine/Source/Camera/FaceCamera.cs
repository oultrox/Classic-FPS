using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    private Vector3 cameraDirection;
    private Transform cameraTransform;
    private Transform selfTransform;
	
    // Use this for initialization
	void Awake ()
    {
        cameraTransform = Camera.main.transform;
        selfTransform = this.transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        cameraDirection = cameraTransform.forward;
        cameraDirection.y = 0;
        selfTransform.rotation = Quaternion.LookRotation(cameraDirection);
	}
}
