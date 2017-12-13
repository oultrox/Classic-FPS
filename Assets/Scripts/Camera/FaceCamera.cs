using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    private Vector3 cameraDirection;
    private Transform cameraTransform;
	
    // Use this for initialization
	void Awake ()
    {
        cameraTransform = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        cameraDirection = cameraTransform.forward;
        cameraDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(cameraDirection);
	}
}
