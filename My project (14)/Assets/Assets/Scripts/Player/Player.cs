using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 6f;
    public float jumpForce = 10f;
    public float glideFallSpeed = 1.2f; // velocidade de queda ao planar (menor = mais lento)

    [Header("Pulo Responsivo")]
    public float coyoteTime = 0.15f; // tolerância ao sair do chão
    float coyoteTimeCounter;

    [Header("Checagem de Chão")]
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    [Header("Combate")]
    public float attackDuration = 0.3f; // tempo do ataque
    public float defendDuration = 0.5f; // tempo da defesa
    bool isAttacking = false;
    bool isDefending = false;

    public Rigidbody2D rb;
    bool isGrounded;
    bool faceRight = true;
    float inputX;

   public int limitador_de_pulo = 1;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
    }

    void Update()
    {
        // Não se move durante ataque ou defesa
        if (!isAttacking && !isDefending)
        {
            inputX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        }

        // Flipar sprite
        if (inputX > 0 && !faceRight) Flip();
        else if (inputX < 0 && faceRight) Flip();

        // --- Checar chão ---
        isGrounded = IsGrounded();

        // --- Coyote Time ---
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // --- Pular ---
        if (Input.GetKeyDown(KeyCode.Space) && limitador_de_pulo > 0 )
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            coyoteTimeCounter = 0f;
            limitador_de_pulo--;
        }



        // --- Planar ---
        if (!isGrounded && Input.GetKey(KeyCode.Space) && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -glideFallSpeed);
        }

        // --- Ataque ---
        if (Input.GetMouseButtonDown(0) && !isAttacking && !isDefending)
        {
            StartCoroutine(Attack());
        }

        // --- Defesa ---
        if (Input.GetMouseButtonDown(1) && !isDefending && isGrounded && !isAttacking)
        {
            StartCoroutine(Defend());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero; // para movimento durante ataque
        // Aqui você pode colocar: anim.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    IEnumerator Defend()
    {
        isDefending = true;
        rb.velocity = Vector2.zero;
        // Aqui você pode colocar: anim.SetBool("Defend", true);
        yield return new WaitForSeconds(defendDuration);
        // Aqui você pode colocar: anim.SetBool("Defend", false);
        isDefending = false;
    }

    bool IsGrounded()
    {
        Vector2 pos = transform.position;
        float extraHeight = groundCheckDistance;

        RaycastHit2D hitLeft = Physics2D.Raycast(pos + new Vector2(-0.2f, 0f), Vector2.down, extraHeight, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(pos + new Vector2(0.2f, 0f), Vector2.down, extraHeight, groundLayer);

        Debug.DrawRay(pos + new Vector2(-0.2f, 0f), Vector2.down * extraHeight, Color.yellow);
        Debug.DrawRay(pos + new Vector2(0.2f, 0f), Vector2.down * extraHeight, Color.yellow);

        return hitLeft.collider != null || hitRight.collider != null;
    }

    void Flip()
    {
        faceRight = !faceRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Chao"))
        {
            Debug.Log("aabbbcccc");
            limitador_de_pulo = 1;
        }
    }

}
