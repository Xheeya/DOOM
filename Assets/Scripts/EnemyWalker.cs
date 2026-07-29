using UnityEngine;
using System.Collections;

public class EnemyWalker : Enemy
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float attackRange = 1.5f;
    private enum State
    {
        Appearing,
        Following,
        Attacking,
        Death,
    }
    private State currentState = State.Appearing;
    private bool IsInRange => Vector3.Distance(transform.position, player.position) <= attackRange;
    public override void OnEnable()
    {
        base.OnEnable();
        currentState = State.Appearing;
        StartCoroutine(AppearCoroutine());
    }
    private IEnumerator AppearCoroutine()
    {
        animator.Play("Appear", 0, 0f);
        yield return animator.WaitForCurrentAnimation();
        currentState = State.Following;
    }
    private void Update()
    {
        if (health.IsDead) return;
        if (CheckWin()) return;
        if (currentState == State.Following)
        {
            if (IsInRange)
            {
                currentState = State.Attacking;
                StartCoroutine(AttackCoroutine());
            }
            else
            {
                animator.Play("Run");
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                transform.LookAt(player);
            }
        }
    }
    private IEnumerator AttackCoroutine()
    {
        yield return null;
    }
}

