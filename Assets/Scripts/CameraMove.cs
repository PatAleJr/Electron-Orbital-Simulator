using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform camOrigin;
    private Camera cam;

    public float moveSpeed;
    public float rotationMultiplier;

    public float minZoom = 1f;
    public float maxZoom = 20f;
    public float zoomSpeed;
    public float currentZoom;

    public float zoomIncrements;

    public bool rotatingCam;

    //Arrows to move origin
    //Mouse to rotate cam around origin
    //Scroll to change distance from origin

    private void Start()
    {
        cam = Camera.main;
        camOrigin = transform;
        rotatingCam = false;

        currentZoom = cam.transform.localPosition.z;
    }

    public void zoom(int increment)
    {
        float zoom = zoomIncrements * increment;
        currentZoom = cam.transform.localPosition.z + zoom;
        currentZoom = Mathf.Clamp(currentZoom, -maxZoom, -minZoom);
        cam.transform.localPosition = new Vector3(0f, 0f, currentZoom);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                rotatingCam = true;
            } 
        }

        if (Input.GetButtonUp("Fire1"))
        {
            rotatingCam = false;
        }

        if (rotatingCam)
        {
            float hor = Input.GetAxis("Mouse X") * rotationMultiplier * Time.deltaTime;
            float ver = -Input.GetAxis("Mouse Y") * rotationMultiplier * Time.deltaTime;

            camOrigin.Rotate(new Vector3(ver, hor, 0));
        }

        //Zoom
        float zoom = zoomSpeed * Time.deltaTime * Input.mouseScrollDelta.y;
        currentZoom = cam.transform.localPosition.z + zoom;
        currentZoom = Mathf.Clamp(currentZoom, -maxZoom, -minZoom);
        cam.transform.localPosition = new Vector3(0f, 0f, currentZoom);
    }
}
