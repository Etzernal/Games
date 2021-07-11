using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private bool moving;
    public float speed = 130;
    public float maxSpeed = 10;
    public float upSpeed = 50;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public Transform enemyLocation;
    private bool onGroundState = true;
    private Animator marioAnimator;
    private AudioSource marioAudio;


    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();

        // subscribe to player event
        GameManager.OnPlayerDeath += PlayerDiesSequence;
    }

    void FixedUpdate()
    {
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }
        else
        {
            marioBody.velocity = new Vector2(0, marioBody.velocity.y);
        }

        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            // Animation
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
                marioAnimator.SetTrigger("onSkid");
        }
        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            // Animation
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
                marioAnimator.SetTrigger("onSkid");
        }

        // Jump
        if (Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
        }

        // Animation
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        marioAnimator.SetBool("onGround", onGroundState);

        // Consumable mushrooms
        if (Input.GetKeyDown("z"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z, this.gameObject);
        }
        if (Input.GetKeyDown("x"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X, this.gameObject);
        }

    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles"))
        {
            onGroundState = true; // back on ground
        };
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    }



    void PlayJumpSound()
    {
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void PlayerDiesSequence()
    {
        // Mario dies
        Debug.Log("Mario dies");
        StartCoroutine(flatten());
    }

    IEnumerator flatten()
    {
        Debug.Log("Flatten starts");
        int steps = 10;
        float stepper = 1.0f / (float)steps;

        for (int i = 0; i < steps; i++)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y - stepper, this.transform.localScale.z);
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Flatten ends");
    }
    
}
