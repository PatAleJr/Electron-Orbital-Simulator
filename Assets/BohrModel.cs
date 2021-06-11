using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BohrModel : MonoBehaviour
{
    public GameObject BohrModelParent;
    private bool bohrModelIsActive;

    public GameObject subshellPrefab;
    public List<Subshell> subshells = new List<Subshell>();

    private Atom atom;
    private int pqn;

    void Start()
    {
        atom = GetComponent<Atom>();
        pqn = atom.pqn;
    }

    public void activateModel(bool activate)
    {
        BohrModelParent.SetActive(activate);
        bohrModelIsActive = activate;

        updateSubshells();
    }

    private void clearSubshells()
    {
        foreach (Subshell subshell in subshells)
            Destroy(subshell.gameObject);

        subshells.Clear();
    }

    public void updateSubshells()
    {
        if (atom == null)
            atom = GetComponent<Atom>();

        pqn = atom.pqn;

        clearSubshells();

        for (int i = 0; i < pqn; i++)
        {
            int n = i + 1;  //Convert index in array to pqn

            GameObject _s = Instantiate(subshellPrefab, BohrModelParent.transform);
            Subshell s = _s.GetComponent<Subshell>();

            subshells.Add(s);

            s.pqn = n;
            s.numElectrons = atom.electronShells[n];

            if (n == pqn)
                s.valence = true;
        }
    }

}
