using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Vector3 direction;

    private void Awake()
    {
        Destroy(this, 10);
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }
    private void Update()
    {
        //transform.Translate(direction * Time.deltaTime * speed);
    }
}
