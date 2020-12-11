using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
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

    float encounterDetectionRadius = 0.75f;

    [Header("BattleTransition")]
    [SerializeField]
    BattleTransitionManager battleTransitionManager;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("OnTallGrass", 1.0f, encnounterCheckFrequency);
    }

    private void OnTallGrass()
    {
        if (rigidBody.velocity.magnitude >= 0.1f)
        {
            
            Collider2D collision = Physics2D.OverlapCircle(groundCheckTransform.position, encounterDetectionRadius, EncounterLayer);

            if (collision)
            {
                float roll = Random.Range(0.0f, 100.0f);
                Debug.Log("Rolled a: " + roll);
                if (roll <= encounterChance)
                {
                    battleTransitionManager.EnterEncounter();

                }

            }
        }
    }

    //visualize the shpoere for the overlap circle check
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(groundCheckTransform.position, encounterDetectionRadius);
    }
}
