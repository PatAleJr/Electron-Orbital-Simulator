using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalCoordinateSystem : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public ProbabilityPoint[,,] points;
    public Vector3[] points_Vectors;

    public int totalPoints;

    public float diameter = 1;
    private float radius = 1;
    private float spacing;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        radius = diameter / 2;

        createPoints();

        renderPoints();
    }

    public void createPoints()
    {
        int pointsPerD = Mathf.RoundToInt( Mathf.Pow((float)totalPoints, 1f/3f) );
        float spacing = diameter / pointsPerD;


        points = new ProbabilityPoint[pointsPerD, pointsPerD, pointsPerD];
        points_Vectors = new Vector3[totalPoints];

        float x, y, z;
        int num = 0;

        for (int i = 0; i < pointsPerD; i++)
        {
            x = i * spacing;

            for (int j = 0; j < pointsPerD; j++)
            {
                y = j * spacing;

                for (int k = 0; k < pointsPerD; k++)
                {
                    z = k * spacing;

                    Vector3 pos = new Vector3(x - diameter/2, y - diameter / 2, z - diameter / 2);

                    float distance = Mathf.Sqrt(Mathf.Pow(pos.x, 2) + Mathf.Pow(pos.y, 2) + Mathf.Pow(pos.z, 2));

                    if (distance > radius)
                        continue;

                    points[i, j, k] = new ProbabilityPoint();
                    points_Vectors[num] = pos;

                    num++;
                }

            }

        }
    }

    public void renderPoints()
    {
        Mesh _mesh = generateMesh();
        meshFilter.mesh = _mesh;
    }

    public Mesh generateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = points_Vectors;
        // You can also apply UVs or vertex colours here.

        int[] indices = new int[points_Vectors.Length];
        for (int i = 0; i < points_Vectors.Length; i++)
            indices[i] = i;

        mesh.SetIndices(indices, MeshTopology.Points, 0);

        return mesh;
    }
}
