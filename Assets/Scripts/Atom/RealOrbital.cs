using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealOrbital : MonoBehaviour
{

    public GameObject[,,] points;
    public GameObject probabilityPointPrefab;

    public int numSpheres;
    public int totalPoints;
    public float spacing;

    [Header("Points")]
    public float baseScale = 0.02f; 

    public int Z;

    public static double a0 = 5.29177210903 * Mathf.Pow(10, -11);
    public static double h = 6.582119569 * Mathf.Pow(10, -16);
    public static double e = 1.6021766208 * Mathf.Pow(10, -19);

    void Start()
    {
        /*
        double a = 1 / (Mathf.Sqrt(Mathf.PI));
        double b = Mathf.Pow((float)((double)Z / a0), (float)(3.0 / 2.0));
        double p = Z * r / a0;
        double c = Mathf.Pow((float)e, -(float)p);

        points = new GameObject[ballsPerDimension, ballsPerDimension, ballsPerDimension];
        spacing = length / ballsPerDimension;
        */

        points = new GameObject[numSpheres, totalPoints, totalPoints];

        createBalls();
    }

    public void createBalls()
    {
        double summation = numSpheres * (numSpheres + 1) * (2*numSpheres + 1) / 6;
        double totalArea = 4 * Mathf.PI * summation;

        int num = 1;    //For debuging

        for (int n = 1; n <= numSpheres; n++)
        {
            //Find how many points in this sphere
            float r = n * spacing;
            float area = 4 * Mathf.PI * Mathf.Pow( (float) r, 2);

            int pointsToMake = Mathf.RoundToInt( (float) (area * totalPoints / totalArea) );

            //Make em
            float angleRad =  (2 * Mathf.PI) / pointsToMake ;
            float x, y, z;

            int i = 0;

            for (float a = 0; a < 2 * Mathf.PI; a += angleRad)
            {
                int j = 0;

                for (float b = 0; b < 2 * Mathf.PI; b += angleRad)
                {
                    x = r * Mathf.Cos(a) * Mathf.Sin(b);
                    y = r * Mathf.Sin(a) * Mathf.Sin(b);
                    z = r * Mathf.Sin(b);

                    Vector3 pos = new Vector3(x, y, z);
                    points[n - 1, i, j] = Instantiate(probabilityPointPrefab);
                    points[n - 1, i, j].transform.position = pos;
                    points[n - 1, i, j].name += num;

                    j++;

                    num++;
                }
                i++;
            }

        }

    }

    /*
     *      for (float a = 0; a < 2*Mathf.PI; a += angleRad)
            {
                x = r * Mathf.Cos(a);
                y = r * Mathf.Sin(a);

                int j = 0;

                for (float b = 0; b < 2*Mathf.PI; b += angleRad)
                {
                    z = r * Mathf.Sin(b);

                    Vector3 pos = new Vector3(x, y, z);
                    points[n, i, j] = Instantiate(probabilityPointPrefab);
                    points[n, i, j].transform.position = pos;

                    j++;
                }
                i++;
            }
     * */

    public void _createBalls()
    {
        int xx = 0, yy = 0, zz = 0;
        xx = 0;

        for (int i = 0; i < points.GetLength(0); i++)
        {
            for (int j = 0; j < points.GetLength(1); j++)
            {
                for (int k = 0; k < points.GetLength(2); k++)
                {
                    Vector3 pos = new Vector3(xx, yy, zz);
                    pos *= (float)spacing;
                    points[i, j, k] = Instantiate(probabilityPointPrefab);
                    points[i, j, k].transform.position = pos;

                    zz++;
                }

                zz = 0;
                yy++;
            }

            yy = 0;
            xx++;
        }
    }
}
