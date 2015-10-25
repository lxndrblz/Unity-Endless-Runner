/*
@(#)File:           EditorList.cs
@(#)Version:        Version 1
@(#)Last changed:   30.09.2014 19:54
@(#)Purpose:        
@(#)Author:         Alexander Bilz
@(#)Product:        Endless Runner
@(#)Language: 		C-Sharp
*/

using UnityEditor;
using UnityEngine;
//The class System is needed to get the flag options
using System;

[Flags]
public enum EditorListOption {
	None = 0,
	ListSize = 1,
	ListLabel = 2,
	ElementLabels = 4,
	Buttons = 8,
	Default = ListSize | ListLabel | ElementLabels,
	NoElementLabels = ListSize | ListLabel,
	All = Default | Buttons
}

public static class EditorList {

	private static GUIContent
		//Set the title and tooltips for the different buttons
		duplicateButtonContent = new GUIContent("+", "Add"),
		deleteButtonContent = new GUIContent("-", "Delete"),
		addButtonContent = new GUIContent("+", "Add first element");

	//Set the width of the buttons
	private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

	public static void Show (SerializedProperty list, string str_lbl, string tooltip, EditorListOption options = EditorListOption.Default) {

		bool showListLabel = (options & EditorListOption.ListLabel) != 0;
		bool showListSize = (options & EditorListOption.ListSize) != 0;
		if (showListLabel) {
			EditorGUILayout.PropertyField(list, new GUIContent(str_lbl, tooltip));
			EditorGUI.indentLevel += 1;
		}
		if (!showListLabel || list.isExpanded) {
			SerializedProperty size = list.FindPropertyRelative("Array.size");
			if (showListSize) {
				EditorGUILayout.PropertyField(size);
			}
			if (size.hasMultipleDifferentValues) {
				EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
			}
			else {
				ShowElements(list, options);
			}
		}
		if (showListLabel) {
			EditorGUI.indentLevel -= 1;
		}
	}

	private static void ShowElements (SerializedProperty list, EditorListOption options) {

		bool showButtons = (options & EditorListOption.Buttons) != 0;
		/*
		 * Loop through each element in the array
		 */
		for (int i = 0; i < list.arraySize; i++) {
				//Place the objects side by side
				EditorGUILayout.BeginHorizontal();
				//Create an Input Slot
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));

				//Show both buttons plus and minus
				ShowButtons(list, i);
				EditorGUILayout.EndHorizontal();
				//Continue placing the objects below
		}
		if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton)) {
			list.arraySize += 1;
		}
	}

	/*
	 * 
	 * Handle the Part that is responsible for adding and removing additional elements
	 * 
	 * 
	 */

	private static void ShowButtons (SerializedProperty list, int index) {
		/*
		 * 
		 * If a new element will be added to the array of gameobjects
		 * 
		 */
		if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth)) {
			list.InsertArrayElementAtIndex(index);
		}
		/*
		 * 
		 * An element will be removed from the array of gameobjects
		 * 
		 */
		if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth)) {
			int oldSize = list.arraySize;
			list.DeleteArrayElementAtIndex(index);
			//reset the array size
			if (list.arraySize == oldSize) {
				list.DeleteArrayElementAtIndex(index);
			}
		}
	}
}