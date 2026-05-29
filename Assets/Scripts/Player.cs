using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform gunPosition;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Text ammoText;
    [SerializeField]
    private UnityEvent onGunGrabbed;
    [SerializeField]
    private UnityEvent onGunDropped;
    private Gun currentGun;
    private void Start() 
    {
     onGunDropped?.Invoke();   
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Gun"))
        {
            currentGun = other.GetComponent<Gun>();
            currentGun.GrabGun(gunPosition, ammoText);
            onGunGrabbed?.Invoke();
            currentGun.OnGunEmpty+= DropGun;
        }
    }
    private void Update()
    {
        if (currentGun != null)
        {
            currentGun.HandleFire(inputManager.LeftButtonPressed, inputManager.LeftButtonHeld);
            if (inputManager.RightButtonPressed)
            {
                currentGun.ChargeGun();
            }
                }      
    }
    public void DropGun()
    {
        if (currentGun == null) return;
        Destroy(currentGun.gameObject);
        currentGun = null;
        onGunDropped?.Invoke();
    }
}

