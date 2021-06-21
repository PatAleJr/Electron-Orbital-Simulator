using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityPoint
{
    //Diameter of ball around this point
    private double pointDiameter;    //In Unity units
    private double bohrPointDiameter;    //In Bohr radii

    public double probability;

    private Vector3 pos;
    private Vector3 sphericalCoords;

    private double radius;   //In a0
    private float theta;    //Radians
    private float phi;  //Radians

    private int index;

    public double r1, r2;

    public ProbabilityPoint(Vector3 position, double pointDiameter, double bohrPointDiameter, int index)
    {
        pos = position;

        radius = Mathf.Sqrt( pos.x * pos.x + pos.y * pos.y + pos.z * pos.z );
        radius *= SphericalCoordinateSystem.coordsSystem.bohrRadiusPerUnit;

        r1 = System.Math.Abs(radius - (bohrPointDiameter) / 2) * RealOrbital.a0;
        r2 = System.Math.Abs(radius + (bohrPointDiameter) / 2) * RealOrbital.a0;

        float theta = Mathf.Atan2(pos.y , pos.x);
        float hyp = Mathf.Sqrt( pos.x * pos.x + pos.y * pos.y);   //Hyp on x and y plane (used to find phi)
        float phi = Mathf.Atan2( hyp , pos.z );

        sphericalCoords = new Vector3((float)radius, theta, phi);

        this.index = index;
        this.pointDiameter = pointDiameter;
        this.bohrPointDiameter = bohrPointDiameter;
    }

    public Vector3 getSphericalCoords() => sphericalCoords;

    public Vector3 getPosition() => pos;

    public (double r1, double r2) getRadiusRange() => (r1, r2);

    public void setProbability(double _probability)  { probability = _probability; }
}
