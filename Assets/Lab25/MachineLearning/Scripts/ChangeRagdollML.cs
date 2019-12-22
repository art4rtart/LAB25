using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeRagdollML : MonoBehaviour
{
    public GameObject charObj;
    public GameObject ragdollObj;

    [Header("Generator Setting")]
    private GeneratorML generator;

    private Health infectee;
    public int InfecteeID;
    public float dieTime = 2.5f;

    public bool attackedByElectricStick;
    private Her0inEnemy heroinEnemyScr;
    private RagDollDIeCtrl ragDollScr;

    private void Start()
    {
        infectee = GetComponentInChildren<Health>();
        heroinEnemyScr = charObj.GetComponent<Her0inEnemy>();
        ragDollScr = ragdollObj.GetComponent<RagDollDIeCtrl>();

        generator = GameObject.Find("Generator").GetComponent<GeneratorML>();


    }

    public IEnumerator ChangeRagdoll()
    {
        PlayerCtrl.Instance.m_WalkSpeed = 4;
        PlayerCtrl.Instance.m_RunSpeed = 10;

        CopyAnimCharacterTransformToRagdoll(charObj.transform, ragdollObj.transform);
        //if (ragDollScr.Electricity != null) ragdollObj.GetComponent<RagDollDIeCtrl>().Electricity.GetComponent<ParticleSystem>().Stop();

        //if (attackedByElectricStick && this.gameObject.transform.GetChild(0).GetComponent<Health>().hp < 0)
        //{
        //    AudioSource tmpAudio = ragDollScr.Electricity.GetComponent<AudioSource>();

        //    ragDollScr.Electricity.GetComponent<ParticleSystem>().Play();
        //    tmpAudio.clip = ragDollScr.hitSound[Random.Range(0, ragDollScr.hitSound.Length)];
        //    tmpAudio.Play();
        //}

        //charObj.gameObject.SetActive(false);
        //ragdollObj.gameObject.SetActive(true);

        generator.enemyPool[InfecteeID].RemoveItem(transform.gameObject, null, generator.transform);
        yield return new WaitForSeconds(2.5f);

        //charObj.gameObject.SetActive(true);
        //ragdollObj.gameObject.SetActive(false);
        //attackedByElectricStick = false;
        //infectee.hp = infectee.maxHp;

       


        //if( transform.name[0] == 'h')
        //    generator.enemyPool.RemoveItem(transform.gameObject, null, generator.transform.parent);

        //else if (transform.name[0] == 'B')
        //    generator.enemyPool2.RemoveItem(transform.gameObject, null, generator.transform.parent);

        //else if (transform.name[0] == 'T')
        //    generator.enemyPool3.RemoveItem(transform.gameObject, null, generator.transform.parent);
    }

    public void RemoveItem()
    {
        generator.enemyPool[InfecteeID].RemoveItem(transform.gameObject, null, generator.transform);
    }
    private void CopyAnimCharacterTransformToRagdoll(Transform origin, Transform rag)
    {
        if (heroinEnemyScr.isLimpid == false)
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
        else
        {
            for (int i = 0; i < rag.childCount; i++)
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
}
