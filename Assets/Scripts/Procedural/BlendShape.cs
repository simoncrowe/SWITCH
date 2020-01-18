using UnityEngine;
[RequireComponent (typeof (MeshFilter))]
public abstract class BlendShape : MonoBehaviour {
	public Mesh[] shapes;
	public float[] weights;
	protected Vector3[][] differenceVerts;
	protected Vector3[] baseVerts;
	protected Mesh baseMesh;
	
	void Start () {
		MeshFilter baseMeshFilter = 
			(MeshFilter)this.GetComponent("MeshFilter");
		baseMesh = baseMeshFilter.mesh;
		baseVerts = new Vector3[baseMesh.vertices.Length];
		for (int vertIdx = 0; vertIdx < baseVerts.Length; vertIdx ++) {
			baseVerts[vertIdx] = baseMesh.vertices[vertIdx];	
		}
		differenceVerts = new Vector3[shapes.Length][];
		for (int shapeIdx = 0; shapeIdx < shapes.Length; shapeIdx ++) {
			differenceVerts[shapeIdx] = 
				new Vector3[shapes[shapeIdx].vertices.Length];
			for (int i = 0; i < differenceVerts[shapeIdx].Length; i ++) {
				differenceVerts[shapeIdx][i] = 
					shapes[shapeIdx].vertices[i] - baseVerts[i];
				// For each vertex in each blend shape mesh, we calculate 
				// and store the difference between that vertex's position 
				// and the position of the corrosponding vertex in the base
				// mesh.
			}
		}
		Initialize(); 	// The Initialize method must be overriden in any 
						//class that inherits from this one.
	}
	void Update () {
		ProcessWeights(); 	// ProcessWeights can be overriden and filled 
							// with anything that needs to be done every 
							// frame to ensure wieghts are correct.
		Vector3 addendVert;
		Vector3[] newVerts = new Vector3[baseMesh.vertices.Length];
		for (int vertIdx = 0; vertIdx < baseVerts.Length; vertIdx ++) {
			addendVert = Vector3.zero;
			for (int shapeIdx = 0; 
				shapeIdx < shapes.Length; shapeIdx ++) {
				addendVert += differenceVerts[shapeIdx][vertIdx]
				* weights[shapeIdx];
				// The differences in poition we calcualted earlier are 
				// added together for each vertex - each multiplied by 
				// a particular blend shapes's weight.
			}
			newVerts[vertIdx] = baseVerts[vertIdx] + addendVert;
			// The sum of all weighted differences are added to the base 
			// vertices, producing blended vertex positions!
		}
		baseMesh.vertices = newVerts;
	}
	protected abstract void ProcessWeights();
	protected abstract void Initialize();
}