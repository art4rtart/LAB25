using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoxBlur : MonoBehaviour
{
	public static BoxBlur Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<BoxBlur>();
			return instance;
		}
	}
	private static BoxBlur instance;

	public Material BlurMaterial;
	[Range(0, 30)]
	public float Iterations;
	[Range(0, 4)]
	public float DownRes;

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		int width = src.width >> (int)DownRes;
		int height = src.height >> (int)DownRes;

		RenderTexture rt = RenderTexture.GetTemporary(width, height);
		Graphics.Blit(src, rt);

		for (int i = 0; i < Iterations; i++)
		{
			RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
			Graphics.Blit(rt, rt2, BlurMaterial);
			RenderTexture.ReleaseTemporary(rt);
			rt = rt2;
		}

		Graphics.Blit(rt, dst);
		RenderTexture.ReleaseTemporary(rt);
	}
}