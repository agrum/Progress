﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.View
{
	public class Node : Selectable
	{			
		private ViewModel.INode viewModel = null;
		protected Vector2 positionMultiplier = new Vector2();

		private readonly int enterHash = Animator.StringToHash("Enter");
		private readonly int leaveHash = Animator.StringToHash("Leave");
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
        private Transform specialization;
        private WestText level;
        private WestText handicap;

        protected override void Start()
		{
			started = true;

			interactable = true;
			transition = Transition.None;

            base.Start();

            OnSelectionChanged(isSelected);
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
            specialization = childTranform.Find("Specialization");
            level = specialization.Find("Level").GetComponent<WestText>();
            handicap = specialization.Find("Handicap").GetComponent<WestText>();

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
            OnSelectionChanged(viewModel.Selected());
        }

		override protected void OnDestroy()
		{
			base.OnDestroy();
            if (viewModel == null)
                return;
                
			viewModel.SkillChanged -= OnSkillChanged;
			viewModel.ScaleChanged -= OnScaleChanged;
			viewModel.SelectionChanged -= OnSelectionChanged;
			viewModel = null;
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
                specialization.gameObject.SetActive(viewModel.Level() != 0);
                level.color = viewModel.Mat().color;
                level.Format(viewModel.Level().ToString());
                handicap.color = viewModel.Mat().color;
                handicap.Format(viewModel.Handicap().ToString());
                OnSelectionChanged(true);
            }
			else
			{
				OnSelectionChanged(false);
				pulse.gameObject.SetActive(false);
				icon.gameObject.SetActive(false);
				iconWhite.gameObject.SetActive(false);
                specialization.gameObject.SetActive(false);
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
			isSelected = selected_;

			if (!started || !animator.gameObject.activeSelf)
				return;
            
			animator.ResetTrigger(unselectHash);
			animator.ResetTrigger(selectHash);
                
			animator.SetTrigger(isSelected ? selectHash : unselectHash);
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