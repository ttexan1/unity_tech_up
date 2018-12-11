using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    float velocity = 0.4f;
    float generalTime = 0.0f;
    new Rigidbody rigidbody;

    // Use this for initialization
    void Start () {
        rigidbody = this.transform.GetComponent<Rigidbody>();
        PlayerPrefs.SetString("playerName", "playerD");
    }

	// Update is called once per frame
	void Update () {
        generalTime += Time.deltaTime;
		this.transform.position += new Vector3(0, 0, velocity * 0.3f);
        Move();
	}

    void Move()
    {
        if (Input.GetKey("left")) {
            this.transform.position -= new Vector3(velocity*1.0f, 0, 0);
        }
        if (Input.GetKey("right")) {
            this.transform.position += new Vector3(velocity * 1.0f, 0, 0);
        }
        if (Input.GetKey("up")) {
            float accelTime = Time.deltaTime;
            if (accelTime < 1.0f) {
                this.transform.position += new Vector3(0, 0, velocity * 1.0f);
                rigidbody.AddForce(new Vector3(0, 0, 5.0f));
            }
        }
        if (Input.GetKeyDown("down")) {
            float accelTime = Time.deltaTime;
            if (accelTime < 1.0f){
                rigidbody.AddForce(new Vector3(0, 0, -5.0f));
            }
        }
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "enemy"){
            PlayerPrefs.SetFloat("scoreTime", generalTime);
            SceneManager.LoadScene("Result");
        }
    }
}
