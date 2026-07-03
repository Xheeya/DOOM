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
private int cartrigeBullets;
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
public void ChargeGun(bool playAnimation=true)
    {
        if (totalBullets<=0 || cartrigeBullets == gunData.cartridgeSize) return;
        SoundManager.instance.Play(gunData.reloadSoundName);
        cartrigeBullets = Mathf.Min(gunData.cartridgeSize, totalBullets);
        totalBullets -= cartrigeBullets;
        if (playAnimation)animator.Play("Charge", 0, 0f);
        UpdateAmmoText();
    }
private void UpdateAmmoText()
    {
        ammoText.text = $"{cartrigeBullets}/{totalBullets}";
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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
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
        GameObject bullet = PoolManager.Instance.GetObject(bulletPrefab, bulletPivot.position);
        bullet.SetActive(false);
        bullet.transform.position = bulletPivot.position;
        bullet.transform.LookAt(targetPoint);
        bullet.SetActive(true);
        SoundManager.instance.Play(gunData.shootSoundName);
        animator.Play("Shoot", 0, 0f);
    }
 
    public void HandleFire(bool pressed, bool held)
    {
        if(gunData.gunType == GunType.Automatic)
        {
            if (held)
            {
                TryShoot();
            }
        }
       else if(gunData.gunType == GunType.SemiAutomatic)
        {
            if (pressed)
            {
                TryShoot();
            }
        }
    }
 
    private void TryShoot()
    {
        if(totalBullets <= 0 && cartrigeBullets <= 0)
        {
            SoundManager.instance.Play(gunData.dropSoundName);
            onGunEmpty?.Invoke();
            return;
        }
        if(cartrigeBullets > 0 && Time.time >= nextFireTime)
        {
            Shoot();
            cartrigeBullets--;
            UpdateAmmoText();
            nextFireTime = Time.time + 1f / gunData.fireRate;
        }
    }
}