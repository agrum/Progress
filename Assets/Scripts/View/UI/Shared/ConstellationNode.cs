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
			public delegate void OnClickedDelegate();
			public event OnClickedDelegate clickedEvent;
			public delegate void OnHoveredDelegate(bool hovered);
			public event OnHoveredDelegate hoveredEvent;
			
			private string iconPath = null;
			private Material mat = null;
			private Vector2 position;
			protected Vector2 positionMultiplier = new Vector2();
			protected float scale;

			private readonly int enterHash = Animator.StringToHash("Enter");
			private readonly int leaveHash = Animator.StringToHash("Leave");
			private readonly int clickHash = Animator.StringToHash("Click");
			private readonly int unselectHash = Animator.StringToHash("Unselect");
			private readonly int selectHash = Animator.StringToHash("Select");
			private bool started = false;
			
			private bool isSelected = false;
			
			private Transform childTranform;
			private Image stroke;
			private Image pulse;
			private Image fill;
			private Image icon;
			private Image iconWhite;

			protected override void Start()
			{
				started = true;

				isSelected = true;

				interactable = true;
				transition = Transition.None;

				base.Start();
			}

			public void Setup(string iconPath_, Material mat_, Vector2 position_)
			{
				iconPath = iconPath_;
				mat = mat_;
				position = position_;

				childTranform = gameObject.transform.Find("GameObject");
				stroke = childTranform.Find("HexagonStroke").GetComponent<Image>();
				pulse = childTranform.Find("Pulse").GetComponent<Image>();
				fill = childTranform.Find("HexagonFill").GetComponent<Image>();
				icon = childTranform.Find("Icon").GetComponent<Image>();
				iconWhite = childTranform.Find("White").Find("Icon").GetComponent<Image>();

				positionMultiplier.x = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
				positionMultiplier.y = 0.75f;
				
				scale = 1.0f;
				positionMultiplier.x = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
				positionMultiplier.y = 0.75f;

				Scale(scale);

				stroke.material = mat;
				pulse.material = mat;
				fill.material = mat;
				icon.material = mat;

				if (iconPath_ != null)
				{
					//string path = "Icons/" + viewModel.Skill().UpperCamelCaseKey + "/" + viewModel.Skill().Json["name"];
					UnityEngine.Object prefabObject = Resources.Load(iconPath);
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
					Selected = false;
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

			public bool Selected
			{
				get
				{
					return isSelected;
				}
				set
				{
					isSelected = value;

					if (!started)
						return;

					animator.ResetTrigger(unselectHash);
					animator.ResetTrigger(selectHash);
					animator.ResetTrigger(clickHash);

					animator.SetTrigger(clickHash);
					animator.SetTrigger(isSelected ? selectHash : unselectHash);
				}
			}

			override public void OnPointerEnter(PointerEventData eventData)
			{
				animator.ResetTrigger(leaveHash);
				animator.SetTrigger(enterHash);
				hoveredEvent(true);
			}

			override public void OnPointerExit(PointerEventData eventData)
			{
				animator.ResetTrigger(enterHash);
				animator.SetTrigger(leaveHash);
				hoveredEvent(false);
			}

			override public void OnPointerUp(PointerEventData eventData)
			{
				clickedEvent();
			}
		}
	}
}