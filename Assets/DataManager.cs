using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
public class StageData
{
	public int currentBullet;
	public int totalBullet;
	public int health;
	public int armor;
	public int adrenalineCount;
	public int medicalKitCount;
}

public class DataManager : MonoBehaviour
{
	public static DataManager Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<DataManager>();
			return instance;
		}
	}
	private static DataManager instance;

	int health;
	int armor;
	int currentBullet;
	int totalBullet;
	int medicalKitCount;
	int adrenalineCount;

	public Vector3 PlayerPosition;
	public int stageNum;

	public Transform[] playerTransform;

	List<int> dataCollector = new List<int>();
	int saveElement = 7;

	public bool initGame;

	void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public void SaveStageNum(int stageNum)
	{
		this.stageNum = stageNum;
	}

	public void SavePlayerData(int health, int armor)
	{
		this.health = health;
		this.armor = armor;
	}

	public void SaveWeaponData(int currentBullet, int totalBullet)
	{
		this.currentBullet = currentBullet;
		this.totalBullet = totalBullet;
	}

	public void SaveItemData(int medicalKitCount, int adrenalineCount)
	{
		this.medicalKitCount = medicalKitCount;
		this.adrenalineCount = adrenalineCount;
	}

	public void SetTotalData()
	{
		string path = "Assets/Resources/" + FindObjectOfType<MoveToNextScene>().currentStageNum + "StageSavedData.txt";

		StreamWriter writer = new StreamWriter(path, false);

		writer.WriteLine("StageNum " + stageNum.ToString());

		writer.WriteLine("Health " + health.ToString());
		writer.WriteLine("Armor " + armor.ToString());

		writer.WriteLine("CurrentBullet " + currentBullet.ToString());
		writer.WriteLine("TotalBullet " + totalBullet.ToString());

		writer.WriteLine("CedicalKitCount " + medicalKitCount.ToString());
		writer.WriteLine("AdrenalineCount " + adrenalineCount.ToString());

		playerTransform[FindObjectOfType<MoveToNextScene>().currentStageNum - 1].position = PlayerCtrl.Instance.gameObject.transform.position;
		playerTransform[FindObjectOfType<MoveToNextScene>().currentStageNum - 1].rotation = PlayerCtrl.Instance.gameObject.transform.rotation;

		writer.Close();
	}

	public void GetTotalData(int sceneIndex)
	{
		string path = "Assets/Resources/" + sceneIndex + "StageSavedData.txt";
		StreamReader reader = new StreamReader(path);

		for (int i = 0; i < saveElement; i++)
		{
			string line = reader.ReadLine();
			int count = 0;
			for (int j = 0; j < line.Length; j++) { if (line[j] != ' ') count++; else break; }
			for (int j = 0; j < count; j++) line = line.Substring(1);
			dataCollector.Add(int.Parse(line));
		}

		reader.Close();

		stageNum = dataCollector[0];
		PlayerManager.hp = dataCollector[1];
		PlayerManager.armor = dataCollector[2];
		WeaponCtrl.Instance.akCurrentBullets = dataCollector[3];
		WeaponCtrl.Instance.akBulletsTotal = dataCollector[4];
		ItemManager.Instance.medicalKitCount = dataCollector[5];
		ItemManager.Instance.adrenalineCount = dataCollector[6];

		PlayerManager.Instance.TeleportFlag = true;
		PlayerManager.Instance.TeleportPos = playerTransform[sceneIndex - 1].position;
	}

	public StageData[] stageData;
	public void InitGame()
	{
		int stageIndex = FindObjectOfType<MoveToNextScene>().currentStageNum - 1;

		PlayerManager.hp = stageData[stageIndex].health;
		PlayerManager.armor = stageData[stageIndex].armor;
		WeaponCtrl.Instance.akCurrentBullets = stageData[stageIndex].currentBullet;
		WeaponCtrl.Instance.akBulletsTotal = stageData[stageIndex].totalBullet;
		ItemManager.Instance.medicalKitCount = stageData[stageIndex].medicalKitCount;
		ItemManager.Instance.adrenalineCount = stageData[stageIndex].adrenalineCount;
	}
}
