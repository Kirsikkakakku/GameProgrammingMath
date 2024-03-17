using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OrientedPoint
{
    public Vector3 Position;
    public Quaternion Rotation;

    public OrientedPoint(Vector3 pos, Quaternion rot)
    {
        Position = pos;
        Rotation = rot;
    }

    public Vector3 LocalToWorldPosition(Vector3 localSpacePos)
    {
        return Position + Rotation * localSpacePos;
    }

    public Vector3 LocalToWorldVector(Vector3 localSpacePos)
    {
        return Rotation * localSpacePos;
    }
}
