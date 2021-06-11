using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nucleus : MonoBehaviour
{
    private int atomicWeight = 1;

    public float nucleonRadius = 0.01f;
    private float nucleonVolume;

    public float nucleusRadius;
    public float nucleusVolume;

    public float atomicWeightToNucleusSize = 0.05f;

    private void Start()
    {
        nucleonVolume = Helper.radiusToVolume(nucleonRadius);

        updateAtomicWeight(atomicWeight);
    }

    public void updateAtomicWeight(int _atomicWeight)
    {
        atomicWeight = _atomicWeight;

        nucleusVolume = atomicWeight * nucleonVolume;
        nucleusRadius = Helper.volumeToRadius(nucleusVolume);

        //Update scale of nucleus
        transform.localScale = new Vector3(nucleusRadius, nucleusRadius, nucleusRadius);
    }
}
