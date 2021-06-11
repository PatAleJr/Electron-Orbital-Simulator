using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AtomScaling : MonoBehaviour
{
    public CameraMove cameraMove;
    public TextMeshProUGUI scaleText;

    //zoom1 / zoom2 = pixels2 / pixels1
    //pixels2 / scaleWidth = pm1 / pmScale
    private const float zoom1 = 5f; //Zoom of the camera
    private const float pixels1 = 128f; //Width in pixels of the atom when cam zoom is 5
    private const float pm1 = 120f; //Length in picometers
    private float scaleWidth;   //Width of the horizontal line (scale)

    public RectTransform Scale;

    private int numDashes;
    public int dashInterval = 250;  //In picometers, what distance between dashes
    public GameObject dashPrefab;
    public List<GameObject> dashes; 

    private void Start()
    {
        scaleWidth = Scale.rect.width;
    }

    public void updateScale()
    {
        float zoom2 = Mathf.Abs(cameraMove.currentZoom);
        int pixels2 = Mathf.FloorToInt((zoom1 / zoom2) * pixels1);
        float pm2 = (scaleWidth / pixels2) * pm1;

        int picometers = Mathf.FloorToInt(pm2); //Length of the scale

        scaleText.text = picometers + "pm";
        scaleText.text = "";

        //How many vertical dashes there should be
        int _numDashes = Mathf.FloorToInt(picometers / dashInterval);

        //Recreates dashes if needs different number of them
        if (_numDashes != numDashes)
        {
            //Removes dashes from list
            for (int i = dashes.Count-1; i >= 0; i--)
            {
                GameObject d = dashes[i];
                dashes.Remove(d);
                Destroy(d.gameObject);
            }

            //Recreates them
            for (int i = 0; i < _numDashes; i++)
            {
                GameObject d = Instantiate(dashPrefab, Scale);
                dashes.Add(d);

                TextMeshProUGUI dashMeasure = d.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                dashMeasure.text = ((i + 1) * dashInterval).ToString();
                dashMeasure.text = "";
            }

            numDashes = _numDashes;
        }

        //Moves dashes according to zoom
        for (int i = 0; i < dashes.Count; i++)
        {
            Transform d = dashes[i].transform;

            float percentLength = (dashInterval * (i + 1)) / (float)picometers;    //How far along the scale should it be
            float xx = percentLength * scaleWidth - (scaleWidth / 2);

            d.transform.localPosition = new Vector2(xx, 0f);
        }
    }

    void verticalLines()
    {

    }

    void Update()
    {
        //TODO: Call this when camera changes only
        updateScale();
    }
}
