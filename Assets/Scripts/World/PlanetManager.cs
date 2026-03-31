using UnityEngine;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour
{
    [SerializeField] private int planetRadius = 10;
    [SerializeField] private Material blockMaterial;
    [SerializeField] private Material planetCoreMaterial;
    
    private Dictionary<Vector3Int, GameObject> blocks = new Dictionary<Vector3Int, GameObject>();
    private GameObject blockContainer;
    
    private static PlanetManager instance;
    public static PlanetManager Instance => instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GeneratePlanet();
    }

    private void GeneratePlanet()
    {
        blockContainer = new GameObject("BlockContainer");
        blockContainer.transform.parent = transform;
        
        // Generate initial planet blocks in a spherical pattern
        for (int x = -planetRadius; x <= planetRadius; x++)
        {
            for (int y = -planetRadius; y <= planetRadius; y++)
            {
                for (int z = -planetRadius; z <= planetRadius; z++)
                {
                    float distance = Mathf.Sqrt(x * x + y * y + z * z);
                    
                    // Create shell of blocks
                    if (Mathf.Abs(distance - planetRadius) < 2f)
                    {
                        Vector3Int blockPos = new Vector3Int(x, y, z);
                        CreateBlock(blockPos);
                    }
                }
            }
        }
    }

    private void CreateBlock(Vector3Int position)
    {
        if (blocks.ContainsKey(position)) return;
        
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.name = $"Block_{position}";
        block.transform.parent = blockContainer.transform;
        block.transform.position = position;
        block.transform.localScale = Vector3.one;
        
        Collider collider = block.GetComponent<Collider>();
        if (collider != null) collider.enabled = true;
        
        Rigidbody blockRb = block.GetComponent<Rigidbody>();
        if (blockRb != null)
        {
            blockRb.isKinematic = true;
        }
        
        Renderer renderer = block.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = blockMaterial;
        }
        
        blocks[position] = block;
    }

    public void DestroyBlock(Vector3Int position)
    {
        if (blocks.TryGetValue(position, out GameObject block))
        {
            Destroy(block);
            blocks.Remove(position);
        }
    }

    public bool IsBlockAt(Vector3Int position)
    {
        return blocks.ContainsKey(position);
    }

    public void PlaceBlock(Vector3 worldPosition)
    {
        Vector3Int gridPos = Vector3Int.FloorToInt(worldPosition);
        CreateBlock(gridPos);
    }
}