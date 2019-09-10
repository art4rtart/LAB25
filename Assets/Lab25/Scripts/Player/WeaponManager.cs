using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<WeaponManager>();
            return instance;
        }
    }

    private static WeaponManager instance;

    public RuntimeAnimatorController[] weaponCtrls;
    public Animator anim;

    public int weaponNum;
    private int curWeaponNum = 0;

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
        else if (Input.GetMouseButtonUp(2))
            UIManager.Instance.TextUpdate();
    }

    public void ChangeAxeToIronBongAnim()
    {
        if (anim.runtimeAnimatorController == weaponCtrls[0])
            anim.runtimeAnimatorController = weaponCtrls[2];
        else if (anim.runtimeAnimatorController == weaponCtrls[1])
            anim.runtimeAnimatorController = weaponCtrls[3];
    }
}
