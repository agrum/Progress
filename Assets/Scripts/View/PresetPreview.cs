using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace West
{
	namespace View
	{
		public class PresetPreview : MonoBehaviour
		{
			public delegate void OnSizeChangedDelegate();
			public event OnSizeChangedDelegate SizeChangedEvent = delegate { };

			private ViewModel.PresetPreview viewModel;

			private RectTransform rectTransform = null;
			private Rect lastRect = new Rect();

			public void SetContext(ViewModel.PresetPreview viewModel_)
			{
				Debug.Assert(viewModel_ != null);

				viewModel = viewModel_;
				viewModel.AttachChild += AttachChild;
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
				viewModel.AttachChild -= AttachChild;
				viewModel = null;
			}

			public void AttachChild(GameObject gob)
			{
				gob.transform.SetParent(transform);
			}
		}
	}
}
