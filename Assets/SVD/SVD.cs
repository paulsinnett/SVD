using System.Collections.Generic;
using UnityEngine;

public class SVD
{
    static public Vector3 GetCentroid(List<Vector3> points)
    {
        Vector3 centre = Vector3.zero;
        foreach (Vector3 point in points)
        {
            centre += point;
        }
        return centre /= points.Count;
    }

    static public Matrix4x4 CalculateCovarianceMatrix(List<Vector3> points, Vector3 point)
    {
        float x2 = 0;
        float y2 = 0;
        float z2 = 0;
        float xy = 0;
        float xz = 0;
        float yz = 0;
        Matrix4x4 covariance = new Matrix4x4();
        return Matrix4x4.identity;
    }

    static public Plane FindBestFit(List<Vector3> points)
    {
        Vector3 centroid = GetCentroid(points);
        Matrix4x4 covariance = CalculateCovarianceMatrix(points, centroid);
        return new Plane();
    }
}
