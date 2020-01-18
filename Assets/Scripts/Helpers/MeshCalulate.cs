using UnityEngine;

public static class MeshCalulate {

	public static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3) {
		//simplified low-level implementation of p.Dot(p2.Cross(p3))/6f
		float v321 = p3.x * p2.y * p1.z;
		float v231 = p2.x * p3.y * p1.z;
		float v312 = p3.x * p1.y * p2.z;
		float v132 = p1.x * p3.y * p2.z;
		float v213 = p2.x * p1.y * p3.z;
		float v123 = p1.x * p2.y * p3.z;
		return (-v321 + v231 + v312 - v132 - v213 + v123)/6f; 
		//Source formula multiplied by 1/6 - unsure if important
	}

	public static float Volume(Mesh mesh) {
		float volume = 0;
		for (int triangleIndex = 0; triangleIndex < mesh.triangles.Length; triangleIndex += 3) {
			volume += SignedVolumeOfTriangle(mesh.vertices[mesh.triangles[triangleIndex]],
			                                 mesh.vertices[mesh.triangles[triangleIndex+1]],
			                                 mesh.vertices[mesh.triangles[triangleIndex+2]]);
		}
		return Mathf.Abs(volume);
	}

}
