using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private Gun gun;
    [SerializeField] private Transform target = null;

    private int hitPointMax = 3;
    private int hitPoint = 3;

    private void Start()
    {
        gun = GetComponentInChildren<Gun>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    
        gun.SetTarget(target);
    }

    public Bullet.OwnerType GetOwnerType()
    {
        return Bullet.OwnerType.Enemy;
    }

    private void Awake()
    {
    }

    public void TakeDamage(int amount)
    {
        hitPoint -= amount;
        if (hitPoint <= 0)
        {
            gun.ResetAim();
            gameObject.SetActive(false);
        }
    }

    public void Revive()
    {
        hitPoint = hitPointMax;
        gameObject.SetActive(true);
    }
}
