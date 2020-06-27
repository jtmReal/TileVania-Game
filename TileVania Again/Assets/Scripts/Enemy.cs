using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PolygonCollider2D myEnemyCollider;
    // Start is called before the first frame update
    void Start()
    {
        myEnemyCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DeadEnemy();
    }

    private void DeadEnemy()
    {
        if (myEnemyCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            Destroy(gameObject);
        }
    }
}
