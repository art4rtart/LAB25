using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDieRag : MonoBehaviour
{
	public GameObject rag;
	public GameObject fps;

	public GameObject player;
	public GameObject gameManager;
	public GameObject subCamera;

	public Animator fadeAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
		{
			fps.SetActive(false);
			rag.SetActive(true);

			Invoke("FadeOut", 2f);
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			subCamera.SetActive(true);
			SceneManager.LoadScene("2. Title");
		}
    }

	void FadeOut()
	{
		fadeAnim.SetTrigger("SceneEnd");
	}
}
