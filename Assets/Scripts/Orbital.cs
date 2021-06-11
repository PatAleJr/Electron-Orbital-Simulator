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

    public void Start()
    {
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
