using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Credit to https://www.youtube.com/watch?v=eJEpeUH1EMg&t=424s
/// <summary>
/// Builds terrain from scratch
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class MeshGen : MonoBehaviour
{
    [SerializeField]
    Vector3Int m_lBound = Vector3Int.zero, m_uBound = Vector3Int.one;
    public float threshold;
    public float noiseScale;
    Mesh mesh;
    public Map map;
    Vector3[] vertices;
    int[] triangles;
    private void Init()
    {
        mesh = new Mesh();
        map = new Map(m_lBound, m_uBound, transform.position, noiseScale, threshold);
        GetComponent<MeshFilter>().mesh = mesh;
    }
    private void OnValidate()
    {
        UpdateMesh();
    }
    public void UpdateMesh()
    {
        //if (mesh == null || map.m_chunks == null)
        Init();
        mesh.Clear();
        List<int> triPoints = new List<int>();
        int triCatcher = 0, countedEdges = 0;
        List<Vector3> edgeLocs = new List<Vector3>(new Vector3[12 * (Chunk.SIDE_LENGTH - 1) * (Chunk.SIDE_LENGTH - 1) * (Chunk.SIDE_LENGTH - 1)]);
        //Debug.Log("Length: " + map.m_chunks.Count);
        foreach (var _chunk in map.m_chunks)
        {
            Chunk chunk = _chunk.Value;
            for (int xDir = 0; xDir < Chunk.SIDE_LENGTH - 1; ++xDir)
            {
                for (int yDir = 0; yDir < Chunk.SIDE_LENGTH - 1; ++yDir)
                {
                    for (int zDir = 0; zDir < Chunk.SIDE_LENGTH - 1; ++zDir)
                    {
                        foreach (Vector3 point in MarchTables.edgeLocations)
                        {
                            Vector3 v = edgeLocs[countedEdges];
                            v.x = point.x + xDir + chunk.m_location.x;
                            v.y = point.y + yDir + chunk.m_location.y;
                            v.z = point.z + zDir + chunk.m_location.z;
                            edgeLocs[countedEdges++] = v;
                        }
                        int location = chunk.getCube((byte)xDir, (byte)yDir, (byte)zDir);
                        //Debug.Log(location);
                        while (MarchTables.triangulation[location, triCatcher] != -1)
                        {
                            //Debug.Log("Hello World!");
                            triPoints.Add(MarchTables.triangulation[location, triCatcher++] + countedEdges - 12);
                            triPoints.Add(MarchTables.triangulation[location, triCatcher++] + countedEdges - 12);
                            triPoints.Add(MarchTables.triangulation[location, triCatcher++] + countedEdges - 12);
                        }
                        triCatcher = 0;
                    }
                }
            }
        }
        mesh.vertices = edgeLocs.ToArray();
        mesh.triangles = triPoints.ToArray();//.Reverse().ToArray();
        mesh.RecalculateNormals();
    }
    //private void OnDrawGizmos()
    //{
    //    if (mesh == null || map.m_chunks == null)
    //        Init();
    //    int i = 0;
    //    foreach (var chunk in map.m_chunks)
    //    {
    //        i++;
    //        Gizmos.color = new Color(1 - chunk.Key.magnitude, chunk.Key.magnitude, 0, 0.2f);
    //        Gizmos.DrawCube(new Vector3(chunk.Key.x + 8, chunk.Key.y + 8, chunk.Key.z + 8), Vector3.Scale(transform.localScale, Vector3.one * 16));
    //    }
    //    Debug.Log(i);
    //}
    public void UpdateNewThreshold(float threshold)
    {
        this.threshold = threshold;
        UpdateMesh();
    }
    public void UpdateNewNoiseScale(float ns)
    {
        noiseScale = ns;
        UpdateMesh();
    }
}