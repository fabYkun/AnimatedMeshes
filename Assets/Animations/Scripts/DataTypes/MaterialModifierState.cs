using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class                        MaterialModifierData
{
    public Vector2                  tilingBegin;
    public Vector2                  tilingEnd;
    public AnimationCurve           xTilingCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve           yTilingCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public Vector2                  offsetBegin;
    public Vector2                  offsetEnd;
    public AnimationCurve           xOffsetCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve           yOffsetCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public Color                    tintBegin = Color.white;
    public Color                    tintEnd = Color.white;
    public AnimationCurve           tintCurve = AnimationCurve.Linear(0, 0, 1, 1);
}

[System.Serializable]
public class                        MaterialModifierState
{
    public Vector2                  tiling;
    public Vector2                  offset;
    public Color                    tint;
}