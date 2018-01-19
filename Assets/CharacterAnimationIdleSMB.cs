using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationIdleSMB : StateMachineBehaviour {
	private readonly int m_differenceFacingWalkingHash = Animator.StringToHash("differenceFacingWalking");
	private readonly int m_isWalkingHash = Animator.StringToHash("isWalking");

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Player player = animator.GetComponent<Player>();
		animator.SetFloat(m_differenceFacingWalkingHash, player.facingDirection - player.walkingDirection, 0.5f, Time.deltaTime);
		animator.SetBool(m_isWalkingHash, player.hasDestination);
	}
}
