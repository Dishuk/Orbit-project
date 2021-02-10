using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    public Action<Interactions, GameObject> InteractionAction = default;

    private void Start()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            InteractionAction?.Invoke(collision.GetComponent<InteractionUnit>().interactionType, collision.gameObject);
        }
    }
}
