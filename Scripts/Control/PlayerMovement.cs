using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private CharacterController cc;
	private InputController ic;
	private SoundLibrary sl;
	private AudioSource aso;
	private AudioClip jump;
	private AudioClip walk;
	private bool isWalkPlaying = false;

	[SerializeField] private float walkSpeed = default;
	[SerializeField] private float jumpHeight = default;
	public float GetWalkSpeed() { return walkSpeed; }

	[SerializeField] private Transform groundCheck = default;
	[SerializeField] private float groundDistance = default;
	[SerializeField] private float smoothFallFactor = default;
	[SerializeField] private LayerMask groundMask = default;
	private bool isGrounded;
	private bool movementDisabled = false;
	public bool GetIsGrounded() { return isGrounded; }
	public void SetMDisabled(bool value) { movementDisabled = value; }

	[SerializeField] private float gravityFactor = default;
	public float GetGravityFactor() { return gravityFactor; }
	Vector3 velocity;
	public void SetVelocity(Vector3 value) { velocity = value; }

	void Start()
	{
		aso = GetComponent<AudioSource>();
		sl = FindObjectOfType<SoundLibrary>();
		jump = sl.GetPlayerJump();
		walk = sl.GetPlayerStep();
		gravityFactor *= -9.81f;
		cc = GetComponentInChildren<CharacterController>();
		ic = FindObjectOfType<InputController>();
	}

	// Update is called once per frame
	void Update()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
		if (isGrounded && velocity.y < 0)
		{
			velocity.y = smoothFallFactor;
			velocity.x = 0;
			velocity.z = 0;
			movementDisabled = false;
		}
		if (!gameObject.GetComponent<Unit>().IsDead() && cc)
		{
			float x = Input.GetAxis("Horizontal");
			float z = Input.GetAxis("Vertical");
			Vector3 movement = transform.right * x + transform.forward * z;
			if (!movementDisabled)
				cc.Move(movement * walkSpeed * Time.deltaTime);
			if (Input.GetKeyDown(ic.Jump) && isGrounded && !movementDisabled)
			{
				velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityFactor);
				aso.PlayOneShot(jump);
			}
			velocity.y += gravityFactor * Time.deltaTime;
			cc.Move(velocity * Time.deltaTime);
			if ((Input.GetKey(ic.MoveUp) || Input.GetKey(ic.MoveDown) || Input.GetKey(ic.MoveLeft) || Input.GetKey(ic.MoveRight)) && isGrounded && !isWalkPlaying)
				StartCoroutine("playWalk");
		}
	}

	IEnumerator playWalk()
	{
		isWalkPlaying = true;
		aso.PlayOneShot(walk);
		yield return new WaitForSeconds(walk.length);
		isWalkPlaying = false;
	}
}
