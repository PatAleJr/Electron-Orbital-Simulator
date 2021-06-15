using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbitals : MonoBehaviour
{
    private Atom atom;

    public GameObject electronCloudParent;
    private bool electronCloudIsActive = true;

    public GameObject S_Prefab;
    public GameObject P_Prefab;
    public GameObject D_Prefab;
    public GameObject D_Prefab2;

    public List<GameObject> orbitals = new List<GameObject>();

    public bool[] hasOrbital = new bool[27];
    public bool[] selectedOrbital = new bool[27];

    private int valenceElectrons;

    void Start()
    {
        atom = GetComponent<Atom>();

        //Make all orbitals
        make_S_Orbital(1);

        make_S_Orbital(2);
        make_P_Orbitals(2);

        make_S_Orbital(3);
        make_P_Orbitals(3);

        make_S_Orbital(4);
        make_D_Orbitals(4);
        make_P_Orbitals(4);

        make_S_Orbital(5);
        make_D_Orbitals(5);
        make_P_Orbitals(5);

        deactivateAllOrbitals();
    }

    public void activateModel(bool activate)
    {
        electronCloudParent.SetActive(activate);
        activateCorrectOrbitals();
        electronCloudIsActive = activate;
    }

    public void deactivateAllOrbitals()
    {
        for (int i = 0; i < orbitals.Count; i++)
        {
            orbitals[i].SetActive(false);
            hasOrbital[i] = false;
            selectedOrbital[i] = false;
        }
    }

    public void make_S_Orbital(int pqn)
    {
        GameObject newOrbital = Instantiate(S_Prefab, electronCloudParent.transform);
        orbitals.Add(newOrbital);

        float scale = BohrSubshell.radii[pqn]*2;
        newOrbital.transform.localScale = new Vector3(scale, scale, scale);
    }
    public void make_P_Orbitals(int pqn)
    {
        int startIndex = orbitals.Count;
        GameObject orbital;

        orbital = Instantiate(P_Prefab, electronCloudParent.transform);
        orbitals.Add(orbital);

        orbital = Instantiate(P_Prefab, electronCloudParent.transform);
        orbital.transform.rotation = Quaternion.Euler(90, 0, 0);
        orbitals.Add(orbital);

        orbital = Instantiate(P_Prefab, electronCloudParent.transform);
        orbital.transform.rotation = Quaternion.Euler(0, 0, 90);
        orbitals.Add(orbital);

        float scale = BohrSubshell.radii[pqn]*2;
        for (int i = startIndex; i < orbitals.Count; i++)
            orbitals[i].transform.localScale = new Vector3(scale, scale, scale);
    }
    public void make_D_Orbitals(int pqn)
    {
        int startIndex = orbitals.Count;
        GameObject orbital;

        orbital = Instantiate(D_Prefab, electronCloudParent.transform);
        orbitals.Add(orbital);

        orbital = Instantiate(D_Prefab, electronCloudParent.transform);
        orbital.transform.rotation = Quaternion.Euler(90, 0, 0);
        orbitals.Add(orbital);

        orbital = Instantiate(D_Prefab, electronCloudParent.transform);
        orbital.transform.rotation = Quaternion.Euler(0, 90, 0);
        orbitals.Add(orbital);

        orbital = Instantiate(D_Prefab, electronCloudParent.transform);
        orbital.transform.rotation = Quaternion.Euler(90, 45, 0);
        orbitals.Add(orbital);

        orbital = Instantiate(D_Prefab2, electronCloudParent.transform);
        orbitals.Add(orbital);

        float scale = BohrSubshell.radii[pqn]*2;
        for (int i = startIndex; i < orbitals.Count; i++)
            orbitals[i].transform.localScale = new Vector3(scale, scale, scale);
    }

    public void selectOrbital(int index, bool selected)
    {
        selectedOrbital[index] = selected;

        if (allAreUnselected())    //Make em all visible
        {
            for (int i = 0; i < orbitals.Count; i++)
                if (hasOrbital[i]) orbitals[i].SetActive(true);
        }
        else     //Only show the selected ones
        {
            for (int i = 0; i < orbitals.Count; i++)
                orbitals[i].SetActive(hasOrbital[i] && selectedOrbital[i]);

            orbitals[index].SetActive(hasOrbital[index] && selected);
        }
    }

    private bool allAreUnselected()
    {
        foreach (bool visible in selectedOrbital)
            if (visible)    return false;

        return true;
    }

    public void updateModel(int valenceElectrons)
    {
        deactivateAllOrbitals();
        this.valenceElectrons = valenceElectrons;
        StartCoroutine(activateOrbitals());
    }

    void activateCorrectOrbitals()
    {
        for (int i = 0; i < atom.numberOfOrbitals; i++)
        {
            //Activates orbitals, but deselects all
            bool shouldBeVisible = hasOrbital[i] && (selectedOrbital[i] || allAreUnselected());
            orbitals[i].SetActive( shouldBeVisible );
        }
    }

    IEnumerator activateOrbitals()
    {
        yield return new WaitUntil(hasOrbitals);

        for (int i = 0; i < atom.numberOfOrbitals; i++)
        {
            //Activates orbitals, but deselects all
            hasOrbital[i] = true;
            selectedOrbital[i] = false;

            //Figure out principle quantum number
            int e = (i + 1) * 2; int pqn = 1;
            for (int j = 1; j < Data.ePerShell.Length; j++)
            {
                e -= Data.ePerShell[j];
                if (e <= 0)
                {
                    pqn = j;
                    break;
                }
            }

            orbitals[i].SetActive(true);
            orbitals[i].GetComponent<Orbital>().setup(pqn, atom);   //Orbital must be active otherwise error
        }

        yield return null;
    }

    bool hasOrbitals() => orbitals.Count > 0;
}