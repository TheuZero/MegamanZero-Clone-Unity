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

    float speed = 30;
    float finalPos = 18;
    float direction;

    public float dmg = 1;

    //BulletHitDelegate BulletHitDelegate;
    //delegate void BulletHitDelegate(int dmg);
    //event BulletHitDelegate BulletHit;
    //event Action<int> OnBulletHit;

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
    }

    public void OnTriggerEnter2D (Collider2D target){
        if(target.isTrigger != true && target.tag == "Enemy"){
            gameObject.SetActive(false);
            target.gameObject.GetComponent<EnemyDamage>().takeDamage(dmg);
        }
    }
}
