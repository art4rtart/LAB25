using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class AgentManager : MonoBehaviour
{
    // Player Specification
    public static int hp = 100;
    public static int armor = 0;
    private float maxHp;
    private float maxArmor = 0;
    public static Transform myPos;


    private void Start()
    {
        maxHp = hp;
    }

    //private void Update()
    //{
    //    //Debug.Log(hp);
    //    myPos = transform;
    //}

    public static void ApplyDamage(int damage)
    {
        if (armor <= 0)
        {
            if (hp <= 0)
            {
                hp = 0;
            }
            else
            {
                hp -= damage;

                //UpdateHP();
            }
        }
        else
        {
            armor -= damage;
        }

    }

    private void recoverArmorGage(int recoverGage)
    {
        armor += recoverGage;
        if (armor > 100)
            armor = 100;
    }
  
    
}
