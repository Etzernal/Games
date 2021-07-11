using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector3 scaler;
    private AudioSource collectcoinAudio;
    private ObjectType enemyType;
    public GameObject spawnManagerObject;
    private SpawnManager spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        collectcoinAudio = GetComponent<AudioSource>();
        spawnManagerObject = GameObject.Find("EnemySpawnManager");
        spawnManager = spawnManagerObject.GetComponent<SpawnManager>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        collectcoinAudio.PlayOneShot(collectcoinAudio.clip);
        if (col.gameObject.CompareTag("Player"))
        {
            // New Enemy 
            enemyType = Random.Range(0, 2) == 0 ? ObjectType.gombaEnemy : ObjectType.greenEnemy;
            spawnManager.spawnFromPooler(enemyType);
            CentralManager.centralManagerInstance.increaseScore();
            Destroy(gameObject);
        }
    }
}
