using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
public class EnemySniper : Enemy
{
[SerializeField]
private float range = 10f;
[SerializeField]
private float fireRate = 3f;
[SerializeField]
private float aimTime = 4f;
[SerializeField]
private Text timerText;
[SerializeField]
private LaserBeam laserBeam;
[SerializeField]
private float nextFireTime;
private bool IsInRange => Vector3.Distance(transform.position, player.position) <= range;
public override void OnEnable()
    {
        base.OnEnable();
        laserBeam.SetActive(false);
        nextFireTime = 0f;
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        animator.Play("Idle", 0, 0f);
        SoundManager.instance.Play("sniper_appear");
    }
    private void Update()
    {
        if (IsInRange && Time.time >= nextFireTime)
        {
            StartCoroutine(AimAndShoot());
            nextFireTime = Time.time + fireRate;
        }
    }
    private IEnumerator AimAndShoot()
    {
        SoundManager.instance.Play("sniper_spotted");
        animator.Play("Aim", 0, 0f);
        yield return animator.WaitForCurrentAnimation();
        StartCoroutine(Shoot());
    }
    private IEnumerator Shoot ()
    {
        laserBeam.SetActive(true);
        laserBeam.Target = player;
        float duration = aimTime;
        while (duration > 0f);
        {
            duration --;
            timerText.text = duration.ToString();
            yield return new WaitForSeconds(1f);
        }
        SoundManager.instance.Play("sniper_shoot");
        laserBeam.SetActive(false);
        player.GetComponent<Health>().TakeDamage(damage);
    }
    public override void Die()
    {
        base.Die();
        SoundManager.instance.Play("sniper_death");
    }
}
 