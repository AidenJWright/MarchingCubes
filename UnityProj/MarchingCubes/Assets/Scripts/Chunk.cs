using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct Chunk
{
    public Vector3Int m_location;
    public const int SIZE = 4096;//4368;
    public const int SIDE_LENGTH = 16;
    public BitArray noiseMap;
    public Chunk(Vector3Int location)
    {
        m_location = location;
        noiseMap = new BitArray(SIZE);
    }
    /// <summary>
    ///  Returns cube data for cube based on lower left frontmost point of cube.
    /// </summary>
    /// <returns></returns>
    public byte getCube(byte x, byte y, byte z)
    {
        //Debug.Log(m_location);
        byte value = 0b00000000;
        value |= noiseMap.Get(SIDE_LENGTH * SIDE_LENGTH * (z + 1) + SIDE_LENGTH * y + x) ? (byte)1 : (byte)0;               //0         4--------5
        value |= noiseMap.Get(SIDE_LENGTH * SIDE_LENGTH * (z + 1) + SIDE_LENGTH * y + x + 1) ? (byte)2 : (byte)0;           //1        /|       /|
        value |= noiseMap.Get(SIDE_LENGTH * SIDE_LENGTH * z + SIDE_LENGTH * y + x + 1) ? (byte)4 : (byte)0;                 //2       / |      / |
        value |= noiseMap.Get(SIDE_LENGTH * SIDE_LENGTH * z + SIDE_LENGTH * y + x) ? (byte)8 : (byte)0;                     //3      7--+-----6  |
        value |= noiseMap.Get(SIDE_LENGTH * SIDE_LENGTH * (z + 1) + SIDE_LENGTH * (y + 1) + x) ? (byte)16 : (byte)0;        //4      |  0-----+--1
        value |= noiseMap.Get(SIDE_LENGTH * SIDE_LENGTH * (z + 1) + SIDE_LENGTH * (y + 1) + x + 1) ? (byte)32 : (byte)0;    //5      | /      | /
        value |= noiseMap.Get(SIDE_LENGTH * SIDE_LENGTH * z + SIDE_LENGTH * (y + 1) + x + 1) ? (byte)64 : (byte)0;          //6      |/       |/
        value |= noiseMap.Get(SIDE_LENGTH * SIDE_LENGTH * z + SIDE_LENGTH * (y + 1) + x) ? (byte)128 : (byte)0;             //7      3--------2 
        return (byte)value;
    }
    /// <summary>
    /// Gets the index of point flag in Chunk's noiseMap BitArray based on 3D point.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public int GetIndexOfPoint(Vector3Int point) { return SIDE_LENGTH * SIDE_LENGTH * point.z + SIDE_LENGTH * point.y + point.x; }
    public int GetIndexOfPoint(int x, int y, int z) { return SIDE_LENGTH * SIDE_LENGTH * z + SIDE_LENGTH * y + x; }
}