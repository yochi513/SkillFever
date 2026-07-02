using UnityEngine;

public class FireBall : MonoBehaviour
{
    //ボール関連
    public float speed = 20f;
    public float lifeTime = 5f;
    Rigidbody rb;

    //衝突エフェクト
    public GameObject HitEffectPrefab;

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
            Instantiate(HitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
