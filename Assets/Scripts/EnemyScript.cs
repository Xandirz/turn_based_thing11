using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyScript : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float stoppingDistance = 1.0f;
    public float maxDistance = 5f;
    public float distanceMoved;
    public float moveDistance = 1f;
    private Vector3 previousPosition;
    private SpriteRenderer spriteRenderer;
    public float hp = 2;
    public float maxHp = 2;
    private bool isMoving = false;
    public GameObject playerObject;
    public GameObject daggerPrefab;
    public float daggerSpeed = 5f;
    public GameObject healthBar;
    public bool gonnaAttack = false;
    public bool bowEnemy = false;
    public float objectCheckRange = 0.5f;
    public Collider2D myCollider2D;
    public EnemyPathfinding enemyPathfinding;
    public float flipThreshold = 0.1f; // how close the player needs to be to trigger flipping

    //PATHFIDNGD
    public LayerMask obstacleMask;
    public string enemyTag = "Enemy";
    private void Start()
    {
        hp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar.GetComponent<TextMeshPro>().text = hp.ToString(CultureInfo.InvariantCulture) + "/" +
                                                     maxHp.ToString(CultureInfo.InvariantCulture);

        myCollider2D = GetComponent<Collider2D>();
        playerObject = GameObject.FindWithTag("Player");
        
        moveDistance = Random.Range(0.9f, 1.2f);
        
    }


    private void OnMouseOver()
    {
        spriteRenderer.color = Color.red;
      
    }

    private void OnMouseExit()
    {
        spriteRenderer.color =  Color.white;
    }

    public IEnumerator MoveToPlayer(Transform player)
    {
        distanceMoved = 0;
        previousPosition = transform.position;

        isMoving = true;
        
        
        //обычный сценарий - игрок далеко - надо подойти
        while (Vector3.Distance(transform.position, player.position) > stoppingDistance && distanceMoved < moveDistance)
        { 
        var deltaTime = Mathf.Clamp(Time.deltaTime, 0f, 0.002f);
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * deltaTime);
        distanceMoved = Vector3.Distance(previousPosition, transform.position);
        yield return null;
        }
    
        
        if (!bowEnemy)
        {
            while (Vector3.Distance(transform.position, player.position) < stoppingDistance &&
                   distanceMoved < moveDistance)
            {
                var deltaTime = Mathf.Clamp(Time.deltaTime, 0f, 0.002f);
                Vector3 targetPosition = transform.position + (transform.position - player.position).normalized * moveDistance;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * deltaTime);
                distanceMoved = Vector3.Distance(previousPosition, transform.position);
                yield return null;
            }
        }
        
        
        
        if (bowEnemy)
        {
            while (Vector3.Distance(transform.position, player.position) < stoppingDistance &&
                   distanceMoved < moveDistance)
            {
                var deltaTime = Mathf.Clamp(Time.deltaTime, 0f, 0.002f);
                Vector3 targetPosition = transform.position + (transform.position - player.position).normalized * moveDistance;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * deltaTime);
                distanceMoved = Vector3.Distance(previousPosition, transform.position);
                yield return null;
            }
        }

        
        /*Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, objectCheckRange);
        if (hitObjects != null)
        {
            foreach (Collider2D obj in hitObjects)
            {
                if (obj.CompareTag("Enemy"))
                {
                    if (obj != myCollider2D)
                    {


                        var deltaTime = Mathf.Clamp(Time.deltaTime, 0f, 0.002f);

                        var goAway = new Vector3(0, 1f, 0); //todo нужно чтобы он отходил в противоположную сторону
                        //todo не работает плавное движение
                        //todo можно ли вычислить куда отходить из позиции задетого препядствия и места игрока
                        //- чтобы враг отходил на такое расстояние чтобы на следующий же ход не столкнуться опять
                        //transform.position = Vector3.MoveTowards(transform.position, transform.position+iamwalkinghere, moveSpeed * deltaTime);
                        transform.position = new Vector3(transform.position.x, transform.position.y+goAway.y);
                    }
                }
            }

            hitObjects = null;
        }*/

        
        
        
        
        
        
        isMoving = false;

        if (!bowEnemy)
        {
            if (Vector3.Distance(transform.position, player.position) <= stoppingDistance)
            {
                gonnaAttack = true;
                //  Debug.Log("attack player");
                // StartCoroutine(EnemyAttack());
            }
        }
        
        if (bowEnemy)
        {
            if (Vector3.Distance(transform.position, player.position) >= stoppingDistance && stoppingDistance < maxDistance)
            {
                gonnaAttack = true;

              //  Debug.Log("attack bow player");
               // StartCoroutine(EnemyAttack());
            }
        }
    }

    void Update()
    {
        

        // check if the player is to the left of the enemy and facing right
        if (playerObject.transform.position.x < transform.position.x && spriteRenderer.flipX == false)
        {
            // flip the enemy horizontally and update its facing direction
            spriteRenderer.flipX = true;
        }
        // check if the player is to the right of the enemy and facing left
        else if (playerObject.transform.position.x > transform.position.x && spriteRenderer.flipX == true)
        {
            // flip the enemy horizontally and update its facing direction
            spriteRenderer.flipX = false;
        
       

            // Only move if it's this enemy's turn
            if (enabled && !isMoving)
            {
                // Do the enemy's turn
            }
        }
    }

    public void Attack()
    {
        StartCoroutine(EnemyAttack());
    }
    public IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(1f);
        Vector2 direction = (playerObject.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(daggerPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * daggerSpeed;
        gonnaAttack = false;
    }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        healthBar.GetComponent<TextMeshPro>().text = hp.ToString(CultureInfo.InvariantCulture) + "/" +
                                                     maxHp.ToString(CultureInfo.InvariantCulture);
      
    }

    public void Die()
    {
        Destroy(gameObject);

    }
    
    void OnDrawGizmosSelected()
    {
      

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, objectCheckRange);
    }
   

}
