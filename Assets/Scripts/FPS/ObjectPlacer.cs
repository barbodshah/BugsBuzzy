using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public static ObjectPlacer instance;

    public Transform objectTarget;
    public GameObject currentBuildingPrefab;

    public PlacableObject currentObject;

    public bool isPlacing;

    Camera mainCam;

    private void Awake()
    {
        instance = this;
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isPlacing = true;

            Ray ray = new Ray(mainCam.transform.position, objectTarget.position - mainCam.transform.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                currentObject = Instantiate(currentBuildingPrefab, hit.point, objectTarget.rotation).GetComponent<PlacableObject>();
            }
        }
        if (isPlacing)
        {
            Ray ray = new Ray(mainCam.transform.position, objectTarget.position - mainCam.transform.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                currentObject.transform.position = hit.point;
                currentObject.transform.rotation = objectTarget.rotation;
            }

            if(Input.GetKeyDown(KeyCode.H))
            {
                BuildBuilding();
            }
        }
    }
    public bool BuildBuilding()
    {
        if (currentObject.buildable)
        {
            currentObject.BuildObject();

            Destroy(currentObject.gameObject);
            currentObject = null;

            isPlacing = false;

            return true;
        }
        return false;
    }
    public void DiscardBuilding()
    {
        isPlacing = false;

        Destroy(currentObject.gameObject);
        currentObject = null;
    }
    public void BuildingSelected(GameObject prefab)
    {
        currentBuildingPrefab = prefab;

        isPlacing = true;

        Ray ray = new Ray(mainCam.transform.position, objectTarget.position - mainCam.transform.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            currentObject = Instantiate(currentBuildingPrefab, hit.point, objectTarget.rotation).GetComponent<PlacableObject>();
        }
    }
}
