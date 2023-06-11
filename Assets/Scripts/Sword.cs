using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange;
    public int attackDamage;
    public PlayerScript playerScript;

    public float rotationSpeed = 10f; 
    private Transform parentTransform;
    
    
    private void Start()
    {
        parentTransform = transform.parent;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && playerScript.canAttack && playerScript._swordCooldownService.cooldown < 1)
        {
            Attack();
            Debug.Log("ATTACK");
        }
        
        if (Input.GetKey(KeyCode.R))
        {
            RotateTowardsMouse();
            // moveRadius.SetActive(false);
        }
    }

    void Attack()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        if (hitObjects != null)
        {
            foreach (Collider2D obj in hitObjects)
            {
                if (obj.CompareTag("Enemy"))
                {
                    obj.GetComponent<EnemyScript>().TakeDamage(attackDamage);
                    playerScript.GetCooldown("sword");

                    playerScript.canAttack = false;

                }
            }

            hitObjects = null;
            
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Set a distance from the camera
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = mouseWorldPosition - parentTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        parentTransform.rotation = Quaternion.Slerp(parentTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
 
}
