using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrick : MonoBehaviour
{    
    public GameObject prefab;
    private bool broken = false; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !broken)
        {
            broken = true;
            // assume we have 5 debris per box
            for (int x = 0; x < 5; x++)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponentInChildren<EdgeCollider2D>().enabled = false;
            //Destroy(gameObject);
        }
    }
}
