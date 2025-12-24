using System.Collections.Generic;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class TestSVD
{
    [Test]
    public void TestCentroid()
    {
        List<Vector3> points = new ();
        points.Add(new Vector3(1, 2, 3));
        points.Add(new Vector3(2, 3, 4));
        points.Add(new Vector3(3, 2, 5));
        points.Add(new Vector3(4, 4, 4));

        Vector3 centroid = SVD.GetCentroid(points);
        Assert.That(centroid, Is.EqualTo(new Vector3(2.5f, 2.75f, 4.0f)));
    }

    [Test]
    public void TestCovarianceMatrix()
    {
        List<Vector3> points = new ();
        points.Add(new Vector3(1, 2, 3));
        points.Add(new Vector3(2, 3, 4));
        points.Add(new Vector3(3, 2, 5));
        points.Add(new Vector3(4, 4, 4));

        Vector3 centroid = SVD.GetCentroid(points);
        Matrix4x4 covariance = SVD.CalculateCovarianceMatrix(points, centroid);
        Matrix4x4 expected = new Matrix4x4(
            new Vector4(5.0f, 2.5f, 2.0f, 0.0f),
            new Vector4(2.5f, 2.75f, 0.0f, 0.0f),
            new Vector4(2.0f, 0.0f, 2.0f, 0.0f),
            new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
        );
        Assert.That(covariance, Is.EqualTo(expected));
    }

    [Test]
    public void TestGivensMatrix()
    {
        int row = 0;
        int column = 1;
        float theta = math.PI / 4; // 45 degrees
        math.sincos(theta, out float sin, out float cos);

        Matrix4x4 givensMatrix = SVD.GivensMatrix(row, column, theta);

        // initialize by columns
        Matrix4x4 expected = new Matrix4x4(
            new Vector4(cos, -sin, 0, 0),
            new Vector4(sin, cos, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1)
        );

        // Expected matrix:
        // [  c  s  0  0 ]
        // [ -s  c  0  0 ]
        // [  0  0  1  0 ]
        // [  0  0  0  1 ]

        Assert.That(givensMatrix, Is.EqualTo(expected));
    }

    [Test]
    public void TestFindingLargestOffDiagonalMatrix()
    {
        Matrix4x4 matrix = new Matrix4x4(
            new Vector4(1, 2, 3, 0),
            new Vector4(2, 1, 2, 0),
            new Vector4(3, 2, 1, 0),
            new Vector4(0, 0, 0, 1));

        SVD.FindLargestOffDiagonalValues(matrix, out int row, out int column, out float largest);
        Assert.That(row, Is.EqualTo(0));
        Assert.That(column, Is.EqualTo(2));
        Assert.That(largest, Is.EqualTo(3));
    }
}
