using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BattleState { 
    START,
    PLAYER1,
    PLAYER2,
    WIN,
    LOOSE,
    PROCESSING
}

/// <summary>
/// Controls the game flow and manages the turns and abilities
/// </summary>
public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    public AbilityList MasterAbilityList;

    //knows the state of the current game
    public BattleState state;
    [Header("Characters Loading In")]
    //which characters to instantiate
    public GameObject playerCharacter;
    public GameObject enemyCharacter;

    public WaifuMasterList Waifus;

    [Header("Our Characters")]
    //details of our charatcer
    WaifuDetails playerDetails;
    WaifuDetails enemyDetails;

    [Header("User Interface")]
    public TextMeshProUGUI dialogueText;

    // the UI panels for the respective characters
    public DetailsUI playerDetailsUI;
    public DetailsUI enemyDetailsUI;
    public TextMeshProUGUI[] abilitiesButtons;

    [Header("AudioEffect")]
    [SerializeField]
    AudioSource audioSource;

    //player controller to get the abilities from it
    [SerializeField]
    private PlayerController playerController;

    //store our player abilities
    [SerializeField]
    private List<Ability> playerAbilities;

    public 


    // Start is called before the first frame update
    void Start()
    {
        //set up battle state
        state = BattleState.START;
        //get the player controller from the player that was not destroyed on load in city scene
        playerController = SpawnPoint.player.GetComponent<PlayerController>();
        //create the game players
        StartCoroutine(CreatePlayers());
    }

   /// <summary>
   /// create the players in the battle scene
   /// </summary>
   /// <returns></returns>
    IEnumerator CreatePlayers()
    {
        //create our player
        CreatePlayer();
        //create enemy player
        CreateEnemy();
        //update UI of halth and names
        UpdateCharactersUI();
        //update the abilities selection UI
        UpdateAbilityUI();

        yield return new WaitForSeconds(3.0f);

        //currently have player 1 always start
        state = BattleState.PLAYER1;

        Player1Turn();

    }

    /// <summary>
    /// create the player character and set up the values
    /// </summary>
    void CreatePlayer()
    {
        //instantiate player and get their detials
        GameObject player = Instantiate(playerCharacter);
        playerDetails = player.GetComponent<WaifuDetails>();
        //playerDetails.waifu = Waifus.waifuList[PlayerPrefs.GetInt("Player1")];
        //transfer the specifications from the scriptable objects to our player
        playerDetails.waifu = Waifus.waifuList[0];
        playerDetails.waifuSprite.sprite = playerDetails.waifu.characterImage;
        playerDetails.Health = playerDetails.waifu.HealthMax;

        //update our default abilities with our saved abilities
        OverideAbilities();
    }

    /// <summary>
    /// sets the characters abilities to the custom abilites that were saved
    /// </summary>
    void OverideAbilities()
    {
        for (int i = 0; i < 5; i++)
        {

            playerAbilities.Add(playerController.GetAbility(i));
        }

    }

    /// <summary>
    /// Create the enemy player and their details
    /// </summary>

    void CreateEnemy()
    {
        //nstantiate enemy and get their detials
        GameObject enemy = Instantiate(enemyCharacter);
        enemyDetails = enemy.GetComponent<WaifuDetails>();
        enemyDetails.waifu = Waifus.waifuList[UnityEngine.Random.Range(1, 4)];

        //transfer the specifications from the scriptable objects to our enemy
        enemyDetails.waifuSprite.sprite = enemyDetails.waifu.characterImage;
        enemyDetails.Health = enemyDetails.waifu.HealthMax;

        
        dialogueText.text = "You face off against " + enemyDetails.waifu.CharacterName;
    }


    /// <summary>
    /// updates the UI for the ability menu
    /// </summary>
    void UpdateAbilityUI()
    {
        for (int i = 0; i < 5; i++)
        {
            //check if player ability is not valid to have
            if (playerAbilities[i] == null)
            {
                //deactivate the button for the ability
                abilitiesButtons[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                //override the ability
                abilitiesButtons[i].text = playerAbilities[i].AbilityName;
            }
        }
        
    }


    /// <summary>
    /// Called when an attack occurs to process the attack effect
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns></returns>
    IEnumerator Attack (int ability, WaifuDetails attacker, WaifuDetails defender )
    {
        bool defenderDefeated = false;
        bool attackerDefeated = false;
        //if player 1 use player abilities if not use the preset abilities for enemy
        Ability move = (attacker == playerDetails ?  playerAbilities[ability]: attacker.waifu.MyAbilties.abilityList[ability]);
        dialogueText.text = attacker.waifu.CharacterName + " uses " + move.AbilityName+ "!";
        yield return new WaitForSeconds(1);


        //generate a hit chance for our roll
        float hitChance = UnityEngine.Random.Range(0.0f, 1.0f);

        //check to see if the hit will succeed
        if (hitChance > move.PercentChance)
        {
            //missed so update the text
            dialogueText.text = attacker.waifu.CharacterName + " failed to use " + move.AbilityName + "!";
            yield return new WaitForSeconds(1);
        }
        else // ability hit succeeds
        {
            //check if escaping
            if (move.AbilityName == "Escape")
            {
                dialogueText.text = attacker.waifu.CharacterName + " managed to " + move.AbilityName + "!";

                Debug.Log("Escaped!");
                Escape(); //return to the city scene
            }    
            defenderDefeated = defender.TakeDamage((int)(attacker.waifu.Attack * move.AttackMultipier * (1.0f + 0.05f * attacker.buffs[(int)BUFF_ARRAY.ATTACK])));
            attackerDefeated = attacker.Recoil((int)(move.CostHp));

            // buffs and debuffs
            attacker.buffs[(int)BUFF_ARRAY.ATTACK] = move.SelfBuff[(int)BUFF_ARRAY.ATTACK];
            attacker.buffs[(int)BUFF_ARRAY.DEFENCE] = move.SelfBuff[(int)BUFF_ARRAY.DEFENCE];
            attacker.buffs[(int)BUFF_ARRAY.LOVE] = move.SelfBuff[(int)BUFF_ARRAY.LOVE];
            attacker.buffs[(int)BUFF_ARRAY.ATTACK] -= move.SelfDebuff[(int)BUFF_ARRAY.ATTACK];
            attacker.buffs[(int)BUFF_ARRAY.DEFENCE] -= move.SelfDebuff[(int)BUFF_ARRAY.DEFENCE];
            attacker.buffs[(int)BUFF_ARRAY.LOVE] -= move.SelfDebuff[(int)BUFF_ARRAY.LOVE];

            defender.buffs[(int)BUFF_ARRAY.ATTACK] = move.SelfBuff[(int)BUFF_ARRAY.ATTACK];
            defender.buffs[(int)BUFF_ARRAY.DEFENCE] = move.SelfBuff[(int)BUFF_ARRAY.DEFENCE];
            defender.buffs[(int)BUFF_ARRAY.LOVE] = move.SelfBuff[(int)BUFF_ARRAY.LOVE];
            defender.buffs[(int)BUFF_ARRAY.ATTACK] -= move.SelfDebuff[(int)BUFF_ARRAY.ATTACK];
            defender.buffs[(int)BUFF_ARRAY.DEFENCE] -= move.SelfDebuff[(int)BUFF_ARRAY.DEFENCE];
            defender.buffs[(int)BUFF_ARRAY.LOVE] -= move.SelfDebuff[(int)BUFF_ARRAY.LOVE];

            dialogueText.text = move.Description;
            yield return new WaitForSeconds(1);

            //process healing
            attacker.Rest((int)(attacker.waifu.Love * move.LoveMultiplier * (1.0f + 0.05f * attacker.buffs[(int)BUFF_ARRAY.LOVE])));

            //play ability effects
            move.PlayEffects(attacker.transform, defender.transform);
            if (move.Clip != null)
            {
                audioSource.clip = move.Clip;
                audioSource.Play();
            }

        }
        //update ui elements
        UpdateCharactersUI();

        yield return new WaitForSeconds(2);

        //check states
        if (CheckEnemyWin())
        {
            state = BattleState.LOOSE;
            EndBattle();
        }
        if (CheckPlayerWin())
        {
            state = BattleState.WIN;
            EndBattle();
        }
        
        //transition turns
        if (state == BattleState.PLAYER2)
        {
            state = BattleState.PLAYER1;
            Player1Turn();
        }
        else if (state == BattleState.PROCESSING)
        {
            state = BattleState.PLAYER2;
            StartCoroutine(EnemyTurn());
        }
        
    }

    /// <summary>
    /// check if the player won
    /// </summary>
    /// <returns></returns>
    bool CheckPlayerWin()
    {
        /// return true if playerHasWon
        if (enemyDetails.Health <= 0)
            return true;
        return false;
    }

    /// <summary>
    /// check if the player lost
    /// </summary>
    /// <returns></returns>
    bool CheckEnemyWin()
    {
        if (playerDetails.Health <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// refresh the ui elements for the character details
    /// </summary>
    void UpdateCharactersUI()
    {
        enemyDetailsUI.FillUI(enemyDetails);
        playerDetailsUI.FillUI(playerDetails);

    }

    /// <summary>
    /// request move dialogue
    /// </summary>
    void Player1Turn()
    {
        dialogueText.text = "What would you like your waifu to do?";
    }

    /// <summary>
    /// end the battle and transitions to the city
    /// </summary>
    void EndBattle()
    {
        //check the result
        if (state == BattleState.WIN)
        {
            //give new ability to player if they won
            GrantPlayerNewAbility();

            SpawnPoint.player.GetComponent<BattleTransitionManager>().ExitEncounter();
        }
        else if (state == BattleState.LOOSE)
        {
            dialogueText.text = "You lost the battle!";
            PlayerLost();
        }
    }

    /// <summary>
    /// return to the city scene at the last save
    /// </summary>
    void PlayerLost()
    {
        //load the last save. important to do before exiting encounter
        SpawnPoint.player.GetComponent<PlayerController>().LoadSaveData();

        SpawnPoint.player.GetComponent<BattleTransitionManager>().ExitEncounter();
    }

    /// <summary>
    /// flee from the battle with no penalty
    /// </summary>
    void Escape()
    {
        SpawnPoint.player.GetComponent<BattleTransitionManager>().ExitEncounter();
    }

    /// <summary>
    /// gives the player a new ability or replaces an existing one if full
    /// </summary>
    void GrantPlayerNewAbility()
    {
        dialogueText.text = "You won the battle!";
        bool givenAbility = false;

        //iterate check to grant the ability
        while (!givenAbility)
        {
            //determine the random ability
            int randomMove = UnityEngine.Random.Range(1, MasterAbilityList.abilityList.Length);
            bool duplicate = false;
            for (int i = 0; i < 4; i++)
            {
                //chek if the ability already exists on the player
                if (playerController.GetAbility(i) != null)
                    if (playerController.GetAbility(i).AbilityName == MasterAbilityList.abilityList[randomMove].AbilityName)
                    {
                        duplicate = true;
                    }
            }
            if (!duplicate) //ability does not already exist
            {
                for (int i = 0; i < 4; i++)
                {
                    if (givenAbility == false)
                        if (playerController.GetAbility(i) == null) //check for the first empty ability
                        {
                            playerController.SetAbility(i, MasterAbilityList.abilityList[randomMove]); //add the ability to the empty spot
                            givenAbility = true;
                        }
                }
                if (givenAbility == false) //no empty ability
                {
                    //place the new ability in a random position - note it does not override the first and last ability per gameplay
                    playerController.SetAbility(UnityEngine.Random.Range(1, 4), MasterAbilityList.abilityList[randomMove]);
                    //playerDetails.waifu.MyAbilties.abilityList[UnityEngine.Random.Range(1, 4)] = MasterAbilityList.abilityList[randomMove];
                    givenAbility = true;
                }
            }
        }
    }


    /// <summary>
    /// enemy turn enumerator
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyTurn()
    {
        //update text
        dialogueText.text = enemyDetails.waifu.CharacterName + " attacks!";

        yield return new WaitForSeconds(1.0f);
        int move = 4; //set the move to rest by default

        float moveChance = UnityEngine.Random.Range(0.0f, 1.0f);
        // if move chance is above the players health % then it will heal. This makes it more likely to heal the lower health it is.

        //determine what move the waifu will do based on their aggressiveness and remaining health percentage
        if ((1+enemyDetails.waifu.Aggressiveness)*((float)enemyDetails.Health / (float)enemyDetails.waifu.HealthMax) > moveChance)
        {
            move = (int)UnityEngine.Random.Range(0, 4); // update the move if determined it will not heal
        }
        Debug.Log("Enemy Move: " + move + " - " + enemyDetails.waifu.MyAbilties.abilityList[move].AbilityName);
        //trigger the enemy attack on their selected move
        StartCoroutine(Attack(move, enemyDetails, playerDetails));

        yield return new WaitForSeconds(1.0f);
    }

    /// <summary>
    /// player uses button 1
    /// </summary>
    /// <param name="buttonSound"></param>
    public void OnButtonAttack1(AudioSource buttonSound)
    {
        if (state != BattleState.PLAYER1)
            return;
        state = BattleState.PROCESSING;
        buttonSound.Play();
        StartCoroutine(Attack(0, playerDetails, enemyDetails)) ;
        

    }

    /// <summary>
    /// player uses button 2
    /// </summary>
    /// <param name="buttonSound"></param>
    public void OnButtonAttack2(AudioSource buttonSound)
    {
        if (state != BattleState.PLAYER1)
            return;
        state = BattleState.PROCESSING;
        buttonSound.Play();
        StartCoroutine(Attack(1, playerDetails, enemyDetails));
    }

    /// <summary>
    /// player uses button 3
    /// </summary>
    /// <param name="buttonSound"></param>
    public void OnButtonAttack3(AudioSource buttonSound)
    {
        if (state != BattleState.PLAYER1)
            return;
        state = BattleState.PROCESSING;
        buttonSound.Play();
        StartCoroutine(Attack(2, playerDetails, enemyDetails));
        
    }

    /// <summary>
    /// player uses button 4
    /// </summary>
    /// <param name="buttonSound"></param>
    public void OnButtonGuardUp(AudioSource buttonSound)
    {
        if (state != BattleState.PLAYER1)
            return;
        state = BattleState.PROCESSING;
        buttonSound.Play();
        StartCoroutine(Attack(3, playerDetails, enemyDetails));
        
    }


    /// <summary>
    /// player uses button 5
    /// </summary>
    /// <param name="buttonSound"></param>
    public void OnButtonRest(AudioSource buttonSound)
    {
        if (state != BattleState.PLAYER1)
            return;
        state = BattleState.PROCESSING;
        buttonSound.Play();
        StartCoroutine(Attack(4, playerDetails, enemyDetails));
    }
     

}
