using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class FireMagicShot : MonoBehaviour
{
    [Header("魔法")]
    public GameObject fireBallPrefab;
    public Transform shotPoint;

    [Header("ため攻撃")]
    [SerializeField] private float chargeThreshold = 0.7f; // これ以上でため攻撃
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float chargedSizeMultiplier = 2f;

    [Header("ためゲージUI")]
    [SerializeField] private Image chargeImage;
    [SerializeField] private TMP_Text chargeText;

    [Header("クールタイム")]
    [SerializeField] float cooldown = 5f;
    private float currentCooldown = 0f;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TMP_Text cooldownText;

    [Header("杖")]
    [SerializeField] private Transform wand;
    private Quaternion originalRotation;

    [Header("ため中の杖演出")]
    [SerializeField] private float chargeWandMove = 0.08f; // 上下する幅
    [SerializeField] private float chargeWandSpeed = 8f;   // 揺れる速さ
    [SerializeField] private float chargeWandTilt = 12f;   // 上向き角度

    [Header("SE")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireSE;

    private bool isCharging;
    private float chargeTime;
    private bool inputReady = false;
    private Vector3 originalPosition;


    void Start()
    {
        originalPosition = wand.localPosition;
        originalRotation = wand.localRotation;
        isCharging = false;

        chargeImage.gameObject.SetActive(false);
        chargeText.gameObject.SetActive(false);

    }


    void Update()
    {
        Cooldown();

        FireInput();
        UpdateWandAnimation();

        wand.localRotation = Quaternion.Slerp(wand.localRotation, originalRotation, Time.deltaTime * 10f);
       
    }

    void Cooldown()
    {
        //クールダウン
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;

            cooldownImage.color = Color.gray;
            cooldownImage.fillAmount = currentCooldown / cooldown;

            cooldownText.text = Mathf.Ceil(currentCooldown).ToString();
        }
        else
        {
            cooldownImage.fillAmount = 0;
            cooldownImage.color = Color.green;
            cooldownText.text = "READY";
        }
    }

    void FireInput()
    {

        // 一度Eキーが離されるまで、開始直後の入力は無視する
        if (!inputReady)
        {
            if (!Keyboard.current.eKey.isPressed)
            {
                inputReady = true;
            }
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame && currentCooldown <= 0)
        {
            isCharging = true;
            chargeTime = 0f;
        }

        if (isCharging && Keyboard.current.eKey.isPressed)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Min(chargeTime, maxChargeTime);


            //UI表示
            chargeImage.gameObject.SetActive(true);
            chargeText.gameObject.SetActive(true);
            cooldownImage.gameObject.SetActive(false);
            cooldownText.gameObject.SetActive(false);


            float chargeRate = chargeTime / maxChargeTime;
            chargeImage.fillAmount=chargeRate;

            if (chargeTime >= chargeThreshold)
            {
                chargeImage.color = Color.red;
                chargeText.text = "CHARGED";
            }
            else
            {
                chargeImage.color = Color.yellow;
                chargeText.text = "CHARGE";

            }
         }

     

        if (isCharging && Keyboard.current.eKey.wasReleasedThisFrame)
        {
            bool isChargedShot = chargeTime >= chargeThreshold;

            GameObject fireBall = Instantiate(
                fireBallPrefab,
                shotPoint.position,
                shotPoint.rotation
            );

            if (isChargedShot)
            {
                fireBall.transform.localScale *= chargedSizeMultiplier;
            }

            currentCooldown = cooldown;
            audioSource.PlayOneShot(fireSE);
            wand.localRotation *= Quaternion.Euler(-10f, 0f, 0f);

            isCharging = false;
            chargeImage.gameObject.SetActive(false);
            chargeText.gameObject.SetActive(false);
            cooldownImage.gameObject.SetActive(true);
            cooldownText.gameObject.SetActive(true);
        }

       
    }
    void UpdateWandAnimation()
    {
        //ため中の火球を大きくする
        fireBallPrefab.transform.localScale = Vector3.Lerp(
         fireBallPrefab.transform.localScale, Vector3.one * 2f, Time.deltaTime * 3f);
        if (isCharging)
        {
            // ためるほど揺れを大きくする
            float power = chargeTime / maxChargeTime;

            // 上下に揺らす
            float y = Mathf.Sin(Time.time * chargeWandSpeed) * chargeWandMove * (0.3f + power);
            Vector3 targetPosition = originalPosition + new Vector3(0f, y, 0f);

            // 少し上へ向ける
            Quaternion targetRotation =
                originalRotation * Quaternion.Euler(-chargeWandTilt * power, 0f, 0f);

            wand.localPosition = Vector3.Lerp(
                wand.localPosition,
                targetPosition,
                Time.deltaTime * 12f
            );

            wand.localRotation = Quaternion.Slerp(
                wand.localRotation,
                targetRotation,
                Time.deltaTime * 12f
            );
        }
        else
        {
            // 発射後・待機中は元の位置と角度へ戻す
            wand.localPosition = Vector3.Lerp(
                wand.localPosition,
                originalPosition,
                Time.deltaTime * 10f
            );

            wand.localRotation = Quaternion.Slerp(
                wand.localRotation,
                originalRotation,
                Time.deltaTime * 10f
            );
        }
    }
}
