using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BestHTTP;
using SimpleJSON;
using UnityEngine.UI;

namespace West
{
	namespace Scene
	{
		class PresetSelection : MonoBehaviour
		{
			public View.NodeTextualDetails nodeTextualDetails = null;
			public RectTransform contentElement = null;
			public HorizontalLayoutGroup horizontalLayout = null;
			public GameObject presetColumnPrefab = null;

			private List<View.PresetColumn> presetColumnList = new List<View.PresetColumn>();

			void Start()
			{
				Debug.Assert(nodeTextualDetails != null);
				Debug.Assert(horizontalLayout != null);
				Debug.Assert(presetColumnPrefab != null);

				App.Content.Account.Load(() =>
				{
					Setup();
				});
			}

			private void Setup()
			{
				//return if object died while waiting for answer
				if (this == null)
					return;

				nodeTextualDetails.Setup(null);

				//setup existing preset columns
				foreach (var preset in App.Content.Account.PresetList)
				{
					GameObject gob = Instantiate(presetColumnPrefab);
					View.PresetColumn presetColumn = gob.GetComponent<View.PresetColumn>();
                    presetColumn.ColumnDestroyedEvent += OnPresetRemoved;

                    presetColumn.Setup(preset, View.PresetColumn.Mode.Display, nodeTextualDetails);
					presetColumn.transform.SetParent(horizontalLayout.transform, false);
					presetColumnList.Add(presetColumn);
				}

                //add empty column to add presets.
                {
                    GameObject gob = Instantiate(presetColumnPrefab);
                    View.PresetColumn presetColumn = gob.GetComponent<View.PresetColumn>();
                    presetColumn.Setup(null, View.PresetColumn.Mode.Display, nodeTextualDetails);
                    presetColumn.transform.SetParent(horizontalLayout.transform, false);
                    presetColumnList.Add(presetColumn);
                }

                ArrangeUI();
            }

            private void OnPresetRemoved(View.PresetColumn column_)
            {
                presetColumnList.Remove(column_);
                Destroy(column_.gameObject);
                ArrangeUI();
            }

            private void ArrangeUI()
            {
                contentElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150.0f * presetColumnList.Count);
            }
        }
	}
}
