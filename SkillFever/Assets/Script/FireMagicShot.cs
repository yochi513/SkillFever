using UnityEngine;
using UnityEngine.InputSystem;

public class FireMagicShot : MonoBehaviour
{
   public GameObject fireBallPrefab;
   public Transform shotPoint;

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Instantiate(fireBallPrefab, shotPoint.position, shotPoint.rotation);
        }
    }
}
