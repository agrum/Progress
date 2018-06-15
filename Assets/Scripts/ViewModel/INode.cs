using UnityEngine;
using SimpleJSON;

namespace West.ViewModel
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
