using System;
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
			public class AppContent
			{
				private Session Session;
				public GameSettings GameSettings { get; private set; }
				public AbilityList AbilityList { get; private set; }
				public ClassList ClassList { get; private set; }
				public KitList KitList { get; private set; }
				public Constellation Constellation { get; private set; }

				public AppContent()
				{
					Session = new Session();
					GameSettings = new GameSettings(Session);
					AbilityList = new AbilityList(GameSettings);
					ClassList = new ClassList(GameSettings);
					KitList = new KitList(GameSettings);
					Constellation = new Constellation(AbilityList, ClassList, KitList);
				}
			}
		}
	}
}
