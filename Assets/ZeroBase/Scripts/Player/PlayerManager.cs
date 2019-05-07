using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
	public ItemManager itemManager;
    // Player Specification
    public static float hp = 100f;
    public static float armor = 100;
    private float maxHp;
    private float maxArmor = 100;

    // Teleport Attribute
    public bool teleportFlag = false;
    public Vector3 TeleportPos = Vector3.zero;
    public bool isEnd = false;

    // References
    private CharacterController characterController;
    private GameObject infecteeParent;
    //public NavMeshAgent _nvAgent;

    public static bool isHit = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        maxHp = hp;
        infecteeParent = GameObject.Find("Generator");
        
        //DontDestroyOnLoad(gameObject);
    }

    public static void ApplyDamage(float damage)
    {
        isHit = true;
        if (armor <= 0)
        {
            if (hp <= 0)
            {
                hp = 0;
                // Die;
                //Debug.Log("You Died!");
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
            //UpdateArmor();
        }
		// 마크
        //UIManager.takeDamge = true;
    }

    private void recoverArmorGage(int recoverGage)
    {
        armor += recoverGage;
        if (armor > 100)
            armor = 100;
    }

    private void OnTriggerStay(Collider collision)
    {
        //Debug.Log("TCollision");
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (collision.CompareTag("Heal"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ApplyDamage(-10);
                    Destroy(collision.gameObject);
                }
            }
            else if (collision.CompareTag("Armor"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    recoverArmorGage(20);
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("CCollision");
       
        if (collision.gameObject.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Item"))
        {
            //Debug.Log("CCCollision");
            //Destroy(hit.gameObject);
        }
    }
}
