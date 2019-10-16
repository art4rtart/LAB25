using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ZombieScanner : MonoBehaviour
{
	public static ZombieScanner Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<ZombieScanner>();
			return instance;
		}
	}
	private static ZombieScanner instance;

	public Material EffectMaterial;
	public float ScanDistance = 0;

	private Camera cam;
	public float scanSpeed = 25f;
	public bool scanning;
	GameObject player;

	void Awake()
	{
		player = FindObjectOfType<PlayerCtrl>().gameObject;
	}

	public float findRadius = 10f;
	public LayerMask zombieMask;
	public bool isUTRSMode;
	void Update()
	{
		if (scanning)
		{
			if (ScanDistance < 100f)
				ScanDistance += Time.deltaTime * scanSpeed;
			else scanning = false;

			if (!isUTRSMode)
			{
				Collider[] zombieInRadius = Physics.OverlapSphere(player.transform.position, findRadius, zombieMask);

				for (int i = 0; i < zombieInRadius.Length; i++)
				{
					if (Vector3.Distance(this.transform.position, zombieInRadius[i].transform.position) <= ScanDistance)
					{
						if (zombieInRadius[i].GetComponent<Her0inEnemy>() != null && !zombieInRadius[i].GetComponent<Her0inEnemy>().spawnEffect.enabled)
							zombieInRadius[i].GetComponent<Her0inEnemy>().spawnEffect.enabled = true;
					}
				}
			}
		}
		else
			ScanDistance = 0;
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
