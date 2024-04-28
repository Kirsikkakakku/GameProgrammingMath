using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class BezierPath : MonoBehaviour
{
    [SerializeField]
    Mesh2D shape2D;

    [SerializeField] BezierPoint[] points;
    [SerializeField] GameObject obj;
    [SerializeField] GameObject obj1;
    [SerializeField] TMP_Text sliderValueText;
    [SerializeField] Slider slider;

    [SerializeField] private Mesh mesh;
    [SerializeField] private MeshFilter meshFilter;

    private Vector3 anchor1, anchor2, anchor3, anchor4, anchorNext1, anchorNext2, anchorNext3, anchorNext4;
    private Vector3 anc1, anc2, anc3, anc4;

    [Range(3, 255)]
    public int Segments = 32;

    [Range(0, 1)]
    public float t = 0f;

    public float roadScale = 1f;

    public bool Loop = true;

    private int x = 1;

    public bool DrawLines = true;
    public bool DrawBezier = true;

    public float lapTime = 10f;

    private float interpolation = 0f;

    OrientedPoint GetOrientedPoint(float t, Vector3 anc1, Vector3 ctrl1, Vector3 ctrl2, Vector3 anc2)
    {
        Vector3 point1 = Vector3.Lerp(anc1, ctrl1, t);
        Vector3 point2 = Vector3.Lerp(ctrl1, ctrl2, t);
        Vector3 point3 = Vector3.Lerp(ctrl2, anc2, t);

        Vector3 point4 = Vector3.Lerp(point1, point2, t);
        Vector3 point5 = Vector3.Lerp(point2, point3, t);

        Vector3 point6 = Vector3.Lerp(point4, point5, t);

        OrientedPoint op;
        op.Position = point6;
        op.Rotation = Quaternion.LookRotation(point5 - point4);
        return op;
    }

    private void Start()
    {
        anc1 = points[0].GetAnchorPoint();
        anc2 = points[0].GetSecondControlPoint();
        anc3 = points[1].GetFirstControlPoint();
        anc4 = points[1].GetAnchorPoint();
        OnSliderValueChanged();
    }

    private void Update()
    {
        interpolation += Time.deltaTime;
        t = Mathf.Lerp(0, 1, interpolation / (lapTime / points.Length));

        GetBezierPointAndRotation();
    }

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

        int n = points.Length;
        int nSegments = n - 1;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        for (int i = 0; i < n - 1; i++)
        {
            Vector3 firstAnchor = points[i].GetAnchorPoint();
            Vector3 secondAnchor = points[i + 1].GetAnchorPoint();

            Vector3 firstControl = points[i].GetSecondControlPoint();
            Vector3 secondControl = points[i + 1].GetFirstControlPoint();

            if (DrawBezier)  Handles.DrawBezier(firstAnchor, secondAnchor, firstControl, secondControl, Color.green, null, 3);
        }

        if (Loop)
        {
            Vector3 firstAnchor = points[n - 1].GetAnchorPoint();
            Vector3 secondAnchor = points[0].GetAnchorPoint();

            Vector3 firstControl = points[n - 1].GetSecondControlPoint();
            Vector3 secondControl = points[0].GetFirstControlPoint();

            if (DrawBezier) Handles.DrawBezier(firstAnchor, secondAnchor, firstControl, secondControl, Color.green, null, 3);
        }

        for (int segment = 0; segment <= Segments - 1; segment++)
        {

            float TSegment = (float)segment / Segments;
            float TSegmentNext = (float)(segment + 1) / Segments;
            if (segment == Segments - 1 && Loop) TSegmentNext = 0;

            int segStart = Mathf.FloorToInt(TSegment * nSegments);
            int segStartNext = Mathf.FloorToInt(TSegmentNext * nSegments);

            if (Loop)
            {
                segStart = Mathf.FloorToInt(TSegment * n);
                segStartNext = Mathf.FloorToInt(TSegmentNext * n);
            }

            if (segStart >= nSegments && !Loop)
            {
                segStart = nSegments - 1;
            }

            if (segStartNext >= nSegments && !Loop)
            {
                segStartNext = nSegments - 1;
            }

            if (segStart >= n && Loop)
            {
                segStart = n - 1;
            }

            if (segStartNext >= n && Loop)
            {
                segStartNext = n - 1;
            }

            float tActual = (float)nSegments * TSegment - segStart * 1.0f;
            float tActualNext = nSegments * TSegmentNext - segStartNext * 1.0f;

            if (Loop)
            {
                tActual = (float)n * TSegment - segStart * 1.0f;
                tActualNext = n * TSegmentNext - segStartNext * 1.0f;
            }

            anchor1 = points[segStart].GetAnchorPoint();
            anchor2 = points[segStart].GetSecondControlPoint();

            if (segStart >= nSegments && Loop)
            {
                anchor3 = points[0].GetFirstControlPoint();
                anchor4 = points[0].GetAnchorPoint();
            }

            else
            {
                anchor3 = points[segStart + 1].GetFirstControlPoint();
                anchor4 = points[segStart + 1].GetAnchorPoint();
            }

            anchorNext1 = points[segStartNext].GetAnchorPoint();
            anchorNext2 = points[segStartNext].GetSecondControlPoint();


            if (segStartNext >= nSegments && Loop)
            {
                anchorNext3 = points[0].GetFirstControlPoint();
                anchorNext4 = points[0].GetAnchorPoint();
            }

            else
            {
                anchorNext3 = points[segStartNext + 1].GetFirstControlPoint();
                anchorNext4 = points[segStartNext + 1].GetAnchorPoint();
            }

            OrientedPoint op = GetOrientedPoint(tActual, anchor1, anchor2, anchor3, anchor4);
            OrientedPoint opNext = GetOrientedPoint(tActualNext, anchorNext1, anchorNext2, anchorNext3, anchorNext4);

            Vector3[] verts = shape2D.vertices.Select(v => op.LocalToWorldPosition(v.point * roadScale)).ToArray();
            Vector3[] vertsNext = shape2D.vertices.Select(vNext => opNext.LocalToWorldPosition(vNext.point * roadScale)).ToArray();

            for (int i = 0; i < verts.Length; i += 2)
            {
                vertices.Add(verts[i]);
                vertices.Add(vertsNext[i]);
                normals.Add(op.LocalToWorldVector(shape2D.vertices[i].normal * roadScale));
                normals.Add(op.LocalToWorldVector(shape2D.vertices[i + 1].normal * roadScale));
                uvs.Add(new Vector2(shape2D.vertices[i].u, TSegment));
                uvs.Add(new Vector2(shape2D.vertices[i + 1].u, TSegmentNext));
            }

            if (DrawLines)
            {
                if (Loop)
                {
                    for (int i = 0; i < vertices.Count; i += 2)
                    {
                        Handles.DrawLine(vertices[i], vertices[i + 1]);
                    }
                }

                else
                {
                    for (int i = 0; i < vertices.Count-16; i += 2)
                    {
                        Handles.DrawLine(vertices[i], vertices[i + 1]);
                    }
                }

                for (int i = 0; i < shape2D.lineIndices.Length; i += 2)
                {
                    Vector3 a = verts[shape2D.lineIndices[i]];
                    Vector3 b = verts[shape2D.lineIndices[i + 1]];
                    Handles.color = Color.white;
                    Handles.DrawLine(a, b);
                    Handles.color = Color.red;
                }
            }
        }

        for (int ring = 0; ring <= Segments - 1; ring++)
        {
            int rootIndex = ring * shape2D.vertices.Length;
            int rootIndexNext = (ring + 1) * shape2D.vertices.Length;
            //Special case for the last segment
            if (ring == Segments - 1 && Loop) rootIndexNext = 0;
            if (ring == Segments - 1 && !Loop) break;

            for (int line = 0; line < shape2D.lineIndices.Length; line += 2)
            {
                int lineIndexA = shape2D.lineIndices[line];
                int lineIndexB = shape2D.lineIndices[line + 1];
                int currentA = rootIndex + lineIndexA;
                int currentB = rootIndex + lineIndexB;
                int nextA = rootIndexNext + lineIndexA;
                int nextB = rootIndexNext + lineIndexB;
                triangles.Add(currentA);
                triangles.Add(nextA);
                triangles.Add(nextB);
                triangles.Add(currentA);
                triangles.Add(nextB);
                triangles.Add(currentB);
            }
        }



        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);
        mesh.RecalculateNormals();
        mesh.SetUVs(0, uvs);

        meshFilter.sharedMesh = mesh;
    }

    private void GetBezierPointAndRotation()
    {
        if (t >= 1)
        {
            t = 0;
            interpolation = 0;

            if (x == points.Length - 1)
            {
                anc1 = points[x].GetAnchorPoint();
                anc2 = points[x].GetSecondControlPoint();
                anc3 = points[0].GetFirstControlPoint();
                anc4 = points[0].GetAnchorPoint();

                x = 0;
            }

            else
            {
                anc1 = points[x].GetAnchorPoint();
                anc2 = points[x].GetSecondControlPoint();
                anc3 = points[x + 1].GetFirstControlPoint();
                anc4 = points[x + 1].GetAnchorPoint();

                x += 1;
            }
        }

        Vector3 point1 = Vector3.Lerp(anc1, anc2, t);
        Vector3 point2 = Vector3.Lerp(anc2, anc3, t);
        Vector3 point3 = Vector3.Lerp(anc3, anc4, t);

        Vector3 point4 = Vector3.Lerp(point1, point2, t);
        Vector3 point5 = Vector3.Lerp(point2, point3, t);

        Vector3 point6 = Vector3.Lerp(point4, point5, t);

        Quaternion rotation = Quaternion.LookRotation(point5 - point4);

        obj.transform.position = point6;
        obj.transform.rotation = rotation;
    }

    public void OnSliderValueChanged()
    {
        sliderValueText.text = slider.value.ToString();
        lapTime = slider.value;
    }
}
