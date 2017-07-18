using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class                        ComplexeAnimationState
{
    public AnimationPattern         pattern;
    public AnimationFX              animation;
    public float                    life;
    public float                    delay;
    public bool                     launched = false;
    public bool                     destroyed = false;
}
