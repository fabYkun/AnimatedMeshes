using UnityEngine;
using UnityEditor;
using System.Collections;

/*
**  Will not work with multiple animations
**  => Do not subscribe to AditorApplication delegate but a delegate created here ?
**  => Unsubscribe any complexeAnimation on launching a new preview ?
*/
public class                    PreviewTime
{
    public static float         Time
    {
        get
        {
            if (Application.isPlaying)
                return (UnityEngine.Time.timeSinceLevelLoad);
            return EditorPrefs.GetFloat("PreviewTime", 0f);
        }
        set
        {
            EditorPrefs.SetFloat("PreviewTime", value);
        }
    }
}