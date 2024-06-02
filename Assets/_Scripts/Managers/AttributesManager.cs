using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    public HealthBar healthBar;

    public int maxHealth = 100;
    [SerializeField] int _health;
    [SerializeField] int _attack = 20;
    [SerializeField] int _xp = 20;


    private void Start()
    {
        _health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        healthBar.SetHealth(_health);
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<AttributesManager>();
        if (atm != null)
        {
            atm.TakeDamage(_attack);
        }
    }
}
