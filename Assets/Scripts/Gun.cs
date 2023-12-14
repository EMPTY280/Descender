using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    [SerializeField] private Bullet.OwnerType ownerType = Bullet.OwnerType.Player;
    [SerializeField] Transform firePoint = null;
    [SerializeField] Transform spriteTransform = null;

    [SerializeField] float velocity = 10f;

    [Header("AI")]
    [SerializeField] private Transform target = null;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private float aimTime = 2f;
    [SerializeField] private LayerMask groundMask;
    private float aimTimeCurrent = 0f;

    private void Update()
    {
        float rotation = transform.rotation.eulerAngles.z;
        if (rotation >= 90f && rotation < 270f)
            spriteTransform.localScale = new Vector3(1, -1, 1);
        else
            spriteTransform.localScale = new Vector3(1, 1, 1);

        if (target)
        {
            if (RotateToTarget())
            {
                aimTimeCurrent += Time.deltaTime;
                if (aimTimeCurrent >= aimTime)
                {
                    aimTimeCurrent = 0;
                    Fire();
                }
            }
        }
    }

    private bool RotateToTarget()
    {
        Vector3 targetPos = target.position;
        targetPos.y += 0.5f;

        Vector3 targetVec = (targetPos - transform.position);
        if (Physics2D.Raycast(transform.position, targetVec.normalized, targetVec.magnitude,groundMask))
            return false;

        Vector2 targetVector = targetPos - transform.position;
        float degree = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.Euler(0f, 0f, degree);
        Quaternion final = Quaternion.RotateTowards(from, to, rotateSpeed * Time.deltaTime);

        transform.rotation = final;

        if (from == to) return true;
        else return false; ;

    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void ResetAim()
    {
        aimTimeCurrent = 0f;
    }
    public void Fire()
    {
        Bullet b = BulletPool.Instance.GetBullet();
        b.Speed = velocity;
        b.Direction = transform.rotation.eulerAngles.z;
        b.Fire(firePoint.position, ownerType);
    }
}
