using UnityEngine;

public class Homing : MonoBehaviour
{
	private Transform target;
	public bool hasUnitComponent { get; set; }
	public bool hasBulletComponent { get; set; }
	[SerializeField] private Transform defaulttarget = default;
	public void SetTarget(Transform value) { target = value; }
	public Transform GetTarget() { return target; }
	// Angular speed in radians per sec.
	[SerializeField] private float turnspeed = 1.3f;
	[SerializeField] private float stoppingdistance = 0;

	[SerializeField] private float speed = 100f;
	public float GetSpeed() { return speed; }
	public void SetSpeed(float value) { speed = value; }

	public float GetTurnSpeed() { return turnspeed; }
	public void SetTurnSpeed(float value) { turnspeed = value; }
	public void SetDefaultTarget(Transform value) { defaulttarget = value; }
	public Transform GetDefaultTarget() { return defaulttarget; }

	private void Start()
	{
		hasBulletComponent = GetComponent<Bullet>();
		hasUnitComponent = GetComponent<Unit>();

		if (GetComponent<BasicAI>())
			stoppingdistance = GetComponent<BasicAI>().GetStoppingDistance();
		if (!defaulttarget)
			target = GameObject.Find("Player").transform;
		else
			target = defaulttarget;


		if (GetComponent<Bullet>())
			if (GetComponent<Bullet>().GetParent())
				if (GetComponent<Bullet>().GetParent().GetComponent<BasicAI>())
					target = GetComponent<Bullet>().GetParent().GetComponent<BasicAI>().GetTarget();

	}

	public virtual void Update()
	{
		ConfirmTarget(gameObject);
	}

	public void ConfirmTarget(GameObject thisUnit)
	{
		if (!target)
		{
			target = Unit.GetClosestEnemy(thisUnit);
		}
		else if (target.GetComponent<Unit>() && target.GetComponent<Unit>().IsDead())
		{
			target = Unit.GetClosestEnemy(thisUnit);
		}
		else if (defaulttarget)
			target = defaulttarget;
		else
			target = GameObject.Find("Player").transform;
	}

	public static void TurnSelf(float turnspeed, Transform target, Transform thisUnit)
	{
		bool hasCollider = target.GetComponentInChildren<Collider>();
		Vector3 targetDirection = (hasCollider ? target.GetComponentInChildren<Collider>().bounds.center : target.position) - thisUnit.position;
		DoTurn(turnspeed, thisUnit, targetDirection);
	}

	public static void TurnSelf(float turnspeed, Vector3 direction, Transform thisUnit)
	{
		DoTurn(turnspeed, thisUnit, direction);
	}

	public static void SlowYTurn(float turnspeed, Transform target, Transform thisUnit, float slowfactor)
	{
		bool hasCollider = target.GetComponentInChildren<Collider>();
		Vector3 targetDirection = (hasCollider ? target.GetComponentInChildren<Collider>().bounds.center : target.position) - thisUnit.position;
		targetDirection.y /= slowfactor;
		DoTurn(turnspeed, thisUnit, targetDirection);
	}

	public static void DoTurn(float turnspeed, Transform thisUnit, Vector3 direction)
	{
		float singleStep = turnspeed * Time.deltaTime;
		Vector3 newDirection = Vector3.RotateTowards(thisUnit.forward, direction, singleStep, 0.0f);
		Debug.DrawRay(thisUnit.position, newDirection, Color.red);
		thisUnit.rotation = Quaternion.LookRotation(newDirection);
	}

	public virtual void ChaseForward(float speed)
	{
		if (target)
			if (Vector3.Distance(transform.position, target.position) > stoppingdistance)
				transform.position += transform.forward * Time.deltaTime * speed;
	}
}