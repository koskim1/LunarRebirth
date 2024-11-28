using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public GameObject Fireball;
    public Transform shootPoint;

    private Fireball fireball;


    private bool canShootFireball = false;
    private float shootSpeed = 17f;
    // Start is called before the first frame update
    void Start()
    {
        fireball = GetComponent<Fireball>();
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

    void SpawnAndShoot()
    {
        GameObject fireball = Instantiate(Fireball, shootPoint.transform.position, shootPoint.rotation);

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {

            rb.AddForce(transform.forward.normalized * shootSpeed, ForceMode.Impulse);
        } 
    }

}
