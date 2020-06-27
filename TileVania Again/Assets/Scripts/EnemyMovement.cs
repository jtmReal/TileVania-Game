using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);//moves right
        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);//moves left
        }

    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;//So if its facing right the x has to be greater than 0 making this bool true
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);//forces flipping of enemy whenever collides with something
    }
}
