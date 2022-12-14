using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Jump Stuff")]
	[Space]

	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] private float jumpGravity;
	[SerializeField] private float fallGravity;
	private bool isJumping;

	[Header("Wall Jump and Wall Slide")]
	[Space]

	[SerializeField] Vector2 wallJumpForce;
	[SerializeField] float wallJumpTime = 0.5f;

	[SerializeField] private float wallCheckRayLength = 0.5f;
	private bool isTouchingWallRight;
	private bool isTouchingWallLeft;

	//private RaycastHit2D[] wallHitLeft;
	private RaycastHit2D[] wallHitRight;

	private bool wallSliding;
	[SerializeField] Transform wallRayPosRight;
	//[SerializeField] Transform wallRayPosLeft;
	[SerializeField] float wallSlideVelocity = 1f;

	[Header("Audio Stuff")]
	[Space]

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] public AudioClip groundedSoundEffect;
    [SerializeField] private AudioClip wallJumpingSoundEffect;
    AudioSource audioSource;

    [Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
        audioSource = GetComponent<AudioSource>();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		if(m_Rigidbody2D.velocity.y < 0)
        {
			m_Rigidbody2D.gravityScale = fallGravity;
        }

		else if (m_Rigidbody2D.velocity.y > 0 && !isJumping)
		{
			m_Rigidbody2D.gravityScale= jumpGravity;
		}

        else
        {
			m_Rigidbody2D.gravityScale = 1;
        }



		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
				{
					audioSource.PlayOneShot(groundedSoundEffect);
                    OnLandEvent.Invoke();
				}
			}
		}
	}


	public void Move(float move, bool crouch, bool jump)
	{
		isJumping = jump;

		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}


        //wallHitLeft = Physics2D.RaycastAll(wallRayPosLeft.position, -wallRayPosLeft.transform.right, wallCheckRayLength);
        wallHitRight = Physics2D.RaycastAll(wallRayPosRight.position, Vector2.right * Mathf.Sign(transform.localScale.x), wallCheckRayLength);

        //isTouchingWallLeft = rayCheck(wallHitLeft);
        isTouchingWallRight = rayCheck(wallHitRight);

        //draws rays for walldetection
        drawRays();

        //WALL JUMPING
        if (jump && isTouchingWallRight && !m_Grounded)
        {
            audioSource.PlayOneShot(wallJumpingSoundEffect);
            Debug.Log("wallJumpRight");
            StartCoroutine(wallJumpCoolDown());
            m_Rigidbody2D.velocity = new Vector2(wallJumpForce.x * -Mathf.Sign(transform.localScale.x), wallJumpForce.y);

        }
        //END



        // If the player should jump...
        if (m_Grounded && jump)
		{
			jumpSoundEffect.Play();
			// Add a vertical force to the player.
			Debug.Log("jump");
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			isJumping = false;
		}

		//Wall Sliding

		if ((isTouchingWallRight || isTouchingWallLeft ) && !m_Grounded && move != 0)
		{
			wallSliding = true;
        }
        else
        {
			wallSliding = false;
        }

        if (wallSliding)
        {
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, -wallSlideVelocity, float.MaxValue));
        }
		
	}


	IEnumerator wallJumpCoolDown()
	{
		m_MovementSmoothing = 0.3f;
		yield return new WaitForSeconds(wallJumpTime);
		m_MovementSmoothing = 0;
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


	bool rayCheck(RaycastHit2D[] rays)
	{
		foreach(RaycastHit2D hit in rays)
		{
			if(hit.collider != null)
			{
				if(hit.collider.tag != "Player")
				{
					return true;
				}
			}
		}
		return false;
	
	}

	void drawRays()
	{
		//Debug.DrawRay(wallRayPosLeft.position, Vector2.left * wallCheckRayLength, Color.green);
		Debug.DrawRay(wallRayPosRight.position, Vector2.right * Mathf.Sign(transform.localScale.x) * wallCheckRayLength, Color.green);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
	}

}