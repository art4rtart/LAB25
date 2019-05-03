using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeRagDoll : MonoBehaviour
{
    public GameObject charObj;
    public GameObject ragdollObj;


    private InfecteeCtrl myInfecteeCtrl;

    private void Start()
    {
        myInfecteeCtrl = GetComponentInChildren<InfecteeCtrl>();

    }

    public IEnumerator ChangeRagdoll()
    {
        CopyAnimCharacterTransformToRagdoll(charObj.transform, ragdollObj.transform);

        charObj.gameObject.SetActive(false);
        ragdollObj.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        charObj.gameObject.SetActive(true);
        ragdollObj.gameObject.SetActive(false);

        myInfecteeCtrl.hp = myInfecteeCtrl.maxHp;

        InfecteeGenerator.enemyPool.RemoveItem(transform.gameObject, null,  InfecteeGenerator.parent);
    }

    private void CopyAnimCharacterTransformToRagdoll(Transform origin, Transform rag)
    {
        for (int i = 0; i < rag.transform.childCount; i++)
        {
            if (origin.transform.childCount != 0)
            {
                CopyAnimCharacterTransformToRagdoll(origin.transform.GetChild(i), rag.transform.GetChild(i));
            }
            rag.transform.GetChild(i).localPosition = origin.transform.GetChild(i).localPosition;
            rag.transform.GetChild(i).localRotation = origin.transform.GetChild(i).localRotation;
        }
        ragdollObj.transform.position = charObj.transform.position;
        ragdollObj.transform.rotation = charObj.transform.rotation;
    }
}
