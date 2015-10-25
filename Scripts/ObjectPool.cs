using UnityEngine;
using System;
//using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {

	public static ObjectPool current;
	//Amount of elements pooled per type
	public int pooledAmount = 5;
	public bool willGrow = true;
	//private List<pool> pooledObjects;
	private List<pool> pooledObjects = new List<pool>();
	public class pool 
		{
		public GameObject go { get; set; }
		public EndlessRunner.Parts type { get; set; }

		}


	// Use this for initialization
	void Start () {
		//pooledObjects = new List<pool>();


		for (int ii = 0; ii < Enum.GetNames(typeof(EndlessRunner.Parts)).Length; ii++) {
			
					
			for (int i = 0; i < pooledAmount; i++) {
				EndlessRunner.Parts mytype;
				mytype = (EndlessRunner.Parts)ii;
				GameObject obj = (GameObject)Instantiate(EndlessRunner.currentEndlessRunner.GetGameObjectParts(mytype));
				obj.SetActive(false);
				pooledObjects.Add(new pool(){go=obj, type = mytype});
			}
			
		}
	}
	void Awake(){
		current = this;
	}
	public GameObject GetPooledObject(EndlessRunner.Parts typeToPool){

			for (int i = 0; i < pooledObjects.Count; i++) {
				if (!pooledObjects[i].go.activeInHierarchy && pooledObjects[i].type == typeToPool) {
					return pooledObjects[i].go;
				}
			}
		if (willGrow) {
			//Randomly select an element out of one list
			GameObject obj = (GameObject)Instantiate(EndlessRunner.currentEndlessRunner.GetGameObjectParts(typeToPool));
			pooledObjects.Add(new pool(){go=obj, type = typeToPool});
			return obj;
		}
		return null;
	}
}
