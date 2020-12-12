using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Player Controller handles the players information and movement outside of battle.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 10;

    [SerializeField]
    Rigidbody2D rigidBody;

    [SerializeField]
    List<Ability> Abilities;

    [SerializeField]
    AbilityList MasterAbilityList;

    private void Start()
    {
        SavePositionButtonBehaviour.OnSave.AddListener(OnSave);
        LoadSaveData();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Movement is done here
    /// </summary>
    // Update is called once per frame
    void Update()
    {

        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementVector *= speed;
        rigidBody.velocity = movementVector;

    }

    /// <summary>
    /// This saves the players current information. like position and current abilties to player prefs.
    /// </summary>
    void OnSave()
    {
        string saveStr = "";
        saveStr += transform.position.x.ToString() + ',' + transform.position.y.ToString() + ',' + transform.position.z.ToString();
        PlayerPrefs.SetString(gameObject.name + "Position", saveStr);

        for (int i = 0; i < 5; i++)
        {
            if (Abilities[i] != null)
                PlayerPrefs.SetInt(gameObject.name + "Ability" + i, Abilities[i].AbilityId);
            else
            {
                PlayerPrefs.SetInt(gameObject.name + "Ability" + i, -1);
                Debug.Log("Empty Ability");
            }
        }
    }
    /// <summary>
    /// Loads Player Prefs and sets position and abilities where it was last time.
    /// </summary>
    public void LoadSaveData()
    {
        string loadedData = PlayerPrefs.GetString(gameObject.name + "Position", "");
        Debug.Log("Loaded position: " + loadedData);
        //want to make sure there is a position to parse    
        if (loadedData != "")
        {
            char[] delimiters = new char[] { ',' };
            string[] splitData = loadedData.Split(delimiters);
            //Debug.Log("position string split");
            transform.position = new Vector3(float.Parse(splitData[0]), float.Parse(splitData[1]), float.Parse(splitData[2]));
        }
        //int loadedInt = PlayerPrefs.GetInt(gameObject.name + "Ability" + 0);
        for (int i = 0; i < 5; i++)
        {
            int loadedInt = PlayerPrefs.GetInt(gameObject.name + "Ability" + i, -2);
            //Debug.Log(i + "Ability Loaded: " + loadedInt);
            if (loadedInt == -1)
            {
                Abilities[i] = null;
            }
            else if (loadedInt >= 0)
            {
                Abilities[i] = MasterAbilityList.abilityList[loadedInt];
            }
            //else // -2 means new gamefile no saved prefs
            //{
                
            //}
        }
    }

    /// <summary>
    /// used to get an abilties at index 'id'
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Ability GetAbility(int id)
    {
        return Abilities[id];
    }


    /// <summary>
    /// used to set ability at index 'id' to 'newAbiltiy'
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newAbility"></param>
    public void SetAbility(int id, Ability newAbility)
    {
        Abilities[id] = newAbility;
    }
}
