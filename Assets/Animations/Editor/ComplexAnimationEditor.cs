using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(ComplexAnimation))]
public class                ComplexAnimationEditor : Editor
{
    ComplexAnimation        animation;
    bool                    pausePreview = false;
    float                   lastTime;

    public override void    OnInspectorGUI()
    {
        if (target == null) return;
        this.animation = this.target as ComplexAnimation;
        this.DrawAnimations();
    }

    void                    DrawAnimations()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        this.DrawSettings();
        GUILayout.Label("Animations", EditorStyles.boldLabel);
        this.DrawHeaderTable();
        for (int i = 0; i < this.animation.animations.Count; ++i)
            this.DrawAnimation(i);
        this.DrawAddAnimationButton();
        this.DrawPreviewAnimation();
    }

    void                    DrawHeaderTable()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Pattern", EditorStyles.boldLabel);
            GUILayout.Label("AnimationFx", EditorStyles.boldLabel);
            GUILayout.Label("Life", EditorStyles.boldLabel);
            GUILayout.Label("Delay", EditorStyles.boldLabel);
            GUILayout.Label("Delete", EditorStyles.boldLabel);
        }
        GUILayout.EndHorizontal();
    }
    void                    DrawSettings()
    {
        EditorGUI.BeginChangeCheck();
        float newLifetime = EditorGUILayout.FloatField("Lifetime", this.animation.lifetime);
        float newSpeed = EditorGUILayout.FloatField("Speed", this.animation.speed);

        if (newLifetime < 0) newLifetime = 0;
        if (newSpeed < 0) newSpeed = 0;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(this.animation, "Undo Inspector (Complex Animation)");
            this.animation.lifetime = newLifetime;
            this.animation.speed = newSpeed;
            EditorUtility.SetDirty(this.animation);
        }
    }
    void                    DrawAnimation(int index)
    {
        if (index < 0 || index >= this.animation.animations.Count) return;
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        {
            EditorGUI.BeginChangeCheck();
            AnimationPattern newPattern = EditorGUILayout.ObjectField(this.animation.animations[index].pattern, typeof(AnimationPattern), false, GUILayout.MinWidth(100)) as AnimationPattern;
            AnimationFX newAnimation = EditorGUILayout.ObjectField(this.animation.animations[index].animation, typeof(AnimationFX), false, GUILayout.MinWidth(100)) as AnimationFX;
            float newLife = EditorGUILayout.FloatField(this.animation.animations[index].life, GUILayout.MinWidth(15));
            float newDelay = EditorGUILayout.FloatField(this.animation.animations[index].delay, GUILayout.MinWidth(15));
            if (newLife < 0) newLife = 0;
            if (newDelay < 0) newDelay = 0;

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this.animation, "Undo Inspector (Complex Animation)");

                this.animation.animations[index].pattern = newPattern;
                this.animation.animations[index].animation = newAnimation;
                this.animation.animations[index].delay = newDelay;
                this.animation.animations[index].life = newLife;
                EditorUtility.SetDirty(this.animation);
            }
            /*
            if (GUILayout.Button("Preview"))
            {
                if (this.animation.animations[index].pattern)
                    Debug.Log("Preview of pattern " + this.animation.animations[index].pattern.patternName);
                else
                    Debug.LogWarning("Pattern is missing");
            }
            */
            if (GUILayout.Button("X"))
            {
                Undo.RecordObject(this.animation, "Undo Inspector (Complex Animation)");
                this.animation.animations.RemoveAt(index);
                EditorUtility.SetDirty(this.animation);
            }
        }
        GUILayout.EndHorizontal();
    }

    void                    DrawAddAnimationButton()
    {
        if (GUILayout.Button("Add Animation", GUILayout.Height(15)))
        {
            Undo.RecordObject(this.animation, "Add new Animation");
            this.animation.animations.Add(new ComplexeAnimationState());
            EditorUtility.SetDirty(this.animation);
        }
    }

    void                    DrawPreviewAnimation()
    {
        EditorGUILayout.Space();
        GUILayout.Label("Preview Time: " + Mathf.Floor(PreviewTime.Time / 60) + ":" + Mathf.Floor(PreviewTime.Time % 60).ToString("00"));
        GUILayout.Label("Preview", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Start", GUILayout.Height(15))) this.StartPreview();
            if (GUILayout.Button("Pause", GUILayout.Height(15))) this.PausePreview();
            if (GUILayout.Button("Stop", GUILayout.Height(15))) this.StopPreview();
        }
        GUILayout.EndHorizontal();
    }

    void                    StartPreview()
    {
        this.StopPreview();
        this.pausePreview = false;
        this.lastTime = Time.realtimeSinceStartup;
        EditorApplication.update += this.OnUpdate;
        Debug.Log("Preview of the complex pattern");
    }

    void                    PausePreview()
    {
        for (int i = 0; i < this.animation.animations.Count; ++i)
            if (!this.animation.animations[i].pattern)
            {
                Debug.LogWarning("Could not preview complex animation because one or more pattern are missing");
                return;
            }
        this.pausePreview = true;
        Debug.Log("Pause preview of the complex pattern");
        // preview all
    }

    bool                    StopPreview()
    {
        EditorApplication.update -= this.OnUpdate;
        PreviewTime.Time = 0f;
        for (int i = 0; i < this.animation.animations.Count; ++i)
        {
            if (!this.animation.animations[i].pattern)
            {
                Debug.LogWarning("Could not preview complex animation because one or more pattern are missing");
                return false;
            }
            if (this.animation.animations[i].launched && !this.animation.animations[i].destroyed)
                this.animation.animations[i].pattern.delete();
            this.animation.animations[i].launched = false;
            this.animation.animations[i].destroyed = false;
        }
        Debug.Log("Stop preview of the complex pattern");
        return true;
    }

    void                    OnUpdate()
    {
        if (this.animation.speed != 0 && !this.pausePreview)
        {
            PreviewTime.Time += (Time.realtimeSinceStartup - this.lastTime) * this.animation.speed;
            float time = PreviewTime.Time;
            this.Repaint();
            for (int i = 0; i < this.animation.animations.Count; ++i)
            {
                if (!this.animation.animations[i].launched && this.animation.animations[i].delay <= time)
                {
                    this.animation.animations[i].launched = true;
                    this.animation.animations[i].pattern.populate(this.animation);
                    Debug.Log(i + "launched");
                }
                if (this.animation.animations[i].launched && !this.animation.animations[i].destroyed)
                {
                    if (this.animation.animations[i].life > time - this.animation.animations[i].delay)
                        this.animation.animations[i].pattern.preview(time / this.animation.lifetime);
                    else
                    {
                        this.animation.animations[i].pattern.delete();
                        this.animation.animations[i].destroyed = true;
                    }
                }
            }
            SceneView.RepaintAll();
        }
        this.lastTime = Time.realtimeSinceStartup;
    }
}