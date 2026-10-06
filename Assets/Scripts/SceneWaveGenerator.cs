using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneWaveGenerator : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Mesh mesh;
    public float x;
    public Vector3[] verts;

    void Start()
    {
        meshFilter = transform.Find("Plane").GetComponent<MeshFilter>();
        mesh = meshFilter.sharedMesh;
        x = Random.Range(0, 1000);
        verts = new Vector3[mesh.vertices.Length];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        verts = mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            float z = Mathf.PerlinNoise(verts[i].x+x, verts[i].y);
            verts[i] = new Vector3(verts[i].x, verts[i].y, z/5);
        }
        mesh.vertices= verts;
        meshFilter.sharedMesh = mesh;
        x += 0.004f;
    }
}
