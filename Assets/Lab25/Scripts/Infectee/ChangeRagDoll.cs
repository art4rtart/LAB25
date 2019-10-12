using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeRagDoll : MonoBehaviour
{
    public GameObject charObj;
    public GameObject ragdollObj;

	[Header("Generator Setting")]
	public bool isGeneratorExist;
    public bool isDgeneratorExist;
    private InfecteeGenerator generator;
    private DefenseGenerator dGenerator;

    private Health infectee;
	public int InfecteeID;
	public float dieTime = 2.5f;

	public bool attackedByElectricStick;

	private void Start()
    {
        infectee = GetComponentInChildren<Health>();

        if (isGeneratorExist)
        {
            generator = GameObject.Find("Generator").GetComponent<InfecteeGenerator>();
        }
        else if ( isDgeneratorExist)
        {
            dGenerator = GameObject.Find("Generator").GetComponent<DefenseGenerator>();
        }
    }

    public IEnumerator ChangeRagdoll()
    {
        CopyAnimCharacterTransformToRagdoll(charObj.transform, ragdollObj.transform);
		if(ragdollObj.GetComponent<RagDollDIeCtrl>().Electricity != null) ragdollObj.GetComponent<RagDollDIeCtrl>().Electricity.GetComponent<ParticleSystem>().Stop();

		if (attackedByElectricStick)
		{
			ragdollObj.GetComponent<RagDollDIeCtrl>().Electricity.GetComponent<ParticleSystem>().Play();
			ragdollObj.GetComponent<RagDollDIeCtrl>().Electricity.GetComponent<AudioSource>().clip = ragdollObj.GetComponent<RagDollDIeCtrl>().hitSound[Random.Range(0, ragdollObj.GetComponent<RagDollDIeCtrl>().hitSound.Length)];
			ragdollObj.GetComponent<RagDollDIeCtrl>().Electricity.GetComponent<AudioSource>().Play();
		}

		charObj.gameObject.SetActive(false);
        ragdollObj.gameObject.SetActive(true);

        yield return new WaitForSeconds(dieTime);
      
        charObj.gameObject.SetActive(true);
        ragdollObj.gameObject.SetActive(false);
		attackedByElectricStick = false;
		infectee.hp = infectee.maxHp;

        if( !isDgeneratorExist )
            generator.enemyPool[InfecteeID].RemoveItem(transform.gameObject, null, generator.transform);
        else
            dGenerator.enemyPool[InfecteeID].RemoveItem(transform.gameObject, null, dGenerator.transform);

        //if( transform.name[0] == 'h')
        //    generator.enemyPool.RemoveItem(transform.gameObject, null, generator.transform.parent);

        //else if (transform.name[0] == 'B')
        //    generator.enemyPool2.RemoveItem(transform.gameObject, null, generator.transform.parent);

        //else if (transform.name[0] == 'T')
        //    generator.enemyPool3.RemoveItem(transform.gameObject, null, generator.transform.parent);
    }

    private void CopyAnimCharacterTransformToRagdoll(Transform origin, Transform rag)
    {
        for (int i = 0; i < origin.childCount; i++)
        {
            if (origin.childCount != 0)
            {
                CopyAnimCharacterTransformToRagdoll(origin.GetChild(i), rag.GetChild(i));
            }
            rag.GetChild(i).localPosition = origin.GetChild(i).localPosition;
            rag.GetChild(i).localRotation = origin.GetChild(i).localRotation;
        }
        ragdollObj.transform.position = charObj.transform.position;
        ragdollObj.transform.rotation = charObj.transform.rotation;
    }
}
