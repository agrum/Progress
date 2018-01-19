using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationIdleSMB : StateMachineBehaviour {
	private readonly int m_differenceFacingWalkingHash = Animator.StringToHash("differenceFacingWalking");
	private readonly int m_isWalkingHash = Animator.StringToHash("isWalking");

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Player player = animator.GetComponent<Player>();
		float angle = (player.facingDirection - player.walkingDirection) % 360.0f;
		if (angle < 0.0f)
			angle += 360.0f;
		animator.SetFloat(m_differenceFacingWalkingHash, angle);//, 10.0f, Time.deltaTime);
		animator.SetBool(m_isWalkingHash, player.hasDestination);
	}
}
