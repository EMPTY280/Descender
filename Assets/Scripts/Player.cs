using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb = null;

    /* ----- MOVEMENT ----- */
    [Header("   Move Power")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float jumpPower = 10.0f;

    [Header("   Ground Check")]
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private Vector2 groundBox = new Vector2(1.5f, 1.5f);
    [SerializeField] private Vector2 groundBoxOffset = new Vector2(0f, 0f);
    [SerializeField] private float groundDistanceMax = 0.05f;
    [SerializeField] private LayerMask groundMask;

#if UNITY_EDITOR
    [SerializeField] private Vector4 gizmoColor = new Vector4(0f, 1f, 1f, 0.5f);
#endif

    /* ----- HEAD DIRECTION ----- */
    private int headDirection = 0;

    /* ----- ANIMATION ----- */
    [Header("   Animation")]
    private Animator animator;
    [SerializeField] Transform spriteTransform = null;

    /* ----- HP ----- */
    private int hitPoint = 3;
    private bool bDead = false;
    [SerializeField] Image hpImage = null;
    [SerializeField] GameObject gameover = null;

    /* ----- GUN ----- */
    [SerializeField] Gun gun;
    [SerializeField] Bullet.OwnerType ownerType = Bullet.OwnerType.Player;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position + (Vector3)groundBoxOffset, groundBox);
    }
#endif

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gun = GetComponentInChildren<Gun>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateGrounded();
        Getinput();
    }

    private void UpdateGrounded()
    {
        Vector3 origin = transform.position + (Vector3)groundBoxOffset;
        isGrounded = true;
        if (!Physics2D.BoxCast(origin, groundBox, 0f, Vector2.down, groundDistanceMax, groundMask))
            isGrounded = false;
        if (rb.velocity.y != 0.0f)
            isGrounded = false;
        animator.SetBool("isGround", isGrounded);
    }

    private void Getinput()
    {
        if (bDead)
        {
            if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene("SampleScene");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.Z)) Jump();

        if (Input.GetKey(KeyCode.LeftArrow)) SetHSPeed(-moveSpeed);
        else if (Input.GetKey(KeyCode.RightArrow)) SetHSPeed(moveSpeed);
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) SetHSPeed(0.0f);

        if (Input.GetKey(KeyCode.UpArrow)) SetHeadDirection(1);
        else if (Input.GetKey(KeyCode.DownArrow)) SetHeadDirection(-1);
        else SetHeadDirection(0);

        if (Input.GetKeyDown(KeyCode.X)) gun.Fire();
    }

    public void Jump()
    {
        if (!isGrounded) return;

        Vector2 velocity = rb.velocity;
        velocity.y = jumpPower;
        rb.velocity = velocity;
    }

    public void SetHSPeed(float hSpeed)
    {
        Vector2 velocity = rb.velocity;
        velocity.x = hSpeed;
        rb.velocity = velocity;

        animator.SetBool("isMoving", true);
        bool isMoving = true;
        Vector2 localScale = spriteTransform.localScale;

        if (hSpeed > 0.0f)
        {
            localScale.x = 1.0f;
        }
        else if (hSpeed < 0.0f)
        {
            localScale.x = -1.0f;
        }
        else
            isMoving = false;

        animator.SetBool("isMoving", isMoving);
        spriteTransform.localScale = localScale;
    }

    public void SetHeadDirection(int direction)
    {
        headDirection = direction;

        int isRight = spriteTransform.localScale.x == 1 ? 1 : -1;
        float headUp = rb.velocity.x == 0 ? 90 : 45;

        float gunDIrection = 0;
        if (isRight == 1)
            gunDIrection += headUp * direction;
        else
        {
            gunDIrection = 180;
            gunDIrection -= headUp * direction;
        }

        gun.transform.rotation = Quaternion.Euler(0, 0, gunDIrection);
        animator.SetInteger("head", direction);
    }

    public void ResetVSpeed()
    {
        Vector2 velocity = rb.velocity;
        velocity.y = 0;
        rb.velocity = velocity;
    }

    public void TakeDamage(int amount)
    {
        hitPoint -= amount;
        hpImage.fillAmount = hitPoint * 0.34f;
        if (hitPoint <= 0)
        {
            bDead = true;
            animator.SetBool("dead", true);
            gun.gameObject.SetActive(false);
            gameover.SetActive(true);
        }
    }

    public Bullet.OwnerType GetOwnerType()
    {
        return ownerType;
    }
}
