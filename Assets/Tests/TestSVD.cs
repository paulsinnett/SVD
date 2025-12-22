using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSVD
{
    [Test]
    public void TestCentroid()
    {
        List<Vector3> points = new ();
        points.Add(new Vector3(1, 2, 3));
        points.Add(new Vector3(2, 2, 2));
        points.Add(new Vector3(3, 2, 1));

        Vector3 centroid = SVD.GetCentroid(points);
        Assert.That(centroid, Is.EqualTo(new Vector3(2, 2, 2)));
    }
}
