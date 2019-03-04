using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float velocity;
	public Vector3 distance;
    public GameObject player;
    GameObject bullet;
	Vector3 center;
	Vector3 min, max, size;
    Vector2 initialPos;
    float speed;
    float finalPos;
    float direction;


	// Use this for initialization
	void Start () {


	}

	void Awake(){

        
	}
	
	// Update is called once per frame
	void Update () {

        movement();

    }

    public void movement() {
        
        if (direction == 1)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        if (transform.position.x > initialPos.x+finalPos)
        {
            gameObject.SetActive(false);
        }
        if (transform.position.x < initialPos.x-finalPos)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        direction = Mathf.Sign(player.transform.localScale.x);
        initialPos = transform.position;
        finalPos = 30;
        speed = 25;
    }
}
