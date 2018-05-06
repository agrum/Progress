using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleJSON;

namespace West
{
	namespace View
	{
		public class ConstellationNode : Selectable
		{
			public enum State
			{
				Unselectable,
				Selectable,
				Selected
			}

			public delegate void OnSelectedDelegate(ConstellationNode node, bool selected);
			public event OnSelectedDelegate selectedEvent;

			protected Model.ConstellationNode model;
			protected Material mat;
			protected Vector2 position = new Vector2();
			protected Vector2 positionMultiplier = new Vector2();
			protected float scale;

			private readonly int enterHash = Animator.StringToHash("Enter");
			private readonly int leaveHash = Animator.StringToHash("Leave");
			private readonly int clickInHash = Animator.StringToHash("ClickIn");
			private readonly int clickOutHash = Animator.StringToHash("ClickOut");
			private readonly int isUnselectableHash = Animator.StringToHash("IsUnselectable");
			private readonly int isSelectableHash = Animator.StringToHash("IsSelectable");
			private bool started = false;
			
			private State state = State.Unselectable;
			private State preStartedState = State.Unselectable;

			public void Setup(Model.ConstellationNode model_, Material mat_, Vector2 position_)
			{
				positionMultiplier.x = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
				positionMultiplier.y = 0.75f;
				
				mat = mat_;
				scale = 1.0f;
				position = position_;
				positionMultiplier.x = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
				positionMultiplier.y = 0.75f;

				Scale(scale);

				Transform childTranform = gameObject.transform.Find("GameObject");
				Image stroke = childTranform.Find("HexagonStroke").GetComponent<Image>();
				Image pulse = childTranform.Find("Pulse").GetComponent<Image>();
				Image fill = childTranform.Find("HexagonFill").GetComponent<Image>();
				Image icon = childTranform.Find("Icon").GetComponent<Image>();

				stroke.material = mat;
				pulse.material = mat;
				fill.material = mat;
				icon.material = mat;

				Setup(model_);
			}

			public void Setup(Model.ConstellationNode model_)
			{
				model = model_;

				Transform childTranform = gameObject.transform.Find("GameObject");
				Image pulse = childTranform.Find("Pulse").GetComponent<Image>();
				Image icon = childTranform.Find("Icon").GetComponent<Image>();
				Image iconWhite = childTranform.Find("White").Find("Icon").GetComponent<Image>();

				if (model != null && model.Json != null)
				{
					string path = "Icons/" + model.UpperCamelCaseKey + "/" + model.Json["name"];
					UnityEngine.Object prefabObject = Resources.Load(path);
					Texture2D texture = prefabObject as Texture2D;
					Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1.0f);

					pulse.gameObject.SetActive(true);
					icon.gameObject.SetActive(true);
					iconWhite.gameObject.SetActive(true);
					icon.overrideSprite = sprite;
					iconWhite.overrideSprite = sprite;
				}
				else
				{
					SelectableState = State.Unselectable;
					pulse.gameObject.SetActive(false);
					icon.gameObject.SetActive(false);
					iconWhite.gameObject.SetActive(false);
				}
			}

			public virtual void Scale(float scale_)
			{
				scale = scale_;
				gameObject.transform.localPosition = new Vector3(position.x * positionMultiplier.x, position.y * positionMultiplier.y, 0) * scale; ;
				gameObject.transform.localScale = Vector3.one * scale;
				gameObject.transform.localRotation = Quaternion.identity;
			}

			override protected void Start()
			{
				started = true;

				SelectableState = preStartedState;

				interactable = true;
				transition = Transition.None;

				base.Start();
			}

			public Model.ConstellationNode Model { get { return model; } }

			public State SelectableState
			{
				get
				{
					return state;
				}
				set
				{
					if (!started)
					{
						preStartedState = value;
						return;
					}

					animator.ResetTrigger(isUnselectableHash);
					animator.ResetTrigger(isSelectableHash);
					animator.ResetTrigger(clickInHash);
					animator.ResetTrigger(clickOutHash);

					var oldState = state;
					state = value;
					if (state == State.Unselectable)
					{
						animator.SetTrigger(isUnselectableHash);
					}
					else
					{
						if (oldState != State.Selected)
							animator.SetTrigger(isSelectableHash);
						if (state == State.Selected || oldState == State.Selected)
						{
							animator.SetTrigger((state == State.Selected) ? clickInHash : clickOutHash);
						}
					}
				}
			}

			override public void OnPointerEnter(PointerEventData eventData)
			{
				animator.ResetTrigger(leaveHash);
				animator.SetTrigger(enterHash);
			}

			override public void OnPointerExit(PointerEventData eventData)
			{
				animator.ResetTrigger(enterHash);
				animator.SetTrigger(leaveHash);
			}

			override public void OnPointerUp(PointerEventData eventData)
			{
				if (SelectableState != State.Unselectable)
					selectedEvent(this, SelectableState == State.Selectable);
			}
		}
	}
}