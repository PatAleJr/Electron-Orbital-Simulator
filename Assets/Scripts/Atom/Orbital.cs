using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital : MonoBehaviour
{
    public int electrons;
    public int n;
    public bool valence;

    public Atom atom;

    public string type; //corresponds to l (0 = s, 1 = p, 2 = d)

    private List<MeshRenderer> renderers = new List<MeshRenderer>();
    public Color myColor;

    private RealOrbital myRealOrbital;

    public RealOrbital[] realOrbitals;

    public void Start()
    {
        realOrbitals = new RealOrbital[]
        {
            new Orbital_1s()
        };

        foreach (Transform child in transform)
        {
            if (child.GetComponent<MeshRenderer>() != null)
            {
                renderers.Add(child.GetComponent<MeshRenderer>());
            }
        }
    }

    public void setup(int _pqn, Atom _atom)
    {
        n = _pqn;
        type = gameObject.tag;
        atom = _atom;
        valence = n == atom.pqn;
        gameObject.name = n + type;

        Color newColor = OrbitalColors.OC.getColor(type, n, atom.pqn);
        myColor = newColor;

        if (isActiveAndEnabled) StartCoroutine(_setColor(myColor));

        //Real orbital
        string realOrbitalClassName = "Orbital_" + n + name;
        System.Type realOrbitalClass = System.Type.GetType(realOrbitalClassName);
        myRealOrbital = (RealOrbital)System.Activator.CreateInstance(realOrbitalClass);
    }

    public void OnEnable()
    {
        StartCoroutine(_setColor(myColor));
    }

    IEnumerator _setColor(Color newColor)
    {
        while (renderers.Count == 0)
            yield return null;

        foreach (MeshRenderer renderer in renderers)
            renderer.material.color = newColor;
    }
}
