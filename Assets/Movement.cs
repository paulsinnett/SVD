using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    public float speed = 5;
    public InputActionReference move;
    public Terrain terrain;
    TerrainData terrainData;
    Vector3 position;

    public void Start()
    {
        terrainData = terrain.terrainData;
    }

    public void OnEnable()
    {
        move.action.Enable();
    }

    public void OnDisable()
    {
        move.action.Disable();
    }

    public void Update()
    {
        Vector2 movement = move.action.ReadValue<Vector2>();
        position += speed * Time.deltaTime * new Vector3(movement.x, 0, movement.y);
        transform.position = position;
        
        int width = 10;
        int depth = 10;
        float height = terrainData.heightmapScale.y;
        float scale = terrainData.size.x;
        List<Vector3> terrainSamples = new ();
        Vector3 sample = Vector3.zero;
        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= depth; y++)
            {
                sample.x = position.x + (x - width / 2);
                sample.z = position.z + (y - depth / 2);
                sample.y = terrain.SampleHeight(sample);
                terrainSamples.Add(sample);
            }
        }
        SVD.FindBestFit(terrainSamples, out Plane plane, out Vector3 centroid);
        Vector3 direction = Vector3.Cross(Vector3.right, plane.normal);
        transform.rotation = Quaternion.LookRotation(direction, plane.normal);
        centroid += plane.normal * 0.01f;
        transform.position = centroid;
    }
}
