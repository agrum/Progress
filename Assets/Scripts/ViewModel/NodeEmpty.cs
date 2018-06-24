﻿using UnityEngine;
using System.Collections.Generic;
using System;

namespace West.ViewModel
{
	public class NodeEmpty : INode
	{
		public event OnVoidDelegate SkillChanged = delegate { };
		public event OnBoolDelegate SelectionChanged = delegate { };
		public event OnFloatDelegate ScaleChanged = delegate { };
        
		private Model.Json scale;

		private Material mat;
		private Vector2 position;
        List<Model.Skill> selectedSkillList = null;

        public NodeEmpty(
            Model.Json scale_,
            Material mat_,
            Vector2 position_)
        {
            Debug.Assert(mat_ != null);
            
            scale = scale_;
            mat = mat_;
            position = position_;

            scale.ChangedEvent += OnScaleUpdated;
        }

		~NodeEmpty()
		{
			scale.ChangedEvent -= OnScaleUpdated;

			SkillChanged = null;
			SelectionChanged = null;
			ScaleChanged = null;
		}

		public virtual void OnScaleUpdated(string key)
		{
			ScaleChanged((float)scale["scale"].AsDouble);
        }

        public bool Selected()
        {
            return false;
        }

        public string IconPath()
		{
			return null;
		}

		public Material Mat()
		{
			return mat;
		}

		public Vector2 Position()
		{
			return position;
		}

		public void Hovered(bool on)
		{

		}

		public void Clicked()
		{
			
		}
	}
}