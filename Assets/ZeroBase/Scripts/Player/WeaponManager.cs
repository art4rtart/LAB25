using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public RuntimeAnimatorController[] weaponCtrls;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (anim.runtimeAnimatorController == weaponCtrls[0])
                anim.runtimeAnimatorController = weaponCtrls[1];
            else
                anim.runtimeAnimatorController = weaponCtrls[0];
        }
    }
}
