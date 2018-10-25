using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private Rigidbody rb;
    public float damageBullet = 20f;
    public float speedBullet = 1500f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyBullet());
    }

    public void Move(Vector3 direction) {
        Debug.Log("MOVING BULLET");
        rb.AddForce(direction * speedBullet);
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerHealth>().ReceiveDamage(damageBullet);
            Destroy(this.gameObject);
        }
    }
}

