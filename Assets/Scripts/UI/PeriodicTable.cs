using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PeriodicTable : MonoBehaviour
{
    public static PeriodicTable periodicTable;
    private void Awake()
    {
        if (PeriodicTable.periodicTable != this)
        {
            if (PeriodicTable.periodicTable != null)
                Destroy(PeriodicTable.periodicTable);
            periodicTable = this;
        }
    }

    public enum ColorType { Group = 0, Block = 1, Metal = 2, State = 3};
    public ColorType selectedColorType;

    public Element[] elements;

    public TMP_Dropdown dropdown;

    [Header("Colors - families")]
    public Color nobleGases;
    public Color halogens;
    public Color alkalines;
    public Color alkalineEarths;
    public Color transitionMetals;
    public Color other;

    [Header("Colors - orbital block")]
    public Color sBlock;
    public Color pBlock;
    public Color dBlock;

    [Header("Colors - type")]   //Metal / nonMetal / Metalloid
    public Color metal;
    public Color nonMetal;
    public Color metalloid;

    [Header("Colors - State")]   //Metal / nonMetal / Metalloid
    public Color solid;
    public Color liquid;
    public Color gas;

    [Header("Selection")]
    public Element selectedElement;

    void Start()
    {
        //Sets up the elements
        int n = 1;
        foreach (Element element in elements)
        {
            element.setUp(n);
            element.updateColor(ColorType.Group);
            n++;
        }

        ParticleManager.particleManager.selectedAtom.updateElement(PeriodicTable.periodicTable.elements[0]);
    }

    public Color getElementColor(Element.Family fam)
    {
        Color col = Color.white;

        switch (fam)
        {
            case Element.Family.Alkalines:
                col = alkalines;
                break;

            case Element.Family.AlkalineEarths:
                col = alkalineEarths;
                break;

            case Element.Family.TransitionMetals:
                col = transitionMetals;
                break;

            case Element.Family.Other:
                col = other;
                break;

            case Element.Family.Halogens:
                col = halogens;
                break;

            case Element.Family.NobleGases:
                col = nobleGases;
                break;
        }
        return col;
    }

    public Color getElementColor(Element.Block block)
    {
        Color col = Color.white;

        switch (block)
        {
            case Element.Block.S:
                col = sBlock;
                break;

            case Element.Block.P:
                col = pBlock;
                break;

            case Element.Block.D:
                col = dBlock;
                break;
        }

        return col;
    }

    public Color getElementColor(Element.Type type)
    {
        Color col = Color.white;

        switch (type)
        {
            case Element.Type.Metal:
                col = metal;
                break;

            case Element.Type.NonMetal:
                col = nonMetal;
                break;

            case Element.Type.Metalloid:
                col = metalloid;
                break;
        }

        return col;
    }

    public Color getElementColor(Element.State state)
    {
        Color col = Color.white;

        switch (state)
        {
            case Element.State.Solid:
                col = solid;
                break;

            case Element.State.Liquid:
                col = liquid;
                break;

            case Element.State.Gas:
                col = gas;
                break;
        }

        return col;
    }

    public void setColorType(int x) //Parameter not used, but is needed
    {
        selectedColorType = (ColorType)dropdown.value;

        foreach (Element element in elements)
        {
            element.updateColor(selectedColorType);
        }
    }
}
