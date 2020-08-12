using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	private Animator animator;
	private Player player;
	private Vector3 originalPosition;
	public Vector3 GetGunSpawnLoc() { return originalPosition; }
	public Animator GetAnimator() { return animator; }
	private InputController ic;
	private PlayerMovement pm;

	void Start()
	{
		pm = FindObjectOfType<PlayerMovement>();
		ic = FindObjectOfType<InputController>();
		originalPosition = new Vector3(0, -0.427f, 0.63f);
		player = FindObjectOfType<Player>();
		animator = GetComponent<Animator>();
	}
	void Update()
	{
		//shooting animation logic
		if (Input.GetMouseButton(ic.LeftClick))
		{
			if (player.GetCurrentWeapon())
			{
				//single shot weapons
				if (player.GetCurrentWeapon().name == "Shotgun" || player.GetCurrentWeapon().name == "Crucifier")
				{
					if (GetComponentInChildren<PlayerWeapon>().GetReadyToFire())
					{
						animator.SetTrigger("SingleShot");
						animator.SetBool("IsShooting", false);
						animator.SetBool("IsMoving", false);
					}
					else if (GetComponentInChildren<Shotgun>())
					{
						if (GetComponentInChildren<Shotgun>().GetReadyToFire())
						{
							animator.SetTrigger("SingleShot");
							animator.SetBool("IsShooting", false);
							animator.SetBool("IsMoving", false);
						}
					}
				}
				//charge weapon fire
				else if (player.GetCurrentWeapon().name == "Reaver" || player.GetCurrentWeapon().name == "Devourer" || player.GetCurrentWeapon().name == "Void Cannon" )
				{
					if (GetComponentInChildren<ChargingWeapon>().isFiring)
					{
						animator.SetBool("IsShooting", true);
						animator.SetBool("IsMoving", false);
					}
					else
					{
						animator.SetBool("IsShooting", false);
						animator.SetBool("IsMoving", false);
					}
				}
				//all others
				else
				{
					animator.SetBool("IsShooting", true);
					animator.SetBool("IsMoving", false);
				}
			}
		}
		//moving animation
		else if ((Input.GetKey(ic.MoveUp)|| Input.GetKey(ic.MoveDown) || Input.GetKey(ic.MoveLeft) || Input.GetKey(ic.MoveRight)) && pm.GetIsGrounded())
		{
			animator.SetBool("IsMoving", true);
		}
		else if(!GetComponentInChildren<ChargingWeapon>())
		{
			animator.SetBool("IsShooting", false);
			animator.SetBool("IsMoving", false);
		}
		else
			animator.SetBool("IsMoving", false);

		//stop continuous shooting for any weapon except reaver
		if (Input.GetMouseButtonUp(ic.LeftClick))
		{
			if (!GetComponentInChildren<ChargingWeapon>())
			{
				animator.SetBool("IsShooting", false);
			}
		}
	}
	public void SpawnWeapon()
	{
		player.SpawnWeapon();
	}
	public void SwapDone()
	{
		player.SetIsSwapping(false);
		animator.SetTrigger("SwapDone");
	}
	public void StartCasting()
	{
		player.RaiseAndCastLHand();
	}
}
