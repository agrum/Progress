using UnityEngine;
using SimpleJSON;

namespace West.ViewModel
{
	public delegate void OnVoidDelegate();
	public delegate void OnBoolDelegate(bool value);
	public delegate void OnFloatDelegate(float value);
	public delegate void OnStringDelegate(string value);
	public delegate void OnJsonDelegate(JSONNode value);
	public delegate void OnGameObjectDelegate(GameObject obj);

	public interface INode
	{
		event OnVoidDelegate SkillChanged;
		event OnBoolDelegate SelectionChanged;
		event OnFloatDelegate ScaleChanged;

		string IconPath();
		Material Mat();
		Vector2 Position();

		void Clicked();
		void Hovered(bool on);
	}
}
