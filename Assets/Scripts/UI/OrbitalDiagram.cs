using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrbitalDiagram : MonoBehaviour
{
    public static OrbitalDiagram orbitalDiagram;

    public float sX = -150f;
    public float pX;
    public float dX;

    public float gapX = 50f;

    public float startY = 0f;
    public float gapY = 64f;

    public GameObject sBoxPrefab;
    public GameObject pBoxPrefab;
    public GameObject dBoxPrefab;

    public GameObject arrowPrefab;
    public float arrowOffset = 8f;

    public GameObject orbitalLabelPrefab;    //where it says "2p" below squares
    public float labelOffset = -32f;

    public List<GameObject> arrows;
    public List<GameObject> boxes;
    public List<OrbitalBox> boxScripts;

    public Transform contentOrigin;  //scrollbar content
    public Transform content;
    public GameObject empty;

    public ToggleButton valenceOnlyButton;
    public bool selectValence = false;

    public void Awake()
    {
        if (OrbitalDiagram.orbitalDiagram != this)
        {
            if (OrbitalDiagram.orbitalDiagram != null)
                Destroy(OrbitalDiagram.orbitalDiagram);
            OrbitalDiagram.orbitalDiagram = this;
        }
    }

    public void toggleValenceBoxes()
    {
        selectValence = !selectValence;

        if (selectValence)
        {
            selectValenceBoxes();
        }
        else {
            deselectAll();
        }
    }

    public void selectValenceBoxes()
    {
        foreach (OrbitalBox box in boxScripts)  //Selects all valence, deselects all others
        {
            if ((int)(box.label[0] - '0') == ParticleManager.particleManager.selectedAtom.pqn)
            {
                box.clickBox(true);
            } else
            {
                box.clickBox(false);
            }
        }
    }

    public void updateValenceButton()
    {
        if (!onlyValenceIsSelected())
        {
            valenceOnlyButton.toggle(false);
            selectValence = false;
        }
    }

    public bool onlyValenceIsSelected()
    {

        foreach (OrbitalBox box in boxScripts)  //Selects all valence, deselects all others
        {
            if ((int)(box.label[0] - '0') == ParticleManager.particleManager.selectedAtom.pqn)
            {
                if (!box.selected)
                    return false;
            }
            else
            {
                if (box.selected)
                    return false;
            }
        }

        return true;
    }

    public void deselectAll()
    {
        foreach (OrbitalBox box in boxScripts)  //Selects all valence, deselects all others
        {
            box.clickBox(false);
        }
    }

    public void fillBox(Transform box, int electrons)
    {
        List<GameObject> arrowsInBox = new List<GameObject>();

        GameObject arrow = Instantiate(arrowPrefab, box);
        arrow.transform.position += new Vector3(-arrowOffset, 0, 0);
        arrows.Add(arrow);
        arrowsInBox.Add(arrow);
        

        if (electrons == 2)
        {
            GameObject arrow2 = Instantiate(arrowPrefab, box);
            arrow2.transform.position += new Vector3(arrowOffset, 0, 0);
            arrow2.transform.localScale = new Vector3(1, -1, 0);
            arrows.Add(arrow2);
            arrowsInBox.Add(arrow2);
        }

        box.GetComponent<OrbitalBox>().updateArrows(arrowsInBox);
    }

    public void destroyDiagram()
    {
        foreach (GameObject b in boxes)
            Destroy(b);

        arrows.Clear();
        boxes.Clear();
        boxScripts.Clear();

        /*
        GameObject newOrigin = Instantiate(empty, content);
        newOrigin.transform.position = contentOrigin.transform.position;
        Destroy(contentOrigin.gameObject);
        contentOrigin = newOrigin.transform;
        */
    }

    public void updateDiagram(Atom atom)
    {
        if (boxes.Count > 0)
            destroyDiagram();

        int electrons = atom.atomicNumber;
        float y = startY;

        int numberOfOrbitals = 0;

        for (int pqn = 1; pqn < 6; pqn++)
        {
            GameObject newBox;

            //S

            //Make Box
            numberOfOrbitals++;
            newBox = makeOrbitalBoxes(pqn, "s", y);

            //Fill Boxes
            int electronsUsed = (electrons > 1) ? 2 : 1;

            //Chromium and molybdenum, copper and silver
            if (pqn >= 4 && (electrons == 6 || electrons == 11))
                electronsUsed = 1;

            fillBox(newBox.transform, electronsUsed);
            electrons -= electronsUsed;

            if (electrons <= 0)
                break;

            y += gapY;

            int orbitalsToAdd;  //For p and d orbitals

            //D
            if (pqn >= 4)
            {
                newBox = makeOrbitalBoxes(pqn-1, "d", y);

                orbitalsToAdd = electrons;
                if (orbitalsToAdd > 5) orbitalsToAdd = 5;
                numberOfOrbitals += orbitalsToAdd;

                //Fill boxes
                Transform boxD;
                for (int i = 0; i < 5; i++)
                {
                    boxD = newBox.transform.GetChild(i);

                    if (electrons > 5 - i)
                    {
                        fillBox(boxD, 2);
                        electrons -= 2;
                        electronsUsed += 2;
                    }
                    else if (electrons > 0)
                    {
                        fillBox(boxD, 1);
                        electrons--;
                        electronsUsed++;
                    }
                }

                if (electrons == 0)
                    break;

                y += gapY;
            }

            //P
            if (pqn < 2) continue;

            newBox = makeOrbitalBoxes(pqn, "p", y);

            //Fill boxes
            orbitalsToAdd = electrons;
            if (orbitalsToAdd > 3) orbitalsToAdd = 3;
            numberOfOrbitals += orbitalsToAdd;

            Transform box;
            for (int i = 0; i < 3; i++)
            {
                box = newBox.transform.GetChild(i);

                if (electrons > 3 - i)
                {
                    fillBox(box, 2);
                    electrons -= 2;
                    electronsUsed += 2;
                }
                else if (electrons > 0)
                {
                    fillBox(box, 1);
                    electrons--;
                    electronsUsed++;
                }
            }

            if (electrons == 0)
                break;

            y += gapY;
        }

        ParticleManager.particleManager.selectedAtom.numberOfOrbitals = numberOfOrbitals;
        setBoxNums();
        updateValenceButton();
    }

    public GameObject makeOrbitalBoxes(int pqn, string subshell, float y)
    {
        GameObject boxToMake = null;

        float x = 0;
        switch (subshell)
        {
            case "s":
                x = sX;
                boxToMake = sBoxPrefab;
                break;
            case "p":
                x = pX;
                boxToMake = pBoxPrefab;
                break;
            case "d":
                x = dX;
                boxToMake = dBoxPrefab;
                break;
        }

        GameObject newBox = Instantiate(boxToMake, contentOrigin);
        boxes.Add(newBox);
        //newBox.transform.position = new Vector2(contentOrigin.position.x + x, contentOrigin.position.y + y);
        newBox.transform.position = new Vector2(newBox.transform.position.x, contentOrigin.position.y + y);

        //Labels it
        GameObject newLabel = Instantiate(orbitalLabelPrefab, newBox.transform);
        newLabel.transform.position += new Vector3(0, labelOffset, 0);
        newLabel.GetComponent<TextMeshProUGUI>().text = pqn + subshell;

        //Tags it
        if (newBox.tag == "UIBox")
        {
            OrbitalBox b = newBox.GetComponent<OrbitalBox>();
            boxScripts.Add(b);
            b.label = pqn + subshell;   
        }
        else
        {
            OrbitalBox[] children = newBox.transform.GetComponentsInChildren<OrbitalBox>();
            foreach (OrbitalBox child in children)
            {
                boxScripts.Add(child);
                child.label = pqn + subshell;
            }
        }

        return newBox;
    }

    private void setBoxNums()
    {
        for (int i = 0; i < boxScripts.Count; i++)
        {
            boxScripts[i].orbitalIndex = i;
        }
    }
}
