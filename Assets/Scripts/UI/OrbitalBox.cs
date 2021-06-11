using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbitalBox : MonoBehaviour
{
    private Orbitals orbitals;

    public int orbitalIndex;
    public string label;  //Needed because is refferenced by other scripts

    public bool selected = false;

    public Image boxImage;
    public Color selectedColor;
    public Color unselecetColor;

    public Button button;

    public List<Image> arrows = new List<Image>();

    public void Start()
    {
        button = transform.GetComponentInChildren<Button>();
        button.transform.SetAsLastSibling();

        orbitals = ParticleManager.particleManager.selectedAtom.getOrbitals();

        foreach (Image arrow in arrows)
            arrow.color = selected ? selectedColor : unselecetColor;
    }

    public void updateArrows(List<GameObject> _arrows)
    {
        arrows.Clear();

        foreach (GameObject arrow in _arrows)
            arrows.Add(arrow.GetComponent<Image>());

        if (button != null)
            button.transform.SetAsLastSibling();
    }

    public void clickBox()
    {
        selected = !selected;
        boxImage.color = selected ? selectedColor : unselecetColor;

        foreach (Image arrow in arrows)
            arrow.color = selected ? selectedColor : unselecetColor;

        orbitals.selectOrbital(orbitalIndex, selected);
        OrbitalDiagram.orbitalDiagram.updateValenceButton();
    }

    public void clickBox(bool _selected)
    {
        selected = _selected;
        boxImage.color = selected ? selectedColor : unselecetColor;

        foreach (Image arrow in arrows)
            arrow.color = selected ? selectedColor : unselecetColor;

        orbitals.selectOrbital(orbitalIndex, selected);
    }
}
