using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ZombieScanner : MonoBehaviour
{
	public Material EffectMaterial;
	public float ScanDistance;

	private Camera cam;
	public float scanSpeed = 25f;
	bool scanning;
	GameObject player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update()
	{
		if (scanning)
		{
			if (ScanDistance < 100f)
				ScanDistance += Time.deltaTime * scanSpeed;
			else scanning = false;
		}
		else
			ScanDistance = 0;

		if (Input.GetKeyDown(KeyCode.C))
		{
			scanning = true;
			ScanDistance = 0;
		}
	}

	void OnEnable()
	{
		cam = GetComponent<Camera>();
		cam.depthTextureMode = DepthTextureMode.Depth;
	}

	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		EffectMaterial.SetVector("_WorldSpaceScannerPos", player.transform.position);
		EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
		RaycastCornerBlit(src, dst, EffectMaterial);
	}

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
	{
		float camFar = cam.farClipPlane;
		float camFov = cam.fieldOfView;
		float camAspect = cam.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = cam.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = cam.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (cam.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (cam.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (cam.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (cam.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit
		RenderTexture.active = dest;

		mat.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}
}
