using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPool : MonoBehaviour {
	public GameObject Sword;
	public List<GameObject> pooledSwords;
	public int amountToPool;

	public static SwordPool SharedInstance;
	

	public GameObject getSword(){
		for (int i = 0; i < pooledSwords.Count; i++) {
			//2
			if (!pooledSwords[i].activeInHierarchy) {
				return pooledSwords[i];
			}
		}
		//3   
		return null;
	}
	void Awake(){
		SharedInstance = this;
	}
	void Start () {
		for (int i = 0; i < amountToPool; i++) {
			GameObject obj = (GameObject)Instantiate (Sword);
			obj.SetActive (false);
			pooledSwords.Add (obj);
		
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
