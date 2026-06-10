using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Gun: MonoBehaviour
{
    [SerializeField]
private Animator animator;
[SerializeField]
private Rotate rotateScript;
[SerializeField]
private GunData gunData;
[SerializeField]
private Transform bulletPivot;
[SerializeField]
private GameObject bulletPrefab;
private Text ammoText;
private float nextFireTime;
private int totalBullets;
private int cartridgeBullets;
private UnityEvent onGunEmpty = new UnityEvent();
public UnityEvent OnGunEmpty
    {
      set => onGunEmpty = value;
      get => onGunEmpty;  
    }
public void GrabGun (Transform gunPosition, Text bulletsText)
{
    ammoText = bulletsText;
nextFireTime = 0f;
totalBullets = gunData.totalBullets;
transform.SetParent(gunPosition);
transform.localPosition = Vector3.zero;
transform.localRotation = Quaternion.identity;
animator. Play ("Idle", 0, 0f);
rotateScript.canRotate = false;
gameObject.GetComponent<Collider>().enabled = false;
ChargeGun(false);
}
public void ChargeGun(bool playAnimation = true)
    {
        if (totalBullets <= 0 || cartridgeBullets == gunData.cartridgeSize) return;
        SoundManager.instance.Play(gunData.reloadSoundName);
        cartridgeBullets = Mathf.Min(gunData.cartridgeSize, totalBullets);
        totalBullets -= cartridgeBullets;
        if (playAnimation) animator.Play("Charge", 0, 0f);
        UpdateAmmoText();
    }
    private void UpdateAmmoText()
    {
        ammoText.text = $"{cartridgeBullets} / {totalBullets}";
    }
    private void DamageEnemy(GameObject enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            enemy.GetComponent<Health>().TakeDamage(gunData.damage);
        }
    }
    public void Shoot()
    {
        float rayDistance = 1000f;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            targetPoint = hit.point;
            DamageEnemy(hit.collider.gameObject);
        }
        else
        {
            targetPoint = ray.GetPoint(rayDistance);
        }
        Vector3 direction = (targetPoint - transform.position).normalized;
        bulletPivot.forward = direction;
        GameObject bullet = Instantiate(bulletPrefab, bulletPivot.position, bulletPivot.rotation);
        bullet.transform.LookAt(targetPoint);
        SoundManager.instance.Play(gunData.shootSoundName);
        animator.Play("Shoot", 0, 0f);
    }
    public void HandleFire(bool pressed, bool held)
    {
        if (gunData.gunType == GunType.Automatic)
        {
            if (held)
            {
                TryShoot();
            }
        }
        else if (gunData.gunType == GunType.SemiAutomatic)
        {
            if (pressed)
            {
                TryShoot();
            }
        }
    }
    private void TryShoot()
    {
        if (totalBullets <= 0 && cartridgeBullets <= 0)
        {
            onGunEmpty?.Invoke();
            return;
        }
        if (cartridgeBullets > 0 && Time.time >= nextFireTime)
        {
            Shoot();
            cartridgeBullets--;
            UpdateAmmoText();
            nextFireTime = Time.time + 1f / gunData.fireRate;
        }
    }
}