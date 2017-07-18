using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// maybe a mistake
public class                        AnimationCtrl : MonoBehaviour
{
    AnimationPattern                pattern;
    AnimationFX                     fx;

    public void                     Instantiation(AnimationPattern pattern, AnimationFX fx)
    {
        this.pattern = pattern;
        this.fx = fx;
    }

    public void                     OnUpdate(float liferatio)
    {

    }
}