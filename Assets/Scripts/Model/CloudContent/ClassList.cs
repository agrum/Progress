﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using BestHTTP;

namespace West
{
	namespace Model
	{
		namespace CloudContent
		{
			public class ClassList : Base
			{
				public JSONNode Json { get; private set; } = null;

				protected override void Build(OnBuilt onBuilt_)
				{
					App.Server.Request(
					HTTPMethods.Get,
					"class",
					(JSONNode json_) =>
					{
						Json = json_;
						onBuilt_();
					}).Send();
				}

				public ClassList(GameSettings gameSettings_)
				{
					dependencyList.Add(gameSettings_);
				}
			}
		}
	}
}
