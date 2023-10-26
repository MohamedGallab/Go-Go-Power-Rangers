using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject _tile;

    List<GameObject> _tiles = new List<GameObject>();

    [SerializeField]
    List<GameObject> _spawnables;

    void Start()
    {
        _tiles.Add(Instantiate(_tile, new Vector3(0, 0, 0), Quaternion.identity));
    }

    void Update()
    {
        while (_tiles.Count < 5)
        {
            SpawnTile();
        }

        GameObject firstTile = _tiles.First();

        if (firstTile.transform.position.z < -15)
        {
            Destroy(firstTile);
            _tiles.Remove(firstTile);
        }
    }
    
    public void DisableItems()
    {
        GameObject firstTile = _tiles.First();
        foreach (Transform child in firstTile.transform)
        {
            if (!child.CompareTag("Untagged"))
            {
                child.gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void RemoveAllObstacles()
    {
        foreach (GameObject tile in _tiles)
        {
            foreach (Transform child in tile.transform)
            {
                if (child.CompareTag("Obstacle"))
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    void SpawnTile()
    {
        GameObject prevTile = _tiles.Last();
        GameObject lastTile = Instantiate(_tile, new Vector3(0, 0, prevTile.transform.position.z + 20), Quaternion.identity);
        _tiles.Add(lastTile);

        int obstacles = 0;

        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            GameObject item = _spawnables[Random.Range(0, _spawnables.Count)];

            if (item.CompareTag("Obstacle"))
            {
                obstacles++;
            }

            if (obstacles == 3)
            {
                item = _spawnables[Random.Range(0, _spawnables.Count - 1)];
            }

            Instantiate(item, new Vector3((i - 1) * 3, 0.5f, lastTile.transform.position.z), Quaternion.identity, lastTile.transform);
        }
    }
}
