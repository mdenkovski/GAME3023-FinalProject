using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// spawns in the player character and keeps reference to it
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;

    public static PlayerController player = null;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            //spawn the player and store him
            player = Instantiate(playerPrefab, transform.position, transform.rotation).GetComponent<PlayerController>();
        }
    }
}
