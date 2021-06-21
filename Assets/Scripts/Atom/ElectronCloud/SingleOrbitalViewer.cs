using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleOrbitalViewer : MonoBehaviour
{

    RealOrbital myRealOrbital;

    void Start()
    {
        myRealOrbital = new Orbital_1s();
        myRealOrbital.Z = 1;

        StartCoroutine(showOrbital());
    }

    IEnumerator showOrbital()
    {
        yield return new WaitUntil(SphericalCoordinateSystem.coordsSystem.pointsExist);

        yield return new WaitForSeconds(1f);

        myRealOrbital.generateCloud();
        SphericalCoordinateSystem.coordsSystem.updateModel();
    }

}
