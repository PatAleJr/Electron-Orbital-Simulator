using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital_1s : RealOrbital
{

    public override double calculateProbability(double r1, double r2)
    {
        double probability = 0.0f;

        double a, b, term1, term2, term3, coefficient2;

        double coefficient = 4.0 / System.Math.Pow(a0, 3);

        coefficient2 = System.Math.Pow(e, -2 * r1 / a0);
        term1 = -(a0 * r1 * r1) / 2.0;
        term2 = (a0 * a0 * r1) / 2.0;
        term3 = (a0 * a0 * a0) / 4.0;
        a = coefficient2 * (term1 - term2 - term3);

        coefficient2 = System.Math.Pow(e, -2 * r2 / a0);
        term1 = -a0 * r2 * r2 / 2.0;
        term2 = (a0 * a0) * r2 / 2.0;
        term3 = (a0 * a0 * a0) / 4.0;
        b = coefficient2 * (term1 - term2 - term3);

        probability = (b - a) * coefficient;

        return probability;
    }

}
