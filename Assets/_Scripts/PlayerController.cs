using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 10;

    [SerializeField]
    Rigidbody2D rigidBody;

    [Header("Encounters")]
    [SerializeField]
    [Range(0, 100)]
    float encounterChance;

    [SerializeField]
    Transform groundCheckTransform;

    [SerializeField]
    LayerMask EncounterLayer;

    [SerializeField]
    float encnounterCheckFrequency;

    [Header("BattleTransition")]
    [SerializeField]
    BattleTransitionManager battleTransitionManager;

    private void Start()
    {
        SavePositionButtonBehaviour.OnSave.AddListener(OnSave);
        LoadPosition();
        InvokeRepeating("OnTallGrass", 1.0f, encnounterCheckFrequency);
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {

        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementVector *= speed;
        rigidBody.velocity = movementVector;

        

    }


    void OnSave()
    {
        string saveStr = "";
        saveStr += transform.position.x.ToString() + ',' + transform.position.y.ToString() + ',' + transform.position.z.ToString();
        PlayerPrefs.SetString(gameObject.name + "Position", saveStr);
    }

    void LoadPosition()
    {
        string loadedData = PlayerPrefs.GetString(gameObject.name + "Position", "");
        Debug.Log("Loaded position: " + loadedData);
        //want to make sure there is a position to parse    
        if (loadedData != "")
        {
            char[] delimiters = new char[] { ',' };
            string[] splitData = loadedData.Split(delimiters);
            Debug.Log("position string split");
            transform.position = new Vector3(float.Parse(splitData[0]), float.Parse(splitData[1]), float.Parse(splitData[2]));
        }

    }

    private void OnTallGrass()
    {
        if (rigidBody.velocity.magnitude >= 0.1f)
        {
            Collider2D collision = Physics2D.OverlapCircle(groundCheckTransform.position, 0.2f, EncounterLayer);

            if (collision)
            {
                float roll = Random.Range(0.0f, 100.0f);
                //Debug.Log("Rolled a: " + roll);
                if(roll <= encounterChance)
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

        Gizmos.DrawWireSphere(groundCheckTransform.position, 0.2f);
    }

   
}
