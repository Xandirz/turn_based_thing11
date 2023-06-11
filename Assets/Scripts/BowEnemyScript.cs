using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowEnemyScript : MonoBehaviour
{
  public float moveSpeed = 1.0f;
    public float stoppingDistance = 5f;
    public float distanceMoved;
    public float moveDistance = 1f;
    private Vector3 previousPosition;
    private SpriteRenderer spriteRenderer;
    public float hp = 2;
    private bool isMoving = false;
    public GameObject playerObject;
    public GameObject daggerPrefab;
    public float daggerSpeed = 5f;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }


    private void OnMouseOver()
    {
        spriteRenderer.color = Color.red;
    }

    private void OnMouseExit()
    {
        //делаю цвет как у белого квадрата которомуро в едите поставил цвет

        spriteRenderer.color =  new Color(217f/255f, 79f/255f, 79f/255f); 
    }

    public IEnumerator MoveToPlayer(Transform player)
    {
        distanceMoved = 0;
        previousPosition = transform.position;

        isMoving = true;

        while (Vector3.Distance(transform.position, player.position) > stoppingDistance && distanceMoved < moveDistance)
        {
            var deltaTime = Mathf.Clamp(Time.deltaTime, 0f, 0.002f);
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * deltaTime);
            distanceMoved = Vector3.Distance(previousPosition, transform.position);
            yield return null;
        }
        
        while (Vector3.Distance(transform.position, player.position) < stoppingDistance && distanceMoved < moveDistance)
        {
            var deltaTime = Mathf.Clamp(Time.deltaTime, 0f, 0.002f);
            transform.position = Vector3.MoveTowards(transform.position, -player.position, moveSpeed * deltaTime);
            distanceMoved = Vector3.Distance(previousPosition, transform.position);
            yield return null;
        }

        isMoving = false;

        if (Vector3.Distance(transform.position, player.position) == stoppingDistance)
        {
            Debug.Log("attack player");
            StartCoroutine(EnemyAttack());
        }
    }

    void Update()
    {
        // Only move if it's this enemy's turn
        if (enabled && !isMoving)
        {
            // Do the enemy's turn
        }
    }

    public IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(1f);
        Vector2 direction = (playerObject.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(daggerPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * daggerSpeed;
    }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
      
    }

    public void Die()
    {
        Destroy(gameObject);

    }
}
