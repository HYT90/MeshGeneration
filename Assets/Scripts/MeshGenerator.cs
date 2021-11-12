using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    public int xSize = 5;
    public int ySize = 10;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
    }

    private void Update()
    {
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        
        for(int i = 0, k = 0; i <= xSize; ++i)
        {
            for(int j = 0; j <= ySize; ++j)
            {
                //float z = Mathf.PerlinNoise(i * .3f, j * .3f) * 3f;
                vertices[k++] = new Vector3(i, j, 0);
            }
        }

        triangles = new int[xSize * ySize * 6];
        int vert = 0, tris = 0;

        for (int j = 0; j < xSize; ++j)
        {
            for (int i = 0; i < ySize; ++i)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + 1;
                triangles[tris + 2] = vert + ySize + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + ySize + 2;
                triangles[tris + 5] = vert + ySize + 1;

                vert++;
                tris += 6;
            }
            vert++;
        }

        /*
        uv = new Vector2[4];

        uv[0] = Vector2.zero;
        uv[1] = Vector2.up;
        uv[2] = Vector2.right;
        uv[3] = Vector2.one;
        */

        StartCoroutine(SetUV());
    }

    IEnumerator SetUV()
    {
        uv = new Vector2[vertices.Length];

        for (int x = 0, i = 0; x <= xSize; ++x)
        {
            for (int y = 0; y <= ySize; ++y)
            {
                uv[i++] = new Vector2((float)(x) / xSize, (float)(y) / ySize);
                //uv[i++] = new Vector2((float)(xSize - y) / xSize, (float)(ySize - x) / ySize);
                yield return 0;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for(int i = 0; i<vertices.Length; ++i)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
