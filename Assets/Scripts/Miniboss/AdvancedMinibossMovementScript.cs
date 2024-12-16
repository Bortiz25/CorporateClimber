using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedBossMovement : MonoBehaviour
{
    public enum BossState
    {
        Idle,
        MoveToPosition,
        Dash,
        Dodge
    }

    [Header("General Settings")]
    public float baseSpeed = 3f;
    public Transform playerTransform;
    public Rigidbody2D rb;

    [Header("Attack Phase Settings")]
    public List<Transform> attackPositions;
    public float positionSwitchInterval = 3f;

    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    public float dashInterval = 4f;
    [Range(0,1)] public float dashProbability = 0.2f; // 20% chance
    private bool canDash = true;

    [Header("Dodge Settings")]
    public float dodgeSpeed = 6f;
    public float dodgeDuration = 0.3f;
    public float dodgeInterval = 1.5f;
    [Range(0,1)] public float dodgeChance = 0.3f;
    private bool canDodge = true;

    private Vector2 currentTarget;
    private BossState currentState = BossState.Idle;
    private float lastPositionSwitchTime;

    void Start()
    {
        if (!playerTransform) 
        {
            // If the player isn't assigned, try to find it by tag
            GameObject player = GameObject.FindWithTag("PlayerTransform");
            if (player) playerTransform = player.transform;
        }
        
        rb = GetComponent<Rigidbody2D>();
        lastPositionSwitchTime = Time.time;
        SwitchToNextPosition();
        ChangeState(BossState.MoveToPosition);
    }

    void Update()
    {
        HandleStateLogic();
        AttemptStateTransitions();
    }

    private void HandleStateLogic()
    {
        switch (currentState)
        {
            case BossState.Idle:
                // Perhaps look at the player or prepare for next action
                rb.velocity = Vector2.zero;
                break;
            
            case BossState.MoveToPosition:
                MoveTowards(currentTarget, baseSpeed);
                break;

            case BossState.Dash:
                // Dashing handled by coroutine
                break;

            case BossState.Dodge:
                // Dodging handled by coroutine
                break;
        }
    }

    private void AttemptStateTransitions()
    {
        if (Time.time - lastPositionSwitchTime > positionSwitchInterval && currentState == BossState.MoveToPosition)
        {
            SwitchToNextPosition();
        }

        // Attempt a dash if conditions are met
        if (currentState == BossState.MoveToPosition && canDash && Random.value < dashProbability)
        {
            StartCoroutine(PerformDash());
        }

        // Attempt a dodge if conditions are met (e.g., player is aiming or after certain conditions)
        if (currentState == BossState.MoveToPosition && canDodge && Random.value < dodgeChance)
        {
            StartCoroutine(PerformDodge());
        }
    }

    private void SwitchToNextPosition()
    {
        if (attackPositions != null && attackPositions.Count > 0)
        {
            Transform randomPos = attackPositions[Random.Range(0, attackPositions.Count)];
            currentTarget = randomPos.position;
        }
        lastPositionSwitchTime = Time.time;
        ChangeState(BossState.MoveToPosition);
    }

    private void MoveTowards(Vector2 target, float speed)
    {
        Vector2 direction = (target - rb.position).normalized;
        rb.velocity = direction * speed;
    }

    private IEnumerator PerformDash()
    {
        ChangeState(BossState.Dash);
        canDash = false;

        Vector2 dashDirection = (playerTransform != null) 
            ? ((Vector2)playerTransform.position - rb.position).normalized // Dash towards player
            : Random.insideUnitCircle.normalized;

        rb.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);

        ChangeState(BossState.MoveToPosition);
        yield return new WaitForSeconds(dashInterval);
        canDash = true;
    }

    private IEnumerator PerformDodge()
    {
        ChangeState(BossState.Dodge);
        canDodge = false;

        // Dodge perpendicular to player direction
        if (playerTransform != null)
        {
            Vector2 toPlayer = (playerTransform.position - transform.position);
            Vector2 perpendicular = Vector2.Perpendicular(toPlayer).normalized;
            // Randomize dodge direction left or right of player
            perpendicular *= (Random.value > 0.5f) ? 1 : -1;

            rb.velocity = perpendicular * dodgeSpeed;
        }
        else
        {
            rb.velocity = Random.insideUnitCircle.normalized * dodgeSpeed;
        }

        yield return new WaitForSeconds(dodgeDuration);
        ChangeState(BossState.MoveToPosition);
        yield return new WaitForSeconds(dodgeInterval);
        canDodge = true;
    }

    private void ChangeState(BossState newState)
    {
        currentState = newState;
    }

    void OnDrawGizmos()
    {
        // Visualize attack positions
        if (attackPositions != null)
        {
            Gizmos.color = Color.red;
            foreach (Transform pos in attackPositions)
            {
                Gizmos.DrawWireSphere(pos.position, 0.5f);
            }
        }

        // Visualize current target
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(currentTarget, 0.3f);
    }
}
