using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class           AnimationPattern : ScriptableObject
{
    public string               patternName;
    public int                  childs;
    protected List<GameObject>  childrens = new List<GameObject>();
    //public float              duration;

    public abstract void        populate(ComplexAnimation animation);
    public abstract void        excecute(float liferatio);
    public abstract void        preview(float liferatio);
    public virtual void         delete()
    {
        for (int i = 0; i < this.childrens.Count; ++i)
            DestroyImmediate(this.childrens[i]);
        this.childrens.Clear();
    }
}