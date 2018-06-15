using UnityEngine;

namespace West.ViewModel
{
	public interface INodeMap : IBase
	{
		event OnElementAdded NodeAdded;

        void PopulateNodes();
        void SizeChanged(Rect rect);
    }
}
