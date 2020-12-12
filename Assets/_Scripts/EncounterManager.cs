using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Encounter Manager handles how often encounters are collided with. 
/// </summary>
public class EncounterManager : MonoBehaviour
{
    // % chance on having an encounter
    [Header("Encounters")]
    [SerializeField]
    [Range(0, 100)]
    float encounterChance;


    [SerializeField]
    Transform groundCheckTransform;
    [SerializeField]
    Rigidbody2D rigidBody;

    [SerializeField]
    LayerMask EncounterLayer;

    [SerializeField]
    float encnounterCheckFrequency;

    float encounterDetectionRadius = 0.35f;

    [Header("BattleTransition")]
    [SerializeField]
    BattleTransitionManager battleTransitionManager;

    // Start is called before the first frame update
    void Start()
    {
        // Here we call the check after 1 second and every half second afterwards
        InvokeRepeating("OnTallGrass", 1.0f, encnounterCheckFrequency);
    }

    private void OnTallGrass()
    {
        if (rigidBody.velocity.magnitude >= 0.1f) // only if the player is moving does it check for collision
        {
            
            Collider2D collision = Physics2D.OverlapCircle(groundCheckTransform.position, encounterDetectionRadius, EncounterLayer);
            // check if the detection radius is overlaping with the encounter layer

            if (collision)
            {
                float roll = Random.Range(0.0f, 100.0f); // if overlap then check against encounter chance
                Debug.Log("Rolled a: " + roll);
                if (roll <= encounterChance)
                {
                    battleTransitionManager.EnterEncounter();
                    // transistion to battle if has an encounter.
                }

            }
        }
    }

    /// <summary>
    /// Debug draw to check overlap circle
    /// </summary>
    //visualize the shpoere for the overlap circle check
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(groundCheckTransform.position, encounterDetectionRadius);
    }
}
