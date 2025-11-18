using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    public List<GameObject> obstacles = new List<GameObject>();
    public bool buildable;

    public GameObject mainObject;
    public Material green, red;

    MeshRenderer[] meshRenderers;

    private void Awake()
    {
        buildable = true;
        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material = green;
        }
    }

    public void BuildObject()
    {
        Instantiate(mainObject, transform.position, transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        obstacles.Add(other.gameObject);
        buildable = false;

        if (!buildable)
        {
            foreach(MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material = red;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        obstacles.Remove(other.gameObject);
        if (obstacles.Count == 0) buildable = true;

        if (buildable)
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material = green;
            }
        }
    }
}
