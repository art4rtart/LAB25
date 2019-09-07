using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreCtrl : MonoBehaviour
{
    Health info;

    private void Start()
    {
        info = GetComponent<Health>();    
    }

    public void AfterDie()
    {
        Destroy(this.gameObject);
    }
}
