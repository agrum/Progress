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
			public delegate void OnSelectedDelegate(ConstellationNode node, bool selected);
			public event OnSelectedDelegate selectedEvent;

			private Model.ConstellationNode model;
			private Material mat;
			private Vector2 positionMultiplier = new Vector2();
			private float scale;

			private readonly int enterHash = Animator.StringToHash("Enter");
			private readonly int leaveHash = Animator.StringToHash("Leave");
			private readonly int clickInHash = Animator.StringToHash("ClickIn");
			private readonly int clickOutHash = Animator.StringToHash("ClickOut");
			private readonly int isUnselectableHash = Animator.StringToHash("IsUnselectable");
			private readonly int isSelectableHash = Animator.StringToHash("IsSelectable");
			private int selectLayerIndex;
			private bool preStartedSelectableNode = false;
			private bool started = false;
			
			private bool selected = false;
			private bool isSelectable = false;

			public ConstellationNode()
			{
				positionMultiplier.x = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
				positionMultiplier.y = 0.75f;
			}

			public void Setup(Model.ConstellationNode model_, Material mat_)
			{
				model = model_;
				mat = mat_;
				scale = 1.0f;
				
				Scale(scale);

				Transform childTranform = gameObject.transform.Find("GameObject");
				Image stroke = childTranform.Find("HexagonStroke").GetComponent<Image>();
				Image pulse = childTranform.Find("Pulse").GetComponent<Image>();
				Image fill = childTranform.Find("HexagonFill").GetComponent<Image>();
				Image icon = childTranform.Find("Icon").GetComponent<Image>();
				Image iconWhite = childTranform.Find("White").Find("Icon").GetComponent<Image>();

				stroke.material = mat;
				pulse.material = mat;
				fill.material = mat;
				icon.material = mat;

				if (model.Json != null)
				{
					string path = "Icons/" + model.UpperCamelCaseKey + "/" + model.Json["name"];
					UnityEngine.Object prefabObject = Resources.Load(path);
					Texture2D texture = prefabObject as Texture2D;
					Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1.0f);

					icon.overrideSprite = sprite;
					iconWhite.overrideSprite = sprite;
				}
				else
				{
					icon.overrideSprite = null;
					iconWhite.overrideSprite = null;
				}
			}

			public void Scale(float scale_)
			{
				scale = scale_;
				gameObject.transform.localPosition = new Vector3(model.Position.x * positionMultiplier.x, model.Position.y * positionMultiplier.y, 0) * scale; ;
				gameObject.transform.localScale = Vector3.one * scale;
				gameObject.transform.localRotation = Quaternion.identity;
			}

			override protected void Start()
			{
				started = true;

				SelectableNode = preStartedSelectableNode;

				base.Start();
			}

			public Model.ConstellationNode Model { get { return model; } }

			public bool SelectableNode
			{
				get
				{
					return isSelectable;
				}
				set
				{
					if (!started)
					{
						preStartedSelectableNode = value;
						return;
					}

					isSelectable = value;
					if (isSelectable)
					{
						animator.ResetTrigger(isSelectableHash);
						animator.SetTrigger(isSelectableHash);
					}
					else
					{
						selected = false;
						animator.ResetTrigger(isSelectableHash);
						animator.SetTrigger(isUnselectableHash);
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
				if (isSelectable)
				{
					selected = !selected;
					selectedEvent(this, selected);
					animator.ResetTrigger(isSelectableHash);
					animator.ResetTrigger(clickInHash);
					animator.ResetTrigger(clickOutHash);
					animator.SetTrigger(selected ? clickInHash : clickOutHash);
				}
			}
		}
	}
}