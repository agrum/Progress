using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class Player : MonoBehaviour
	{
		public enum AbilityState
		{
			None,
			Aiming,
			Casting,
			Channeling
		}

		//public Data.Skill.Skill abilityQ;
		//public Data.Skill.Skill abilityW;
		//public Data.Skill.Skill abilityE;
		//public Data.Skill.Skill abilityR;
		public float speedBase;
		public Keybinding keybinding;
		public float hitBoxRadius;
        
		internal float crouch = 0.0f;
		internal float facingDirection = 0.0f;
		internal float walkingDirection = 0.0f;
		internal bool hasDestination = false;

		private AbilityState state = AbilityState.None;
		private Vector3 destination;
		private Vector3 direction;
		private float sqrMaxSpeed;
		//private Data.Skill.Skill activeAbility = null;
		private Terrain terrain;
		[HideInInspector]
		public Rigidbody2D rigbody2D;
		[HideInInspector]
		public CircleCollider2D col2D;

		internal AbilityState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
				if (value == AbilityState.Casting)
					hasDestination = false;
			}
		}

		public void Move()
		{
			if (state != AbilityState.Casting)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				int layerMask = 1 << LayerMask.NameToLayer("Terrain");
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100, layerMask))
				{
					destination = hit.point;
					direction = destination - transform.position;
					direction.Normalize();
					hasDestination = true;
				}
			}
			if (hasDestination)
			{
				float localFacingDirection = Vector3.Angle(new Vector3(0.0f, 0.0f, 1.0f), direction);
				float isRight = Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), direction);
				if (isRight < 0.0f)
					localFacingDirection *= -1.0f;
				walkingDirection = localFacingDirection;
				if (state != AbilityState.Aiming)
					facingDirection = localFacingDirection;
			}
		}

		public float GetSpeed()
		{
            return speedBase;// * speed.amount;
		}

		void Start()
		{
			//abilityQ.keybind = keybinding.q;
			//abilityW.keybind = keybinding.w;
			//abilityE.keybind = keybinding.e;
			//abilityR.keybind = keybinding.q;
			//abilityQ.player = this;
			//abilityW.player = this;
			//abilityE.player = this;
			//abilityR.player = this;

			state = AbilityState.None;

			terrain = GameObject.Find("Terrain").GetComponent<Terrain>();

			Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("TerrainRoughCollider"));
			Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("VisionCollider"));

			var pathCollidersContainer = TerrainManager.Instance.ColliderContainer;
			GameObject circleColliderGO = new GameObject("CircleCollider");
			rigbody2D = circleColliderGO.AddComponent<Rigidbody2D>();
			rigbody2D.transform.position = new Vector3(transform.position.x, transform.position.z, 0);
			col2D = circleColliderGO.AddComponent<CircleCollider2D>();
			col2D.radius = hitBoxRadius;
			circleColliderGO.transform.parent = pathCollidersContainer.transform;
			circleColliderGO.layer = LayerMask.NameToLayer("Player");
		}

		void Update()
		{
			UpdatePosition();
			TakeKeyboardInput();
		}

		void UpdatePosition()
		{
			if (hasDestination && Vector3.Dot(direction, destination - transform.position) < 0.0f)
			{
				hasDestination = false;
				rigbody2D.velocity = Vector2.zero;
			}
			transform.eulerAngles = new Vector3(0, facingDirection, 0);
			if (hasDestination)
			{
				rigbody2D.velocity = new Vector2(direction.x, direction.z) * GetSpeed();
			}
			transform.position = new Vector3(rigbody2D.transform.position.x, 0, rigbody2D.transform.position.y);
			transform.position = new Vector3(transform.position.x, terrain.SampleHeight(transform.position), transform.position.z);
		}

		void TakeKeyboardInput()
		{
			//Ability newActiveAbility = null;
			//if (Input.GetKeyDown(keybinding.q))
			//	newActiveAbility = abilityQ;
			//else if (Input.GetKeyDown(keybinding.w))
			//	newActiveAbility = abilityW;
			//else if (Input.GetKeyDown(keybinding.e))
			//	newActiveAbility = abilityE;
			//else if (Input.GetKeyDown(keybinding.r))
			//	newActiveAbility = abilityR;

			//if (state == AbilityState.None)
			//	activeAbility = null;
			//if (newActiveAbility && newActiveAbility.CanActivate() && state != AbilityState.Casting)
			//{
			//	if (activeAbility && activeAbility != newActiveAbility && state != AbilityState.Aiming)
			//		activeAbility.Cancel();
			//	activeAbility = newActiveAbility;
			//	activeAbility.Activate();
			//}
		}
	}
}