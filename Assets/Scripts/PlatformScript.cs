using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public static PlatformScript Instance { get; set; }
    public float jumpForce = 10f;
    public bool IsMegaJump = false;


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (SaveManager.Instance.state.activeUpgrade != 4)
        {
            return;
        }
        else
        {
            IsMegaJump = true;
        }

    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {

            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (IsMegaJump)
                {
                    Vector2 velocity = rb.velocity;
                    velocity.y = Random.Range(15f,25f);
                    rb.velocity = velocity;
                }
                else
                {
                    Vector2 velocity = rb.velocity;
                    velocity.y = jumpForce;
                    rb.velocity = velocity;
                }
            }
        }
    }
}
