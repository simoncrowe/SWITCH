using UnityEngine;
using System.Collections.Generic;

public class RippleMesh : MonoBehaviour {
	
	public int verticesX = 32;
	public int verticesZ = 32;
	public float Spacing = 1f;
	public float waveScale = 0.001f;
	public int splashForce = 1000;
	public int damper = 3;

	private Mesh mesh;
	private int[] buffer0;
	private int[] buffer1;
	private bool swap = true;
	int[] currentBuffer;
	
	void ContructMesh () {
		Vector3[] vertices = new Vector3 [verticesX*verticesZ];
	 	Vector2[] uvs = new Vector2 [verticesX*verticesZ];
	 	Vector3[] normals = new Vector3[verticesX*verticesZ];
		mesh =  new Mesh();
		int counter = 0;
		for (int x = 0; x < verticesX; x ++){
			for (int z = 0; z < verticesZ; z ++){
				vertices[counter] = new Vector3(x*Spacing,0,z*Spacing);
				uvs[counter] = new Vector2((float)x/verticesX,(float)z/verticesZ);
				normals[counter] = new Vector3(0,1,0);
				counter ++;
			}
		}
		counter = 0;
		List<int> tris = new List<int>();
		for (int rows = 0; rows < verticesX-1; rows++){
			for (int collums = 0; collums < verticesZ-1; collums++){
				tris.Add(counter);
				tris.Add(counter+1);
				tris.Add(counter+verticesZ);
				
				tris.Add(counter+verticesZ+1);
				tris.Add(counter+verticesZ);
				tris.Add(counter+1);
				counter++;
			}
		counter++;
		}
		int[] trisArray = new int[tris.Count];
		tris.CopyTo(trisArray);
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.normals = normals;
		mesh.triangles = trisArray;
		GetComponent<MeshFilter>().mesh = mesh;
		;
		mesh.RecalculateBounds ();	
	}
	
	void Start () {
		ContructMesh ();
		buffer0 = new int[mesh.vertices.Length];
		buffer1 = new int[mesh.vertices.Length];
		InvokeRepeating ("TestSplash",.5f,30);
//		InvokeRepeating ("RippleSimulation",0,1);
	}
	
	void Update () {
		RippleSimulation();
	}
	
	void RippleSimulation () {
		if (swap) {
			processSurface(buffer0,buffer1);
			currentBuffer = buffer1;
		}
		else {
			processSurface(buffer1,buffer0);
			currentBuffer = buffer0;
		}
		swap = !swap;
		Vector3[] newVertices = new Vector3[mesh.vertices.Length];
		for (int i = 0; i < mesh.vertices.Length; i++){//Bottleneck
			newVertices[i] = mesh.vertices[i];
			newVertices[i].y = currentBuffer[i] * waveScale;
		}
		mesh.vertices = newVertices;
		mesh.RecalculateNormals();

	}
	
	void SplashAtPoint (int x, int z) {
		int position = ((x * verticesZ)+z);
		buffer0[position] = splashForce;
//		Moore Neighbourhood:
/*		buffer0[position + 1] = splashForce;
		buffer0[position - verticesZ + 1] = splashForce;
		buffer0[position - verticesZ] = splashForce;
		buffer0[position - verticesZ - 1] = splashForce;
		buffer0[position - 1] = splashForce;
		buffer0[position + verticesZ - 1] = splashForce;
		buffer0[position + verticesZ] = splashForce;
		buffer0[position + verticesZ + 1] = splashForce;*/
	}
	
	void processSurface(int[] source, int[] dest) {
	int position;
		for (int x = 1; x < verticesX - 1; x ++) {
			for (int z = 1; z < verticesZ - 1; z ++) {
				position = (x * (verticesZ)) + z;
				dest [position] = (((source[position - 1] + 
									 source[position + 1] + 
									 source[position - verticesZ] + 
									 source[position + verticesZ]) >> 2) - dest[position]);  
			   dest[position] -= (int)(dest[position] >> damper);
			}			
		}	
	}
	void TestSplash () {
		SplashAtPoint(Random.Range(1,verticesX-1),Random.Range(1,verticesZ-1));
	}
}