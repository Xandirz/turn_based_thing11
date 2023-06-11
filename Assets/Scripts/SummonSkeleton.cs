using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkeleton : MonoBehaviour
{
    
        public float attackRange;
    public PlayerScript playerScript;
    public GameObject summonObject;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && playerScript.canAttack)
        {
            Summon();
        }
    }

    void Summon()
    {
            Debug.Log("Summon");
            Instantiate(summonObject, transform.position, Quaternion.identity);
    }
    
    void OnDrawGizmosSelected()
    {
     

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
