using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealOrbital : MonoBehaviour
{

    public GameObject[,,] balls;
    public GameObject probabilityPointPrefab;

    public double length = 1;
    public int ballsPerDimension = 20;
    public double spacing;

    public int Z;

    public static double a0 = 5.29177210903 * Mathf.Pow(10, -11);
    public static double h = 6.582119569 * Mathf.Pow(10, -16);
    public static double e = 1.6021766208 * Mathf.Pow(10, -19);

    void Start()
    {
        double a = 1 / (Mathf.Sqrt(Mathf.PI));
        double b = Mathf.Pow((float)((double)Z / a0), (float)(3.0 / 2.0));

        balls = new GameObject[ballsPerDimension, ballsPerDimension, ballsPerDimension];
        spacing = length / ballsPerDimension;

        createBalls();
    }

    public void createBalls()
    {

        int xx = 0, yy = 0, zz = 0;
        xx = 0;

        for (int i = 0; i < balls.GetLength(0); i++)
        {
            for (int j = 0; j < balls.GetLength(1); j++)
            {
                for (int k = 0; k < balls.GetLength(2); k++)
                {
                    Vector3 pos = new Vector3(xx, yy, zz);
                    pos *= (float)spacing;
                    balls[i, j, k] = Instantiate(probabilityPointPrefab);
                    balls[i, j, k].transform.position = pos;

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
