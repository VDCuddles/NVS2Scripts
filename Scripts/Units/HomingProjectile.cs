using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Homing
{
	public override void Update()
	{
		if (GetTarget())
			TurnSelf(GetTurnSpeed(), GetTarget(), transform);
		ChaseForward(GetSpeed());
		if (hasBulletComponent)
			if (GetComponent<Bullet>().GetParent())
				if (GetComponent<Bullet>().GetParent().GetComponent<BasicAI>())
					SetTarget(GetComponent<Bullet>().GetParent().GetComponent<BasicAI>().GetTarget());

	}

	public override void ChaseForward(float speed)
	{
		transform.position += transform.forward * Time.deltaTime * speed;
	}

}
