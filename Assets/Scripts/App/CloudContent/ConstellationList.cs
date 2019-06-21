using System.Collections;
using SimpleJSON;
using BestHTTP;

namespace Assets.Scripts.CloudContent
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

		protected override IEnumerator Build()
		{
			yield return App.Server.Request(
			HTTPMethods.Get,
			"constellation",
			(JSONNode json_) =>
			{
				Json = json_;

				foreach (var almostJson in Json)
					Table.Add(almostJson.Value["_id"], new Model.Constellation(almostJson.Value));
			}).Send();
		}

		public ConstellationList(SkillList skillList_)
		{
			dependencyList.Add(skillList_);
		}
	}
}
