using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieDissolve : MonoBehaviour
{
	public float spawnEffectTime = 2;
	public AnimationCurve fadeIn;
	public float timer = 0;
	Renderer _renderer;

	int shaderProperty;

	void Start()
	{
		shaderProperty = Shader.PropertyToID("_Dissolved");
		_renderer = GetComponent<Renderer>();
	}

	void Update()
	{
		if (timer < spawnEffectTime)
		{
			timer += Time.deltaTime;
		}
		_renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));
	}
}
