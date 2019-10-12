using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			DataManager.Instance.SaveStageNum(FindObjectOfType<MoveToNextScene>().currentStageNum);
			DataManager.Instance.SavePlayerData((int)PlayerManager.hp, (int)PlayerManager.armor);
			DataManager.Instance.SaveWeaponData(WeaponCtrl.Instance.akCurrentBullets, WeaponCtrl.Instance.akBulletsTotal);
			DataManager.Instance.SaveItemData(ItemManager.Instance.medicalKitCount, ItemManager.Instance.adrenalineCount);
			DataManager.Instance.SetTotalData();
			this.gameObject.SetActive(false);
		}
	}
}
