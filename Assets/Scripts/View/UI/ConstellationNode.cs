﻿using System.Collections;
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

			private readonly int enterHash = Animator.StringToHash("Enter");
			private readonly int leaveHash = Animator.StringToHash("Leave");
			private readonly int clickInHash = Animator.StringToHash("ClickIn");
			private readonly int clickOutHash = Animator.StringToHash("ClickOut");
			private readonly int inHash = Animator.StringToHash("In");
			private readonly int selectedHash = Animator.StringToHash("Selected");
			private readonly int isUnselectableHash = Animator.StringToHash("IsUnselectable");
			private readonly int isSelectableHash = Animator.StringToHash("IsSelectable");
			private int highlightLayerIndex;
			private int selectLayerIndex;
			private bool preStartedSelectableNode = false;
			private bool started = false;

			private bool hovered = false;
			private bool selected = false;
			private bool isSelectable = false;
			private int highlightNextTrigger = 0;
			private int selectNextTrigger = 0;

			public void Setup(Model.ConstellationNode model_, Material mat_, Vector2 positionMultiplier_, float scale_)
			{
				model = model_;
				mat = mat_;

				gameObject.transform.localPosition = new Vector3(model.Position.x * positionMultiplier_.x, model.Position.y * positionMultiplier_.y, 0);
				gameObject.transform.localScale = Vector3.one * scale_;
				gameObject.transform.localRotation = Quaternion.identity;

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
					Object prefabObject = Resources.Load(path);
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

			override protected void Start()
			{
				started = true;

				SelectableNode = preStartedSelectableNode;

				//animator_ = GetComponent<Animator>();
				highlightLayerIndex = animator.GetLayerIndex("Hover Layer");
				selectLayerIndex = animator.GetLayerIndex("Click Layer");

				base.Start();
			}

			public void Update()
			{
				if (highlightNextTrigger != 0 && !animator.IsInTransition(highlightLayerIndex))
				{
					if ((animator.GetCurrentAnimatorStateInfo(highlightLayerIndex).shortNameHash == inHash) != hovered)
					{
						animator.SetTrigger(highlightNextTrigger);
						highlightNextTrigger = 0;
					}
				}
				if (selectNextTrigger != 0 && !animator.IsInTransition(selectLayerIndex))
				{
					if ((animator.GetCurrentAnimatorStateInfo(selectLayerIndex).shortNameHash == selectedHash) != selected)
					{
						animator.SetTrigger(selectNextTrigger);
						selectNextTrigger = 0;
					}
				}
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
				hovered = true;
				highlightNextTrigger = enterHash;
			}

			override public void OnPointerExit(PointerEventData eventData)
			{
				hovered = false;
				highlightNextTrigger = leaveHash;
			}

			override public void OnPointerUp(PointerEventData eventData)
			{
				if (isSelectable)
				{
					selected = !selected;
					selectedEvent(this, selected);
					animator.ResetTrigger(isSelectableHash);
					selectNextTrigger = selected ? clickInHash : clickOutHash;
				}
			}
		}
	}
}