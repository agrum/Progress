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
	public delegate IBase Factory();
	public delegate void OnElementAdded(Factory factory);

	public interface IBase
	{

	}
}