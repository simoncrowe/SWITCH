using UnityEngine;
using System.Collections;

public class SinkRippleTexture : RippleTexture {

	public Faucet faucet;
	public float planeWidth = 1f;
	public float planeDepth = 1f;
	Mesh plane;
	float nextSplash = 0f;
	public float minSplashInterval = 0.05f;
	public float maxSplashInterval = 0.1f;
	

	protected override void Initialisation () {
		ConstructPlane ();
		pixelsX = (int)(nmWidth*plane.uv[1].x);
		pixelsY = (int)(nmHeight*plane.uv[2].y);
	}

    protected override void UpdateLogic() {
		if (faucet.TotalWaterRate > 0) {
			if (nextSplash < Time.time){
				SplashAtPoint((pixelsX/2) + Random.Range(-1,1),(pixelsY/2) + Random.Range(-8,-6),
					(int)(splashForce*faucet.TotalWaterRate));
				nextSplash = Time.time + Random.Range (minSplashInterval,maxSplashInterval);
			}
		}
    }

    void ConstructPlane () {
		Vector3[] verts = new Vector3 [] {
			new Vector3 (-(planeWidth/2),0,-(planeDepth/2)),
			new Vector3 (planeWidth-(planeWidth/2),0,-(planeDepth/2)),
			new Vector3 (-(planeWidth/2),0,planeDepth-(planeDepth/2)), 
			new Vector3	(planeWidth-(planeWidth/2),0,planeDepth-(planeDepth/2))};
		Vector3[] normals = new Vector3[] {
			new Vector3 (0,1,0),
			new Vector3 (0,1,0),
			new Vector3 (0,1,0),
			new Vector3 (0,1,0)};
		int[] tris = new int[] {2,1,0,1,2,3};
		Vector2[] uvs = new Vector2[] {
			new Vector2 (0,0),
			new Vector2 ((planeWidth > planeDepth)? 1 : planeWidth/planeDepth,0),
			new Vector2 (0,(planeDepth > planeWidth)? 1 : planeDepth/planeWidth),
			new Vector2 ((planeWidth > planeDepth)? 1 : planeWidth/planeDepth,
			             (planeDepth > planeWidth)? 1 : planeDepth/planeWidth)};
		
		plane = new Mesh();
		plane.vertices = verts;
		plane.normals = normals;
		plane.triangles = tris;
		plane.uv = uvs;
		GetComponent<MeshFilter>().mesh = plane;
		plane.RecalculateBounds();
		TangentSolver.Solve(plane);
		GetComponent<MeshCollider>().sharedMesh = plane;
		
	}
}