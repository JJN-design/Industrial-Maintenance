using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyLine : MonoBehaviour
{
	[Tooltip("The prefab for a box\nMUST HAVE AssemblyBox SCRIPT")]
	[SerializeField] private GameObject m_boxPrefab;

	[Tooltip("The list of nodes, from start to end\nMachines must be in the order: Woodchipper, Painter, Press, Boot")]
	[SerializeField] private AssemblyNode[] m_nodes;

	[Tooltip("The boot script")]
	public Boot m_boot;

	/// <summary>
	/// Gets the node list
	/// </summary>
	/// <returns></returns>
	public AssemblyNode[] GetNodes() { return m_nodes; }

	[Tooltip("The machine manager")]
	[SerializeField] private MachineManager m_manager;

	/// <summary>
	/// Creates a new box on the assembly line
	/// </summary>
	public void CreateBox()
	{
		GameObject newBox = Instantiate(m_boxPrefab, m_nodes[0].transform);
		newBox.GetComponent<AssemblyBox>().Create(this, m_manager);
	}
}
