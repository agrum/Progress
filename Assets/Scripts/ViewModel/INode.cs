using UnityEngine;
using SimpleJSON;

namespace Assets.Scripts.ViewModel
{
	public interface INode : IBase
	{
		event OnVoidDelegate SkillChanged;
		event OnBoolDelegate SelectionChanged;
		event OnFloatDelegate ScaleChanged;

        bool Selected();
        string IconPath();
		Material Mat();
		Vector2 Position();

        void Clicked();
        void Hovered(bool on);
	}
}
