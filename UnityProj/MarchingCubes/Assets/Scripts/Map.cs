using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public struct Map
{
    Vector3 transform;
    Vector3Int m_lBound, m_uBound;
    float[,,] m_worldNoise;
    public Dictionary<Vector3Int, Chunk> m_chunks;
    float m_threshold;
    public Map(Vector3Int lowerBound, Vector3Int upperBound, Vector3 t, float noiseScale = 0.05f, float threshold = 0.5f)
    {
        transform = t;
        m_lBound = lowerBound;
        m_uBound = upperBound;
        m_worldNoise = SimplexNoise.Noise.Calc3D((m_uBound.x - m_lBound.x) * Chunk.SIDE_LENGTH
                                               , (m_uBound.y - m_lBound.y) * Chunk.SIDE_LENGTH
                                               , (m_uBound.z - m_lBound.z) * Chunk.SIDE_LENGTH, noiseScale, transform.x, transform.y, transform.z);

        //foreach (int i in m_worldNoise) Debug.Log(i);
        m_chunks = new Dictionary<Vector3Int, Chunk>(m_uBound.x - m_lBound.x * m_uBound.y - m_lBound.y * m_uBound.z - m_lBound.z);
        m_threshold = threshold;
        for (int x = m_lBound.x; x < m_uBound.x; x += 16)
            for (int y = m_lBound.y; y < m_uBound.y; y += 16)
                for (int z = m_lBound.z; z < m_uBound.z; z += 16)
                    AddChunk(new Vector3Int(x, y, z));
    }
    void AddChunk(Vector3Int location)
    {
        m_chunks.Add(location, new Chunk(location));
        Chunk chunk = m_chunks[location];
        for (int _x = 0; _x < Chunk.SIDE_LENGTH; ++_x)
            for (int _y = 0; _y < Chunk.SIDE_LENGTH; ++_y)
                for (int _z = 0; _z < Chunk.SIDE_LENGTH; ++_z)
                {
                    float debugger = m_worldNoise[location.x - m_lBound.x + _x, location.y - m_lBound.y + _y, location.z - m_lBound.z + _z];
                    int temp = chunk.GetIndexOfPoint(_x, _y, _z);
                    chunk.noiseMap.Set(temp, debugger > m_threshold);
                }
    }
    void RemoveChunk(Vector3Int location)
    {
        m_chunks.Remove(location);
    }
    public void NewBounds(Vector3Int lowerBound, Vector3Int upperBound)
    {
        Dictionary<Vector3Int, Chunk> reducedBounds = new Dictionary<Vector3Int, Chunk>();
        // = m_chunks.Where(chunk => chunk.Key.x > lowerBound.x ||
        //                           chunk.Key.y > lowerBound.y ||
        //                           chunk.Key.z > lowerBound.z ||
        //                           chunk.Key.x < upperBound.x ||
        //                           chunk.Key.y < upperBound.y ||
        //                           chunk.Key.z < upperBound.z).ToDictionary(i => i.Key, i => i.Value);
        Vector3Int tempCompare = new Vector3Int();
        for (int x = lowerBound.x; x < upperBound.x; ++x)
            for (int y = lowerBound.y; y < upperBound.y; ++y)
                for (int z = lowerBound.z; z < upperBound.z; ++z)
                {
                    tempCompare.Set(x, y, z);
                    if (!reducedBounds.ContainsKey(tempCompare))
                        reducedBounds.Add(tempCompare, new Chunk(tempCompare));
                }
        m_chunks = reducedBounds;
    }
    //public void NewNoiseScale(float noiseScale)
    //{
    //    m_worldNoise = SimplexNoise.Noise.Calc3D(m_uBound.y - m_lBound.x, m_uBound.y - m_lBound.y, m_uBound.z - m_lBound.z, noiseScale);
    //}
    public void NewThreshold(float threshold)
    {
        m_threshold = threshold;
        foreach (var chunk in m_chunks)
        {
            chunk.Value.noiseMap.Set(chunk.Value.GetIndexOfPoint(chunk.Key), m_worldNoise[chunk.Key.x, chunk.Key.y, chunk.Key.z] * 0.00390625f > m_threshold);
        }
    }
    public static void GetChunkMeshData(Chunk chunk, out List<int> triPoints, out List<Vector3> edgeLocs)
    {
        triPoints = new List<int>();
        int triCatcher = 0, countedEdges = 0;
        edgeLocs = new List<Vector3>(new Vector3[12 * (Chunk.SIDE_LENGTH - 1) * (Chunk.SIDE_LENGTH - 1) * (Chunk.SIDE_LENGTH - 1)]);
        for (byte xDir = 0; xDir < Chunk.SIDE_LENGTH - 1; ++xDir)
        {
            for (byte yDir = 0; yDir < Chunk.SIDE_LENGTH - 1; ++yDir)
            {
                for (byte zDir = 0; zDir < Chunk.SIDE_LENGTH - 1; ++zDir)
                {
                    foreach (Vector3 point in MarchTables.edgeLocations)
                    {
                        Vector3 v = edgeLocs[countedEdges];
                        v.x = point.x + xDir;
                        v.y = point.y + yDir;
                        v.z = point.z + zDir;
                        edgeLocs[countedEdges++] = v;
                    }
                    int location = chunk.getCube(xDir, yDir, zDir);
                    while (MarchTables.triangulation[location, triCatcher] != -1)
                    {
                        triPoints.Add(MarchTables.triangulation[location, triCatcher++] + countedEdges - 12);
                        triPoints.Add(MarchTables.triangulation[location, triCatcher++] + countedEdges - 12);
                        triPoints.Add(MarchTables.triangulation[location, triCatcher++] + countedEdges - 12);
                    }
                    triCatcher = 0;
                }
            }
        }
    }
}