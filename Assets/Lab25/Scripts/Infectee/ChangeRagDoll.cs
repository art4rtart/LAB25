using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeRagDoll : MonoBehaviour
{
    public GameObject charObj;
    public GameObject ragdollObj;

	[Header("Generator Setting")]
	public bool isGeneratorExist;
    private InfecteeGenerator generator;

    private Health infectee;
	public int InfecteeID;
	public float dieTime = 2.5f;
    private void Start()
    {
        infectee = GetComponentInChildren<Health>();

        if(isGeneratorExist) generator = GameObject.Find("Generator").GetComponent<InfecteeGenerator>();
    }

    public IEnumerator ChangeRagdoll()
    {
        CopyAnimCharacterTransformToRagdoll(charObj.transform, ragdollObj.transform);
    
        charObj.gameObject.SetActive(false);
        ragdollObj.gameObject.SetActive(true);

        yield return new WaitForSeconds(dieTime);
      
        charObj.gameObject.SetActive(true);
        ragdollObj.gameObject.SetActive(false);

        infectee.hp = infectee.maxHp;

		generator.enemyPool[InfecteeID].RemoveItem(transform.gameObject, null, generator.transform.parent);

        //if( transform.name[0] == 'h')
        //    generator.enemyPool.RemoveItem(transform.gameObject, null, generator.transform.parent);

        //else if (transform.name[0] == 'B')
        //    generator.enemyPool2.RemoveItem(transform.gameObject, null, generator.transform.parent);

        //else if (transform.name[0] == 'T')
        //    generator.enemyPool3.RemoveItem(transform.gameObject, null, generator.transform.parent);
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
