using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationIdleSMB : StateMachineBehaviour {
	public float crouchTransitionTime = 0.5f;

	private readonly int m_differenceFacingWalkingHash = Animator.StringToHash("differenceFacingWalking");
	private readonly int m_isWalkingHash = Animator.StringToHash("isWalking");
	private readonly int m_walkingSpeedHash = Animator.StringToHash("walkingSpeed");
	private readonly int m_crouchHash = Animator.StringToHash("crouch");
	private int m_crouchingLayerIndex = -1;

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(m_crouchingLayerIndex == -1)
			m_crouchingLayerIndex = animator.GetLayerIndex("Crouching");

		Player player = animator.GetComponent<Player>();
		float angle = (player.facingDirection - player.walkingDirection) % 360.0f;
		if (angle < 0.0f)
			angle += 360.0f;
		animator.SetFloat(m_differenceFacingWalkingHash, angle);
		animator.SetBool(m_isWalkingHash, player.hasDestination);
		animator.SetFloat(m_walkingSpeedHash, player.GetSpeed());
		animator.SetFloat(m_crouchHash, player.crouch, crouchTransitionTime, Time.deltaTime);
		animator.SetLayerWeight(m_crouchingLayerIndex, animator.GetFloat(m_crouchHash));
	}
}
