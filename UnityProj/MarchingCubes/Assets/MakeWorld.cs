using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;

public class MakeWorld : MonoBehaviour
{
    Vector3 oldWorld;
    public Vector3 worldDimensions;
    [SerializeField]
    GameObject chunk;
    private float t;
    public float threshold;
    private float n;
    public float noiseScale;
    public bool newSeed;
    List<MeshGen> kids;
    private void Awake()
    {
        kids = new List<MeshGen>();
        for (int xDir = 0; xDir < worldDimensions.x; ++xDir)
        {
            for (int yDir = 0; yDir < worldDimensions.y; ++yDir)
            {
                for (int zDir = 0; zDir < worldDimensions.z; ++zDir)
                {
                    SpawnChunk(xDir, yDir, zDir);
                }
            }
        }
    }
    private void Update()
    {
        if(worldDimensions != oldWorld || t != threshold || n != noiseScale)
        {
            t = threshold;
            n = noiseScale;
            oldWorld = worldDimensions;
            StopAllCoroutines();
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            StartCoroutine(ChunkLoader());
        }
        //if(t != threshold && !bigRoutine)
        //{
        //    t = threshold;
        //    activeCoroutines.Add(StartCoroutine(ThresholdReset()));
        //}
        //if(n != noiseScale && !bigRoutine)
        //{
        //    n = noiseScale;
        //    activeCoroutines.Add(StartCoroutine(NoiseScaleReset()));
        //}
    }
    IEnumerator ChunkLoader()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int xDir = 0; xDir < worldDimensions.x; ++xDir)
        {
            for (int yDir = 0; yDir < worldDimensions.y; ++yDir)
            {
                for (int zDir = 0; zDir < worldDimensions.z; ++zDir)
                {
                    SpawnChunk(xDir, yDir, zDir);
                    if (stopwatch.ElapsedMilliseconds > 15)
                    {
                        stopwatch.Stop();
                        yield return new WaitForEndOfFrame();
                        stopwatch.Restart();
                    }
                } 
            } 
        }
    }
    //IEnumerator ThresholdReset()
    //{
    //    Stopwatch stopwatch = new Stopwatch();
    //    stopwatch.Start();
    //    List<MeshGen> k = kids;
    //    foreach (var v in k)
    //    {
    //        v.UpdateNewThreshold(threshold);
    //        if (stopwatch.ElapsedMilliseconds > 15)
    //        {
    //            stopwatch.Stop();
    //            yield return new WaitForEndOfFrame();
    //            stopwatch.Restart();
    //            if (k != kids)
    //                yield break;
    //        }
    //    }
    //}
    //IEnumerator NoiseScaleReset()
    //{
    //    Stopwatch stopwatch = new Stopwatch();
    //    stopwatch.Start();
    //    List<MeshGen> k = kids;
    //    foreach (var v in k)
    //    {
    //        v.UpdateNewNoiseScale(noiseScale);
    //        if (stopwatch.ElapsedMilliseconds > 15)
    //        {
    //            stopwatch.Stop();
    //            yield return new WaitForEndOfFrame();
    //            stopwatch.Restart();
    //            if (k != kids)
    //                yield break;
    //        }
    //    }
    //}
    void SpawnChunk(int xDir, int yDir, int zDir)
    {
        var o = Instantiate(chunk, new Vector3(xDir * 15, yDir * 15, zDir * 15) - worldDimensions/2, Quaternion.identity, transform);
        MeshGen k = o.GetComponent<MeshGen>();
        k.threshold = threshold;
        k.noiseScale = noiseScale;
        k.UpdateMesh();
        kids.Add(k);
    }
}
