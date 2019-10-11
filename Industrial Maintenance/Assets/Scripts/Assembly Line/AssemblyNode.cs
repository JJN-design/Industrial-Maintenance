using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
	NORMAL,
	WOODCHIPPER,
	PAINTER,
	PRESS,
	BOOT,
	FINISH
}

public class AssemblyNode : MonoBehaviour
{
	[Header("Node variables")]
	[Tooltip("Whether the box should visible between this node and the next")]
	[SerializeField] private bool m_visible;
	[Tooltip("What type of node this is, and what should be done to the box at this node")]
	[SerializeField] private NodeType m_type;
	[Tooltip("How long it should take to move from this node to the next")]
	[SerializeField] private float m_moveTime;

	/// <summary>
	/// Gets the visible state of the node
	/// </summary>
	/// <returns>Whether or not boxes should be visible at this node</returns>
	public bool GetVisible() { return m_visible; }

	/// <summary>
	/// Gets the node type
	/// </summary>
	/// <returns>What type of node this is</returns>
	public NodeType GetNodeType() { return m_type; }

	/// <summary>
	/// Gets the movement time of the node
	/// </summary>
	/// <returns>How long it takes to move to the next node</returns>
	public float GetMoveTime() { return m_moveTime; }
}
