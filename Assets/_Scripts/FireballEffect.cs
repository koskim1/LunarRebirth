using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballEffect : Effect
{
    public GameObject fireballPrefab;
    public Transform firePoint;

    public FireballEffect(GameObject fireballPrefab, Transform firePoint)
    {
        this.fireballPrefab = fireballPrefab;
        this.firePoint = firePoint;
    }

    public override void ApplyEffect()
    {
        GameObject fireballInstance = GameObject.Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
        Fireball fireballScript = fireballInstance.GetComponent<Fireball>();
    }
}
