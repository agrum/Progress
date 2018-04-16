using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace West
{
	public class ConstellationNodeSelectable : Selectable
	{
		public delegate void OnSelectedDelegate();
		public event OnSelectedDelegate selectedEvent;

		private readonly int enterHash = Animator.StringToHash("Enter");
		private readonly int leaveHash = Animator.StringToHash("Leave");
		private readonly int clickHash = Animator.StringToHash("Click");
		private int baseLayerIndex;

		private Animator animator_;
		private int nextTrigger = 0;

		override public void OnPointerEnter(PointerEventData eventData)
		{
			ScheduleTrigger(enterHash);
		}

		override public void OnPointerExit(PointerEventData eventData)
		{
			ScheduleTrigger(leaveHash);
		}

		override public void OnPointerUp(PointerEventData eventData)
		{
			ScheduleTrigger(clickHash);
		}

		override protected void Start()
		{
			animator_ = GetComponent<Animator>();
			baseLayerIndex = animator_.GetLayerIndex("Base Layer");

			base.Start();
		}

		public void Update()
		{
			if(nextTrigger != 0 && !animator_.IsInTransition(baseLayerIndex))
			{
				animator_.SetTrigger(nextTrigger);
				nextTrigger = 0;
			}
		}

		private void ScheduleTrigger(int hash)
		{
			//if (animator_.IsInTransition(baseLayerIndex))
				nextTrigger = hash;
			//else
			//	animator.SetTrigger(clickHash);
		}
	}
}