using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix : MonoBehaviour
{
    public GameObject g;

    void Start()
    {
        var t = new Matrix4x4();

        Debug.Log(t);
    }

}
