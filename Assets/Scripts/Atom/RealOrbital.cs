using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealOrbital : MonoBehaviour
{
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
        */
    }
}
