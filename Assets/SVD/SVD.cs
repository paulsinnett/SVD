using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

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

    static public Matrix4x4 CalculateCovarianceMatrix(List<Vector3> points, Vector3 centroid)
    {
        float x2 = 0;
        float y2 = 0;
        float z2 = 0;
        float xy = 0;
        float xz = 0;
        float yz = 0;
        foreach (Vector3 point in points)
        {
            Vector3 d = point - centroid;
            float x = d.x;
            float y = d.y;
            float z = d.z;
            x2 += x * x;
            y2 += y * y;
            z2 += z * z;
            xy += x * y;
            xz += x * z;
            yz += y * z;
        }
        return new Matrix4x4(
            new Vector4(x2, xy, xz, 0),
            new Vector4(xy, y2, yz, 0),
            new Vector4(xz, yz, z2, 0),
            new Vector4(0, 0, 0, 1)
        );
    }

    static public Matrix4x4 GivensMatrix(int row, int column, float angle)
    {
        math.sincos(angle, out float sin, out float cos);
        Matrix4x4 givens = Matrix4x4.identity;
        givens[row, row] = cos;
        givens[row, column] = sin;
        givens[column, row] = -sin;
        givens[column, column] = cos;
        return givens;
    }

    static public void FindLargestOffDiagonalValues(Matrix4x4 matrix, out int row, out int column, out float maximum)
    {
        row = 0;
        column = 1;
        maximum = math.abs(matrix.m01);
        float m02 = math.abs(matrix.m02);
        if (m02 > maximum)
        {
            maximum = m02;
            column = 2; 
        }
        float m12 = math.abs(matrix.m12);
        if (m12 > maximum)
        {
            maximum = m12;
            row = 1;
            column = 2;
        }
    }

    static public Matrix4x4 FindZeroGivensMatrix(Matrix4x4 matrix, int row, int column)
    {
        float angle = 0.5f * math.atan2(2.0f * matrix[row, column], matrix[column, column] - matrix[row, row]);
        Matrix4x4 givens = GivensMatrix(row, column, angle);
        return givens;
    }

    static public void JacobiEigenDecomposition(Matrix4x4 matrix, float error, float [] values, Vector3 [] vectors)
    {
        float maximum;
        Matrix4x4 eigenVectors = Matrix4x4.identity;
        do
        {
            FindLargestOffDiagonalValues(matrix, out int row, out int column, out maximum);
            Matrix4x4 zero = FindZeroGivensMatrix(matrix, row, column);
            matrix = zero.transpose * matrix * zero;
            eigenVectors *= zero;
        }
        while (maximum > error);
        for (int index = 0; index < values.Length; index++)
        {
            values[index] = matrix[index, index];
            vectors[index] = eigenVectors.GetColumn(index);
        }
    }

    static public void FindBestFit(List<Vector3> points, out Plane plane, out Vector3 centroid)
    {
        centroid = GetCentroid(points);
        Matrix4x4 covariance = CalculateCovarianceMatrix(points, centroid);
        float [] values = new float [3];
        Vector3 [] vectors = new Vector3 [3];
        JacobiEigenDecomposition(covariance, 0.001f, values, vectors);
        int column = 0;
        float minimum = values[0];
        if (values[1] < minimum)
        {
            minimum = values[1];
            column = 1;
        }
        if (values[2] < minimum)
        {
            column = 2;
        }
        Vector3 normal = vectors[column];
        if (normal.y <= 0)
        {
            normal = -normal;
        }
        normal.Normalize();
        plane = new Plane(normal, centroid);
    }
}
