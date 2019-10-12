using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<PlayerManager>();
            return instance;
        }
    }

    private static PlayerManager instance;

    public static bool powerOverWhelming;
    // Player Specification
    public static float hp = 100f;
    public static float armor = 0;
    private float maxHp;
    private float maxArmor = 100;

    // Teleport Attribute
    public bool TeleportFlag = false;
    public Vector3 TeleportPos = Vector3.zero;
    public bool isEnd = false;

    // References
    private CharacterController characterController;
    private GameObject infecteeParent;
    //public NavMeshAgent _nvAgent;

    private static Transform mycam;
    public static bool isHit = false;

	private void Awake()
	{
		if (DataManager.Instance == null) return;
		if (DataManager.Instance.initGame) { DataManager.Instance.InitGame(); }
		else { DataManager.Instance.GetTotalData(FindObjectOfType<MoveToNextScene>().currentStageNum); }
		DataManager.Instance.initGame = false;
	}

    private void Start()
    {
		characterController = GetComponent<CharacterController>();
        maxHp = hp;
        infecteeParent = GameObject.Find("Generator");
        mycam = Camera.main.transform;
    }

    public void ApplyDamage(float damage)
    {
        isHit = true;

        // StartCoroutine("PlayHeartbeatSound");

        if (!powerOverWhelming) {
            if (armor <= 0)
            {
                if (hp <= 0)
                    hp = 0;
                else
                    hp -= damage;
            }
            else
            {
                armor -= damage;
            }
        }
        ItemManager.SetPlayerStat();
        UIManager.Instance.TextUpdate();
        GetComponent<TestShake>().Shake();
        //RecoilBack();
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
        if (collision.gameObject.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
        }
    }
}
