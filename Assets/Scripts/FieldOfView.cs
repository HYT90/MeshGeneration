using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float fov = 90f;
    public int rayCount = 2;
    public float viewDistance;
    private Mesh mesh;
    public LayerMask layerMask;
    Vector3 origin;
    public float startingAngle;
    public Transform player;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        SetOrigin(player.position);
    }

    void LateUpdate()
    {
        float angle = 0;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        int verticesIndex = 1;
        int trianglesIndex = 0;

        vertices[0] = origin;

        for (int i = 0; i <= rayCount; i++)
        {
            float radiant = angle * (Mathf.PI / 180f);// degrees to radiants
            Vector3 vertex;
            Physics.Raycast(origin, new Vector3(Mathf.Cos(radiant), Mathf.Sin(radiant)), out RaycastHit hit, viewDistance, layerMask);
            if(hit.collider == null)
            {
                vertex = origin + new Vector3(Mathf.Cos(radiant), Mathf.Sin(radiant)) * viewDistance;
            }else
            {
                vertex = hit.point;
            }

            if(i > 0)
            {
                triangles[trianglesIndex + 0] = 0;
                triangles[trianglesIndex + 1] = verticesIndex - 1;
                triangles[trianglesIndex + 2] = verticesIndex;

                trianglesIndex += 3;
            }

            vertices[verticesIndex] = vertex;
            verticesIndex++;

            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 position)
    {
        origin = position;
    }

    void SetAimDirection(Vector3 aimDirection)
    {
        aimDirection.Normalize();
        float n = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        float t = fov / 2f;
        startingAngle = n < 0 ? n + 360 - t : n - t;
    }
}
