using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    public GameObject enemy;
    public GameObject player;
    float place = 1.0f;
    float enemyCreateTime = 0;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
        Vector3 enemyAppearPosition = player.transform.position + new Vector3(Random.Range(-3.0f, 3.0f), 0, place * 10.0f);
        enemyCreateTime += Time.deltaTime;
        if (enemyCreateTime > 1.0f){
            Instantiate(enemy, enemyAppearPosition, Quaternion.identity);
            enemyCreateTime = 0;
        }
	}
}
