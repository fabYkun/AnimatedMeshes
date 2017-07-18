using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="AnimationFX/Patterns/Rondo")]
public class                    RondoPattern : AnimationPattern
{
    public float                circleInitialRange = 3;
    public float                circleFinalRange = 10;
    public GameObject           prefab;
    private GameObject          childContainer;
    private List<Vector3>       initialPositions = new List<Vector3>();
    private List<Vector3>       finalPositions = new List<Vector3>();

    public override void        populate(ComplexAnimation animation)
    {
        // delete old childrens ?

        float angle = (float) (6.283185307179586476925286766559 / (double) this.childs);

        this.childContainer = new GameObject(this.patternName);
        this.childContainer.transform.SetParent(animation.transform);
        this.childContainer.transform.localPosition = Vector3.zero;
        for (int i = 0; i < this.childs; ++i)
        {
            GameObject          child = Instantiate(this.prefab) as GameObject;
            Vector3             initialPosition = new Vector3((float)Math.Sin(angle * i) * this.circleInitialRange, 0, (float)Math.Cos(angle * i) * this.circleInitialRange);
            Vector3             finalPosition = new Vector3((float)Math.Sin(angle * i) * this.circleFinalRange, 0, (float)Math.Cos(angle * i) * this.circleFinalRange);

            child.name = this.patternName + " " + i;
            child.transform.SetParent(this.childContainer.transform);
            this.initialPositions.Add(new Vector3(initialPosition.x, initialPosition.y, initialPosition.z));
            this.finalPositions.Add(new Vector3(finalPosition.x, finalPosition.y, finalPosition.z));
            child.transform.localPosition = initialPosition;
            this.childrens.Add(child);
        }
    }
    
    public override void        excecute(float liferatio)
    {
        for (int i = 0; i < this.childrens.Count; ++i)
        {
            this.childrens[i].transform.localPosition = Vector3.Lerp(this.initialPositions[i], this.finalPositions[i], liferatio);
        }
    }

    public override void        preview(float liferatio)
    {
        for (int i = 0; i < this.childrens.Count; ++i)
            this.childrens[i].transform.localPosition = Vector3.Lerp(this.initialPositions[i], this.finalPositions[i], liferatio);
    }

    public override void        delete()
    {
        DestroyImmediate(this.childContainer);
        this.initialPositions.Clear();
        this.finalPositions.Clear();
        base.delete();
    }
}