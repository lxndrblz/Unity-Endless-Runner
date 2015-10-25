/****************************** Module Header ******************************\
       Module Name:  EndlessRunner
       Project:      Endless Runner Starter Asset
       Copyright(c)  Alexander Bilz
       Author: 		 Alexander Bilz

       This script is the main part of the Endless Runner Starter Asset. Its
       responsible for generating, monitoring and delting the GameObjects.

       This source is subject to Alexander Bilz.
       All other rights reserved.

       THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
       EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
       WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using UnityEngine;
//Needed for getting the length of enums
using System;
using System.Collections;
//Necessary for using lists and stacks
using System.Collections.Generic;

public class EndlessRunner: MonoBehaviour {
	
	#region Variables
	
	/// <summary>
	/// Enumeration with the different types that this script can handle
	/// </summary>
	public enum Parts : int {
		
		Beginning,
		Forward,
		CornerRight,
		CornerLeft,
		Fork,
		Stairway
		
		
	}
	/// <summary>
	/// Enumeration with the different directions that this script can built.
	/// </summary>
	public enum Direction : int{
		Forward,
		Right,
		Backward,
		Left
	}
	public enum States : int{
		
		MainRoute,
		DependencyRoute,
		NoFork
	}
	public enum PointType : int{
		StartPoint,
		MedianPoint,
		EndPoint
		
	}
	/// <summary>
	/// Array for the different type of gameobjects
	/// </summary>
	public GameObject[] parts_straight_forward;
	/// <summary>
	/// Contains all the corners facing to the right
	/// </summary>
	public GameObject[] parts_corner_right;
	/// <summary>
	/// Contains all the corners facing to the left
	/// </summary>
	public GameObject[] parts_corner_left;
	/// <summary>
	/// Instead of an fork there could also be a corner that is facing Forward :-> Right
	/// </summary>
	public GameObject[] parts_corner_fork;
	/// <summary>
	/// Element that will be placed at the beginning
	/// </summary>
	public GameObject[] parts_straight_begin;
	/// <summary>
	/// Stores different stairway assets
	/// </summary>
	public GameObject[] parts_stairway;
	/// <summary>
	/// Tracks all the Objects that are currently instantiated
	/// </summary>
	public List<InstantiatedObject> list_instiated_object = new List<InstantiatedObject>();
	/// <summary>
	/// The number of subsections that are prebuilt.
	/// </summary>
	public int i_amount_of_objects_prebuilt;
	/// <summary>
	/// Stores the amount of elements from the same part type. Necessary to realize the i_min_same_parts and i_max_same_parts.
	/// </summary>
	private int[] amount_of_elements;
	/// <summary>
	/// Save which object the player is currently walking on
	/// </summary>
	private InstantiatedObject m_playerCell = null;
	private InstantiatedObject m_playerCell_previous = null;
	/// <summary>
	/// Minimum number of elements placed in a row.
	/// </summary>
	public int i_min_same_parts = 5;
	/// <summary>
	/// Maximum number of elements placed in a row.
	/// </summary>
	public int i_max_same_parts = 10;
	
	/// <summary>
	/// A Global ID for unique names.
	/// </summary>
	private int i_global_ID = 0;
	/// <summary>
	/// The create_fork shows if a depricated fork needs to be built.
	/// </summary>
	private bool create_fork = false;
	/// <summary>
	/// GameObject with the PlayerScript
	/// </summary>
	public GameObject player;
	/// <summary>
	/// Boundary value for the collision detection
	/// </summary>
	public float collision_distance = 12.0f;
	/// <summary>
	/// Save the direction of the elements after the fork as they have to be inverted
	/// </summary>
	private Direction direction_after_fork;
	//Two times the size of a standard element
	
	/// <summary>
	/// Turn Instantiation of the dependency of. This is only meant for debugging purpose.
	/// </summary>
	public bool bool_place_dependency;
	
	public static EndlessRunner currentEndlessRunner;
	
	#endregion
	
	#region Class Element
	
	public class InstantiatedObject
	{
		//CellID
		private String id;
		// Basic cell information (its all about the position)
		private Vector3 startPosition;
		private Vector3 endPosition1;
		//Will be only set for T Parts
		private Vector3 endPosition2;
		private Vector3 medianPoint;
		
		//Direction in which the Object is facing
		private Direction cellDirection;
		//Enumeration Part type
		private Parts type;
		//GameObject of the Instiated Object
		private GameObject cellModel;
		//Has the player visited the object
		private bool visited = false;
		private Quaternion rotation = Quaternion.identity;
		/// <summary>
		/// Is this element dependent (fork)
		/// </summary>
		private InstantiatedObject dependency;
		
		//Public Function of the class InstantiatedObject
		public InstantiatedObject Dependency
		{
			get
			{
				return dependency;
			}
			set
			{
				if (value != null)
				{
					dependency = value;
				}
			}
		}
		public Quaternion Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				if (value != null)
				{
					rotation = value;
				}
			}
		}
		
		public bool Visited
		{
			get
			{
				return visited;
			}
			set
			{
				if (value != null)
				{
					visited = value;
				}
			}
		}
		
		public GameObject CellModel
		{
			get
			{
				return cellModel;
			}
			set
			{
				if (value != null)
				{
					cellModel = value;
				}
			}
		}
		
		public Parts Type
		{
			get
			{
				return type;
			}
			set
			{
				if (value != null)
				{
					type = value;
				}
			}
		}
		
		public Direction CellDirection
		{
			get
			{
				return cellDirection;
			}
			set
			{
				if (value != null)
				{
					cellDirection = value;
				}
			}
		}
		
		public String ID
		{
			get
			{
				return id;
			}
			set
			{
				if (value != null)
				{
					id = value;
				}
			}
		}
		
		public Vector3 StartPosition
		{
			get
			{
				return startPosition;
			}
			set
			{
				if (value != null)
				{
					startPosition = value;
				}
			}
		}
		public Vector3 MedianPoint
		{
			get
			{
				return medianPoint;
			}
			set
			{
				if (value != null)
				{
					medianPoint = value;
				}
			}
		}
		
		public Vector3 EndPosition
		{
			get
			{
				return endPosition1;
			}
			set
			{
				if (value != null)
				{
					endPosition1 = value;
				}
			}
		}
		public Vector3 EndPosition2
		{
			get
			{
				return endPosition2;
			}
			set
			{
				if (value != null)
				{
					endPosition2 = value;
				}
			}
		}
		public InstantiatedObject(Vector3 cellPosition, Direction  cellDirection, Parts cellType)
		{
			this.medianPoint  = cellPosition;
			this.cellDirection = cellDirection;
			this.type 	= cellType;
			
		}
	}
	
	#endregion
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		//Resize array
		amount_of_elements = new int[Enum.GetNames(typeof(Parts)).Length];
		InitialBuild();
	}
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake(){
		currentEndlessRunner = this;
	}
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		
		//Delete at first
		BuildOnTheFly(false);
	}
	
	/// <summary>
	/// Initial Build is called only once, it places the "Beginning Part"
	/// </summary>
	void InitialBuild(){
		
		// Setup Root Cell
		InstantiatedObject rootCell = new InstantiatedObject(Vector3.zero, Direction.Forward, Parts.Forward);
		rootCell.Type = Parts.Beginning;
		rootCell.CellModel = PlaceElement(rootCell.Type);
		//Set the three positions
		rootCell.MedianPoint = Vector3.zero;
		//Get the Position of the empty gameObject
		rootCell.EndPosition = rootCell.CellModel.gameObject.transform.GetChild(2).transform.position;
		//Set the rotation to default
		rootCell.Rotation = Quaternion.identity;
		rootCell.ID = UniqueID();
		//Place the Element in the scene
		AdjustPositionRotation(rootCell.CellModel, rootCell.MedianPoint, rootCell.Rotation, rootCell.ID);
		//Set visited flag
		rootCell.Visited = true;
		list_instiated_object.Add(rootCell);
		//Start PlayerScript
		player.GetComponent<PlayerScript>().Initialization(list_instiated_object[0].ID);
		//Activate Build on the fly
		BuildOnTheFly(true);
	}
	
	/// <summary>
	/// Manages all the Build Process that will happen on the fly
	/// </summary>
	/// <param name="ManuallyTriggered">If set to <c>true</c> it will create new parts otherwise it will only try to delete odd ones</param>
	void BuildOnTheFly(bool ManuallyTriggered){
		
		
		
		int startFrom = list_instiated_object.Count;
		
		if(startFrom < i_amount_of_objects_prebuilt){
			//New Objects need to be planed and instantiated
			PlanNextElement(list_instiated_object[startFrom-1], 0);
			CreateCellModel(list_instiated_object[startFrom]);
			
			
		}
		DeleteObsolteParts();
	}
	/// <summary>
	/// Deletes all the parts that the player already visited. If the Player already set visited to true but is still on the element it will not be deleted
	/// </summary>
	/// <summary>
	/// Deletes all the parts that the player already visited. If the Player already set visited to true but is still on the element it will not be deleted
	/// </summary>
	void DeleteObsolteParts(){
		//Ignore the last one
		if(m_playerCell != null && m_playerCell_previous != null){
			for (int i = list_instiated_object.IndexOf(m_playerCell);i>=0;i--){
				//The previous element will not be deleted instantely as this could leas to quite strange efffects
				if (list_instiated_object[i].Visited == true && (list_instiated_object[i]  != m_playerCell)&& (list_instiated_object[i] != m_playerCell_previous))
				{
					if(list_instiated_object[i].Dependency != null){
						GameObject.Find(list_instiated_object[i].Dependency.ID).SetActive(false);
						
					}
					//Remove the GameObject out of the scene
					GameObject.Find(list_instiated_object[i].ID).SetActive(false);
					//Drop the element out the list that stores all the elements
					list_instiated_object.RemoveAt(i);
				}
			}
			
		}
		
		
		
	}
	/// <summary>
	/// Checks if an fork element already exists in the Scene and if its not visited.
	/// </summary>
	/// <returns><c>true</c> If there is an unvisited fork element in the scene <c>false</c> otherwise.</returns>
	bool CheckIfForkExists(){
		
		bool exists;
		exists = false;
		
		
		for (int i = 0; i < list_instiated_object.Count; i++) {
			
			if (list_instiated_object[i].Type == Parts.Fork && list_instiated_object[i].Visited == false) {
				exists =  true;
			}
			
		}
		return exists;
		
	}
	GameObject PlaceElement(Parts type_to_place){
		//This function is responsible for placing all our objects, the Objects that can be currently placed can be seen in the Enumeration Parts
		//Instantiate the variable
		//Set it depending on part_to_place type to a real gameobjet
		
		GameObject obj = ObjectPool.current.GetPooledObject(type_to_place);
		
		
		//Set it to active otherwise it may be pooled two times
		obj.SetActive(true);
		//To still keep it hidden set the visibility
		obj.GetComponent<Renderer>().enabled = false;
		
		if (obj == null) {
			return null;
		}
		//Debug.Log("Local: "+ gameobject_to_place.transform.localPosition.ToString());
		//gameobject_to_place = Instantiate(gameobject_to_place, position_to_place, rotation) as GameObject;
		
		return obj;
	}
	void AdjustPositionRotation(GameObject obj, Vector3 position_to_place,  Quaternion rotation, string ID){
		//Update the Pooled Object with the new informations
		obj.transform.name = ID;
		obj.transform.tag = "PathElement";
		obj.transform.rotation = rotation;
		obj.transform.position = position_to_place;
		//A workaround for activating the colliders properly
		obj.GetComponent<Collider>().enabled = true;
		obj.GetComponent<Collider>().enabled = false;
		obj.GetComponent<Collider>().enabled = true;
		//Make it visable
		obj.GetComponent<Renderer>().enabled = true;
		
	}
	/// <summary>
	/// Selects the GameObject depending on the chosen enum Parts item.
	/// </summary>
	/// <param name="parts">
	/// The element from the enum identifying an array.
	/// </param>
	/// <returns>
	/// <Returns the randomly choosen GameObject from the appropriate serialised array.>
	/// </returns>
	public GameObject GetGameObjectParts(Parts parts){
		switch (parts)
		{
		case Parts.Beginning:
			return parts_straight_begin[UnityEngine.Random.Range(0, parts_straight_begin.Length)];
			break;
		case Parts.CornerRight:
			return parts_corner_right[UnityEngine.Random.Range(0, parts_corner_right.Length)];
			break;
		case Parts.CornerLeft:
			return parts_corner_left[UnityEngine.Random.Range(0, parts_corner_left.Length)];
			break;
		case Parts.Fork:
			return parts_corner_fork[UnityEngine.Random.Range(0,parts_corner_fork.Length)];
			break;
		case Parts.Forward:
			return parts_straight_forward[UnityEngine.Random.Range(0, parts_straight_forward.Length)];
			break;
		case Parts.Stairway:
			return parts_stairway[UnityEngine.Random.Range(0, parts_stairway.Length)];
		default:
			Debug.Log("No proper building mode used (FALLBACK)!");
			return parts_straight_forward[UnityEngine.Random.Range(0, parts_straight_forward.Length)];
		}
	}
	/// <summary>
	/// Plans the next element.
	/// </summary>
	/// <param name="previous">Previous.</param>
	/// <param name="cellCount">Cell count.</param>
	private void PlanNextElement(InstantiatedObject previous, int cellCount)
	{
		
		// Create an instance of the class Instantiated Object
		InstantiatedObject newCell = new InstantiatedObject(Vector3.zero, Direction.Forward, Parts.Forward);
		
		InstantiatedObject prevCell	= previous;
		
		
		/*New local variables*/
		Parts mypart;
		Direction mydirection;
		Quaternion myrotation;
		
		// Assign old Direction
		mydirection = prevCell.CellDirection;
		// Assign old Rotation
		myrotation = prevCell.Rotation;
		//Check if a straight part needs to be placed. This decission is made depending on the min and maximum amount set in the inspector.
		if(amount_of_elements[(int)Parts.Forward]<i_min_same_parts || amount_of_elements[(int)Parts.Forward] == null){
			mypart = Parts.Forward;
			myrotation = GetRotationOfElement(mydirection, prevCell.CellDirection, mypart);
		}
		else{
			bool force;
			if(amount_of_elements[(int)Parts.Forward]>= i_max_same_parts){
				force = true;
			}
			else{
				force = false;
			}
			
			//Choose Part randomly
			mypart = RandomlyChooseElement(force);
			//Choose if Direction needs to be changed
			mydirection = RandomlyChooseDirection(mypart, mydirection);
			//Change the rotation acording to the direction, necessary for corners and all the straight elements
			myrotation = GetRotationOfElement(mydirection, prevCell.CellDirection, mypart);
			//Reset the parts.Forward counter
			if(mypart != Parts.Forward){
				amount_of_elements[(int)Parts.Forward] = 0;
			}
		}
		//Assign the type
		newCell.Type = mypart;
		//Overwrite default Direction
		newCell.CellDirection = mydirection;
		//Overwrite Rotation
		newCell.Rotation = myrotation;
		newCell.CellModel = PlaceElement(newCell.Type);
		//Assign the appropiate Position, depending on direction
		newCell.StartPosition = prevCell.EndPosition;
		
		Vector3 median;
		Vector3 end;
		Vector3 end2;
		
		GetPositionOfPoint(newCell.StartPosition, newCell.CellModel, newCell.Rotation, newCell.Type, newCell.CellDirection, prevCell.CellDirection, out median, out end, out end2);
		
		newCell.MedianPoint = median;
		newCell.EndPosition = end;
		newCell.EndPosition2 = end2;
		//Set Endpoint2
		
		if(prevCell.Type == Parts.Fork){
			
			
			CreateFork(newCell.CellDirection);
		}
		//Get the Name for the Ojbect
		newCell.ID = UniqueID();
		CheckForCollision(newCell.StartPosition, newCell.EndPosition,newCell.CellDirection, newCell.ID);
		//Create the dependency
		if(create_fork == true){
			//Create an Instance
			InstantiatedObject newCellFork = new InstantiatedObject(Vector3.zero, Direction.Forward, Parts.Forward);
			//Copy the type
			newCellFork.Type = newCell.Type;
			//Add to D to clearify that its a dependency object
			newCellFork.ID = "D"+newCell.ID;
			//Copy the Rotation from the main object
			newCellFork.Rotation = newCell.Rotation;
			
			
			
			if(newCell.CellDirection == direction_after_fork ){
				
				newCellFork.CellDirection = InverseDirection(newCell.CellDirection);
				
			}
			else{
				//Keep Direction
				newCellFork.CellDirection = newCell.CellDirection;
			}
			
			//Need to be reworked
			if((newCell.Type == Parts.CornerLeft || newCell.Type == Parts.CornerRight)){
				
				if(prevCell.CellDirection == direction_after_fork || newCell.CellDirection == direction_after_fork){
					//Invert Type
					Parts type;
					if(newCell.Type == Parts.CornerRight){
						type = Parts.CornerLeft;
					}
					else {
						type = Parts.CornerRight;
					}
					//Get new Rotation
					Quaternion rot;
					//Cell Model
					GameObject cm;
					//Recalculate Rotation mind the Dependency previous position
					rot = GetRotationOfElement(newCellFork.CellDirection, prevCell.Dependency.CellDirection, type);
					
					//Assign the type
					newCellFork.Type = type;
					//Assign the rotation
					newCellFork.Rotation = rot;
					
					
				}
				
				
			}
			
			if(newCellFork.Type == Parts.Stairway && newCell.CellDirection == direction_after_fork){
				
				newCellFork.Rotation =GetRotationOfElement(newCellFork.CellDirection, prevCell.Dependency.CellDirection, newCellFork.Type);
			}
			
			Vector3 pos;
			Direction prevdir;
			prevdir = newCellFork.CellDirection;
			if(prevCell.Dependency != null){
				//A dependency element was previously placed
				pos = prevCell.Dependency.EndPosition;
				if(newCell.Type == Parts.CornerLeft || newCell.Type == Parts.CornerRight){
					//Corners always need the old direction as a reference to place
					prevdir = prevCell.Dependency.CellDirection;
				}
				
			}
			else{
				//Use the Position of the previous cell
				pos=prevCell.EndPosition2;
				
			}
			newCellFork.CellModel = PlaceElement(newCellFork.Type);
			
			median=Vector3.zero;
			end=Vector3.zero;
			end2=Vector3.zero;
			
			GetPositionOfPoint(pos, newCellFork.CellModel, newCellFork.Rotation, newCellFork.Type, newCellFork.CellDirection, prevdir, out median, out end, out end2);
			
			newCellFork.MedianPoint = median;
			newCellFork.EndPosition = end;
			newCellFork.EndPosition2 = end2;
			//Set Endpoint2
			
			//Set newCellFork as the dependency
			newCell.Dependency = newCellFork;
			
			
		}
		
		//Add the Element to the list
		list_instiated_object.Add(newCell);
		
		
		
		//Increase the counter for the Part
		amount_of_elements[(int)mypart] = amount_of_elements[(int)mypart] + 1;
		
	}
	private Direction InverseDirection(Direction mydirection){
		Direction dir;
		dir = mydirection;
		if(mydirection == Direction.Right){
			dir = Direction.Left;
		}
		else if(mydirection == Direction.Left){
			dir = Direction.Right;
		}
		else if(mydirection == Direction.Forward){
			dir = Direction.Backward;
		}
		else if(mydirection == Direction.Backward){
			dir = Direction.Forward;
		}
		return dir;
	}
	
	private void CreateFork(Direction mydirection){
		if(create_fork == false){
			direction_after_fork = mydirection;
			create_fork = true;
		}
	}
	
	private void GetPositionOfPoint(Vector3 sourcePosition, GameObject sourceObject, Quaternion rotation, Parts type, Direction direction, Direction prevDirection, out Vector3 MedianPoint, out Vector3 EndPoint, out Vector3 EndPoint2){
		
		Vector3 MedianPointSize = (sourceObject.gameObject.transform.GetChild(1).localPosition - sourceObject.gameObject.transform.GetChild(0).localPosition);
	
		//Adjust the position of the Median Point
		//MedianPointSize = new Vector3(MedianPointSize.x+ sourceObject.renderer.bounds.center.x, MedianPointSize.y, MedianPointSize.z + sourceObject.renderer.bounds.center.z);
		//Vector3 EndPointSize = (sourceObject.gameObject.transform.GetChild(1).localPosition + sourceObject.gameObject.transform.GetChild(2).localPosition);
		Vector3 EndPointSize = (sourceObject.gameObject.transform.GetChild(2).localPosition);
		
		
		Vector3 EndPointSize2 = Vector3.zero;
		
		if(type == Parts.Fork){
			EndPointSize2 = (sourceObject.gameObject.transform.GetChild(3).localPosition);
		}
		
		Direction directionreset;
		directionreset = direction;
		//Keep old Direction if its corner
		
		
		if(type == Parts.CornerLeft || type == Parts.CornerRight || type == Parts.Fork){
			direction = prevDirection;
			
		}
		
		if(direction == Direction.Left || direction == Direction.Right){
			//Switch X and Z Coordinate
			Vector3 tempSize;
			//Set Median Points
			tempSize = MedianPointSize;
			MedianPointSize.x = tempSize.z;
			MedianPointSize.z = tempSize.x;
			//Set EndPoints
			tempSize = EndPointSize;
			EndPointSize.x = tempSize.z;
			EndPointSize.z = tempSize.x;
			//EndPoint2
			tempSize = EndPointSize2;
			EndPointSize2.x = tempSize.z;
			EndPointSize2.z = tempSize.x;
			
			if(direction == Direction.Right){
				MedianPointSize.z = (-1)*MedianPointSize.z;
				EndPointSize.z = (-1)*EndPointSize.z;
				EndPointSize2.z = (-1)*EndPointSize2.z;
			}
			if(direction == Direction.Left){
				MedianPointSize.x = (-1)*MedianPointSize.x;
				EndPointSize.x = (-1)*EndPointSize.x;
				EndPointSize2.x = (-1)*EndPointSize2.x;
			}
			
		}
		if(direction == Direction.Backward){
			//negative Positions
			MedianPointSize.x = (-1)*MedianPointSize.x;
			MedianPointSize.z = (-1)*MedianPointSize.z;
			EndPointSize.x = (-1)*EndPointSize.x;
			EndPointSize.z = (-1)*EndPointSize.z;
			EndPointSize2.x = (-1)*EndPointSize2.x;
			EndPointSize2.z = (-1)*EndPointSize2.z;
		}
		Vector3 pos;
		pos = Vector3.zero;
		
		//Calculate the MedianPoint
		
		pos = new Vector3(sourcePosition.x + MedianPointSize.x, sourcePosition.y + MedianPointSize.y, sourcePosition.z +MedianPointSize.z);
		
		MedianPoint = pos;
		sourcePosition = MedianPoint;
		pos = Vector3.zero;
		//Calculate the EndPoint
		
		pos = new Vector3(sourcePosition.x + EndPointSize.x, sourcePosition.y + EndPointSize.y, sourcePosition.z+ EndPointSize.z);
		EndPoint = pos;
		//Reset Pos calc EndPoint2 for the Fork
		pos = new Vector3(sourcePosition.x+EndPointSize2.x, sourcePosition.y + EndPointSize2.y, sourcePosition.z + EndPointSize2.z);
		EndPoint2 = pos;
		
		
		
		
	}
	
	
	/// <summary>
	/// Gets the rotation of element.
	/// </summary>
	/// <returns>The rotation of element.</returns>
	/// <param name="mydirection">Mydirection.</param>
	/// <param name="olddirection">Olddirection.</param>
	private Quaternion GetRotationOfElement(Direction mydirection, Direction prevdirection, Parts mypart){
		//Debug.Log(prevdirection.ToString() + " - " + mydirection.ToString());
		
		float rotation;
		rotation = 0;
		
		//Rotate straight elements
		rotation = ((float)prevdirection * 90);
		
		
		if(mypart == Parts.CornerLeft || mypart == Parts.CornerRight || mypart == Parts.Fork || mypart == Parts.Stairway){
			
			//Debug.Log( "Prev. Direction: " + prevdirection.ToString() + " New Direction: " + mydirection.ToString() +" Part: "+mypart.ToString());
			
			
			if(mypart == Parts.CornerLeft){
				if(mydirection == Direction.Left && prevdirection == Direction.Forward){
					rotation = 0;
				}
				else if(mydirection == Direction.Backward && prevdirection == Direction.Left){
					rotation = 270;
				}
				else if(mydirection == Direction.Forward && prevdirection == Direction.Right){
					rotation = 90;
				}
				else if(mydirection == Direction.Right && prevdirection == Direction.Backward){
					rotation = 180;
				}
				
			}
			if(mypart == Parts.CornerRight){
				if(mydirection == Direction.Right && prevdirection == Direction.Forward){
					rotation = 0;
				}
				if(mydirection == Direction.Backward && prevdirection == Direction.Right){
					rotation = 90;
				}
				if(mydirection == Direction.Forward && prevdirection == Direction.Left){
					rotation = 270;
				}
				if(mydirection == Direction.Left && prevdirection == Direction.Backward){
					rotation = 180;
				}
			}
			if(mypart == Parts.Fork){
				if(mydirection == Direction.Left && prevdirection == Direction.Forward){
					rotation = 0;
				}
				else if(mydirection == Direction.Forward && prevdirection == Direction.Right){
					rotation = 90;
				}
				else if(mydirection == Direction.Backward && prevdirection == Direction.Left){
					rotation = 270;
				}
				else if(mydirection == Direction.Right && prevdirection == Direction.Backward){
					rotation = 180;
				}
			}
			if(mypart == Parts.Stairway){
				if(mydirection == Direction.Left){
					rotation = 270;
				}
				else if(mydirection == Direction.Forward){
					rotation = 0;
				}
				else if(mydirection == Direction.Backward){
					rotation = 180;
				}
				else if(mydirection == Direction.Right){
					rotation = 90;
				}
			}
		}
		
		
		
		//Return the new rotation
		return Quaternion.Euler(0,rotation, 0);
		
	}
	/// <summary>
	/// Randomly the chooses a Part type depending on if its forced to and if another fork element already exists.
	/// </summary>
	/// <returns>Returns a randomly choosen element</returns>
	/// <param name="force">If set to <c>true</c> it will pick different object than forward or stairs</param>
	private Parts RandomlyChooseElement(bool force){
		Parts mypart;
		//I am starting at index one because I want to place the the beginning part only once
		mypart = (Parts)(UnityEngine.Random.Range(1, Enum.GetNames(typeof(Parts)).Length));
		
		if(force == true && mypart == Parts.Forward && mypart == Parts.Stairway){
			if(CheckIfForkExists() == true){
				mypart = RandomlyChooseCorner();
				
			}
			else{
				if(UnityEngine.Random.Range(0,2)==1){
					mypart = RandomlyChooseCorner();
					
				}
				else{
					mypart = Parts.Fork;
					
				}
			}
		}
		else {
			if(mypart == Parts.Fork && CheckIfForkExists() == true){
				mypart = Parts.Forward;
			}
			//Otherwise it will return any part like stairs etc
		}
		return mypart;
		
	}
	private Parts RandomlyChooseCorner(){
		float random = UnityEngine.Random.Range(0,2);
		if (random == 0) {
			return Parts.CornerRight;
		}
		else{
			return Parts.CornerLeft;
		}
	}
	private Direction RandomlyChooseDirection(Parts mypart, Direction currentDirection){
		
		Direction dir;
		dir =  (Direction)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Direction)).Length));
		if(mypart == Parts.CornerRight || mypart == Parts.CornerLeft){
			if(mypart == Parts.CornerRight){
				//Right
				switch (currentDirection)
				{
				case Direction.Forward:
					dir= Direction.Right;
					break;
				case Direction.Backward:
					dir = Direction.Left;
					break;
				case Direction.Right:
					dir = Direction.Backward;
					break;
				case Direction.Left:
					dir = Direction.Forward;
					break;
					
				}
				
			}
			else if(mypart == Parts.CornerLeft){
				//Left
				switch (currentDirection)
				{
				case Direction.Forward:
					dir= Direction.Left;
					break;
				case Direction.Backward:
					dir = Direction.Right;
					break;
				case Direction.Right:
					dir = Direction.Forward;
					break;
				case Direction.Left:
					dir = Direction.Backward;
					break;
					
				}
				
				
			}
		}
		else if(mypart == Parts.Forward || mypart == Parts.Stairway){
			//Keep direction
			dir = currentDirection;
		}
		else if(mypart == Parts.Fork){
			//Preference
			switch (currentDirection)
			{
			case Direction.Forward:
				dir= Direction.Left;
				break;
			case Direction.Backward:
				dir = Direction.Right;
				break;
			case Direction.Right:
				dir = Direction.Forward;
				break;
			case Direction.Left:
				dir = Direction.Backward;
				break;
				
			}
		}
		//Debug.Log("Current Dir: " +  currentDirection.ToString()+ " Previous Dir: " + dir.ToString()+ " Part: " + mypart.ToString());
		return dir;
	}
	private void CreateCellModel(InstantiatedObject Element)
	{
		
		if (Element.CellModel != null)
		{
			//Place the Element with the correct Type, Position and Rotation
			
			AdjustPositionRotation(Element.CellModel,Element.MedianPoint,Element.Rotation,Element.ID);
			//Create a mirror Object
			//if(Element.Dependency != null && bool_place_dependency == true){
			if(Element.Dependency != null && bool_place_dependency == true){
				//Debug.Log("Dependency to solve");
				//Element.Dependency.Type
				AdjustPositionRotation(Element.Dependency.CellModel, Element.Dependency.MedianPoint, Element.Dependency.Rotation, Element.Dependency.ID);
			}
			
		}
		
	}
	
	/// <summary>
	/// Checks if it is a fork. If its a fork it will return the State.
	/// </summary>
	/// <returns>The State</returns>
	/// <param name="ID">Name of the object</param>
	
	
	public void SetCurrentCell(string ID, Vector3 pos){
		//Check if not null
		
		if(ID != null && ID != ""){
			InstantiatedObject result;
			
			if(ID.Contains("DID")== true){
				//If it is an dependency object check with parent
				ID = ID.Replace("DID","ID");
			}
			//Get the Cell by the ID property passed as an string
			result = list_instiated_object.Find(delegate(InstantiatedObject io){return io.ID == ID;});
			
			
			if (result != null)
			{
				m_playerCell_previous = m_playerCell;
				m_playerCell = result;
				//Set the visited flag
				list_instiated_object[list_instiated_object.IndexOf(m_playerCell)].Visited = true;
				//Check if the last element was a fork Element
				CheckIfIsFork(pos);
				
			}
			
		}
		
	}
	/// <summary>
	/// Checks if is fork.
	/// </summary>
	/// <param name="pos">Position.</param>
	public void CheckIfIsFork(Vector3 pos){
		if(m_playerCell_previous != null && list_instiated_object[list_instiated_object.IndexOf(m_playerCell_previous)].Type== Parts.Fork){
			//Get last ID of current builded elements
			//Compare Position Check if it is Dependent or root
			if(m_playerCell.Dependency.MedianPoint == pos){
				Debug.Log("Fork Route");
				//Set the Mainfork to the last position of the dependency
				list_instiated_object[list_instiated_object.Count-1] = list_instiated_object[list_instiated_object.Count-1].Dependency;
				
				create_fork = false;
				
			}
			else{
				Debug.Log("Main Route");
				
				create_fork = false;
			}
			
		}
		
	}
	/// <summary>
	/// Creates an unique ID for each GameObject
	/// </summary>
	/// <returns>The generated name</returns>
	public string UniqueID(){
		int i = i_global_ID;
		//Increase GlobalID
		i_global_ID = i_global_ID + 1;
		return "ID" + i.ToString();
	}
	
	private bool CheckForCollision(Vector3 startposition, Vector3 endposition, Direction direction, string ID){


			Collider[] hitColliders = Physics.OverlapSphere(startposition, collision_distance);
			int i = 0;

			while (i < hitColliders.Length) {
				if(hitColliders[i].tag == "PathElement"){
					Debug.Log("Collision("+ID+"):" + hitColliders[i].name);
				}
				i++;
			}
	
		return true;
		
	}
	
	
	
	
	
}
