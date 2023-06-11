using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject closestEnemy;
    public Vector3 closestEnemyPos;
    public bool enemyFound;
    public EnemyManager EnemyManager;
    private SpriteRenderer spriteRenderer;
    private int numOfEnemy = 0;
    public bool gonnaAttack;
    public int hp = 1;
    public float stoppingDistance = 1.0f;
    public float maxDistance = 5f;
    public float distanceMoved;
    public float moveDistance = 2f;
    private Vector3 previousPosition;
    public float moveSpeed = 3.0f;
    public GameObject daggerPrefab;
    public float daggerSpeed = 5f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyManager = FindObjectOfType<EnemyManager>();
        FindEnemy();
    }

  
    // Update is called once per frame
    void Update()
    {
     
        if (enemyFound && closestEnemy!=null)
        {
            closestEnemyPos = closestEnemy.transform.position;
            
            // check if the player is to the left of the enemy and facing right
            if (closestEnemy.transform.position.x < transform.position.x && spriteRenderer.flipX == false)
            {
                // flip the enemy horizontally and update its facing direction
                spriteRenderer.flipX = true;
            }
            // check if the player is to the right of the enemy and facing left
            else if (closestEnemy.transform.position.x > transform.position.x && spriteRenderer.flipX == true)
            {
                // flip the enemy horizontally and update its facing direction
                spriteRenderer.flipX = false;

            }
        }
        else FindEnemy();

        if (closestEnemy == null)
        {
            //
            FindEnemy();
        }
        
       if (closestEnemy == null && numOfEnemy > 1)
        {
            hp -= hp;
        }
        
   
    }

    private void FindEnemy()
    {
        closestEnemy = EnemyManager.GetClosestEnemy(transform.position);
        if (closestEnemy != null)
       {
           closestEnemyPos = closestEnemy.transform.position;
           enemyFound = true;
           numOfEnemy += 1;
       } 
      
    }
    
    public IEnumerator MoveToEnemy()
    {
        if (closestEnemy != null)
        {
            distanceMoved = 0;
            previousPosition = transform.position;

            Debug.Log("ДУМАЕТ ИДТИ ЛИ");

            while (Vector3.Distance(transform.position, closestEnemyPos) > stoppingDistance &&
                   distanceMoved < moveDistance)
            {

                var deltaTime = Mathf.Clamp(Time.deltaTime, 0f, 0.002f);
                transform.position = Vector3.MoveTowards(transform.position, closestEnemyPos, moveSpeed * deltaTime);
                distanceMoved = Vector3.Distance(previousPosition, transform.position);
                yield return null;
            }

            if (Vector3.Distance(transform.position, closestEnemyPos) <= stoppingDistance)
            {
                gonnaAttack = true;
                //  Debug.Log("attack player");
                // StartCoroutine(EnemyAttack());
            }
        }
    }


    public void Attack()
    {
        Debug.Log("АТАКУЕТ");
        StartCoroutine(SkeletonAttack());

    }

    public IEnumerator SkeletonAttack()
    {
        FindEnemy();
        yield return new WaitForSeconds(1f);
        Vector2 direction = (closestEnemyPos - transform.position).normalized;
        GameObject bullet = Instantiate(daggerPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * daggerSpeed;
        gonnaAttack = false;
    }

    public void Die()
    {
     Destroy(gameObject);   
    }
}
