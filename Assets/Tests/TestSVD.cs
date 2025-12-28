using System.Collections.Generic;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class TestSVD
{
    public class CompareVector3WithTolerance : IEqualityComparer<Vector3>
    {
        private readonly float tolerance;

        public CompareVector3WithTolerance(float tolerance)
        {
            this.tolerance = tolerance;
        }

        public bool Equals(Vector3 a, Vector3 b)
        {
            return Vector3.Distance(a, b) <= tolerance;
        }

        public int GetHashCode(Vector3 obj)
        {
            throw new System.NotImplementedException();
        }
    }

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

    [Test]
    public void TestJacobiDecomposition()
    {
        Matrix4x4 matrix = new Matrix4x4(
            new Vector4(4, 3, 0, 0),
            new Vector4(3, 1, 0, 0),
            new Vector4(0, 0, 2, 0),
            new Vector4(0, 0, 0, 1)
        );

        float [] values = new float [3];
        Vector3 [] vectors = new Vector3 [3];
        SVD.JacobiEigenDecomposition(matrix, 0.001f, values, vectors);

        Assert.That(values, Is.EqualTo(new float [] { -0.8541f, 5.8541f, 2f }).Within(0.001f));
        Assert.That(vectors[0], Is.EqualTo(new Vector3(0.52573f, -0.85065f, 0)).Using(new CompareVector3WithTolerance(0.001f)));
        Assert.That(vectors[1], Is.EqualTo(new Vector3(0.85065f, 0.52573f, 0)).Using(new CompareVector3WithTolerance(0.001f)));
    }
}
