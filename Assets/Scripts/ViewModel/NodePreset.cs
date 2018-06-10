using UnityEngine;

namespace West
{
	namespace ViewModel
	{
		public class NodePreset
		{
			private View.Node viewNode;
			private View.NodeTextualDetails viewDetails;
			private Model.ConstellationNode modelNode;
			private Model.ConstellationPreset modelPreset;
			private bool canEdit;
			private Material mat;
			private Vector2 position;
			private float scale = 1.0f;

			public NodePreset(
				View.Node viewNode_, 
				View.NodeTextualDetails viewDetails_,
				Model.ConstellationNode modelNode_,
				Model.ConstellationPreset modelPreset_, 
				bool canEdit_, 
				Material mat_, 
				Vector2 position_)
			{
				Debug.Assert(viewNode_ != null);
				Debug.Assert(modelPreset_ != null);
				Debug.Assert(mat_ != null);

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

			~NodePreset()
			{
				viewNode.hoveredEvent -= Hovered;
				viewNode.clickedEvent -= Clicked;
			}

			public void UpdateNode(Model.ConstellationNode modelNode_)
			{
				modelNode = modelNode_;

				viewNode.Setup(
					modelNode == null ? null : "Icons/" + modelNode.Skill.UpperCamelCaseKey + "/" + modelNode.Skill.Json["name"],
					mat,
					position);
			}

			public virtual void Scale(float scale_)
			{
				scale = scale_;
				viewNode.Scale(scale);
			}

			public void Hovered(bool on)
			{
				if (viewDetails != null)
					viewDetails.Setup((on && modelNode != null) ? modelNode.Skill : null, mat);
			}

			public void Clicked()
			{
				if (canEdit && modelNode != null)
				{
					modelPreset.Remove(modelNode.Skill);
					UpdateNode(null);
				}
			}
		}
	}
}