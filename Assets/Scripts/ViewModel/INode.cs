using UnityEngine;

namespace West.ViewModel
{
	public delegate void OnVoidChanged();
	public delegate void OnBoolChanged(bool value);
	public delegate void OnFloatChanged(float value);

	public interface INode
	{
		event OnVoidChanged SkillChanged;
		event OnBoolChanged SelectionChanged;
		event OnFloatChanged ScaleChanged;

		string IconPath();
		Material Mat();
		Vector2 Position();

		void Clicked();
		void Hovered(bool on);
	}
}
