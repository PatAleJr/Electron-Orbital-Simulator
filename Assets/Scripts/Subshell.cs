using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subshell : MonoBehaviour
{
    public int pqn;
    public int numElectrons;
    private int maxElectrons;

    public bool valence;

    public float radius;
    [Range(0.01f, 0.5f)]
    public float lineThickness = 0.05f;
    private LineRenderer line;

    [Range(0, 100)]
    public int segments = 50;

    public static float[] radii = { 0f, 1f, 1.8f, 2.3f, 2.6f, 2.8f, 2.9f, 3.0f };

    public List<GameObject> _electrons = new List<GameObject>();
    public GameObject electronPrefab;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        line.startWidth = lineThickness;
        line.endWidth = lineThickness;

        updateRadius();
    }

    public void updateRadius()
    {
        radius = radii[pqn];
        CreatePoints();
        setElectrons();
    }

    public void CreatePoints()
    {
        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
        }

        line.SetPositions(points);
    }

    public void setElectrons()
    {
        for (int i = 0; i < numElectrons; i++)
        {
            //Create a new electron and set its position
            float degrees = (360 / numElectrons) * i+1;
            float radians = Mathf.Deg2Rad * degrees;

            float x = Mathf.Sin(radians) * radius;
            float y = Mathf.Cos(radians) * radius;

            Vector3 position = new Vector3(x, y, 0);

            GameObject newElectron = Instantiate(electronPrefab, transform);
            _electrons.Add(newElectron);
            newElectron.transform.localPosition = position;
        }
    }
}
