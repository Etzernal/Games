using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameConstants gameConstants;
    public GameObject spawnManagerObject;
    private SpawnManager spawnManager;
    private ObjectType enemyType;
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        spawnManagerObject = GameObject.Find("EnemySpawnManager");
        spawnManager = spawnManagerObject.GetComponent<SpawnManager>();

        // get the starting position
        originalX = transform.position.x;
        moveRight = Random.Range(0, 2) == 0 ? -1 : 1;
        ComputeVelocity();

        GameManager.OnPlayerDeath += EnemyRejoice;
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * gameConstants.maxOffset / gameConstants.enemyPatroltime, 0);
    }
    void MoveGomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }
    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {// move gomba
            MoveGomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            MoveGomba();
        }
    }
    void KillSelf()
    {
        // enemy dies
        CentralManager.centralManagerInstance.increaseScore();
        StartCoroutine(flatten());
        Debug.Log("Kill sequence ends");
    }

    IEnumerator flatten()
    {
        Debug.Log("Flatten starts");
        int steps = 5;
        float stepper = 1.0f / (float)steps;

        for (int i = 0; i < steps; i++)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y - stepper, this.transform.localScale.z);

            // make sure enemy is still above ground
            this.transform.position = new Vector3(this.transform.position.x, gameConstants.groundSurface + GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
            yield return null;
        }
        Debug.Log("Flatten ends");
        this.gameObject.SetActive(false);
        Debug.Log("Enemy returned to pool");

        // New Enemy 
        enemyType = Random.Range(0, 2) == 0 ? ObjectType.gombaEnemy : ObjectType.greenEnemy;
        spawnManager.spawnFromPooler(enemyType);
        yield break;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // check if collides on top
            float yoffset = (other.transform.position.y - this.transform.position.y);
            if (yoffset > 0.75f)
            {
                KillSelf();
            }
            else
            {
                CentralManager.centralManagerInstance.damagePlayer();
            }
        }
    }

    // animation when player is dead
    void EnemyRejoice()
    {
        Debug.Log("Enemy killed Mario");
        // do whatever you want here, animate etc
        // ... 
        //ObjectPooler.SharedInstance.ActiveJump();
        StartCoroutine(spin());

    }

    IEnumerator spin()
    {
        Debug.Log("Spin starts");
        velocity = Vector2.zero;
        for (int i = 0; i < 360; i++)
        {
            this.transform.Rotate(0, 0, i);
            // make sure enemy is still above ground
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("Spin Ends");

    }
}
