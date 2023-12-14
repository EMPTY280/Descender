using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public enum OwnerType
    {
        Player,
        Enemy
    }

    private float speed = 50f;
    private int damage = 1;
    private OwnerType owner = OwnerType.Player;
    private float rangeMax = 10f;
    private float rangeRemain = 10f;
    private SpriteRenderer spriteRenderer;

    public float Direction
    {
        get { return transform.rotation.eulerAngles.z; }
        set { transform.rotation = Quaternion.Euler(0, 0, value); }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public OwnerType Owner
    {
        get { return owner; }
    }

    public float Range
    {
        get { return rangeMax; }
        set { rangeMax = value; }
    }

    public bool Active
    {
        get { return gameObject.activeSelf; }
    }
    
    private void Awake()
    {
        gameObject.SetActive(false);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float travel = speed * Time.deltaTime;
        if (travel > rangeRemain)
            travel = rangeRemain;

        transform.Translate(Vector3.right * travel, Space.Self);

        rangeRemain -= travel;
        if (rangeRemain <= 0f)
            gameObject.SetActive(false);
    }

    public void Fire(Vector2 pos, OwnerType owner)
    {
        this.owner = owner;
        if (owner == OwnerType.Enemy)
            spriteRenderer.color = Color.red;
        else
            spriteRenderer.color = Color.white;
        rangeRemain = rangeMax;
        Vector3 newPos = new(pos.x, pos.y, -5);
        transform.position = newPos;
        gameObject.SetActive(true);
    }

    private void DoDamage(IDamageable target)
    {
        target.TakeDamage(damage);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && target.GetOwnerType() != owner)
        {
            DoDamage(target);
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Terrain"))
        {
            gameObject.SetActive(false);
        }
    }
}
