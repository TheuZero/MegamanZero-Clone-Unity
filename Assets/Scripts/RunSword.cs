using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSword : MonoBehaviour {

	private Vector2 position;
	public GameObject player;
	public GameObject Sword;

	IEnumerator Deactivate(){
		Debug.Log("Corountine start");
		yield return new WaitForSeconds(0.6f);
		//gameObject referencia o objeto que contém o script.
		gameObject.SetActive(false);
		Debug.Log(Time.time);
	}

	void Start () {
        Sword = SwordPool.SharedInstance.getSword();
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.localScale.x >= 1){
			position = new Vector2(player.transform.position.x-2f, player.transform.position.y+2.5f);
		}
		else{
			position = new Vector2(player.transform.position.x+2f, player.transform.position.y+2.5f);
		}
		transform.position = position;
	}

	void OnEnable(){
		transform.position = position;
		gameObject.transform.localScale = player.transform.localScale;
		//StartCoroutine(Deactivate());
	}

	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player");
		
	}
}
