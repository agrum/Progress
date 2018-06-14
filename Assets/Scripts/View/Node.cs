using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace West
{
	namespace View
	{
		public class Node : Selectable
		{			
			private ViewModel.INode viewModel = null;
			protected Vector2 positionMultiplier = new Vector2();

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

			public void SetContext(ViewModel.INode viewModel_)
			{
				Debug.Assert(viewModel_ != null);

				viewModel = viewModel_;
				viewModel.SkillChanged += OnSkillChanged;
				viewModel.ScaleChanged += OnScaleChanged;
				viewModel.SelectionChanged += OnSelectionChanged;

				childTranform = gameObject.transform.Find("GameObject");
				stroke = childTranform.Find("HexagonStroke").GetComponent<Image>();
				pulse = childTranform.Find("Pulse").GetComponent<Image>();
				fill = childTranform.Find("HexagonFill").GetComponent<Image>();
				icon = childTranform.Find("Icon").GetComponent<Image>();
				iconWhite = childTranform.Find("White").Find("Icon").GetComponent<Image>();

				positionMultiplier.x = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
				positionMultiplier.y = 0.75f;

				positionMultiplier.x = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
				positionMultiplier.y = 0.75f;

				OnScaleChanged(1.0f);

				stroke.material = viewModel.Mat();
				pulse.material = viewModel.Mat();
				fill.material = viewModel.Mat();
				icon.material = viewModel.Mat();

				OnSkillChanged();
			}

			override protected void OnDestroy()
			{
				base.OnDestroy();

				if (viewModel != null)
				{
					viewModel.SkillChanged -= OnSkillChanged;
					viewModel.ScaleChanged -= OnScaleChanged;
					viewModel.SelectionChanged -= OnSelectionChanged;
					viewModel = null;
				}
			}

			public void OnSkillChanged()
			{
				if (viewModel.IconPath() != null)
				{
					UnityEngine.Object prefabObject = Resources.Load(viewModel.IconPath());
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
					OnSelectionChanged(false);
					pulse.gameObject.SetActive(false);
					icon.gameObject.SetActive(false);
					iconWhite.gameObject.SetActive(false);
				}
			}

			public void OnScaleChanged(float scale_)
			{
				gameObject.transform.localPosition = new Vector3(viewModel.Position().x * positionMultiplier.x, viewModel.Position().y * positionMultiplier.y, 0) * scale_; ;
				gameObject.transform.localScale = Vector3.one * scale_;
				gameObject.transform.localRotation = Quaternion.identity;
			}

			public void OnSelectionChanged(bool selected_)
			{
				bool oldValue = isSelected;
				isSelected = selected_;

				if (!started)
					return;

				animator.ResetTrigger(unselectHash);
				animator.ResetTrigger(selectHash);
				animator.ResetTrigger(clickHash);

				if (oldValue != isSelected)
				{
					animator.SetTrigger(clickHash);
					animator.SetTrigger(isSelected ? selectHash : unselectHash);
				}
			}

			override public void OnPointerEnter(PointerEventData eventData)
			{
				animator.ResetTrigger(leaveHash);
				animator.SetTrigger(enterHash);
				viewModel.Hovered(true);
			}

			override public void OnPointerExit(PointerEventData eventData)
			{
				animator.ResetTrigger(enterHash);
				animator.SetTrigger(leaveHash);
				viewModel.Hovered(false);
			}

			override public void OnPointerUp(PointerEventData eventData)
			{
				viewModel.Clicked();
			}
		}
	}
}