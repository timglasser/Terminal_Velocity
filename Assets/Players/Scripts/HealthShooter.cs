/*HealthShooter.cs	*/							


using UnityEngine;
using System.Collections;

public class HealthShooter : MonoBehaviour
{

    #region Health Vars
    public bool DV_God_Mode = false;

    private bool DestroybleObject = false;
    private DestroyableObject MyDestroyableObject;

    private bool Player = false;

    public bool Regeneration = false;
    public int AmountRegeneration = 50;
    public float DelayRegeneration = 1.5f;
    private float DRReadyTime = 0f;
    public float SpeedRegeneration = 0.15f;
    private float SRReadyTime = 0f;

    public int MinHealth = 0;
    public int MaxHealth = 100;
    public int CurrentHealth = 100;

    public ArmorTypes ArmorType;

    private float Hardness = 0f;
    private float CalculatedDamage = 0f;


    #endregion

    #region Awake
    void Awake() { 

        Animator anim = GetComponent<Animator>();
        anim.SetInteger("Health", CurrentHealth);


        if (gameObject.tag == TagsManager.Player) Player = true;
        if (gameObject.tag == TagsManager.DestroyableObject) DestroybleObject = true;

        switch (ArmorType)
        {
            case ArmorTypes.None: Hardness = 20f; break;
            case ArmorTypes.Lite: Hardness = 26f; break;
            case ArmorTypes.Medium: Hardness = 33.8f; break;
            case ArmorTypes.Heavy: Hardness = 43.94f; break;
            case ArmorTypes.Ultra: Hardness = 57.12f; break;
        }

        if (DestroybleObject)
        {
            MyDestroyableObject = GetComponent<DestroyableObject>();
            ArmorType = ArmorTypes.None;
            Hardness = 10f;
        }
    }
    #endregion

    #region Update
    void Update()
    {
        if (Regeneration) CalculateRegeneration();
    }
    #endregion

    #region DamageHealth
    public void DamageHealth(int HitPoints)
    {
        if (!DV_God_Mode)
        {
          //  DamageCalculator(BulletMass * BulletSpeed, BulletMass + Hardness);
            DecrementHealth(HitPoints);
            if (Regeneration)
            {
                DRReadyTime = 0f;
                SRReadyTime = 0f;
            }
        }
    }
    #endregion

/*
    #region DamageCalculator
    private void DamageCalculator(float BulletImpulse, float BMHNSumm)
    {
        CalculatedDamage = (((BulletImpulse * 2f) / (2f * BMHNSumm)) * 1.5f) / Hardness;
        DecrementHealth(CalculatedDamage);
    }
    #endregion
*/

    #region CalculateRegeneration
    private void CalculateRegeneration()
    {
        if (CurrentHealth < MaxHealth)
        {
            DRReadyTime += Time.deltaTime;
            if (DRReadyTime > DelayRegeneration)
            {
                SRReadyTime += Time.deltaTime;
                if (SRReadyTime > SpeedRegeneration)
                {
                    IncrementHealth(AmountRegeneration);
                    SRReadyTime = 0f;
                }
            }
        }
        else
        {
            DRReadyTime = 0f;
            SRReadyTime = 0f;
        }
    }
    #endregion

    #region IncrementHealth
    public void IncrementHealth(int Increment)
    {
        CurrentHealth += Increment;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
    }
    #endregion

    #region DecrementHealth
    public void DecrementHealth(int Decrement)
    {
        CurrentHealth -= Decrement;
        if (CurrentHealth < MinHealth)
        {
            if (Player) Application.LoadLevel(Application.loadedLevelName);

            if (DestroybleObject) MyDestroyableObject.DestroyRun();
            else gameObject.SetActive(false);
        }
    }
    #endregion
}
