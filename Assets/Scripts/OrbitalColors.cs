using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalColors : MonoBehaviour
{
    public static OrbitalColors OC;

    public Color S;
    public Color P;
    public Color D;

    public float valenceAlpha = 0.6f;
    public float innerShellAlpha = 0.05f;
    public float alphaChangePerN = 0.01f;
    public float S_relativeTransperency = 0.8f; //Decimal (percentage) of what it would be if P or D

    private void Awake()
    {
        OC = this;
    }

    public Color getColor(string type, int n, int valenceN) //s p d , energy level , how many shells in atom
    {
        Color newColor = Color.white;

        switch (type)
        {
            case "S":
                newColor = S;
                newColor.a *= S_relativeTransperency;
                break;

            case "P":
                newColor = P;
                break;

            case "D":
                newColor = D;
                break;
        }

        if (n == valenceN)
        {
            newColor.a *= valenceAlpha;
        }
        else
        {
            newColor = Color.white;
            float _a = newColor.a * (innerShellAlpha - (alphaChangePerN * (valenceN - n)));
            if (_a < 0) _a = 0;
            newColor.a = _a;
        }

        return newColor;
    }
}
