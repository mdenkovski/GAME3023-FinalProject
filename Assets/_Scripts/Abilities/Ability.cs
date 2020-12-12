using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To keep track which buffs and debuffs affects which stat
/// </summary>
enum BUFF_ARRAY
{
    ATTACK,
    DEFENCE,
    LOVE,
    COUNT
}

/// <summary>
/// Used for Particles to have them play on the correct target
/// </summary>
enum TargetedPlayer
{
    SELF,
    ENEMY
}

/// <summary>
/// Scriptable Object to store all the information about a particular ability.
/// </summary>
[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability", order = 1)]
public class Ability : ScriptableObject
{
    [SerializeField]
    private int abilityID;

    [SerializeField]
    private string abilityName;

    [SerializeField]
    private float attackMultiplier;

    [SerializeField]
    private string description = "This is a move";

    [SerializeField]
    private float percentChance;

    [SerializeField]
    private int costHP;    

    [SerializeField]
    private int[] selfBuff = new int[3];

    [SerializeField]
    private int[] selfDebuff = new int[3];

    [SerializeField]
    private int[] enemyBuff = new int[3];

    [SerializeField]
    private int[] enemyDebuff = new int[3];

    [SerializeField]
    private float LoveHealMultiplier;

    [Header("ParticleSystem")]
    [SerializeField]
    ParticleSystem particleSystem;

    [SerializeField]
    TargetedPlayer target;

    [Header("AudioEffect")]
    [SerializeField]
    AudioClip clip;

    public int AbilityId
    {
        get { return abilityID; }
        set { abilityID = value; }
    }

    public string AbilityName
    {
        get { return abilityName; }
    }

    public float AttackMultipier
    {
        get { return attackMultiplier; }
    }
    
    public string Description
    {
        get { return description; }
    }

    public float PercentChance
    {
        get { return percentChance; }
    }

    public int CostHp
    {
        get { return costHP; }
    }

    public int [] SelfBuff
    {
        get { return selfBuff; }
    }

    public int[] SelfDebuff
    {
        get { return selfDebuff; }
    }

    public int[] EnemyBuff
    {
        get { return enemyBuff; }
    }

    public int[] EnemyDebuff
    {
        get { return enemyDebuff; }
    }

    public float LoveMultiplier
    {
        get { return LoveHealMultiplier; }
    }

    public AudioClip Clip
    {
        get { return clip; }
    }

    /// <summary>
    /// Instantiate a new particle system at target.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    public void PlayEffects(Transform attacker, Transform defender)
    {
        if (particleSystem != null)
        {
            var system = Instantiate(particleSystem, target == TargetedPlayer.SELF ? attacker.Find("Sprite").transform : defender.Find("Sprite").transform);
            
        }
    }

    

}


