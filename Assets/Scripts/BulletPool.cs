using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab = null;
    private Queue<Bullet> bullets = new Queue<Bullet>();
    [SerializeField] [Min(1)] private int initialBullets = 15;

    static BulletPool instance = null;
    public static BulletPool Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindGameObjectWithTag("BulletPool").GetComponent<BulletPool>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (bulletPrefab == null)
            return;
        for (int i = 0; i < initialBullets; i++)
            CreateBullet();
    }

    private Bullet CreateBullet()
    {
        Bullet b = Instantiate(bulletPrefab, transform).GetComponent<Bullet>();
        bullets.Enqueue(b);
        return b;
    }

    public Bullet GetBullet()
    {
        Bullet b;
        if (!bullets.Peek().Active) b = bullets.Dequeue();
        else b = CreateBullet();

        bullets.Enqueue(b);
        return b;
    }

    public void KillAll()
    {
        foreach (Bullet b in bullets)
            b.gameObject.SetActive(false);
    }
}
