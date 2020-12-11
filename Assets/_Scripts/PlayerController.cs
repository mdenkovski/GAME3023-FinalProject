﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
        int loadedInt = PlayerPrefs.GetInt(gameObject.name + "Ability" + i);
        for (int i = 0; i < 5; i++)
        {
            loadedInt = PlayerPrefs.GetInt(gameObject.name + "Ability" + i);
            //Debug.Log(i + "Ability Loaded: " + loadedInt);
            if (loadedInt != -1)
            {
                Abilities[i] = MasterAbilityList.abilityList[loadedInt];
            }
            else
            {
                Abilities[i] = null;
            }
        }
    }

    public Ability GetAbility(int id)
    {
        return Abilities[id];
    }

    public void SetAbility(int id, Ability newAbility)
    {
        Abilities[id] = newAbility;
    }
}
