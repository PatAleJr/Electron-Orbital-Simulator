using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour
{
    [Header("Basic info")]
    public string elementName;
    public int atomicNumber;
    public int atomicWeight;
    public Element element;

    [Header("Electrons")]
    public string EConfig;
    public string EConfigShort;

    public int pqn;
    public int valenceElectrons;
    public int lastDElectrons;
    public int numberOfOrbitals;

    public string[] pqnToNobleGas = { "", "", "[He]", "[Ne]", "[Ar]", "[Kr]", "[Xe]"};

    private UIManager uiManager;
    private Orbitals orbitals;
    private Nucleus nucleus;
    private BohrModel bohrModel;

    [Header("Scaling")]
    public float sizeChangePerValenceE = 0.02f;
    public float sizeChangePerDE = 0.01f;

    public int[] electronShells = new int[8];

    private void Start()
    {
        uiManager = UIManager.uiManager;

        orbitals = GetComponent<Orbitals>();
        nucleus = GetComponentInChildren<Nucleus>();
        bohrModel = GetComponent<BohrModel>();

        //updateelement() is called in start method of periodic table. because data must be initialized before getting information
    }

    public void updateElement(Element element)
    {
        this.element = element;

        atomicNumber = element.atomicNumber;
        atomicWeight = element.atomicWeight;
        EConfig = element.EConfig;
        EConfigShort = element.EConfigShort;
        valenceElectrons = element.valenceElectrons;
        elementName = element.elementName;
        pqn = element.pqn;
        electronShells = element.electronShells;

        orbitals.updateModel(valenceElectrons);
        bohrModel.updateSubshells();

        nucleus.updateAtomicWeight(atomicWeight);

        uiManager.setAtomInfo(this);

        changeScale();

        updateViewMode(ParticleManager.particleManager.currentViewMode);
    }

    public void changeScale()
    {
        //scales down per electron in valence shell
        float newScale = 1 - sizeChangePerValenceE *valenceElectrons;
        newScale -= sizeChangePerDE * lastDElectrons;

        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public GameObject getLastSOrbital()
    {
        //Gets index of last S orbital in orbital list in orbitals component
        int orbitalIndex = 0;
        for (int i = 1; i < pqn; i++)
            orbitalIndex += Data.ePerShell[i] / 2;

        if (orbitals == null)
            return null;
        if (orbitals.orbitals.Count <= orbitalIndex)
            return null;

        //Returns the object of the orbital
        return orbitals.orbitals[orbitalIndex];
    }

    public Orbitals getOrbitals() => orbitals;

    public void updateViewMode(ParticleManager.ViewMode newViewMode)
    {
        if (newViewMode == ParticleManager.ViewMode.ElectronCloud)
        {
            orbitals.activateModel(true);
            bohrModel.activateModel(false);
        }
        else
        {
            bohrModel.activateModel(true);
            orbitals.activateModel(false);
        }
    }
}