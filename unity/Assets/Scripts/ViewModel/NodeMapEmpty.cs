using UnityEngine;

namespace Assets.Scripts.ViewModel
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
