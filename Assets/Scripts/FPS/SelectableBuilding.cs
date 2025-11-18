using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableBuilding : MonoBehaviour
{
    public GameObject buildingPrefab;

    public GameObject buildButton, discardButton;

    public void Clicked()
    {
        ObjectPlacer.instance.BuildingSelected(buildingPrefab);

        buildButton.SetActive(true);
        discardButton.SetActive(true);
    }
    public void BuildBuilding()
    {
        bool built = ObjectPlacer.instance.BuildBuilding();
        if (built == false) return;

        buildButton.SetActive(false);
        discardButton.SetActive(false);
    }
    public void DiscardBuilding()
    {
        ObjectPlacer.instance.DiscardBuilding();

        buildButton.SetActive(false);
        discardButton.SetActive(false);
    }
}
