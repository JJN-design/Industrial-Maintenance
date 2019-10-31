using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxType
{
	LOG,
	PAINTED_LOG,
	STICKERED_LOG,
	PAINTED_STICKERED_LOG,
	PLAIN_BOX,
	PAINTED_BOX,
	STICKERED_BOX,
	PAINTED_STICKERED_BOX,
	FINISHED_BOX
}

public class AssemblyBox : MonoBehaviour
{
	//the type of box this is
	private BoxType m_type = BoxType.LOG;

	//Whether all machines have been working
	private bool m_correctAssembly = true;

	//the assembly line
	private AssemblyLine m_assemblyLine;

	//the machine manager
	private MachineManager m_manager;

	//how long it will take to reach the next node
	private float m_movementTime;
	private float m_timer = 0.0f;

	//how far into a movement we are in percentage
	private float m_movementPercentage = 0.0f;

	//what node we're on
	private int m_nodeCounter;

	[Header("Renderers")]
	[Tooltip("The mesh filter of this box")]
	[SerializeField] private MeshFilter m_meshFilter;
	[Tooltip("The renderer of this box")]
	[SerializeField] private Renderer m_renderer;

	[Header("Meshes")]
	[Tooltip("The log mesh")]
	[SerializeField] private Mesh m_logMesh;
	[Tooltip("The unfinished box mesh")]
	[SerializeField] private Mesh m_boxMesh;
	[Tooltip("The finished box mesh")]
	[SerializeField] private Mesh m_flatpackMesh;

	[Header("Materials")]
	[Tooltip("The material for the basic log")]
	[SerializeField] private Material m_logMat;
	[Tooltip("The material for the painted log")]
	[SerializeField] private Material m_paintedLogMat;
	[Tooltip("The material for the stickered log")]
	[SerializeField] private Material m_stickeredLogMat;
	[Tooltip("The material for the painted and stickered log")]
	[SerializeField] private Material m_paintedStickeredLogMat;
	[Tooltip("The material for the basic box")]
	[SerializeField] private Material m_boxMat;
	[Tooltip("The material for the painted box")]
	[SerializeField] private Material m_paintedBoxMat;
	[Tooltip("The material for the stickered box")]
	[SerializeField] private Material m_stickeredBoxMat;
	[Tooltip("The material for the painted and stickered box")]
	[SerializeField] private Material m_paintedStickeredBoxMat;
	[Tooltip("The material for the finished box")]
	[SerializeField] private Material m_flatpackMat;

	//The previous node the box was at
	private AssemblyNode m_previousNode;

	//the next node the box will be at
	private AssemblyNode m_nextNode;

	/// <summary>
	/// Runs each frame
	/// </summary>
	private void Update()
	{
		//increment timer
		m_timer += Time.deltaTime;

		//calculate percentage of movement
		m_movementPercentage = m_timer / m_movementTime;

		bool movementComplete = false;

		//if movement is complete, clamp percentage and be ready to call next node
		if(m_movementPercentage >= 1.0f)
		{
			m_movementPercentage = 1.0f;
			movementComplete = true;
		}

		//calculate new position and rotation
		Vector3 newPos = Vector3.Lerp(m_previousNode.transform.position, m_nextNode.transform.position, m_movementPercentage);
		Quaternion newRot = Quaternion.Slerp(m_previousNode.transform.rotation, m_nextNode.transform.rotation, m_movementPercentage);

		//set new position and rotation
		transform.position = newPos;
		transform.rotation = newRot;

		//if movement is complete, call next node
		if (movementComplete && m_nodeCounter < m_assemblyLine.GetNodes().Length - 1)
			ChangeNode(m_assemblyLine.GetNodes()[++m_nodeCounter]);
		else if (movementComplete && m_nodeCounter >= m_assemblyLine.GetNodes().Length - 1)
			FinishNode();
	}

	/// <summary>
	/// Creates the box
	/// </summary>
	/// <param name="line">The assembly line manager</param>
	/// <param name="manager">The machine manager</param>
	/// <param name="startNode">The starting node of the assembly line</param>
	public void Create(AssemblyLine line, MachineManager manager)
	{
		//set assembly line and manager
		m_assemblyLine = line;
		m_manager = manager;

		//set initial node and movement times
		m_previousNode = line.GetNodes()[0];
		m_movementTime = m_previousNode.GetMoveTime();
		
		//set next node and counter
		m_nextNode = line.GetNodes()[1];
		m_nodeCounter = 1;

		UpdateMesh();
	}

	/// <summary>
	/// Code to be called when this reaches the last node
	/// </summary>
	private void FinishNode()
	{
		//Increase score if all machines were working
		if (m_correctAssembly)
			ScoreManager.AddScore(1);

		//destroy self
		Destroy(gameObject);
	}

