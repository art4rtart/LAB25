using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfecteeGirlCtrl : MonoBehaviour
{
    public float recognizeRange;
    public Transform target;
    public SkinnedMeshRenderer[] mySkin;
    public int hp;
    public bool isAttacked = false;

    private bool wasBoom = false;
    [HideInInspector]
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wasBoom)
            return;

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= recognizeRange || isAttacked)
        {
            anim.SetBool("isBoom", true);
            wasBoom = true;
        }
    }

    public void ChangeSkinColor()
    {
        mySkin[0].material.color = mySkin[0].material.color + new Color(0.01f, 0, 0);
        mySkin[1].material.color = mySkin[0].material.color + new Color(0.01f, 0, 0);
    }

    public void ApplyDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
            Destroy(gameObject);

    }
}
