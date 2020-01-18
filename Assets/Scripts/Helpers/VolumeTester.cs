using UnityEngine;
using System.Collections;

public class VolumeTester : MonoBehaviour {

	MeshFilter meshFilter;

	void Start () {
		meshFilter = (MeshFilter)this.GetComponent(typeof(MeshFilter));
		print ("The volume of the mesh is " + MeshCalulate.Volume(meshFilter.mesh));
	}
	

}
