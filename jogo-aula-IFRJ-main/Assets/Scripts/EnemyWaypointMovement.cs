using UnityEngine;
using System.Collections.Generic;

public class EnemyWaypointMovement : MonoBehaviour
{
    [Header("Waypoints")]
    public List<Transform> waypoints;
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float waypointReachedDistance = 0.1f;
    public bool loop = true;
    
    [Header("Combat Settings")]
    public float damage = 10f;
    public float attackCooldown = 1f;
    public float knockbackForce = 15f;
    
    private Rigidbody2D rb;
    private int currentWaypointIndex = 0;
    private Vector2 movementDirection;
    private float lastAttackTime;

    // --- VARIÁVEL NOVA ---
    // Define se o sprite original olha para a direita (true) ou esquerda (false)
    private bool olhandoParaDireita = true; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned to the enemy!");
            enabled = false;
            return;
        }
        
        SetTargetWaypoint(currentWaypointIndex);
    }

    void FixedUpdate()
    {
        MoveTowardsWaypoint();
        CheckIfWaypointReached();
    }

    void SetTargetWaypoint(int index)
    {
        if (waypoints.Count == 0) return;
        
        currentWaypointIndex = index;
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        movementDirection = (targetPosition - (Vector2)transform.position).normalized;
    }

    void MoveTowardsWaypoint()
    {
        if (waypoints.Count == 0) return;
        
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        movementDirection = (targetPosition - (Vector2)transform.position).normalized;
        
        // --- LÓGICA NOVA DE VIRAR (FLIP) ---
        // Se a direção X for maior que 0 (Direita) e eu NÃO estiver olhando para direita -> Vire
        if (movementDirection.x > 0 && !olhandoParaDireita)
        {
            Flip();
        }
        // Se a direção X for menor que 0 (Esquerda) e eu ESTIVER olhando para direita -> Vire
        else if (movementDirection.x < 0 && olhandoParaDireita)
        {
            Flip();
        }
        // -----------------------------------

        // Only modify X velocity, preserve Y velocity for jumping/gravity
        rb.linearVelocity = new Vector2(movementDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    // --- FUNÇÃO NOVA ---
    void Flip()
    {
        // Inverte o estado da variável
        olhandoParaDireita = !olhandoParaDireita;

        // Inverte a escala X do objeto
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    void CheckIfWaypointReached()
    {
        if (waypoints.Count == 0) return;
        
        float distanceToWaypoint = Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position);
        
        if (distanceToWaypoint <= waypointReachedDistance)
        {
            GoToNextWaypoint();
        }
    }

    void GoToNextWaypoint()
    {
        currentWaypointIndex++;
        
        if (currentWaypointIndex >= waypoints.Count)
        {
            if (loop)
            {
                currentWaypointIndex = 0;
            }
            else
            {
                enabled = false;
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }
        
        SetTargetWaypoint(currentWaypointIndex);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryAttackPlayer(collision.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryAttackPlayer(collision.gameObject);
        }
    }

    void TryAttackPlayer(GameObject player)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                
                playerHealth.TakeDamage(damage, knockbackDirection, knockbackForce);
                lastAttackTime = Time.time;
            }
        }
    }

    public void Jump(float jumpForce)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}