using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Element : MonoBehaviour
{
    [Header("Basic info")]
    public int atomicNumber;
    public int atomicWeight;
    public string elementName;
    public string symbol;

    [Header("Electrons")]
    public string EConfig;
    public string EConfigShort;

    public int pqn;
    public int valenceElectrons;
    public int lastDElectrons;
    public int lastPElectrons;
    public int numberOfOrbitals;

    public int[] electronShells = new int[6];

    public string[] pqnToNobleGas = { "", "", "[He]", "[Ne]", "[Ar]", "[Kr]", "[Xe]" };

    [Header("UI elements")]
    public TextMeshProUGUI atomicNumberUI;
    public TextMeshProUGUI symbolUI;
    public Image bgImage;

    [Header("Element selection")]
    public Atom currentAtom;
    public bool selected = false;

    public Color usualColor;
    public Color selectedColor;
    private Color currentColor;

    public enum Family { Hydrogen, Alkalines, AlkalineEarths, TransitionMetals, Other, Halogens, NobleGases };
    public enum Type { Metal, Metalloid, NonMetal };
    public enum Block { S, P, D};
    public enum State { Solid, Liquid, Gas};
    [Header("Labels")]
    public Family family;
    public Type type;
    public Block block;
    public State state;

    public void setUp(int atomicNumber)
    {
        this.atomicNumber = atomicNumber;
        this.atomicWeight = Data.atomicWeights[atomicNumber - 1];
        this.elementName = Data.elements[atomicNumber - 1];

        atomicNumberUI.text = atomicNumber.ToString();
        symbolUI.text = Data.symbols[atomicNumber-1];

        //getEConfig must only be called once, otherwise will count twice as many electrons
        (string config, string shortConfig) s = getEConfig();
        EConfig = s.config;
        EConfigShort = s.shortConfig;

        setFamily();
        setBlock();
        setType();
        setState();
    }

    public void setBlock()
    {
        if (lastPElectrons > 0)
        {
            block = Block.P;
        }
        else if (lastDElectrons > 0)
        {
            block = Block.D;
        }
        else {
            block = Block.S;
        }
    }

    public void setFamily()
    {
        Family fam;

        if (valenceElectrons == 1)
        {
            fam = Family.Alkalines;

            if (atomicNumber == 1)
                fam = Family.Hydrogen;

            if (lastDElectrons > 0)
                fam = Family.TransitionMetals;
        }
        else if (valenceElectrons == 2)
        {
            fam = Family.AlkalineEarths;

            if (atomicNumber == 2)  //Helium
                fam = Family.NobleGases;

            if (lastDElectrons > 0)
                fam = Family.TransitionMetals;  //D are not considered valence
        }
        else if (lastPElectrons == 6)
        {
            fam = Family.NobleGases;
        }
        else if (lastPElectrons == 5)
        {
            fam = Family.Halogens;
        }
        else
        {
            fam = Family.Other;
        }

        this.family = fam;
    }

    public void setType()
    {
        string _type = Data.type[atomicNumber-1];

        switch (_type)
        {
            case "Metal":
                type = Type.Metal;
                break;

            case "Non-Metal":
                type = Type.NonMetal;
                break;

            case "Metalloid":
                type = Type.Metalloid;
                break;
        }
    }

    public void setState()
    {
        string _state = Data.state[atomicNumber - 1];

        switch (_state)
        {
            case "Solid":
                state = State.Solid;
                break;

            case "Liquid":
                state = State.Liquid;
                break;

            case "Gas":
                state = State.Gas;
                break;
        }
    }

    public void updateColor(PeriodicTable.ColorType colorType)
    {
        Color col = Color.white;

        switch (colorType)
        {
            case PeriodicTable.ColorType.Group:
                col = PeriodicTable.periodicTable.getElementColor(family);
                break;

            case PeriodicTable.ColorType.Block:
                col = PeriodicTable.periodicTable.getElementColor(block);
                break;

            case PeriodicTable.ColorType.Metal:
                col = PeriodicTable.periodicTable.getElementColor(type);
                break;

            case PeriodicTable.ColorType.State:
                col = PeriodicTable.periodicTable.getElementColor(state);
                break;
        }

        bgImage.color = col;
    }

    public void selectElement()
    {
        selected = true;
        PeriodicTable.periodicTable.selectedElement = this;

        currentAtom = ParticleManager.particleManager.selectedAtom;
        currentAtom.updateElement(this);
    }

    public (string config, string shortConfig) getEConfig()
    {
        int electrons = atomicNumber;
        string config = "";
        string shortConfig = "";

        bool valence = false;
        valenceElectrons = 0;
        lastDElectrons = 0;

        for (int _pqn = 1; _pqn < 6; _pqn++)
        {
            int electronsInShell = 0;

            if (Data.ePerShell[_pqn] - electrons >= 0) //Checks if this is valence shell
            {
                shortConfig += pqnToNobleGas[_pqn] + " ";
                pqn = _pqn;
                valence = true;
            }

            //S
            #region

            (string config, int electronsLeft) e;

            //Chromium and molybdenum, copper and silver
            if (pqn >= 4 && (electrons == 6 || electrons == 11))
            {
                e = fillSubShell(1, _pqn, "s");

                electronsInShell += 1;
                electrons--;
                config += e.config;
            }
            else {
                e = fillSubShell(electrons, _pqn, "s");

                electronsInShell += electrons - e.electronsLeft;
                electrons = e.electronsLeft;
                config += e.config;
            }

            if (valence)
            {
                shortConfig += e.config;
                valenceElectrons = electronsInShell;
            }

            if (electrons == 0)
                break;

            if (electronsInShell >= Data.ePerShell[_pqn])
                continue;
            #endregion

            //D
            #region
            if (_pqn > 3)
            {
                e = fillSubShell(electrons, _pqn - 1, "d");
                electronsInShell += electrons - e.electronsLeft;

                electrons = e.electronsLeft;
                config += e.config;
                if (valence)
                {
                    shortConfig += e.config;
                    lastDElectrons += electronsInShell - 2;   //-2 to remove S orbitals
                    Mathf.Clamp(lastDElectrons, 0, 10);
                }

                if (electrons == 0)
                {
                    pqn = _pqn;
                    break;
                }

                if (electronsInShell >= Data.ePerShell[_pqn])
                    continue;
            }
            #endregion

            //P
            #region
            e = fillSubShell(electrons, _pqn, "p");
            electronsInShell += electrons - e.electronsLeft;

            electrons = e.electronsLeft;
            config += e.config;

            if (valence)
            {
                lastPElectrons = electronsInShell - 2 - lastDElectrons;
                Mathf.Clamp(lastPElectrons, 0, 6);

                shortConfig += e.config;
                valenceElectrons = electronsInShell;

                //do not count e in d orbital
                if (_pqn > 3) valenceElectrons -= 10;
            }

            if (electrons == 0)
            {
                pqn = _pqn;
                break;
            }

            if (electronsInShell >= Data.ePerShell[_pqn])
                continue;
            #endregion
        }

        return (config, shortConfig);
    }

    public (string config, int electronsLeft)
        fillSubShell(int electrons, int pqn, string subshell)
    {
        int maxEToAdd = 0;
        switch (subshell)
        {
            case "s":
                maxEToAdd = 2;
                break;
            case "p":
                maxEToAdd = 6;
                break;
            case "d":
                maxEToAdd = 10;
                break;
            case "f":
                maxEToAdd = 14;
                break;
        }

        int eToAdd = (electrons > maxEToAdd) ? maxEToAdd : electrons;

        //<sup> makes superscript </sup> goes back to normal
        string config = pqn + subshell + "<sup>" + eToAdd + "</sup>" + " ";

        electrons -= eToAdd;
        electronShells[pqn] += eToAdd;

        return (config, electrons);
    }

}
