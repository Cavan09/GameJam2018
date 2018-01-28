using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    [SerializeField] private float m_MaxSpeed = 10f;
    private bool m_FacingRight = true;

    Animator m_Anim { get; set; }

	// Use this for initialization
	void Start () {
        m_Anim = gameObject.GetComponent<Animator>();
        if (m_Anim == null)
            m_Anim = gameObject.AddComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        m_Anim.SetBool("Ground", true);
    }

    public void MoveVertical(Rigidbody2D body, float vertical)
    {
        body.velocity = new Vector2(body.velocity.x, vertical * m_MaxSpeed);
    }

    public void MoveHorizontal(Rigidbody2D body, float horizontal, float ViewX)
    {
        m_Anim.SetFloat("Speed", Mathf.Abs(horizontal));

        body.velocity = new Vector2(horizontal * m_MaxSpeed, body.velocity.y);

        if (ViewX != 0)
        {
            // If the input is moving the player right and the player is facing left...
            if (ViewX > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (ViewX < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        else
        {
            if (horizontal > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (horizontal < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
