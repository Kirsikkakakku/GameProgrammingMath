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

    public Vector3 localToWorld(Vector3 local)
    {
        return Position + Rotation * local;
    }
}
