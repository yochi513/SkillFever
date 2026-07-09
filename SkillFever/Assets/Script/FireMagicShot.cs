using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class FireMagicShot : MonoBehaviour
{
    //魔法
   public GameObject fireBallPrefab;
   public Transform shotPoint;

    //クールタイム
    [SerializeField] float cooldown = 5f;
    private float currentCooldown = 0f;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TMP_Text cooldownText;

    //杖
    [SerializeField] private Transform wand;
    private Quaternion originalRotation;

    //SE
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireSE;
     void Start()
    {
        originalRotation = wand.localRotation;

    }


    void Update()
    {
       
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;

            cooldownImage.color = Color.gray;
            cooldownImage.fillAmount = currentCooldown / cooldown;

            cooldownText.text=Mathf.Ceil(currentCooldown).ToString();
        }
        else
        {
            cooldownImage.fillAmount = 0;
            cooldownImage.color = Color.green;
            cooldownText.text = "READY";
        }

        if (Keyboard.current.eKey.wasPressedThisFrame && currentCooldown<=0)
        {
            currentCooldown = cooldown;
            Instantiate(fireBallPrefab, shotPoint.position, shotPoint.rotation);
            audioSource.PlayOneShot(fireSE);
            wand.localRotation *= Quaternion.Euler(-10f,0f,0f);
        }
        wand.localRotation = Quaternion.Slerp(wand.localRotation,originalRotation, Time.deltaTime * 10f);
    }
}