	/// <summary>
	/// Changes the node
	/// Should be called upon reaching a node
	/// </summary>
	/// <param name="newNode">The next node in line</param>
	public void ChangeNode(AssemblyNode newNode)
	{
		//If node just reached was the end node, call end code
		if(m_previousNode.GetNodeType() == NodeType.FINISH)
		{
			FinishNode();
		}

		//Reset timer
		m_timer = 0.0f;
		m_movementTime = m_nextNode.GetMoveTime();

		//Set new node variables
		m_previousNode = m_nextNode;
		m_nextNode = newNode;

		if (!m_previousNode.GetVisible())
			m_renderer.enabled = false;
		else
			m_renderer.enabled = true;

		//MOVE TO NEXT NODE CODE

		switch (m_previousNode.GetNodeType())
		{
			case (NodeType.NORMAL):
				//do nothing
				break;
			case (NodeType.WOODCHIPPER):
				//Call woodchipper checks
				Woodchipper();
				break;
			case (NodeType.PAINTER):
				//Call painter checks
				Painter();
				break;
			case (NodeType.PRESS):
				//Call press checks
				Press();
				break;
			case (NodeType.BOOT):
				//Call boot code
				Boot();
				break;
		}
	}

	/// <summary>
	/// Code to check woodchipper state and update box type accordingly
	/// </summary>
	private void Woodchipper()
	{
		if (m_type == BoxType.LOG && m_manager.CheckMachine(MachineLists.WOODCHIPPER)) //checks if this is a log and if the woodchipper is working
			m_type = BoxType.PLAIN_BOX; //updates type
		else if (!m_manager.CheckMachine(MachineLists.WOODCHIPPER)) //if woodchipper isn't working, fail assembly process
			m_correctAssembly = false;
		UpdateMesh();
	}

	/// <summary>
	/// Code to check painter status and update box type accordingly
	/// </summary>
	private void Painter()
	{
		if (m_manager.CheckMachine(MachineLists.PAINTER)) //check if painter is working
		{
			if (m_type == BoxType.LOG)
				m_type = BoxType.PAINTED_LOG;
			else if (m_type == BoxType.PLAIN_BOX)
				m_type = BoxType.PAINTED_BOX;
		}
		else if (!m_manager.CheckMachine(MachineLists.PAINTER)) //if painter isn't working, fail assembly process
			m_correctAssembly = false;
		UpdateMesh();
	}

	/// <summary>
	/// Code to check press status and update box type accordingly
	/// </summary>
	private void Press()
	{
		if (m_manager.CheckMachine(MachineLists.PRESS)) //if press is working, set the new box type to be stickered
		{
			switch(m_type)
			{
				case (BoxType.LOG):
					m_type = BoxType.STICKERED_LOG;
					break;
				case (BoxType.PLAIN_BOX):
					m_type = BoxType.STICKERED_BOX;
					break;
				case (BoxType.PAINTED_LOG):
					m_type = BoxType.PAINTED_STICKERED_LOG;
					break;
				case (BoxType.PAINTED_BOX):
					m_type = BoxType.PAINTED_STICKERED_BOX;
					break;
			}
		}
		else if (!m_manager.CheckMachine(MachineLists.PRESS)) //if press isn't working, fail assembly process
			m_correctAssembly = false;
		UpdateMesh();
	}

	/// <summary>
	/// Code to explode any non-finished boxes, and to update finished boxes to a proper state
	/// </summary>
	private void Boot()
	{
		if(m_type != BoxType.PAINTED_STICKERED_BOX) //if box isn't painted, stickered, and a box, explode it
			Explode();
		else //otherwise, stamp it flat
			m_type = BoxType.FINISHED_BOX;
		UpdateMesh();
	}

	/// <summary>
	/// Explode the box
	/// </summary>
	private void Explode()
	{
		m_assemblyLine.m_bootParticles.Play();
		Destroy(gameObject);
	}

	/// <summary>
	/// Updates the mesh and material to match the box type
	/// </summary>
	private void UpdateMesh()
	{
		switch(m_type)
		{
			case (BoxType.LOG):
				m_meshFilter.mesh = m_logMesh;
				m_renderer.material = m_logMat;
				break;
			case (BoxType.PAINTED_LOG):
				m_meshFilter.mesh = m_logMesh;
				m_renderer.material = m_paintedLogMat;
				break;
			case (BoxType.STICKERED_LOG):
				m_meshFilter.mesh = m_logMesh;
				m_renderer.material = m_stickeredLogMat;
				break;
			case (BoxType.PAINTED_STICKERED_LOG):
				m_meshFilter.mesh = m_logMesh;
				m_renderer.material = m_paintedStickeredLogMat;
				break;
			case (BoxType.PLAIN_BOX):
				m_meshFilter.mesh = m_boxMesh;
				m_renderer.material = m_boxMat;
				break;
			case (BoxType.PAINTED_BOX):
				m_meshFilter.mesh = m_boxMesh;
				m_renderer.material = m_paintedBoxMat;
				break;
			case (BoxType.STICKERED_BOX):
				m_meshFilter.mesh = m_boxMesh;
				m_renderer.material = m_stickeredBoxMat;
				break;
			case (BoxType.PAINTED_STICKERED_BOX):
				m_meshFilter.mesh = m_boxMesh;
				m_renderer.material = m_paintedStickeredBoxMat;
				break;
			case (BoxType.FINISHED_BOX):
				m_meshFilter.mesh = m_flatpackMesh;
				m_renderer.material = m_flatpackMat;
				break;
		}
	}
}
