using SimpleJSON;

namespace West.Model
{
	public class Json
	{
		public delegate void OnChanged(string key);
		public event OnChanged ChangedEvent = delegate { };

		private JSONObject json = new JSONObject();

		~Json()
		{
			ChangedEvent = null;
		}

		public JSONNode this[string key]
		{
			get
			{
				return json[key];
			}
			set
			{
				if (json[key] != value)
				{
					json[key] = value;
					ChangedEvent(key);
				}
			}
		}
	}
}
