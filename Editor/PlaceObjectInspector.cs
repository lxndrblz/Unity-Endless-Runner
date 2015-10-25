/*
@(#)File:           PlaceObjectInspector.cs
@(#)Version:        Version 1
@(#)Last changed:   30.09.2014 19:47
@(#)Purpose:        
@(#)Author:         Alexander Bilz
@(#)Product:        Endless Runner
@(#)Language: 		C-Sharp
*/


using UnityEditor;
using UnityEngine;
/*
 * Important to add CustomEditor to tell Unity that the Custom editor will be applied to the appropiate class.
 * Also mind that we are using the editor class instead of MonoBehaviour
 */

[CustomEditor(typeof(EndlessRunner))]
public class PlaceObjectInspector : Editor {

	/*
	 * Responsible for drawing elements in the inspector panel 
	 */

	public override void OnInspectorGUI () {

		/*
		 * Show all variables that we need in the inspector panel by calling the function show. The variables below are bascally all the public variables 
		 * from the Place_Objects class. To make sure that a nice title and tooltip is showing up I am passing it to function show.
		 */
		serializedObject.Update();
		EditorList.Show(serializedObject.FindProperty("i_min_same_parts"),"Min. amount","Minimum amount of elements placed straight");
		EditorList.Show(serializedObject.FindProperty("i_max_same_parts"),"Max. amount","Maximum amount of elements placed straight");
		EditorList.Show(serializedObject.FindProperty("player"),"Player","GameObject where the PlayerScript is attached to");
		EditorList.Show(serializedObject.FindProperty("i_amount_of_objects_prebuilt"),"Amount of subsections","Amount of subsections that is prebuilt");
		EditorList.Show(serializedObject.FindProperty("collision_distance"),"Distance Collision","The distance to check for collisions");
		EditorList.Show(serializedObject.FindProperty("bool_place_dependency"),"Instantiate Dependency","Don't place the dependency of the fork");
		//The arrays for the different building assets
		EditorList.Show(serializedObject.FindProperty("parts_straight_begin"),"Begin elements","Add all your begin elements", EditorListOption.ListLabel | EditorListOption.Buttons);
		EditorList.Show(serializedObject.FindProperty("parts_straight_forward"), "Elements facing forwards","Add all your elements facing forwards",EditorListOption.ListLabel | EditorListOption.Buttons);
		EditorList.Show(serializedObject.FindProperty("parts_corner_right"), "Corner Elements Right","Add all your corners facing to the right side",EditorListOption.ListLabel | EditorListOption.Buttons);
		EditorList.Show(serializedObject.FindProperty("parts_corner_left"), "Corner Elements Left","Add all your corners facing to the left side",EditorListOption.ListLabel | EditorListOption.Buttons);
		EditorList.Show(serializedObject.FindProperty("parts_corner_fork"),"Fork elements","Add all your fork elements", EditorListOption.ListLabel | EditorListOption.Buttons);	
		EditorList.Show(serializedObject.FindProperty("parts_stairway"),"Stairway elements","Add all your stairway assets", EditorListOption.ListLabel | EditorListOption.Buttons);
		//EditorUtility.DisplayDialog("Min-Max Error","Please change the max or the min value! ");


		serializedObject.ApplyModifiedProperties();
	}

}
