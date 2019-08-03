using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventVector3 : UnityEvent<Vector3> { }

public class Health : MonoBehaviour
{
    public int hp;
    public int maxHp;

    public UnityEvent damaged;
    public UnityEvent died;
    public EventVector3 diedByBullet;

    ItemDrop itemDrop;

    // Start is called before the first frame update
    void Awake()
    {
        itemDrop = GetComponent<ItemDrop>();
        hp = maxHp;

        if (diedByBullet == null)
            diedByBullet = new EventVector3();
    }

	public void ApplyDamage(int damage, Vector3 hitpos)
	{
		hp -= damage;

		damaged.Invoke();

		if (hp <= 0)
		{
			diedByBullet.Invoke(hitpos);
			died.Invoke();
			if (itemDrop != null) itemDrop.DropItem();
		}
	}

    //public void ApplyDamage(int damage)
    //{
    //    hp -= damage;

    //    damaged.Invoke();
    //    if (hp <= 0)
    //    {
    //        died.Invoke();
    //        itemDrop.DropItem();
    //    }
    //}

    public void DeathByBomb()
    { 
        hp = 0;

        died.Invoke();
		if (itemDrop != null) itemDrop.DropItem();
    }
}
