using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity=transform.forward*speed;

        Destroy(gameObject, lifeTime);
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BreaksWall1"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
