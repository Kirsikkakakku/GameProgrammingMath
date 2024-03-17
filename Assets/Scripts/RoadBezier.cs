using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RoadBezier : MonoBehaviour
{

    [SerializeField]
    public Mesh2D Roadshape;
    [Range(0.1f, 100f)]
    public float RoadScaler = 1.0f;


    public bool DrawSegments = false;
    public bool DrawBezier = false;

    public BezierPoint[] points;

    public bool ClosedPath = false;

    //[Range(100, 1000)]
    //public int Segments = 500;

    ///[Range(0, 1000)]
    //public int CurrentSegment = 0;

    [Range(0f, 1f)]
    public float TSimulate = 0.0f;

    [Range(3, 255)]
    public int Slices = 32;

    public GameObject MyObject;


    public Mesh mesh;

    OrientedPoint getBezierOrientedPoint(float t, Vector3 first_a, Vector3 first_c,
                                    Vector3 second_c, Vector3 second_a)
    {
        OrientedPoint op;

        Vector3 a = Vector3.Lerp(first_a, first_c, t);
        Vector3 b = Vector3.Lerp(first_c, second_c, t);
        Vector3 c = Vector3.Lerp(second_c, second_a, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 bez = Vector3.Lerp(d, e, t);

        op.Position = bez;

        Quaternion rotation = Quaternion.LookRotation(e - d);
        op.Rotation = rotation;

        return op;
    }

    /*
    // Computes the point on the bezier curve
    Vector3 getBezierPoint(float t, Vector3 first_a, Vector3 first_c, 
                                    Vector3 second_c, Vector3 second_a)
    {
        Vector3 a = Vector3.Lerp(first_a, first_c, t);
        Vector3 b = Vector3.Lerp(first_c, second_c, t);
        Vector3 c = Vector3.Lerp(second_c, second_a, t);
 
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
 
        Vector3 bez = Vector3.Lerp(d, e, t);
        return bez;
    }
 
    Quaternion getBezierRotation(float t, Vector3 first_a, Vector3 first_c,
                                    Vector3 second_c, Vector3 second_a)
    {
        Vector3 a = Vector3.Lerp(first_a, first_c, t);
        Vector3 b = Vector3.Lerp(first_c, second_c, t);
        Vector3 c = Vector3.Lerp(second_c, second_a, t);
 
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
 
        Quaternion rotation = Quaternion.LookRotation(e - d);
        return rotation;
 
    }
    */

    private void OnDrawGizmos()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        else
        {
            mesh.Clear();
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // How many BezierPoints there are?
        int n = points.Length;
        int nSegments = n - 1;

        // Loop from first item in the array until 2nd last item
        // Because we are using index i+1 inside the loop
        for (int i = 0; i < n - 1; i++)
        {
            // I need 2 bezier points: i & i+1
            Vector3 first_anchor = points[i].GetAnchorPoint();
            Vector3 second_anchor = points[i + 1].GetAnchorPoint();

            Vector3 first_control = points[i].GetSecondControlPoint();
            Vector3 second_control = points[i + 1].GetFirstControlPoint();

            if (DrawBezier)
                Handles.DrawBezier(first_anchor, second_anchor, first_control, second_control,
                Color.green, null, 3f);
        }
        if (ClosedPath)
        {
            // The first bezier points is at: n-1 (???)
            // The second point is at index: 0
            Vector3 first_anchor = points[n - 1].GetAnchorPoint();
            Vector3 second_anchor = points[0].GetAnchorPoint();

            Vector3 first_control = points[n - 1].GetSecondControlPoint();
            Vector3 second_control = points[0].GetFirstControlPoint();

            Handles.DrawBezier(first_anchor, second_anchor, first_control, second_control,
                Color.green, null, 3f);
        }


        /*
                    Vector3 first_a = points[seg_start].getAnchorPoint();
                    Vector3 second_a = points[seg_start+1].getAnchorPoint();
 
                    Vector3 first_c = points[seg_start].getSecondControlPoint();
                    Vector3 second_c = points[seg_start+1].getFirstControlPoint();
        */

        // Loop through the slices
        for (int slice = 0; slice <= Slices; slice++)
        {

            float TSlice = (float)slice / Slices;  // 0.0f ... 1.0f
            float TSliceNext = (float)(slice + 1) / Slices;  // 0.0f ... 1.0f

            int seg_start = Mathf.FloorToInt(TSlice * nSegments);
            int seg_start_next = Mathf.FloorToInt(TSliceNext * nSegments);
            if (seg_start >= nSegments)
            {
                seg_start = nSegments - 1;
            }
            if (seg_start_next >= nSegments)
            {
                seg_start_next = nSegments - 1;
            }


            Vector3 first_a = points[seg_start].GetAnchorPoint();
            Vector3 second_a = points[seg_start + 1].GetAnchorPoint();
            Vector3 first_c = points[seg_start].GetSecondControlPoint();
            Vector3 second_c = points[seg_start + 1].GetFirstControlPoint();

            Vector3 first_a_next = points[seg_start_next].GetAnchorPoint();
            Vector3 second_a_next = points[seg_start_next + 1].GetAnchorPoint();
            Vector3 first_c_next = points[seg_start_next].GetSecondControlPoint();
            Vector3 second_c_next = points[seg_start_next + 1].GetFirstControlPoint();


            float TActual = (float)nSegments * TSlice - seg_start * 1.0f;
            float TActualNext = (float)nSegments * TSliceNext - seg_start_next * 1.0f;

            //float TActual = (float) (nSegments) * (TSimulate - seg_start*1.0f / (float)(nSegments));
            // The 1st version:
            //float TActual = (TSimulate - seg_start*1.0f / (float)(nSegments)) / (1.0f / (float)(nSegments));

            OrientedPoint op = getBezierOrientedPoint(TActual, first_a, first_c, second_c, second_a);
            OrientedPoint op_next = getBezierOrientedPoint(TActualNext, first_a_next, first_c_next, second_c_next, second_a_next);


            /*
            if (MyObject)
            {
                MyObject.transform.position = op.Position;
                MyObject.transform.rotation = op.Rotation;
            }
            */

            //Gizmos.color = Color.red;
            for (int i = 0; i < Roadshape.vertices.Length; i++)
            {
                int j = i + 2;
                j = j % Roadshape.vertices.Length;
                Vector3 roadpoint = Roadshape.vertices[i].point;
                Vector3 roadpoint_next = Roadshape.vertices[j].point;

                Vector3 transformed_point = op.LocalToWorldPosition(roadpoint * RoadScaler);
                vertices.Add(transformed_point);

                Vector3 transformed_point_next = op.LocalToWorldPosition(roadpoint_next * RoadScaler);

                Vector3 transformed_point_following = op_next.LocalToWorldPosition(roadpoint * RoadScaler);

                //Gizmos.DrawSphere(transformed_point, HandleUtility.GetHandleSize(transformed_point) * 0.1f);
                if (DrawSegments)
                {
                    Handles.color = Color.white;
                    Handles.DrawLine(transformed_point, transformed_point_next, 3f);
                    Handles.DrawLine(transformed_point, transformed_point_following, 1f);
                }
            }

            // Triangles
            for (int i = 0; i < Roadshape.vertices.Length - 2; i += 2)
            {
                // hack hack
                if (slice == Slices)
                {
                    break;
                }

                int first_start = slice * Roadshape.vertices.Length + i + 1;
                int first_end = first_start + 1;

                int second_start = first_start + Roadshape.vertices.Length;
                int second_end = first_end + Roadshape.vertices.Length;

                // 1st triangle
                triangles.Add(first_start);
                triangles.Add(second_start);
                triangles.Add(second_end);

                // 2nd triangle
                triangles.Add(first_start);
                triangles.Add(second_end);
                triangles.Add(first_end);
            }

            if (slice < Slices)
            {
                // Special case, loop around the 2D mesh
                int index_start = slice * Roadshape.vertices.Length + 15;
                int index_end = slice * Roadshape.vertices.Length;

                int next_start = (slice + 1) * Roadshape.vertices.Length + 15;
                int next_end = (slice + 1) * Roadshape.vertices.Length;

                // 1st triangle
                triangles.Add(index_start);
                triangles.Add(next_start);
                triangles.Add(next_end);

                // 2nd triangle
                triangles.Add(index_start);
                triangles.Add(next_end);
                triangles.Add(index_end);

            }


        }


        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        //}

    }

}