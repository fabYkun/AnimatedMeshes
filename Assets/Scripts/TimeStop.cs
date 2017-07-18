using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour {
    [Range(0.0f, 2.0f)]
    public float scale = 0.5f;
    public bool autodestroy = false;
    public float destroyTime;
	// Use this for initialization
	void Start () {
        Time.timeScale = scale;
        if (autodestroy) Destroy(this.gameObject, destroyTime);
	}
}
