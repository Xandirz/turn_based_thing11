using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowCursor : MonoBehaviour
{
    public GameObject objectToFollow;
    public SpriteRenderer spriteRenderer;
    public KeyCode key;

    public float agroRadius = 1;
    // Update is called once per frame
    void Update()
    {
        var isActive = objectToFollow.activeSelf;
        if (Input.GetKeyDown(key))
        {
            isActive = !isActive;
            objectToFollow.SetActive(isActive);
        }

        if (isActive)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objectToFollow.transform.position =
                new Vector3(mousePosition.x, mousePosition.y, objectToFollow.transform.position.z);

            //spriteRenderer.color = Color.white;
            //todo почитать про церкл каст - мб проблема в vector2 zero
            // if (Physics2D.CircleCast(mousePosition, agroRadius, Vector2.zero, LayerMask.NameToLayer("Enemy")))
            //{
             //   spriteRenderer.color = Color.red;
            //}
        }
    }


}
