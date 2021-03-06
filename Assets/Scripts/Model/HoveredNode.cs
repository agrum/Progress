﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
	public class HoveredSkill
	{
		public delegate void OnChanged();
		public event OnChanged ChangedEvent = delegate { };

		private Skill skill = null;

		public Skill Skill
		{
			get
			{
				return skill;
			}
			set
			{
				skill = value;
				ChangedEvent();
			}
		}
	}
}
