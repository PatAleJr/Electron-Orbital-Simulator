using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalCoordinateSystem : MonoBehaviour
{
    public static SphericalCoordinateSystem coordsSystem;

    private void Awake()
    {
        if (SphericalCoordinateSystem.coordsSystem != this)
        {
            if (SphericalCoordinateSystem.coordsSystem != null)
                Destroy(SphericalCoordinateSystem.coordsSystem);
            coordsSystem = this;
        }
    }

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public int totalPoints;
    public float length = 1;
    public float bohrRadiusPerUnit = 10f;
    private float spacing;

    private ProbabilityPoint[] points;
    private Vector3[] pointPositions;   //Only needed to render mesh for debuging
    private bool _pointsExist = false;

    private List<ProbabilityPoint> visiblePoints = new List<ProbabilityPoint>();
    private List<Vector3> visiblePointPositions = new List<Vector3>();

    void Start()
    {
        _pointsExist = false;

        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        createPoints();
        renderPoints();
    }

    public void createPoints()
    {
        int pointsPerD = Mathf.RoundToInt( Mathf.Pow((float)totalPoints, 1f/3f) );
        totalPoints = pointsPerD * pointsPerD * pointsPerD;
        spacing = length / pointsPerD;

        points = new ProbabilityPoint[totalPoints];
        pointPositions = new Vector3[totalPoints];

        double bohrPointDiameter = spacing * bohrRadiusPerUnit;

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

                    Vector3 pos = new Vector3(x - length / 2, y - length / 2, z - length / 2);

                    float distance = Mathf.Sqrt(Mathf.Pow(pos.x, 2) + Mathf.Pow(pos.y, 2) + Mathf.Pow(pos.z, 2));

                    points[num] = new ProbabilityPoint(pos, spacing, bohrPointDiameter, num);
                    pointPositions[num] = pos;

                    num++;
                }
            }
        }

        _pointsExist = true;
    }

    public void renderPoints()
    {
        Mesh _mesh = generateMesh(pointPositions);
        meshFilter.mesh = _mesh;
    }

    private Mesh generateMesh(Vector3[] pointsToRender)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = pointsToRender;
        // You can also apply UVs or vertex colours here.

        int[] indices = new int[pointsToRender.Length];
        for (int i = 0; i < pointsToRender.Length; i++)
            indices[i] = i;

        mesh.SetIndices(indices, MeshTopology.Points, 0);

        return mesh;
    }

    public void updateModel()
    {
        visiblePointPositions.Clear();
        visiblePoints.Clear();

        foreach (ProbabilityPoint point in points)
        {
            float probability = (float)point.probability;

            float random = Random.Range(0.000f, 1.000f);

            if (random <= probability)
            {
                visiblePoints.Add(point);
                visiblePointPositions.Add(point.getPosition());
            }
        }

        meshFilter.mesh = generateMesh(visiblePointPositions.ToArray());
        Debug.Log(meshFilter.mesh.vertexCount);
    }

    public bool pointsExist() => _pointsExist;

    public ProbabilityPoint[] getPoints() => points;
}
