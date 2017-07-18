using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class                                ComplexAnimation : MonoBehaviour
{
    [HideInInspector]
    public List<ComplexeAnimationState>     animations = new List<ComplexeAnimationState>();
    [HideInInspector]
    public float                            lifetime;   // all animations are aborted when reaching lifetime
    public float                            speed;      // multiplied to deltatime
    private float                           time;       // current time

    // Use this for initialization
    void                    Start ()
    {
        Debug.Log("coucou");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}