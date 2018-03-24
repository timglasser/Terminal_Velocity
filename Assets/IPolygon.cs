using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class IPolygon : MonoBehaviour {

		protected Vector3[] Vertices; // this is called a vertex buffer
		protected Vector3[] Normals;
		protected Vector2[] UV;
		protected int[] Triangles;
		protected int _numsides;
		protected Mesh _polymesh; // the Unity Mesh component
		//public static numShapes;
		void Start () {}
		void Update () {}
}
