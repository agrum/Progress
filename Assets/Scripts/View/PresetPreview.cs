using UnityEngine;

namespace West.View
{
	public class PresetPreview : MonoBehaviour
	{
		public delegate void OnSizeChangedDelegate();
		public event OnSizeChangedDelegate SizeChangedEvent = delegate { };

		private ViewModel.IPresetPreview viewModel;

		private RectTransform rectTransform = null;
		private Rect lastRect = new Rect();

		public void SetContext(ViewModel.IPresetPreview viewModel_)
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
				SizeChangedEvent();
			}
		}

		void OnDestroy()
		{
			viewModel.NodeAdded -= OnNodeAdded;
			viewModel = null;
		}

		public void OnNodeAdded(ViewModel.Factory factory)
		{
			GameObject gob = GameObject.Instantiate(App.Resource.Prefab.ConstellationNode);
			gob.GetComponent<Node>().SetContext(factory() as ViewModel.INode);
			gob.transform.SetParent(transform);
		}
	}
}
