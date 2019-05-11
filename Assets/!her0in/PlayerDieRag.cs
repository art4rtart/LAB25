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
	public GameObject gameOver;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	bool gameOverTrigger;
    // Update is called once per frame
    void Update()
    {
		if(ItemManager.currentHealth <= 0 && !gameOverTrigger) { 
			fps.SetActive(false);
			rag.SetActive(true);
			Invoke("Die", 0.5f);
			Invoke("FadeOut", 2f);
			gameOverTrigger = true;
		}

		//if(Input.GetKeyDown(KeyCode.P))
		//{
		//	subCamera.SetActive(true);
		//	SceneManager.LoadScene("2. Title");
		//}
    }

	void Die()
	{
		gameOver.SetActive(true);
	}

	void FadeOut()
	{
		fadeAnim.SetTrigger("SceneEnd");
		Invoke("GoToTitle", 3f);
	}

	void GoToTitle()
	{
		subCamera.SetActive(true);
		SceneManager.LoadScene("2. Title");
	}
}
