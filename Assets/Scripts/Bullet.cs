using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;
    void Awake()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
    }
}
