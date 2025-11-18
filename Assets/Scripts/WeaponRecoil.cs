using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public Vector3 recoilOffset = new Vector3(0, 0, -0.1f);
    public float recoilReturnSpeed = 5f;
    public float recoilApplySpeed = 20f;

    private Vector3 currentRecoil = Vector3.zero;
    private Vector3 targetRecoil = Vector3.zero;

    private Vector3 originalPos;

    [SerializeField] ReloadableWeapon reloadableWeapon;

    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        if (reloadableWeapon.reloading)
        {
            targetRecoil = originalPos;
            currentRecoil = originalPos;
            return;
        }

        currentRecoil = Vector3.Lerp(currentRecoil, targetRecoil, Time.deltaTime * recoilApplySpeed);
        targetRecoil = Vector3.Lerp(targetRecoil, originalPos, Time.deltaTime * recoilReturnSpeed);

        transform.localPosition = currentRecoil;
    }
    public void ApplyRecoil()
    {
        targetRecoil += recoilOffset;
    }
}
