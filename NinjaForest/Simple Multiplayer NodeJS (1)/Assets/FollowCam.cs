using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    public GameObject target;
    public float dist = 10.0f;
    public float height = 5.0f;
    public float smoothRotate = 5.0f;

    Vector3 cameraPosition;
	// Update is called once per frame
	void LateUpdate () {
        cameraPosition.x = target.transform.position.x + dist;
        cameraPosition.y = target.transform.position.y + height;
        cameraPosition.z = target.transform.position.z + dist;

    }
}
