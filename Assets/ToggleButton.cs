using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Image buttonImage;
    public Color onColor;
    public Color offColor;

    private bool selected = false;

    public GameObject x;

    private void Start()
    {
        selected = false;
        x.SetActive(selected);
    }

    public void toggle()
    {
        selected = !selected;
        //buttonImage.color = selected ? onColor : offColor;
        x.SetActive(selected);
    }

    public void toggle(bool _selected)
    {
        selected = _selected;
        //buttonImage.color = selected ? onColor : offColor;
        x.SetActive(selected);
    }
}
