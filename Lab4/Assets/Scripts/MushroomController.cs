using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector2 currentDirection;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(new Vector2(0, rigidBody.mass * 10), ForceMode2D.Impulse);
        float rand = Random.Range(-10.0f, 10.0f);
        if (rand < 0)
        {
            currentDirection = new Vector2(-1f, 0f);
        }
        else
        {
            currentDirection = new Vector2(1f, 0f);
        }
    }

    void MoveShroom()
    {
        rigidBody.MovePosition(rigidBody.position + new Vector2 (speed,0) * Time.fixedDeltaTime * currentDirection);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentPosition = transform.position;
        Debug.Log(currentPosition);
        
        //Debug.Log(currentDirection);
        Vector2 nextPosition = currentPosition + speed * currentDirection.normalized * Time.fixedDeltaTime;
        rigidBody.MovePosition(nextPosition);
        //MoveShroom();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
        {
            currentDirection *= -1f;
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            speed = 0;
        }
    }
}
