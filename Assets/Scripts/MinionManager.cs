using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    public List <SkeletonScript> skeletons;
    public Transform player;
    public PlayerScript playerScript;
    public float agroRange = 5f;
    public float moveSpeed = 1.0f;
    public float stoppingDistance = 1.0f;
    public float waitTime = 1f;
    public int currentMinionIndex = 0;

    public EnemyManager enemyManager;

    void Start()
    {
        currentMinionIndex = 0;
        
    }
    private void Update()
    {
            FindActiveSkeletons();
            
            
            List<SkeletonScript> skeletonsToDie = new List<SkeletonScript>();

            foreach (SkeletonScript skeleton in skeletons)
            {
                if (skeleton.hp <= 0)
                {
                    skeletonsToDie.Add(skeleton);
                }
            }

            foreach (SkeletonScript skeletonToDie in skeletonsToDie)
            {

                skeletons.Remove(skeletonToDie);
                skeletonToDie.Die();
            }

    }

    public void FindActiveSkeletons()
    {
        Collider2D[] activeSkeletons = Physics2D.OverlapCircleAll(player.position, agroRange);
        if (activeSkeletons != null)
        {
            foreach (Collider2D obj in activeSkeletons)
            {
                if (obj.CompareTag("Skeleton") && obj!=null)
                {
                    SkeletonScript skeletonScript = obj.gameObject.GetComponent<SkeletonScript>();
                    if (skeletonScript != null && !skeletons.Contains(skeletonScript)) // Если враг имеет компонент EnemyScript и не находится в списке врагов в радиусе вокруг игрока
                    {
                        skeletons.Add(skeletonScript); // Добавляем врага в список
                    }
                }
            }

            activeSkeletons = null;
            
        }
        
    }
    
    
  
    
    public void NewTurn()
    {
        currentMinionIndex = 0;

        StartCoroutine(MinonTurns());
    }
    
    public IEnumerator MinonTurns()
    {
        Debug.Log("ХОД МИНЬОНОВ");

        if (skeletons!=null && skeletons.Count>0){
            Debug.Log("ЗДЕСЬ");

            while (currentMinionIndex < skeletons.Count)
            {
                SkeletonScript currentSkeleton = skeletons[currentMinionIndex];

                currentSkeleton.enabled = true;

                // Wait for the enemy to move
                yield return StartCoroutine(currentSkeleton.MoveToEnemy());

                currentSkeleton.enabled = false;

                // Increment to the next enemy
                yield return new WaitForSeconds(waitTime);

                if (currentSkeleton.gonnaAttack)
                {
                    currentSkeleton.Attack();
                
                    yield return new WaitForSeconds(waitTime);
                }

                currentMinionIndex++;
            

            }
            yield return new WaitForSeconds(1f);

            // Call myTurn function in PlayerScript after all enemies have moved
            enemyManager.NewTurn();
            Debug.Log("ХОД ВРАГА");

        }
        else
        {
            enemyManager.NewTurn();

        }
    }
    
    void OnDrawGizmosSelected()
    {
        //рисуем радиус для агро рендж
        if (player.position == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, agroRange);
    }

    }


