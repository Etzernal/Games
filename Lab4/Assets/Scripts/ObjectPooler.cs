using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    gombaEnemy = 0,
    greenEnemy = 1
}

[System.Serializable]
public class ObjectPoolItem
{
    public int amount;
    public GameObject prefab;
    public bool expandPool;
    public ObjectType type;
}

public class ExistingPoolItem
{
    public GameObject gameObject;
    public ObjectType type;

    // constructor
    public ExistingPoolItem(GameObject gameObject, ObjectType type)
    {
        // reference input
        this.gameObject = gameObject;
        this.type = type;
    }
}

public class ObjectPooler : MonoBehaviour
{
    public List<ObjectPoolItem> itemsToPool; // types of different object to pool
    public List<ExistingPoolItem> pooledObjects; // a list of all objects in the pool, of all types
    public static ObjectPooler SharedInstance;
    private Rigidbody2D enemyBody;

    void Awake()
    {
        SharedInstance = this;
        pooledObjects = new List<ExistingPoolItem>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amount; i++)
            {
                // this 'pickup' a local variable, but Unity will not remove it since it exists in the scene
                GameObject pickup = (GameObject)Instantiate(item.prefab);
                pickup.SetActive(false);
                pickup.transform.parent = this.transform;
                ExistingPoolItem e = new ExistingPoolItem(pickup, item.type);
                pooledObjects.Add(e);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetPooledObject(ObjectType type)
    {
        // return inactive pooled object if it matches the type
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy && pooledObjects[i].type == type)
            {
                return pooledObjects[i].gameObject;
            }
        }
        // this will be called when no more active object is present, item to expand pool if required
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.type == type)
            {
                if (item.expandPool)
                {
                    GameObject pickup = (GameObject)Instantiate(item.prefab);
                    pickup.SetActive(false);
                    pickup.transform.parent = this.transform;
                    pooledObjects.Add(new ExistingPoolItem(pickup, item.type));
                    return pickup;
                }
            }
        }
        return null;
    }

    public void ActiveJump()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].gameObject.activeInHierarchy)
            {
                enemyBody =  pooledObjects[i].gameObject.GetComponent<Rigidbody2D>();
                StartCoroutine(CrazyJump());
            }
        }
    }

    IEnumerator CrazyJump()
    {

        // wait for next frame
        yield return null;

        // render for 0.5 second
        for (int step = 0; step < 5; step++)
        {
            Debug.Log("WHEEE");
            enemyBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

            yield return null;
        }

    }
}
