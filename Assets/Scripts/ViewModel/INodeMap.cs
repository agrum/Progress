using UnityEngine;

namespace West.ViewModel
{
	public interface INodeMap : IBase
	{
		event OnElementAdded NodeAdded;

		void SizeChanged(Rect rect);
	}
}
