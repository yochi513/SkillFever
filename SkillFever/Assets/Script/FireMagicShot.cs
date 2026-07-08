using UnityEngine;
using UnityEngine.InputSystem;

public class FireMagicShot : MonoBehaviour
{
    //ñÇñ@
   public GameObject fireBallPrefab;
   public Transform shotPoint;

    //èÒ
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
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Instantiate(fireBallPrefab, shotPoint.position, shotPoint.rotation);
            audioSource.PlayOneShot(fireSE);
            wand.localRotation *= Quaternion.Euler(-10f,0f,0f);
        }
        wand.localRotation = Quaternion.Slerp(wand.localRotation,originalRotation, Time.deltaTime * 10f);
    }
}
