using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfecteeGirlCtrl : MonoBehaviour
{
    Transform target;
    public SkinnedMeshRenderer[] rend;
    public bool isAttacked = false;

    private bool wasBoom = false;
    private AudioSource audiosrc;
    public AudioClip[] soundClips;
    public GameObject boomParticle;
    Health info;

    [HideInInspector]
    private Animator anim;
	private Projector projector;
	private AttackRange attackRange;
	
	void Awake()
	{
		target = FindObjectOfType<PlayerCtrl>().transform;
		projector = this.transform.GetChild(2).GetComponent<Projector>();
		attackRange = this.transform.GetChild(2).GetComponent<AttackRange>();
	}

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audiosrc = GetComponent<AudioSource>();
        info = GetComponent<Health>();

		for (int i = 0; i < bloodRend.Length; i++) { bloodRend[i].material.SetColor(Shader.PropertyToID("_Color"), new Color(1f, 1f, 1f, 0f)); }
		for(int i = 0; i < rend.Length; i++) rend[i].material.SetFloat(Shader.PropertyToID("_Dissolved"), 0f);
	}

    // Update is called once per frame
    void Update()
    {
        if (wasBoom)
            return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= projector.orthographicSize || isAttacked)
        {
			StartCoroutine(Dissolve());
			StartCoroutine(RangeIncrease());
            anim.SetBool("isBoom", true);
            wasBoom = true;
            Invoke("ScreamSoundPlay", 1.5f);
            Invoke("Boom", 2.5f);
        }
    }

    public void ChangeSkinColor()
    {
		for(int i = 0; i < rend.Length; i++) rend[i].material.color += new Color(0.01f, 0, 0);
    }

    public void SetAttackTrigger()
    {
        isAttacked = true;
    }

	void Boom()
    {
		// exlpode Sound
		this.gameObject.tag = "Untagged";
		this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
		audiosrc.clip = soundClips[1];
		audiosrc.Play();
		TestShake.Instance.Shake();

		Instantiate(boomParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
		StartCoroutine(Blood());
		if(Vector3.Distance(this.transform.position, target.transform.position) < projector.orthographicSize) PlayerManager.Instance.ApplyDamage(100);
	}

    private void ScreamSoundPlay()
    {
        if (gameObject.activeSelf)
        {
            audiosrc.clip = soundClips[0];
            audiosrc.Play();
        }
    }

    public void AfterDie()
    {
        if (gameObject.activeSelf)
            Boom();
    }

	public Renderer[] bloodRend;
	IEnumerator Blood()
	{
		float value = 0;

		projector.enabled = false;
		while (value <= 1)
		{
			for (int i = 0; i < bloodRend.Length; i++)
			{
				bloodRend[i].material.SetColor(Shader.PropertyToID("_Color"), new Color(1f, 1f, 1f, value));
			}
			value += Time.deltaTime * 2f;
			yield return null;
		}
	}

	IEnumerator Dissolve()
	{
		float value = 0;

		yield return new WaitForSeconds(2.2f);
		while (value <= 1)
		{
			value += Time.deltaTime * 1.5f;
			for(int i = 0; i < rend.Length; i++) rend[i].material.SetFloat(Shader.PropertyToID("_Dissolved"), value);
			yield return null;
		}
	}

	IEnumerator RangeIncrease()
	{
		attackRange.isExploding = true;
		while (projector.enabled)
		{
			attackRange.circleSize += Time.deltaTime * 5f;
			yield return null;
		}
	}
}
