using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GridManager: MonoBehaviour
{
    //following public variable is used to store the hex model prefab;
    //instantiate it by dragging the prefab on this vari able using unity editor
    public GameObject HexModel;

    //next two variables can also be instantiated using unity editor
    public int gridWidthInHexes = 10;
    public int gridHeightInHexes = 10;


	// faction territories
	private List<Vector2> redHexCoords = new List<Vector2> ();
	private List<Vector2> greenHexCoords = new List<Vector2> ();
	private List<Vector2> yellowHexCoords = new List<Vector2> ();
	private List<Vector2> blueHexCoords = new List<Vector2> ();


	private List<Vector2> assocHexCoords = new List<Vector2> ();
	private Vector2 selHexCoords;
	private Color hexColor; 
	
	// hexagon attributes
	float Radius ; 
   	float Height; 
    float RowHeight; 
    float HalfWidth ; 
    float Width ; 
    float ExtraHeight; 
	
	private GameObject[] _hexagons;  

    // select on world pos from mouse
	public void selectHex( Vector3 wpos){ 
		GameObject prevSelHex = getHexObject (selHexCoords); 
	//	print ("prev sel hex " + prevSelHex);
		if (prevSelHex) {
			// turn prev selection off
			Renderer prevselhexgraphic = prevSelHex.GetComponentInChildren<Renderer> ();	
			prevselhexgraphic.GetComponent<Renderer>().material.color = hexColor;
		}
		selHexCoords = getHexCoords(wpos);	
	//	print ("curr sel hex " + selHexCoords);
		GameObject selHex = getHexObject (selHexCoords);
		if (selHex) {
			// turn new selection on
			Renderer newselhexgraphic = selHex.GetComponentInChildren<Renderer> ();
			newselhexgraphic.GetComponent<Renderer>().material.color = Color.white;	 
		}
	}

	// select on grid position
	private void addAssocHex( Vector2 gridpos){

		assocHexCoords.Add (gridpos);
		GameObject assocHex = getHexObject (gridpos);
		if (assocHex) {
			// turn assoc selection on
			Renderer newselhexgraphic = assocHex.GetComponentInChildren<Renderer> ();
			newselhexgraphic.GetComponent<Renderer>().material.color = Color.grey;	 
		}
	}

	// 
	private void clearAssocHex( ){
		// change back to original color
		foreach (Vector2 element in assocHexCoords){
			GameObject hex = getHexObject (element);
			if (hex) {
				// turn selection on
				Renderer newselhexgraphic = hex.GetComponentInChildren<Renderer> ();
				newselhexgraphic.GetComponent<Renderer>().material.color = hexColor;	 
			}
		}
		// remove ccord elements from list
		assocHexCoords.Clear ();
	}

	private void setHexValues(float r) 
	{ 
     	Radius = r; 
    	Height =2.0f * Radius; 
    	RowHeight =1.5f * Radius; 
    	HalfWidth = (float)Mathf.Sqrt((Radius * Radius) - ((Radius /2.0f) * (Radius /2.0f))); 
    	Width =2.0f*this.HalfWidth; 
    	ExtraHeight = Height - RowHeight; 
		return;
	}

	private GameObject getHexObject(Vector2 pos){
		int index;
		index = (int)pos.x +((int)pos.y*gridWidthInHexes);
//		Debug.Log ("index of selected hex " + index);

		return _hexagons[index];
	}

	private Vector3 getHexCubeCoords(Vector3 position){
		return new Vector3( 
	                   (float)Mathf.Round((1.73f/3.0f* position.x - position.z /3.0f) / Radius), 
	                   (float)Mathf.Round(-(1.73f/3.0f* position.x + position.z /3.0f) / Radius), 
	                   (float)Mathf.Round(0.66f* (position.z /Radius)));
	}

	private Vector2 getHexCoords(Vector3 pos) 
	{ 
    	var pq = pos.x + HalfWidth; 
		var pr = Mathf.Abs (pos.z) +  Radius; 

    	int gridQ = (int)pq / (int)Width; 
    	int gridR = (int)pr / (int)RowHeight; 

		print ("grid rel X " + pq);
 		print ("grid rel Y " + pr);

    	var gridModQ = (int)pq % (int)Width; 
    	var gridModR = (int)pr % (int)RowHeight; 

    	bool gridTypeA =false; 

    	if ((Mathf.Abs(gridR) % 2) == 0) {
			print ("A type slot " + gridR);
			gridTypeA = true; 
		}
		else{
			print ("B type slot " + gridR);
		}

    	var resultR = gridR; 
    	var resultQ = gridQ; 
    	var m = ExtraHeight / HalfWidth; // this is the slope of the pointy top

    	if (gridTypeA) { 
        	
			print ("A type slot " + gridR);
			// left  top
        	if (gridModR < (ExtraHeight - gridModQ * m)) { 
				print ("A top left");
            	resultR = gridR -1; 
            	resultQ = gridQ -1; 
        	} 
        	
			// right top
        	else if (gridModR < (-ExtraHeight + gridModQ * m)) { 
				print ("A top right");
            	resultR = gridR -1; 
            	resultQ = gridQ; 
        	} 
			else{
				// middle	
				print ("A middle");

				resultR = gridR; 
				resultQ = gridQ; 
			}
    	} 
    	else{ // grid type B

			print ("B type slot " + gridR);
        	if (gridModQ >= HalfWidth) { 
            	if (gridModR < (2* ExtraHeight - gridModQ * m)) { 
                	// Top 
					print ("B top ");
                	resultR = gridR -1; 
                	resultQ = gridQ; 
            	} 
            	else { 
                	// Right 
					print ("B right");
                	resultR = gridR; 
                	resultQ = gridQ; 
            	} 
        	} 

        	if (gridModQ < HalfWidth) { 
            	if (gridModR < (gridModQ * m)) {	 
                	// Top 
					print ("B top ");
                	resultR = gridR -1; 
                	resultQ = gridQ; 
            	} 
            	else { 
                	// Left 
					print ("B left");
                	resultR = gridR; 
                	resultQ = gridQ -1; 
            	} 
        	} 
    	} 

    	return new Vector2(resultQ, Mathf.Abs(resultR)); 
	}


    private void setSizes(){
		Renderer hexgraphic = HexModel.GetComponentInChildren<Renderer>();    
        float localHeight = hexgraphic.bounds.size.z;
		float localWidth = hexgraphic.bounds.size.x;
		setHexValues(localHeight/2.0f); // pass in the radius
//		print("height of every hex " + Height);
//		print("width of every hex " + Width);
    }
 
    //Method to calculate the position of the first hexagon tile
    //The center of the hex grid is (0,0,0)
    private Vector3 calcInitPos(){
//        Vector3 initPos;
        //the initial position will be in the left upper corner
        return new Vector3(0.0f,0.0f,0.0f);
    }
 
    //method used to convert hex grid coordinates to game world coordinates
    private Vector3 calcWorldCoord(Vector2 gridPos){
		//Position of the first hex tile
        Vector3 initPos = calcInitPos();
        
		//Every second row is offset by half of the tile width
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = HalfWidth;
 
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = initPos.z - (gridPos.y * Height * 0.75f);
        float x = initPos.x +  offset + (gridPos.x * Width);

        return new Vector3(x, 0.0f, z);
    }
 
    //method which initialises and positions all the tiles
    private void createGrid(){
		//Game object which is the parent of all the hex tiles ( q is along and r is down)
		_hexagons = new GameObject[gridWidthInHexes*gridHeightInHexes];
		for (int r = 0; r < gridHeightInHexes; r++)
        {
            for (int q = 0; q < gridWidthInHexes; q++)
            {
               	GameObject hex;
                
				_hexagons[q+(r*gridWidthInHexes)] = (GameObject)Instantiate(HexModel);
				hex = _hexagons[q+(r*gridWidthInHexes)];
						
				//Current position in grid
                Vector2 gridPos = new Vector2(q, r);
						
                hex.transform.position = calcWorldCoord(gridPos);
                hex.transform.parent = gameObject.transform;
            }
        }
    }
 
    //The grid should be generated on game start
    void Start()
    {
		// save hex color
		if (HexModel) {
			Renderer hgraphic = HexModel.GetComponentInChildren<Renderer> ();
			hexColor = hgraphic.GetComponent<Renderer>().material.color ;
		}
        setSizes();
        createGrid();
    }

	public void setLine(){

	}
	public void setRange(){

	}
	public void setVisible(){

	}
	public void setNeighbors (){
		Vector2 n; 
		int deltaq;
		int deltar;

		int[,,] neighbors = new int[,,] {
			{ {+1,  0}, {+1, -1}, { 0, -1}, {-1, -1}, {-1,  0}, { 0, +1} },
			{ {+1, +1}, {+1,  0}, { 0, -1}, {-1,  0}, {-1, +1}, { 0, +1} }
		};

		// remove previous assoc coords
		clearAssocHex ();

		// neighbours of the seleced coords
		int q = (int)selHexCoords.x;
		int r = (int)selHexCoords.y;

		// note I use & 1 but mod 2 would work if it never returns -1
		int parity = q & 1;

		// i is the 6 directions around the selected hex
		for (int i = 0; i < 6; i++) {
			deltaq = neighbors [parity, i, 0];
			deltar = neighbors [parity, i, 1];

			n.x = q + deltaq;
			n.y = r + deltar;
			addAssocHex (n);
		}
	}
	/*/ Mathemagically (left as exercise for the reader) our 'picking' matrices are 
	/// these, assuming: 
	///  - origin at upper-left corner of hex (0,0);
	///  - 'straight' hex-axis vertically down; and
	///  - 'oblique'  hex-axis up-and-to-right (at 120 degrees from 'straight').
 	private intMatrix  matrixX { 
		get { return new Matrix((3.0F/2.0F)/GridSize.Width,  (3.0F/2.0F)/GridSize.Width,
			                        1.0F/GridSize.Height,       -1.0F/GridSize.Height,  -0.5F,-0.5F); } 
	}
	private Matrix matrixY { 
		get { return new Matrix(       0.0F,                 (3.0F/2.0F)/GridSize.Width,
			                        2.0F/GridSize.Height,         1.0F/GridSize.Height,  -0.5F,-0.5F); } 
	}
	
	/// <summary>Canonical coordinates for a selected hex for a given AutoScroll position.</summary>
	/// <param name="point">Screen point specifying hex to be identified.</param>
	/// <param name="autoScroll">AutoScrollPosition for game-display Panel.</param>
	/// <returns>Canonical coordinates for a hex specified by a screen point.</returns>
	/// <see cref="HexGridAlgorithm.mht"/>
	private Vector2 GetHexCoords(Vector3 point, Size autoScroll) {

	
		return new Vector2( GetCoordinate(matrixX, point), 
		                             GetCoordinate(matrixY, point) );
	}
	
	Size TransposeSize(Size size) { return IsTransposed ? new Size (size.Height, size.Width)
		: size; }
	
	/// <summary>Calculates a (canonical X or Y) grid-coordinate for a point, from the supplied 'picking' matrix.</summary>
	/// <param name="matrix">The 'picking' matrix</param>
	/// <param name="point">The screen point identifying the hex to be 'picked'.</param>
	/// <returns>A (canonical X or Y) grid coordinate of the 'picked' hex.</returns>
	private static int GetCoordinate (Matrix matrix, Vector3 point){
		var pts = new Point[] {point};
		matrix.TransformPoints(pts);
		return (int) Math.Floor( (pts[0].X + pts[0].Z + 2F) / 3F );
	}

	*/
	// Offset coordinates in this project, 
	// but many algorithms are simpler to express in cube coordinates. 
	// Therefore we need to be able to convert back and forth.
	/* convert cube to axial
	q = x
	r = z
			
	// convert axial to cube
	x = q
	z = r
	y = -x-z
			
	// convert cube to even-q offset
	q = x
	r = z + (x + (x&1)) / 2
			
	// convert even-q offset to cube
	x = q
	z = r - (q + (q&1)) / 2
	y = -x-z
			
	// convert cube to odd-q offset
	q = x
	r = z + (x - (x&1)) / 2
			
	// convert odd-q offset to cube
	x = q
	z = r - (q - (q&1)) / 2
	y = -x-z
			
	// convert cube to even-r offset
	q = x + (z + (z&1)) / 2
	r = z
			
	// convert even-r offset to cube
	x = q - (r + (r&1)) / 2
	z = r
	y = -x-z
			
	// convert cube to odd-r offset
	q = x + (z - (z&1)) / 2
	r = z
			
	// convert odd-r offset to cube
	x = q - (r - (r&1)) / 2
	z = r
	y = -x-z
	*/

}
