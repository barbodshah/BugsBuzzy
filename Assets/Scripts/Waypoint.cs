using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    public Image img;

    public Transform parent;
    public Transform target;

    public Camera mainCam;

    public Vector3 offset;
    public float triggerDistance;

    private bool playedAnimation;

    public void Initialize(Transform target, Transform parent, float triggerDistance=15)
    {
        Camera[] cameras = FindObjectsOfType<Camera>();

        foreach (Camera cam in cameras)
        {
            if (cam.enabled)
            {
                mainCam = cam;
                break;
            }
        }

        this.target = target;
        this.parent = parent;
        this.triggerDistance = triggerDistance;
    }
    public void PlayAnimation()
    {
        if (playedAnimation) return;

        playedAnimation = true;
        GetComponent<Animator>().Play("Fade");
    }

    private void Update()
    {
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        if (!target) Destroy(gameObject);
        Vector2 pos = mainCam.WorldToScreenPoint(target.position + offset);

        if (Vector3.Dot(target.position - mainCam.transform.position, mainCam.transform.forward) < 0)
        {
            if(pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        img.transform.position = pos;

        if(Vector3.Distance(target.position, parent.position) < triggerDistance)
        {
            PlayAnimation();
        }
    }
}
