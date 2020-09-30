using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Maze : MonoBehaviour
{
	
	public IntVector2 sizeMaze;	
	
	public MazeCell cellPrefab;
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;
	public MazeWall borderWallPrefab;
	
	private MazeCell[,] cells;
	
	public Room[] rooms;		
		
	public LayerMask m_LayerMask;
	
	public Zombie zombie;
	public int incrNbrZombieByLevel = 8;
	
	private NavMeshSurface navMeshSurface;
	
	public int NbrEnnemies = 10;
	
	private int currentLevel = 0;
	
	public Transform player;
	
	private int distanceMinSpawnZombie = 15;

	
    // Start is called before the first frame update
    void Start()
    {
		navMeshSurface = GetComponent<NavMeshSurface>();
        StartCoroutine(Generate());
    }
	
	public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}
	
	///Labyrinth Generation
	private void GenerateRoom()
	{
		List<Rect> listCreatedRooms = new List<Rect>();

		for( int k = 0; k<rooms.Length; k++){
			Room room = rooms[k];
			//On cherche une place dans le labyrinthe
			for(int nbrTest =0; nbrTest < 4;nbrTest++){
				
				int z_rand = Random.Range(room.size/2,sizeMaze.z-room.size/2);
				int x_rand = Random.Range(room.size/2,sizeMaze.x-room.size/2);
				
				Vector3 coordRoom = new Vector3(x_rand - sizeMaze.x/2+Dimension.instance.sizeCell/2,0f,z_rand - sizeMaze.z/2+Dimension.instance.sizeCell/2);
				Vector3 dimension = new Vector3(room.size,1f,room.size);
				
				Rect roomRect = new Rect(coordRoom.x-room.size/2,coordRoom.z-room.size/2,room.size,room.size);

				bool placeFound = true;
				
				foreach ( Rect roomCreated in listCreatedRooms){
					if(roomCreated.Overlaps(roomRect)){
						placeFound = false;
						break;
					}
				}
				if(placeFound){
					Collider[] hitColliders = Physics.OverlapBox (coordRoom, dimension, Quaternion.identity, m_LayerMask);

					// You can use a for loop more easily, but this will follow with
					// the given unity example api
					int i = 0;
					// Destroy everything in this list
					while (i < hitColliders.Length) {
						if(hitColliders[i].CompareTag("Wall")){
							hitColliders[i].gameObject.SetActive(false);
						}
						i++;
					}
					Room roomCreated = Instantiate(room);
					roomCreated.transform.position = coordRoom;
					
					listCreatedRooms.Add(roomRect);
					break;
				}
			}	
		}
		
	}
	
	public IEnumerator Generate () {
		sizeMaze.x*=Dimension.instance.sizeCell;
		sizeMaze.z*=Dimension.instance.sizeCell;
		cells = new MazeCell[sizeMaze.x, sizeMaze.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);
		while (activeCells.Count > 0) {
			DoNextGenerationStep(activeCells);
		}
		
		yield return null;

		GenerateRoom();
		GenerateEnnemies();
		navMeshSurface.BuildNavMesh();


	}
	
	private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		activeCells.Add(CreateCell(RandomCoordinates));
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null) {
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else {
				CreateWall(currentCell, neighbor, direction);
				// No longer remove the cell here.
			}
		}
		else {
			CreateWall(currentCell, null, direction,true);
			// No longer remove the cell here.
		}
	}
	
	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());

	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction,bool isBorderWall = false ) {
		if (otherCell != null) {
			MazeWall wall = Instantiate(wallPrefab) as MazeWall;
			generateWall(cell, otherCell, direction, wall);
			generateWall(otherCell, cell, direction.GetOpposite(),wall);
		}else if(isBorderWall){
			MazeWall wall = Instantiate(borderWallPrefab) as MazeWall;
			generateWall(cell, otherCell, direction, wall);
			wall.tag = "BorderWall";
		}
	}
	
	private void generateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction, MazeWall wall){
		wall.Initialize(cell, otherCell, direction);
		wall.transform.localScale = new Vector3(1,1,1);
		wall.transform.localPosition=new Vector3(0f,wall.GetComponent<BoxCollider>().size.y*0.5f,0f);
	}

	private MazeCell CreateCell (IntVector2 coordinates) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = coordVec2toVec3(coordinates,0f);
		newCell.transform.localScale = new Vector3(Dimension.instance.sizeCell,Dimension.instance.sizeCell,Dimension.instance.sizeCell);
		return newCell;
	}
	
	public void GenerateEnnemies(){
		int j = 0;
		while (j<(currentLevel*incrNbrZombieByLevel)+NbrEnnemies){
			Vector3 randSpawnCoord = coordVec2toVec3(RandomCoordinates,0f);
			if(Vector3.Distance(randSpawnCoord,player.position)>distanceMinSpawnZombie){
				Zombie ennemy = Instantiate(zombie);
				ennemy.transform.position=randSpawnCoord;
				j++;
			}
		}
	}
	
	public void CreateZombie(){
		while(true){
			Vector3 randSpawnCoord = coordVec2toVec3(RandomCoordinates,0f);
			if(Vector3.Distance(randSpawnCoord,player.position)>distanceMinSpawnZombie){
				Zombie ennemy = Instantiate(zombie);
				ennemy.transform.position=randSpawnCoord;
				break;
			}
		}		
	}
	
	
	
	private IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, sizeMaze.x), Random.Range(0, sizeMaze.z));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < sizeMaze.x && coordinate.z >= 0 && coordinate.z < sizeMaze.z;
	}
	
	private Vector3 coordVec2toVec3(IntVector2 coordOrigin, float valueY = 1f){
		return new Vector3(coordOrigin.x-sizeMaze.x*0.5f+Dimension.instance.sizeCell*0.5f,valueY,coordOrigin.z-sizeMaze.z*0.5f+Dimension.instance.sizeCell*0.5f);
	}
	
	public Vector3 getRandomCellCoordinates()
	{
		return coordVec2toVec3(RandomCoordinates);
	}
	
}
