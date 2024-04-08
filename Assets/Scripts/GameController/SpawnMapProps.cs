using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
public class SpawnMapProps : MonoBehaviour
{
    public List<GameObject> itemsToSpawn;
    public int numberOfItems = 10;
    public Vector2 spawnArea = new Vector(10, 10);

    void StartSpawning()
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            Vector2 randomPosition = new Vector2(Random.Range(-spawnArea.x / 2, spawnArea.x / 2), Random.Range(-spawnArea.y / 2, spawnArea.y / 2));

            GameObject itemToSpawn = itemsToSpawn[Random.Range(0, itemsToSpawn.Length)];
            PhotonNetwork.Instantiate(itemToSpawn.name, randomPosition, Quaternion.identity);

        }
    }
}