using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {

    private bool isBoom = false;
    private void OnEnable()
    {
        Invoke("DestroyParticle", 1.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBoom)
            PlayerManager.Instance.ApplyDamage(25);
        isBoom = true;
    }

    void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
