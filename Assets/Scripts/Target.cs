using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    private MoveBall moveBall;

    public Action<Target> TargetTriggered;
    public Action<Target> TargetUntriggered;
    // Use this for initialization
    void Start () {
        moveBall = FindObjectOfType<MoveBall>();
        moveBall.RegisterTarget(gameObject);
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (TargetTriggered != null)
            TargetTriggered(this);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (TargetUntriggered != null)
            TargetUntriggered(this);
    }

}
