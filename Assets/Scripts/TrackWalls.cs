using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackWalls : MonoBehaviour {
    public Transform follow;
	// Use this for initialization
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, follow.position.y, transform.position.z);
	}
}
