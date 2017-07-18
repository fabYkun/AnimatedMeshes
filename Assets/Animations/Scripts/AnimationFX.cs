using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AnimationFX :         ScriptableObject
{
    public string                           animationName;
    public virtual void                     initialize() { }
    public abstract void                    launch();
}