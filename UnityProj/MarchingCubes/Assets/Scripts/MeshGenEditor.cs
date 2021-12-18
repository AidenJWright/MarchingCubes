using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGen))]
public class MeshGenEditor : Editor
{
    private MeshGen generator;

    private void OnEnable()
    {
        generator = target as MeshGen;

        Tools.hidden = true;
    }
    private void OnDisable()
    {
        Tools.hidden = false;
    }
    private void OnSceneGUI()
    {
        HandleUtility.AddDefaultControl(0);

        //Vector3 drawPos = new Vector3();
        //if (generator.resolutions.x < 10 && generator.resolutions.y < 10 && generator.resolutions.z < 10)
        //    for (int i = 0; i < generator.chunk.SIZE; ++i)
        //    {
        //        Handles.color = generator.chunk.noiseMap.Get(i) ? Color.red : Color.yellow;
        //        drawPos.x = i % generator.chunk.xRes * generator.transform.localScale.x;
        //        drawPos.y = i / generator.chunk.xRes % generator.chunk.yRes * generator.transform.localScale.y;
        //        drawPos.z = i / generator.chunk.xRes / generator.chunk.yRes * generator.transform.localScale.z;
        //        if (Handles.Button(drawPos, Quaternion.identity, 0.1f * generator.transform.localScale.magnitude, 0.15f, Handles.SphereHandleCap))
        //        {
        //            generator.chunk.noiseMap.Set(i, !generator.chunk.noiseMap.Get(i));
        //            generator.UpdateMesh();
        //        }
        //    }
    }
    public override void OnInspectorGUI()
    {
        //Update when changing value in inspector
        //if (base.DrawDefaultInspector())
        //{
        //    generator.EditorTesting();
        //    generator.UpdateMesh();
          

        //    EditorUtility.SetDirty(target);
        //}

        base.OnInspectorGUI();
        {
            if(GUILayout.Button("Reload Cubes"))
            {
                generator.UpdateMesh();

                EditorUtility.SetDirty(target);
            }
            if(GUILayout.Button("Reset Seed"))
            {
                SimplexNoise.Noise.Seed = (int)Time.time;
                generator.UpdateMesh();

                EditorUtility.SetDirty(target);
            }
        }
    }
}
