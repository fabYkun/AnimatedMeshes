using                               System;
using                               System.Collections;
using                               System.Collections.Generic;
using                               UnityEngine;
using                               UnityEditor;

[System.Serializable]
public class                        ConeState
{
    public float                    radiusTop;
    public float                    radiusBottom;
    public float                    length;

    public void                     copy(ConeState other)
    {
        this.radiusBottom = other.radiusBottom;
        this.radiusTop = other.radiusTop;
        this.length = other.length;
    }
}

[System.Serializable]
public class                        AnimatedConeData
{
    public int                      numVertices = 10;
    public ConeState                initialState;
    public ConeState                finalState;
    public AnimationCurve           interpolationCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public MaterialModifier         materialModifier;
}

public class                        AnimatedConeState
{
    public ConeState                currentState;
    public MaterialModifierState    materialModifierState;
    public MeshRenderer             mr;
}

[CreateAssetMenu(menuName = "AnimationFX/AnimatedMeshes/Cone")]
public class                        AnimatedCone : AnimatedMesh
{
    public AnimatedConeData         data;
    public string                   meshPrefabPath = "Assets/Resources/Generated/AnimatedMeshes/Cones/";

    public override object          initialize(MeshFilter mf, MeshRenderer mr)
    {
        AnimatedConeState           state = new AnimatedConeState();

        state.currentState = new ConeState();
        state.materialModifierState = new MaterialModifierState();
        state.mr = mr;
        return (state);
    }

    public override void            updateMesh(AnimatedMeshState state)
    {
        Vector3[]                   vertices = state.mesh.vertices;
        AnimatedConeState           acState = state.data as AnimatedConeState;
        ConeState                   current = acState.currentState;
        float                       lifeRatio = state.lifeRatio;
        int                         numVertices = this.data.numVertices;
        int                         i;

        current.length = Mathf.Lerp(this.data.initialState.length, this.data.finalState.length, this.data.interpolationCurve.Evaluate(lifeRatio));
        current.radiusBottom = Mathf.Lerp(this.data.initialState.radiusBottom, this.data.finalState.radiusBottom, this.data.interpolationCurve.Evaluate(lifeRatio));
        current.radiusTop = Mathf.Lerp(this.data.initialState.radiusTop, this.data.finalState.radiusTop, this.data.interpolationCurve.Evaluate(lifeRatio));
        
        for (i = 0; i < numVertices; ++i)
        {
            float angle = 2 * Mathf.PI * i / numVertices;
            float angleSin = Mathf.Sin(angle);
            float angleCos = Mathf.Cos(angle);

            vertices[i] = new Vector3(current.radiusTop * angleCos, current.radiusTop * angleSin, -current.length);
            vertices[i + numVertices + 1] = new Vector3(current.radiusBottom * angleCos, current.radiusBottom * angleSin, 0);
        }

        // connecting edges
        ++numVertices;
        vertices[i] = new Vector3(vertices[0].x, vertices[0].y, vertices[0].z);
        vertices[i + numVertices] = new Vector3(vertices[numVertices].x, vertices[numVertices].y, vertices[numVertices].z);
        --numVertices;
        state.mesh.vertices = vertices;

        if (this.data.materialModifier && acState.mr)
        {
            this.data.materialModifier.update(acState.materialModifierState, this.data.interpolationCurve.Evaluate(lifeRatio));
            this.data.materialModifier.updateMaterial(acState.materialModifierState, acState.mr.material);
        }
    }

    public override Mesh            generateMesh(AnimatedMeshState state)
    {
        AnimatedConeState           acState = state.data as AnimatedConeState;
        Vector3[]                   vertices;
        Mesh                        mesh;
        int                         numVertices = this.data.numVertices;
        ConeState                   init = this.data.initialState;
        string                      path = meshPrefabPath + "Cone" + this.data.numVertices + "v" + init.radiusTop +
                                    "t" + init.radiusBottom + "b" + init.length + "l" + init.length + "o" + ".asset";

        if (!this.createAsset || !(mesh = (Mesh)AssetDatabase.LoadAssetAtPath(path, typeof(Mesh))))
        {
            mesh = new Mesh();
            mesh.Clear();
            numVertices += 1; // for correct uv mapping (to join correctly the faces)
            vertices = new Vector3[2 * numVertices]; // 0..n-1: top, n..2n-1: bottom
            Vector3[] normals = new Vector3[2 * numVertices];
            Vector2[] uvs = new Vector2[2 * numVertices];
            int[] tris;
            float slope = Mathf.Atan((init.radiusBottom - init.radiusTop) / init.length); // (rad difference)/height
            float slopeSin = Mathf.Sin(slope);
            float slopeCos = Mathf.Cos(slope);
            int i;

            --numVertices;
            for (i = 0; i < numVertices; ++i)
            {
                float angle = 2 * Mathf.PI * i / numVertices;
                float angleSin = Mathf.Sin(angle);
                float angleCos = Mathf.Cos(angle);
                float angleHalf = 2 * Mathf.PI * (i + 0.5f) / numVertices; // for degenerated normals at cone tips
                float angleHalfSin = Mathf.Sin(angleHalf);
                float angleHalfCos = Mathf.Cos(angleHalf);

                vertices[i] = new Vector3(init.radiusTop * angleCos, init.radiusTop * angleSin, -init.length);
                vertices[i + numVertices + 1] = new Vector3(init.radiusBottom * angleCos, init.radiusBottom * angleSin, 0);

                normals[i] = new Vector3(angleCos * slopeCos, angleSin * slopeCos, -slopeSin);
                normals[i + numVertices + 1] = new Vector3(angleCos * slopeCos, angleSin * slopeCos, -slopeSin);

                uvs[i] = new Vector2(1.0f * i / numVertices, 1);
                uvs[i + numVertices + 1] = new Vector2(1.0f * i / numVertices, 0);
            }

            // connecting edges
            ++numVertices;
            vertices[i] = new Vector3(vertices[0].x, vertices[0].y, vertices[0].z);
            vertices[i + numVertices] = new Vector3(vertices[numVertices].x, vertices[numVertices].y, vertices[numVertices].z);
            uvs[i] = new Vector2(1, 1);
            uvs[i + numVertices] = new Vector2(1, 0);
            normals[i] = new Vector3(normals[0].x, normals[0].y, normals[0].z);
            normals[i + numVertices] = new Vector3(normals[numVertices].x, normals[numVertices].y, normals[numVertices].z);


            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;

            // creating triangles
            int cnt = 0;
            tris = new int[numVertices * 6];
            for (i = 0; i < numVertices; i++)
            {
                int ip1 = i + 1;
                if (ip1 == numVertices)
                    ip1 = 0;

                tris[cnt++] = i;
                tris[cnt++] = ip1;
                tris[cnt++] = i + numVertices;

                tris[cnt++] = ip1 + numVertices;
                tris[cnt++] = i + numVertices;
                tris[cnt++] = ip1;
            }
            --numVertices;
            mesh.triangles = tris;

            if (this.createAsset)
            {
                AssetDatabase.CreateAsset(mesh, path);
                AssetDatabase.SaveAssets();
            }
        }
        return (mesh);
    }
}