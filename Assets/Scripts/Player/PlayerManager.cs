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

    // Player Stage
    public static stage currentStage;

    // Teleport Attribute
    public bool teleportFlag = false;
    public Vector3 TeleportPos = Vector3.zero;
    public bool isEnd = false;

    // References
    private CharacterController characterController;
    private GameObject infecteeParent;
    //public NavMeshAgent _nvAgent;
    private Transform targetTr;

    public static bool isHit = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentStage = stage.STAGE_1;
        maxHp = hp;
        infecteeParent = GameObject.Find("Generator");
        
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        //_nvAgent = GetComponent<NavMeshAgent>();
        targetTr = GameObject.Find("Portal").transform;
        //_nvAgent.SetDestination(targetTr.position);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            ApplyDamage(50);
        //_nvAgent.SetDestination(targetTr.position);
    }

    private void FixedUpdate()
    {

        //if( teleportFlag )
        //{
        //    teleportFlag = false;
        //    transform.position = TeleportPos;
        //    Debug.Log(transform.position);
        //    TeleportPos = Vector3.zero;
        //}
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
                Debug.Log("You Died!");
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

    private void UpdateHP()
    {
        //hpText.text = hp + " / " + maxHp;
        //hpBar.localScale = new Vector3(hp / maxHp, 1, 1);
    }

    private void UpdateArmor()
    {
        //armorText.text = armor + " / " + maxArmor;
        //armorBar.localScale = new Vector3(armor / maxArmor, 1, 1);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Portal") && teleportFlag == false)
        {
            InfecteeGenerator.enemyPool.ClearItem();

            foreach (Transform child in infecteeParent.transform)
            {
                child.GetChild(0).gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }

            if (currentStage == stage.STAGE_1)
            {
                SceneManager.LoadScene("Stage2");
                currentStage = stage.STAGE_2;
                TeleportPos = new Vector3(0, 0, -6);
                isEnd = true;
                InfecteeGenerator.stage_EnemyZone = GameObject.FindGameObjectsWithTag("SpawnZone");
            }

            //foreach (Transform child in infecteeParent.transform)
            //{
            //    child.GetChild(0).gameObject.GetComponent<NavMeshAgent>().enabled = true;
            //}
            teleportFlag = true;
        }
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
