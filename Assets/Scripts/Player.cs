using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform gunPosition;
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
            currentGun.GrabGun(gunPosition);
            onGunGrabbed?.Invoke();
        }
    }
}
