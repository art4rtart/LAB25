using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventVector3 : UnityEvent<Vector3> { }

public class Health : MonoBehaviour
{
    public int hp;
    public int maxHp;

    public EventVector3 damaged;
    public UnityEvent died;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;

        if (damaged == null)
            damaged = new EventVector3();
    }

    public void ApplyDamage(int damage, Vector3 hitpos)
    {
        hp -= damage;
        damaged.Invoke(hitpos);

        if (hp <= 0)
            died.Invoke();
    }
}
