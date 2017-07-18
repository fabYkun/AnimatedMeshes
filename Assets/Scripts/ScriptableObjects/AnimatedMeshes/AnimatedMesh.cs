using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class                                AnimatedMeshState
{
    public float                            startTime;
    public float                            endTime;
    public float                            lifeRatio;
    public bool                             finished;
    public Mesh                             mesh;
    public object                           data; // implemented by the animatedMesh itself
}

abstract public class AnimatedMesh :        ScriptableObject
{
    [Tooltip("Animation's duration")]
    public float                            duration = 0.3f;
    [Tooltip("When checked and implemented, creates an asset that contains the initial mesh of the object that will be loaded on future instances")]
    public bool                             createAsset = true;
    [Tooltip("Should the mesh autodestroy when the animation is finished ? (if not memory leaks are in the hands of the animatedMesh (does not work when createAsset is checked))")]
    public bool                             autoDestroy = true;

    /**
    **  Method to call before launching the animation
    **/
    public AnimatedMeshState                start(MeshFilter mf, MeshRenderer mr = null)
    {
        AnimatedMeshState                   state = new AnimatedMeshState();

        state.startTime = Time.time;
        state.endTime = state.startTime + duration;
        state.finished = false;
        state.data = this.initialize(mf, mr);
        state.mesh = this.generateMesh(state);
        return (state);
    }

    /**
    **  Returns the parameters that will be given to the other functions
    **/
    public abstract                         object initialize(MeshFilter mf, MeshRenderer mr = null);

    public void                             update(AnimatedMeshState state)
    {
        if (!(state.finished = (state.lifeRatio = (Time.time - state.startTime) / this.duration) > 1))
            this.updateMesh(state);
        else
            this.destroy(state);
    }

    /**
    ** Generate the mesh for the data given as parameter (that could contain a meshFilter for example)
    **/
    public abstract Mesh                    generateMesh(AnimatedMeshState state);

    /**
    ** Use this.lifeRatio to update the data
    **/
    public abstract void                    updateMesh(AnimatedMeshState data);

    /**
    ** Default Destroy function, please override
    **/
    public virtual void                     destroy(AnimatedMeshState state)
    {
        if (this.autoDestroy && state.mesh != null)
        {
            if (!this.createAsset) Destroy(state.mesh);
            state.mesh = null;
        }
    }
}