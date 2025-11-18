using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    private Camera mainCam;
    private bool initialized;

    public Vector3 worldTarget;
    public Vector3 worldOffset;

    void Start()
    {
        mainCam = Camera.main;
    }

    public void Init(Transform worldTarget, Vector3 worldOffset)
    {
        this.worldTarget = worldTarget.position;
        this.worldOffset = worldOffset;

        initialized = true;
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        if (!initialized) return;

        Vector3 screenPos = mainCam.WorldToScreenPoint(worldTarget + worldOffset);

        if (screenPos.z < 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        transform.position = screenPos;
    }
}
