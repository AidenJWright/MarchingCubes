using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeVerts
{
    public static Vector3[] vertices;
    public static EdgeVerts Instance
    {
        get
        {
            if (Instance == null)
            { 
                Instance = new EdgeVerts();
                vertices = new Vector3[]
                {
                    new Vector3 (0.5f,0,0   ),
                    new Vector3 (0   ,0,0.5f),
                    new Vector3 (0.5f,0,1   ),
                    new Vector3 (1   ,0,0.5f),

                    new Vector3 (0,0.5f,0),
                    new Vector3 (0,0.5f,1),
                    new Vector3 (1,0.5f,0),
                    new Vector3 (1,0.5f,1),

                    new Vector3 (0.5f,1,0   ),
                    new Vector3 (0   ,1,0.5f),
                    new Vector3 (0.5f,1,1   ),
                    new Vector3 (1   ,1,0.5f)
                };
            }
            return Instance;
        }
        set { Instance = value; }
    }
}
