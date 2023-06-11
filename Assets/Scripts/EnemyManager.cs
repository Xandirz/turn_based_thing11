using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public List <EnemyScript> enemies;
    public Transform player;
    public PlayerScript playerScript;
    public float agroRange =1f;
    public float moveSpeed = 1.0f;
    public float stoppingDistance = 1.0f;
    public float waitTime = 1f;
    private int currentEnemyIndex;
    //спавн
    public GameObject enemyPrefab;
    public GameObject enemySpawnArea;
    public int spawnFrequency = 2; //на какое число ходов делить чтобы спаунилось

    public bool HasEnemies()
    {
        return enemies.Count > 0;
    }

    void Start()
    {
        currentEnemyIndex = 0;
        StartCoroutine(EnemyTurns());
        
    }

    private void Update()
    {

        List<EnemyScript> enemiesToDie = new List<EnemyScript>();

        foreach (EnemyScript enemy in enemies)
        {
            if (enemy.hp <= 0)
            {
                enemiesToDie.Add(enemy);
            }
        }

        foreach (EnemyScript enemyToDie in enemiesToDie)
        {

            enemies.Remove(enemyToDie);
            enemyToDie.Die();
        }

        FindAgroedEnemies();

        
      
    }

    public void FindAgroedEnemies()
    {
        Collider2D[] agroEnemies = Physics2D.OverlapCircleAll(player.position, agroRange);
        if (agroEnemies != null)
        {
            foreach (Collider2D obj in agroEnemies)
            {
                if (obj.CompareTag("Enemy") && obj!=null)
                {
                    EnemyScript enemyScript = obj.gameObject.GetComponent<EnemyScript>();
                    if (enemyScript != null && !enemies.Contains(enemyScript)) // Если враг имеет компонент EnemyScript и не находится в списке врагов в радиусе вокруг игрока
                    {
                        enemies.Add(enemyScript); // Добавляем врага в список
                    }
                }
            }

            agroEnemies = null;
            
        }
        
    }

    public void SpawnEnemy()
    { 
        if (playerScript.turnsPassed % spawnFrequency == 0)
        {        
            var randPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), 0);
            Vector3 spawnPosition = enemySpawnArea.transform.position;
            spawnPosition += randPosition;
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
        
    
    
    public void NewTurn()
    {
        currentEnemyIndex = 0;

        StartCoroutine(EnemyTurns());
    }

    

    public IEnumerator EnemyTurns()
    {
        Debug.Log("ХОД ВРАГА");
        
        if (enemies!=null && enemies.Count>0){
        
        while (currentEnemyIndex < enemies.Count)
        {
            EnemyScript currentEnemy = enemies[currentEnemyIndex];

            currentEnemy.enabled = true;

            // Wait for the enemy to move
            yield return StartCoroutine(currentEnemy.MoveToPlayer(player));

            currentEnemy.enabled = false;

            // Increment to the next enemy
            yield return new WaitForSeconds(waitTime);

            if (currentEnemy.gonnaAttack)
            {
                currentEnemy.Attack();
                
                yield return new WaitForSeconds(waitTime);
            }

            currentEnemyIndex++;
            

        }

        // Call myTurn function in PlayerScript after all enemies have moved
        yield return new WaitForSeconds(2f);

        playerScript.myTurn();
        Debug.Log("ХОД ИГРОКА");

     } 
        else {
            yield return new WaitForSeconds(2f);

            playerScript.myTurn();
            
        }

      
    }


    public GameObject GetClosestEnemy(Vector2 pos)
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (EnemyScript enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(pos, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.gameObject;
            }
        }
        // set the closest enemy as the target
        if (closestEnemy != null)
        {
            // do something with closestEnemy
        }

        return closestEnemy;
    }



    void OnDrawGizmosSelected()
    {
        //рисуем радиус для агро рендж
        if (player.position == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, agroRange);
    }

}