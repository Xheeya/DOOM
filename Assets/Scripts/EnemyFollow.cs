using UnityEngine;
using System.Collections;

public class EnemyFollow : MonoBehaviour
{
[SerializeField]
private float speed = 3f;
[SerializeField]
private float yPosition = 2f;
[SerializeField]
private float damage = 20f;
[SerializeField]
private float pushForce = 5f;
private Transform player;
private bool isFollowing = true;
private Animator animator;
private void Start()
    {
        animator = GetComponent<Animator>();
        GetComponent<Health>().InitializeHealth();
    }
private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void TakeDamage()
    {
        if (!isFollowing) return;
        isFollowing = false;
        animator.Play("Damage", 0, 0f);
        StartCoroutine(StopAndFollow());
    }
    private IEnumerator StopAndFollow()
    {
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isFollowing = true;
    }
    public void Die()
    {
        StopAllCoroutines();
        GetComponent<Collider>().enabled = false;
        isFollowing = false;
        animator.Play("Death", 0, 0f);
        StartCoroutine(DieCoroutine());
    }
    private IEnumerator DieCoroutine()
    {
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
    private void Update()
    {
        if (!isFollowing) return;
        Vector3 targetPosition = new Vector3(player.position.x, yPosition, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position,
            targetPosition, speed * Time.deltaTime);
        transform.LookAt(targetPosition);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().PushBack(transform, pushForce);
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
