using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnimCone : MonoBehaviour {
    public float delay;
    private float startTime;
    public AnimatedMesh test;
    private AnimatedMeshState state;
    public Material material;
    public bool start = false;

    private MeshFilter mf;
    private MeshRenderer mr;

    // Use this for initialization
    void Start ()
    {
        if (!(mf = this.gameObject.GetComponent<MeshFilter>()))
            mf = this.gameObject.AddComponent<MeshFilter>();
        if (!(mr = this.gameObject.GetComponent<MeshRenderer>()))
            mr = this.gameObject.AddComponent<MeshRenderer>();
        
        mr.material = this.material;
        this.startTime = Time.time + delay;
    }
	
	// Update is called once per frame
	void Update () {
        if (this.startTime <= Time.time && !this.start)
        {
            this.state = test.start(mf, mr);
            this.mf.mesh = this.state.mesh;
            this.start = true;
        }
        if (this.start)
            test.update(state);
	}
}