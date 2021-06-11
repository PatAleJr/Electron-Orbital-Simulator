using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;

    private RectTransform canvasRect;
    private Camera cam;

    private ParticleManager particleManager;
    public OrbitalDiagram orbitalDiagram;

    public GameObject createAtomButton;

    public GameObject simUI;

    public GameObject atomInfoUI;
    public GameObject periodicTable;

    public TextMeshProUGUI elementText;
    public TextMeshProUGUI atomicNumberText;
    public TextMeshProUGUI configText;

    public GameObject incrementANButton;
    public GameObject decrementANButton;

    public Atom currentAtom;

    public static bool shortConfig;

    public GameObject atomScaling;

    public void Awake()
    {
        if (UIManager.uiManager != this)
        {
            if (UIManager.uiManager != null)
                Destroy(UIManager.uiManager);
            uiManager = this;
        }
    }

    public void toggleCreateAtomButton(bool active)
    {
        createAtomButton.SetActive(active);
    }

    public void setAtomInfo(Atom atom)
    {
        currentAtom = atom;

        //Set UI
        simUI.SetActive(true);

        elementText.text = atom.elementName;
        atomicNumberText.text = atom.atomicNumber.ToString();

        if (shortConfig)
        {
            configText.text = atom.EConfigShort;
        } else
        {
            configText.text = atom.EConfig;
        }

        //Orbital Diagram
        orbitalDiagram.updateDiagram(currentAtom);
    }

    public void toggleShortConfig()
    {
        shortConfig = !shortConfig;
        setAtomInfo(currentAtom);
    }

    public void setIncrementANButton(bool active)
    {
        incrementANButton.SetActive(active);
    }

    public void setDecrementANButton(bool active)
    {
        decrementANButton.SetActive(active);
    }

    public void removeAtomInfo()
    {
        simUI.SetActive(false);
    }
}
