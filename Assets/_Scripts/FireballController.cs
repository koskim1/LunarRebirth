using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public GameObject Fireball;
    public Transform shootPoint;

    private bool canShootFireball = false;
    private float shootSpeed = 17f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (canShootFireball && Input.GetKeyDown(KeyCode.E))
        {
            SpawnAndShoot();
        }
    }

    public void EnableFireball()
    {
        canShootFireball = true;
    }

    // Update is called once per frame
    void SpawnAndShoot()
    {
        GameObject fireball = Instantiate(Fireball, shootPoint.transform.position, shootPoint.rotation);

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {

            rb.AddForce(transform.forward.normalized * shootSpeed, ForceMode.Impulse);
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
