using UnityEngine;

namespace West
{
	namespace ViewModel
	{
		public class NodePreset
		{
			private View.ConstellationNode viewNode;
			private View.NodeTextualDetails viewDetails;
			private Model.ConstellationNode modelNode;
			private Model.ConstellationPreset modelPreset;
			private bool canEdit;
			private Material mat;
			private Vector2 position;

			public NodePreset(
				View.ConstellationNode viewNode_, 
				View.NodeTextualDetails viewDetails_,
				Model.ConstellationNode modelNode_,
				Model.ConstellationPreset modelPreset_, 
				bool canEdit_, 
				Material mat_, 
				Vector2 position_)
			{
				Debug.Assert(viewNode != null);
				Debug.Assert(modelPreset != null);
				Debug.Assert(mat != null);

				viewNode = viewNode_;
				viewDetails = viewDetails_;
				modelPreset = modelPreset_;
				canEdit = canEdit_;
				mat = mat_;
				position = position_;
				
				viewNode.hoveredEvent += Hovered;
				viewNode.clickedEvent += Clicked;

				UpdateNode(modelNode_);
			}

			public void UpdateNode(Model.ConstellationNode modelNode_)
			{
				modelNode = modelNode_;

				viewNode.Setup(
					modelNode == null ? null : "Icons/" + modelNode.Skill.UpperCamelCaseKey + "/" + modelNode.Skill.Json["name"],
					mat,
					position);
			}

			public void Hovered(bool on)
			{
				if (viewDetails != null)
					viewDetails.Setup((on && modelNode != null) ? modelNode.Skill : null, mat);
			}

			public void Clicked()
			{
				if (modelNode != null)
					modelPreset.Remove(modelNode);
			}
		}
	}
}