using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager particleManager;

    public List<GameObject> atoms;
    public GameObject atomPrefab;
    public Atom selectedAtom;

    private Camera cam;
    private UIManager uiManager;

    public float doubleClickDelay = 0.1f;
    private float nextClickTime;

    public TMP_Dropdown viewModeDropDown;

    public enum ViewMode {ElectronCloud, Bohr};
    public ViewMode currentViewMode = ViewMode.ElectronCloud;

    private void Awake()
    {
        if (ParticleManager.particleManager != this)
        {
            if (ParticleManager.particleManager != null)
                Destroy(ParticleManager.particleManager);
            particleManager = this;
        } 
    }

    void Start()
    {
        cam = Camera.main;
        deselect();
    }

    public void CreateAtom()
    {
        GameObject newAtom = Instantiate(atomPrefab);
        atoms.Add(newAtom);
        selectAtom(newAtom.GetComponent<Atom>());

        UIManager.uiManager.toggleCreateAtomButton(false);
    }

    public void selectAtom(Atom _selected)
    {
        selectedAtom = _selected;
        UIManager.uiManager.setAtomInfo(selectedAtom);
    }

    public void deselect()
    {
        //UIManager.uiManager.removeAtomInfo();
    }

    private void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject != null)
                {
                    GameObject hitObj = hit.collider.gameObject;

                    if (hitObj.tag == "Atom")
                    {
                        Atom clickedAtom = hitObj.GetComponent<Atom>();
                        if (selectedAtom != clickedAtom)
                            selectAtom(hitObj.GetComponent<Atom>());
                    }
                }
            }   
        }

        if (Input.GetButtonDown("Fire2"))
        {
            deselect();
        }
    }

    public void changeViewMode()
    {
        currentViewMode = (ViewMode)viewModeDropDown.value;
        selectedAtom.updateViewMode(currentViewMode);
    }
}
