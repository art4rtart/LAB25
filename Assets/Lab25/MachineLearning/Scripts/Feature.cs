using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feature : MonoBehaviour
{
	static float totalPolygon;

	static string[] lastName = new string[] { "Oliver", "George", "Harry", "Jack", "Jacob", "Noah", "Charlie",
		"Thomas", "Oscar", "William", "James", "Henry", "Edward", "Joesph",
		"Lucas", "Logan", "Theo", "Herrison" , "Benjamin" , "Louis" , "Dylan" , "Adam",
		"Teddy" , "Tobby", "Jake" , "Louie" , "Elijah" , "Luke" , "Gabriel" , "Alex" , "Kai"   };

	static string[] middleName = new string[] { ".A.", ".B.", ".C.", ".D.", ".E.", ".F.", ".G.", ".H.", ".I.", ".J.",
		".K.", ".L.", ".M.", ".N.", ".O.", ".P.", ".Q.", ".R.", ".S.", ".T.", ".U.",
		".V.", ".W.", ".X.", ".Y . ", ".Z." };

	static string[] firstName = new string[] { "Smith", "Johnson", "Willaims", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor",
		"Green", "Anderson", "Thomas", "Jackson", "White", "Harris", "Cater", "Richardson", "Cox", "Howard",
		"Ward", "Carter", "Baker", "Adams", "Nelson", "Morgan", "Cook", "Stark", "Rogers", "Scoot", "Hill", "Wright",
		"King", "Murphy", "Bell", "Richardson", "Martin", "Young" };

	static string[] departments = new string[] { "VirusRoom", "VacineRoom", "MainLobby", "DisinfectionRoom" };

	static string[] types = new string[] { "Human", "Zombie_1", "Zombie_2", "Transparent", "Police", "Boss" };

	string[] animationStates = new string[] { "Idle_1", "Idle_2", "Idle_3", "Walk_1", "Walk_2", "Walk_3", "Run_1", "Run_2", "Attack_1", "Attack_2", "Attack_3", "Attack_4", "Attack_5", "Crash", "Die", "Shoot", "Push" };

	Animator animator;
	public SkinnedMeshRenderer skinnedMeshRenderer;
	bool initTrigger = false;
	int triggerNum = 0;

	public string gameObjectName;
	public string gameObjectTag;
	public string type;
	public string department;
	public Color eyeColor;
	public float moveSpeed;
	public float bodyMaintainence;          // percentage
	public float currentPolygon;
	public Transform[] Ankles;
	public float distance;
	public string animationName;
	public int nameHash;

	public float maxSpeedValue;

	void Awake()
	{
		animator = GetComponent<Animator>();
	}

	void OnEnable()
	{
		GetComponent<Health>().hp = 100;
		GenerateFeatures();
	}

	public void GenerateFeatures()
	{
		// get tag
		gameObjectTag = this.gameObject.tag;

		// common features
		distance = Random.Range(0f, 30f);
		gameObjectName = lastName[Random.Range(0, lastName.Length)] + middleName[Random.Range(0, middleName.Length)] + firstName[Random.Range(0, firstName.Length)];
		type = GetZombieType(this.gameObject.name);
		department = departments[Random.Range(0, departments.Length)];
		totalPolygon = skinnedMeshRenderer.sharedMesh.triangles.Length * 0.3f;  //? 설마 점의 개수?

		// zombie animation features
		//if (initTrigger) { animator.SetBool(triggerNum.ToString(), false); initTrigger = false; };
		//triggerNum = Random.Range(1, GetTriggerLength(type));
		//animator.SetBool(triggerNum.ToString(), true);

        GetCurrentClipName();
        //
        // zombie features
        if (gameObjectTag == "Infectee") {
			eyeColor = new Color(Random.Range(0.50f, 1f), 0, 0);
			moveSpeed = Random.Range(3.5f, 4.5f);
			maxSpeedValue = 4.5f;
			currentPolygon = totalPolygon - Random.Range(0, totalPolygon * 0.8f);
		}
		// human features
		else
		{
			eyeColor = new Color(Random.Range(0.95f, 1f), Random.Range(0.95f, 1f), Random.Range(0.95f, 1f));
			moveSpeed = Random.Range(5f, 8.5f);
			maxSpeedValue = 8.5f;
			currentPolygon = totalPolygon - Random.Range(0, 100);
		}
        bodyMaintainence = currentPolygon / totalPolygon * 100;
		

		initTrigger = true;
		// print("features generated!");
	}

    public void GetCurrentClipName()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        for (int i = 0; i < animationStates.Length; i++)
        {
            if (stateInfo.IsName(animationStates[i]))
            {
                animationName = animationStates[i];
                return;
            }

            else
            {
                animationName = "NULL";
            }
        }
    }
	

	string GetZombieType(string name)
	{
		string zombieType = "";
		switch (name)
		{
			case "Human":
				zombieType = "Human";
				nameHash = 1;
				break;

			case "KnitInfectee":
				zombieType = "Zombie_1";
				nameHash = 2;
				break;

			case "MedicInfectee":
				zombieType = "Zombie_2";
				nameHash = 3;
				break;

			case "Limpid":
				zombieType = "Transparent";
				nameHash = 4;
				break;

			case "GuardInfectee":
				zombieType = "Police";
				nameHash = 5;
				break;

			case "Charger":
				zombieType = "Boss";
				nameHash = 6;
				break;
		}

		return zombieType;
	}

	int GetTriggerLength(string type)
	{
		int triggerLength = 0;

		switch (type)
		{
			case "Human":
				triggerLength = 5;
				break;

			case "Zombie_1":
				triggerLength = 10;
				break;

			case "Zombie_2":
				triggerLength = 10;
				break;

			case "Transparent":
				triggerLength = 9;
				break;

			case "Police":
				triggerLength = 10;
				break;

			case "Zombie_5":
				triggerLength = 8;
				break;
		}

		return triggerLength;
	}

	public void Die()
	{
		//Debug.Log("I'm dead, because i'm zombie");
	}

	public void Survived()
	{
		//Debug.Log("I've survived, because i'm human");
	}
}
