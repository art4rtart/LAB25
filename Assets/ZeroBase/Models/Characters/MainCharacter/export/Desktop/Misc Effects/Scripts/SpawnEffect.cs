using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour {

    public float spawnEffectTime = 2;
    public AnimationCurve fadeIn;

	public DieDissolve dieDissolve;
    ParticleSystem ps;
    float timer = 0;
    Renderer _renderer;

    int shaderProperty;

	void Start ()
    {
        shaderProperty = Shader.PropertyToID("_Dissolved");
        _renderer = GetComponent<Renderer>();
        ps = GetComponentInChildren <ParticleSystem>();

        var main = ps.main;
        main.duration = spawnEffectTime;
        ps.Play();
    }

	void Update ()
    {
        if (timer < spawnEffectTime)
        {
			timer += Time.deltaTime;
        }

        _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate( Mathf.InverseLerp(0, spawnEffectTime, timer)));
    }

	void OnEnable()
	{
		dieDissolve.timer = 0;
	}
}
