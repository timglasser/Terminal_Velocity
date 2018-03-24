using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

	
public class GridContainer : MonoBehaviour {
		
	public int _numElems;
	private List<IPolygon> _gridElements { get; set; }
	public GameObject GridModel;


	void Start () 
	{
		IPolygon _pn;
		Vector3 pos = new Vector3(0.0f,0.0f,0.0f);
		if (GridModel){

			_gridElements  = new List<IPolygon> ();
			
			for (int i = 0; i < _numElems; i++) {
				//Vector2 position = new Vector2 (i, 0);
				//Vector3 pos = GridToWorld (position);
				Debug.Log ("pos of element = " + pos.x + pos.y + pos.z);
				GameObject gridElem = Instantiate (GridModel);
				_pn = gridElem.GetComponent<Hexagon> ();

				//((Hexagon)_pn).Init(); // set up the mesh vertices, uv's, normals
				this._gridElements.Add (_pn);
				pos.z += 9.0f;
				gridElem.transform.position = pos;
			}
		}
	}
		
	void Update () 
	{
		// put animation code here
	}
	
	private Vector3 GridToWorld(Vector2 hc)
	{
		float x = (hc.x * Hexagon.Width + (((int)hc.y & 1) * Hexagon.Width / 2.0f));
		return new Vector3(x, 0.0f, (float)(hc.y * 1.5f * Hexagon.Radius));
	}	

	/*
	#region weeks 8,9, 10

		private Vector2 ToHex(Vector3 pos)
		{
			var px = pos.x + Globals.HalfWidth;
			var py = pos.z + Globals.Radius;
			
			int gridX = (int)Math.Floor(px / Globals.Width);
			int gridY = (int)Math.Floor(py / Globals.RowHeight);
			
			float gridModX = Math.Abs(px % Globals.Width);
			float gridModY = Math.Abs(py % Globals.RowHeight);
			
			bool gridTypeA = (gridY % 2) == 0;
			
			var resultY = gridY;
			var resultX = gridX;
			var m = Globals.ExtraHeight / Globals.HalfWidth;
			
			if (gridTypeA)
			{
				// middle
				resultY = gridY;
				resultX = gridX;
				// left
				if (gridModY < (Globals.ExtraHeight - gridModX * m))
				{
					resultY = gridY - 1;
					resultX = gridX - 1;
				}
				// right
				else if (gridModY < (-Globals.ExtraHeight + gridModX * m))
				{
					resultY = gridY - 1;
					resultX = gridX;
				}
			}
			else
			{
				if (gridModX >= Globals.HalfWidth)
				{
					if (gridModY < (2 * Globals.ExtraHeight - gridModX * m))
					{
						// Top
						resultY = gridY - 1;
						resultX = gridX;
					}
					else
					{
						// Right
						resultY = gridY;
						resultX = gridX;
					}
				}
				
				if (gridModX < Globals.HalfWidth)
				{
					if (gridModY < (gridModX * m))
					{
						// Top
						resultY = gridY - 1;
						resultX = gridX;
					}
					else
					{
						// Left
						resultY = gridY;
						resultX = gridX - 1;
					}
				}
			}
			
			return new Vector3(resultX, resultY);
		}
		

		
		public IEnumerable<Vector2> GetRing(Vector2 hcrd, int ring)
		{
			var left = new Vector2(hcrd.x - ring, hcrd.y);
			yield return left;
			
			var tx = left.x;
			var ty = left.y;
			for (var i = 1; i < ring + 1; i++)
			{
				tx = NextX(tx, ty);
				ty = ty + 1;
				yield return new Vector2(tx, ty);
			}
			
			var bx = left.x;
			var by = left.y;
			for (var i = 1; i < ring + 1; i++)
			{
				bx = NextX(bx, by);
				by = by - 1;
				yield return new Vector2(bx, by);
			}
			
			for (int i = 1; i <= ring; i++)
			{
				yield return new Vector2(tx + i, ty);
				yield return new Vector2(bx + i, by);
			}
			
			tx += ring;
			bx += ring;
			for (var i = 1; i < ring; i++)
			{
				tx = NextX(tx, ty);
				ty = ty - 1;
				yield return new Vector2(tx, ty);
			}
			for (var i = 1; i < ring; i++)
			{
				bx = NextX(bx, by);
				by = by + 1;
				yield return new Vector2(bx, by);
			}
			
			yield return new Vector2(hcrd.x + ring, hcrd.y);
		}
		
		private int NextX(float x, float y)
		{
			if (y % 2 == 0) return (int)x;
			else return (int)(x + 1);
		}
		
	#endregion
	*/
}
