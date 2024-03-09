using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{

    public enum UvProjection
    {
        AngularRadial,
        ProjectZ
    }

    [Range(3, 200)]
    [SerializeField] int segments = 3;
    [SerializeField] float radius = 5f;
    [SerializeField] float thickness = 1f;

    [SerializeField] UvProjection projection = UvProjection.AngularRadial;

    float outerRadius => radius + thickness;
    int vertexCount => segments * 2;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateMesh()
    {
        //A simple skewed squarish thingy
        Vector3[] newVertices = new Vector3[4];
        newVertices[0] = new Vector3(0f, 0f, 0f);
        newVertices[1] = new Vector3(1f, 0f, 0f);
        newVertices[2] = new Vector3(0.5f, 1f, 0f);
        newVertices[3] = new Vector3(1.5f, 1f, 0);

        int[] newTriangles = new int[6];
        newTriangles[0] = 2;
        newTriangles[1] = 1;
        newTriangles[2] = 0;

        newTriangles[3] = 2;
        newTriangles[4] = 3;
        newTriangles[5] = 1;

        //Create the actual mesh
        Mesh mesh = new Mesh();
        mesh.SetVertices(newVertices);
        mesh.SetTriangles(newTriangles, 0);
        mesh.RecalculateNormals();

        //Get the mesh filter and set the mesh
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        GenerateDisk();
        //GenerateSpheres();
    }

    private void OnValidate()
    {
        //GenerateDisk();
    }

    private void GenerateDisk()
    {
        //The list for vertices
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        for(int i = 0; i < segments; i++)
        {
            float angle = 2.0f*Mathf.PI * i / (float)segments;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 point = new Vector3(x, y, 0f);
            vertices.Add(point);

            normals.Add(Vector3.forward);

            //Direction from center point
            Vector3 dir = Vector3.Normalize(transform.position + point);
            //Outer ring point
            point = dir * thickness + point;
            vertices.Add(point);

            normals.Add(Vector3.forward);

            float t = i / (float)segments;
            uvs.Add(new Vector2(t, 1));
            uvs.Add(new Vector2(t, 0));
        }

        //Just for visualizing the points for testing purposes
        /*
        foreach(Vector3 v in vertices)
        {
            Gizmos.DrawSphere(transform.position + v, 0.5f);
        }
        */

        List<int> tris = new List<int>();
        for (int i = 0; i < vertices.Count-2; i++)
        {
            //For every triangle to be facing the correct way
            if (i%2 == 0)
            {
                //1.
                tris.Add(i);
                //2.
                tris.Add(i + 2);
                //3.
                tris.Add(i + 1);
            }
            else
            {
                //1.
                tris.Add(i);
                //2.
                tris.Add(i + 1);
                //3.
                tris.Add(i + 2);
            }
        }

        //2 of the last triangles
        tris.Add(0);
        tris.Add(vertices.Count - 1);
        tris.Add(vertices.Count-2);

        tris.Add(0);
        tris.Add(1);
        tris.Add(vertices.Count - 1);

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(tris, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        //Get the mesh filter and set the mesh
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void GenerateSpheres()
    {
        for (int i = 0; i < segments; i++)
        {
            float angle = 2.0f * Mathf.PI * i / (float)segments;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position + new Vector3(x, y, 0), 0.5f);
        }
    }
}
