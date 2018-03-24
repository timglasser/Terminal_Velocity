using UnityEngine;
using System.Collections;

public class HexMesh : MonoBehaviour {

	public float height=2.0f;
	public float state;
	public Vector3 wcenter;
	public float rad=5.0f;
	private float hexWidth;
	
	public class Drawing{ 
    	public static Texture2D lineTex;
 
    	public static void DrawLine(Camera cam, Vector3 pointA, Vector3 pointB) { DrawLine(cam,pointA,pointB, GUI.contentColor, 1.0f); }
    	public static void DrawLine(Camera cam, Vector3 pointA, Vector3 pointB, Color color) { DrawLine(cam,pointA,pointB, color, 1.0f); }
	    public static void DrawLine(Camera cam, Vector3 pointA, Vector3 pointB, float width) { DrawLine(cam,pointA,pointB, GUI.contentColor, width); }
    	public static void DrawLine(Camera cam, Vector3 pointA, Vector3 pointB, Color color, float width){
       		Vector2 p1 = Vector2.zero;
       		p1.x = cam.WorldToScreenPoint(pointA).x;
       		p1.y = cam.WorldToScreenPoint(pointA).y;
			Vector2 p2 = Vector2.zero;
       		p2.x = cam.WorldToScreenPoint(pointB).x;
       		p2.y = cam.WorldToScreenPoint(pointB).y;
       		DrawLine(p1,p2,color,width);
    	}

    	public static void DrawLine(Rect rect) { DrawLine(rect, GUI.contentColor, 1.0f); }
    	public static void DrawLine(Rect rect, Color color) { DrawLine(rect, color, 1.0f); }
    	public static void DrawLine(Rect rect, float width) { DrawLine(rect, GUI.contentColor, width); }
    	public static void DrawLine(Rect rect, Color color, float width) { DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width); }
		public static void DrawLine(Vector2 pointA, Vector2 pointB) { DrawLine(pointA, pointB, GUI.contentColor, 1.0f); }
    	public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color) { DrawLine(pointA, pointB, color, 1.0f); }
    	public static void DrawLine(Vector2 pointA, Vector2 pointB, float width) { DrawLine(pointA, pointB, GUI.contentColor, width); }
    	public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
    	{
	       	pointA.x = (int)pointA.x; pointA.y = (int)pointA.y;
       		pointB.x = (int)pointB.x; pointB.y = (int)pointB.y;
 
       		if (!lineTex) { 
				lineTex = new Texture2D(1, 1);
			}
       		Color savedColor = GUI.color;
			GUI.color = color;
 
       		Matrix4x4 matrixBackup = GUI.matrix;
 
       		float angle = Mathf.Atan2(pointB.y-pointA.y, pointB.x-pointA.x)*180f/Mathf.PI;
			float length = (pointA-pointB).magnitude;
        	GUIUtility.RotateAroundPivot(angle, pointA);
        	GUI.DrawTexture(new Rect(pointA.x, pointA.y, length, width), lineTex);
 
        	GUI.matrix = matrixBackup;
        	GUI.color = savedColor;
    	}
	}

	
	// Use this for initialization
	void Start () {
		// square root of 3 gives width
		hexWidth = (1.732f * rad);

		float angle = 0.0f;
		for (int i = 0; i<6; i++){
			vertices[i] = new Vector3(0.0f, 0.0f, 0.0f);
			vertices[i].x = Mathf.Cos(angle)*rad;
			vertices[i].z = Mathf.Sin(angle)*rad;
			vertices[i].y = height;
			angle += 60.0f;
		}
				
	}
	
	// Draws a red line from the the world-space origin to the point (1, 0, 0)
	void  Update () {
		Vector3 currpos =Vector3.zero;
		for (int i=0; i < 10; i++){
			for (int j=0; j < 10; j++){
				drawHex(currpos, rad);
				currpos.x += 1.732f *rad;
			}
			
			currpos =Vector3.zero; 
			if (i%2==0)
				currpos.x += 1.732f *rad;
			currpos.z += i*3.0f*rad; 
		}
	}
	
	/** 
 	* Convert a screen point to a hex coordinate
 	*/
 	public Vector2 PointToCoord(float x, float z) {
 		x = (x - hexWidth/2.0f) / hexWidth;

 		float t1 = z / rad;
		float t2 = Mathf.Floor(x + t1);
 		float r = Mathf.Floor((Mathf.Floor(t1 - x) + t2) / 3.0f); 
 		float q = Mathf.Floor((Mathf.Floor(2.0f * x + 1.0f) + t2) / 3.0f) - r;

		return new Vector2((int) q, (int) r); 
 	}

	
	
	void drawHex(Vector3 pos, float rad){
		
		Vector3 currp, startp=Vector3.zero;
		
		float PI = 3.142f;
		float angle=0.0f;
		int i=0;
		
		for (i=0; i < 7; i++){
    		angle = 2.0f * PI / 6.0f * (i + 0.5f);
    		currp.x = pos.x + rad * Mathf.Cos(angle);
    		currp.z = pos.z + rad * Mathf.Sin(angle);
			currp.y = height;
    		if (i == 0){
        		startp = currp;
			}
    		else{
        		Debug.DrawLine (startp, currp, Color.red);
				startp=currp;
			}
		}	
	}
  
	public float size=6.0f;
	
	private Vector3[] vertices = new Vector3[6];


    void OnGUI(){
        if (!mat) {
            // Debug.LogError("Please Assign a material on the inspector");
            return;
        }
       GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.TRIANGLE_STRIP);
        GL.Color(Color.red);
		
        GL.Vertex(vertices[0]);
        GL.Vertex(vertices[1]);
        GL.Vertex(vertices[2]);
        GL.Vertex(vertices[3]);
	    GL.Vertex(vertices[4]);
        GL.Vertex(vertices[5]);
		
		GL.End();
       GL.PopMatrix();
    }
	
	public Material mat;

    void OnPostRender() {
        if (!mat) {
            // Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.TRIANGLE_STRIP);
        GL.Color(new Color(1, 0, 0, 1));
        GL.Vertex3(2.5F, 5.0F, 0);
        GL.Vertex3(0, 5.0F, 0);
        GL.Vertex3(2.5F, 2.5F, 0);
        GL.Vertex3(0, 2.5F, 0);
        GL.End();
        GL.PopMatrix();
    }

}
