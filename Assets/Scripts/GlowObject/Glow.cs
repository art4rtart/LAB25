using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour {

    public Color GlowColor;
    public float LerpFactor = 10;
    public float timer;

    public Renderer[] Renderers { get; private set; }
    public Color CurrentColor { get { return _currentColor; } }

    private List<Material> _materials = new List<Material>();
    private Color _currentColor;
    private Color _targetColor;

	void Start () {
		Renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in Renderers)
            _materials.AddRange(renderer.materials);

        enabled = false;
    }

	private void OnMouseEnter()
	{
		enabled = true;

		StartCoroutine(TargetColorToGlowColor());
	}

    private void OnMouseExit()
    {
        StopAllCoroutines();
        _targetColor = Color.black;
    }

	void Update () {
        _currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

        for (int i = 0; i < _materials.Count; i++)
            _materials[i].SetColor("_GlowColor", _currentColor);

        if (_currentColor.Equals(_targetColor))
            enabled = false;
        }

    IEnumerator TargetColorToGlowColor()
    {
        _targetColor = GlowColor;
        yield return new WaitForSeconds(timer);
        //StartCoroutine(TargetColorToDefaultColor());
    }

    IEnumerator TargetColorToDefaultColor()
    {
        _targetColor = Color.black;
        yield return new WaitForSeconds(timer);
        //StartCoroutine(TargetColorToGlowColor());
    }
}
