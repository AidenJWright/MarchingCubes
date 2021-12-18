using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager 
{
    private MeshManager() 
    {
        
    }
    private static MeshManager instance = null;
    public static MeshManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MeshManager();
            }
            return instance;
        }
    }
}
