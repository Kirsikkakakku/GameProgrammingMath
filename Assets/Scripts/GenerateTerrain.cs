using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{

    [System.Serializable]
    public class NoiseParams
    {
        [Range(0.1f, 100f)]
        public float AmplitudeScale;
        [Range(0.1f, 100f)]
        public float FrequencyScale;
    }

    [Range(1f, 1000f)]
    public float Size = 100f;

    [Range(2, 255)]
    public int Segments = 100;

    public NoiseParams[] NoiseLayers = new NoiseParams[0];

    public bool ClampBelowZero = true;

    [Range(-10f, 10f)]
    public float ClampBelow = 0f;

    private Mesh mesh = null;

    private void OnValidate()
    {
        GenerateMesh();
    }

    private void Update()
    {
        //GenerateMesh();
    }

    public void GenerateMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        else
        {
            mesh.Clear();
        }

        //List of vertices
        List<Vector3> vertices = new List<Vector3>();
        //List of triangles
        List<int> triangles = new List<int>();

        //Loop

        //Vertices
        for (int y_seg = 0; y_seg <= Segments; y_seg++)
        {
            for (int x_seg = 0; x_seg <= Segments; x_seg++)
            {
                float x = x_seg * (Size / (float)Segments);
                float y = y_seg * (Size / (float)Segments);
                //Plain random noise
                //float z = Random.Range(0.0f, 1.0f) * AmplitudeScaler;
                //Perlin noise
                //Add all noises together
                // - Array of noise parameters (freqyency scale, amplitude scale)
                //
                float z = 0f;

                for (int i = 0; i < NoiseLayers.Length; i++)
                {
                    z += (Mathf.PerlinNoise(x / NoiseLayers[i].FrequencyScale, y / NoiseLayers[i].FrequencyScale) - 0.5f) * NoiseLayers[i].AmplitudeScale;
                }

                if (ClampBelowZero && z < ClampBelow)
                {
                    z = ClampBelow;
                }

                //In Unity, z is forward and y is up
                Vector3 vert = new Vector3(x, z, y);
                //Debug.Log(vert);
                //Add the vertex
                vertices.Add(vert);
            }
        }

        //Triangles
        for (int y_seg = 0; y_seg < Segments; y_seg++)
        {
            for (int x_seg = 0; x_seg < Segments; x_seg++)
            {
                int topLeft = x_seg + y_seg * (Segments + 1);
                int topRight = topLeft + 1;
                int botLeft = topLeft + Segments + 1;
                int botRight = botLeft + 1;

                //1st tri
                triangles.Add(topLeft);
                triangles.Add(botLeft);
                triangles.Add(topRight);

                //2nd tri
                triangles.Add(topRight);
                triangles.Add(botLeft);
                triangles.Add(botRight);
            }
        }

        //Assign the vertices and triangles
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        //Assign the mesh
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
