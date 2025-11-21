using System.Collections;
using UnityEngine;

public class AIDrone : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement Settings")]
    public float maxSpeed = 10f;
    public float acceleration = 6f;
    public float deceleration = 8f;
    public float stoppingDistance = 7f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 4f;
    public float rotationAcceleration = 6f;
    private float currentRotSpeed = 0f;

    [Header("Height Control")]
    public float minHeightY = 5f;
    public float heightSmooth = 2f;

    [Header("Hover Motion")]
    public float hoverAmount = 0.2f;
    public float hoverSpeed = 1.5f;

    [Header("Shooting Settings")]
    public Transform firePoint;
    public float fireRate = 1f;
    public float rayDamage = 10f;
    public float rayDistance = 30f;
    public ParticleSystem muzzleFlash;
    public ParticleSystem destructionParticle;

    [Header("SFX")]
    public AudioSource audioSource;

    [Header("Waypoint")]
    public GameObject WayPointPrefab;
    private GameObject currentWayPoint;
    public Transform headPos;

    private float nextFireTime = 0f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private bool canPlayerPain = false;
    [SerializeField] private float playerPainTime = 4f;

    void Update()
    {
        if (player == null)
        {
            player = LevelManager.instance.playerHeath.transform;
        }

        FollowPlayer();
        TryShootPlayer();

        currentWayPoint = Instantiate(WayPointPrefab, UIController.instance.transform);
        currentWayPoint.GetComponent<Waypoint>().Initialize(headPos, player);
    }

    void FollowPlayer()
    {
        Vector3 targetPos = player.position;
        targetPos.y = minHeightY;

        float distance = Vector3.Distance(transform.position, targetPos);

        // ---------------------------------------
        // ROTATION WITH ACCELERATION
        // ---------------------------------------
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(direction);

        currentRotSpeed = Mathf.Lerp(currentRotSpeed, rotationSpeed, rotationAcceleration * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            currentRotSpeed * Time.deltaTime * 100f
        );

        // ---------------------------------------
        // MOVEMENT WITH ACCEL & DECEL
        // ---------------------------------------
        Vector3 desiredDirection = (targetPos - transform.position).normalized;

        float targetSpeed = (distance > stoppingDistance) ? maxSpeed : 0f;

        if (targetSpeed > velocity.magnitude)
            velocity += desiredDirection * acceleration * Time.deltaTime;
        else
            velocity -= velocity.normalized * deceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // ---------------------------------------
        // APPLY MOVEMENT + HOVER
        // ---------------------------------------
        Vector3 hover = new Vector3(
            Mathf.PerlinNoise(Time.time * hoverSpeed, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * hoverSpeed) - 0.5f,
            Mathf.PerlinNoise(Time.time * hoverSpeed, Time.time * hoverSpeed) - 0.5f
        ) * hoverAmount;

        transform.position += (velocity * Time.deltaTime) + hover;

        // ---------------------------------------
        // HEIGHT SMOOTHING
        // ---------------------------------------
        Vector3 correctedPos = transform.position;
        correctedPos.y = Mathf.Lerp(correctedPos.y, minHeightY, Time.deltaTime * heightSmooth);
        transform.position = correctedPos;
    }

    void TryShootPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= stoppingDistance)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + (1f / fireRate);
            }
        }
    }

    void Shoot()
    {
        AudioManager.instance.PlayMinigunClip(audioSource);

        if (muzzleFlash != null)
            muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, rayDistance))
        {
            Debug.DrawRay(firePoint.position, firePoint.forward * rayDistance, Color.red, 0.2f);

            if (hit.transform.CompareTag("Player"))
            {
                LevelManager.instance.playerHeath.AddHealth(-rayDamage);
                UIController.instance.bloodSplatter.bleedNow = true;

                if (canPlayerPain)
                {
                    canPlayerPain = false;
                    AudioManager.instance.PlayPlayerPain();
                    StartCoroutine(playerPainTimer());
                }
            }
        }
    }
    public void Death()
    {
        AudioManager.instance.PlayExplosionSound();

        destructionParticle.Play();
        destructionParticle.transform.parent = null;
        Destroy(destructionParticle.gameObject, 3);

        Destroy(currentWayPoint);
    }
    IEnumerator playerPainTimer()
    {
        yield return new WaitForSeconds(playerPainTime);
        canPlayerPain = true;
    }
}
