using UnityEngine;

namespace West.View
{
	public class NodeMap : MonoBehaviour
	{
		private ViewModel.INodeMap viewModel;

		private RectTransform rectTransform = null;
		private Rect lastRect = new Rect();

		public void SetContext(ViewModel.INodeMap viewModel_)
		{
			Debug.Assert(viewModel_ != null);

			viewModel = viewModel_;
			viewModel.NodeAdded += OnNodeAdded;
		}

		void Start()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		void Update()
		{
			if (rectTransform && rectTransform.rect != lastRect)
			{
				lastRect = rectTransform.rect;
				viewModel.SizeChanged(lastRect);
			}
		}

		void OnDestroy()
		{
			viewModel.NodeAdded -= OnNodeAdded;
			viewModel = null;
		}

		public void OnNodeAdded(ViewModel.Factory factory)
		{
			GameObject gob = Instantiate(App.Resource.Prefab.ConstellationNode);
			gob.GetComponent<Node>().SetContext(factory() as ViewModel.INode);
			gob.transform.SetParent(transform);
		}
	}
}
