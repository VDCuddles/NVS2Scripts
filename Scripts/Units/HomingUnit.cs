using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingUnit : Homing
{
	[SerializeField] private float slowYfactor = 4;

	public override void Update()
    {
		//chase and turn if target, and target has a unit component
		if (GetTarget() && GetTarget().GetComponent<Unit>())
		{
			if (!GetTarget().GetComponent<Unit>().IsDead())
			{
				SlowYTurn(GetTurnSpeed(), GetTarget(), transform, slowYfactor);
			}

		}
		//updates target if no target (and this homing is an alive unit)
		else if (hasUnitComponent)
			ConfirmTarget(gameObject);
	}
}
