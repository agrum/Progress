using UnityEngine;

namespace West.ViewModel
{
    public class NodeMapEmpty : INodeMap
    {
        public event OnElementAdded NodeAdded = null;

        public NodeMapEmpty()
        {

        }
        
        public void PopulateNodes()
        {

        }

        public void SizeChanged(Rect rect)
        {
            
        }
    }
}
