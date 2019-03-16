using UnityEngine;
using UnityEngine.Rendering;

public class TwinCameraController : MonoBehaviour
{
	[SerializeField]
	private Camera _activeCamera;
	[SerializeField]
	private Camera _hiddenCamera;
	[SerializeField]
	private Renderer _depthHackQuad;
	private CommandBuffer _depthHackBuffer;

	public void SwapCameras()
	{
		_activeCamera.targetTexture = _hiddenCamera.targetTexture;
		_hiddenCamera.targetTexture = null;

		var swapCamera = _activeCamera;
		_activeCamera = _hiddenCamera;
		_hiddenCamera = swapCamera;
		DoDepthHack();
    }

	private void Awake()
	{
		var rt = new RenderTexture(Screen.width, Screen.height, 24);
		Shader.SetGlobalTexture("_TimeCrackTexture", rt);
		_hiddenCamera.targetTexture = rt;

		_depthHackBuffer = new CommandBuffer();
		_depthHackBuffer.ClearRenderTarget(true, true, Color.black, 0);
		_depthHackBuffer.name = "Fancy Depth Magic";
		_depthHackBuffer.DrawRenderer(_depthHackQuad, new Material(Shader.Find("Hidden/DepthHack")));

        DoDepthHack();
    }

	private void DoDepthHack()
	{
		_hiddenCamera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, _depthHackBuffer);
		_activeCamera.RemoveAllCommandBuffers();
    }
}
