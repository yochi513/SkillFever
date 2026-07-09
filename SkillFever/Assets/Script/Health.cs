using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private int maxHP = 100;

    //衝突エフェクト
    public GameObject HitEffectPrefab;

    private int currentHP;

    public bool Gameover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Gameover = false;
        currentHP = maxHP;

        healthSlider.maxValue = maxHP;
        healthSlider.value = currentHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
        {
            currentHP = 0;
            Instantiate(HitEffectPrefab, transform.position, Quaternion.identity);
            Die(); 
        }
        healthSlider.value = currentHP;

    }

    public void Heal(int amount)
    {
        currentHP += amount;
    }

    public void Die()
    {
        Gameover = true;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
