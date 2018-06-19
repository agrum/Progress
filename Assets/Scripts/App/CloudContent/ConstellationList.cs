using System.Collections;
using SimpleJSON;
using BestHTTP;

namespace West.CloudContent
{
	public class ConstellationList : Base
	{
		public JSONNode Json { get; private set; } = null;

		public Model.Constellation this[string uuid_]
		{
			get
			{
				return Table[uuid_] as Model.Constellation;
			}
		}

		private Hashtable Table = new Hashtable();

		protected override void Build(OnBuilt onBuilt_)
		{
			App.Server.Request(
			HTTPMethods.Get,
			"constellation",
			(JSONNode json_) =>
			{
				Json = json_;

				foreach (var almostJson in Json)
					Table.Add(almostJson.Value["_id"], new Model.Constellation(almostJson.Value));
						
				onBuilt_();
			}).Send();
		}

		public ConstellationList(SkillList skillList_)
		{
			dependencyList.Add(skillList_);
		}
	}
}
