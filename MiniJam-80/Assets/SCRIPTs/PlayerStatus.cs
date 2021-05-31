using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float catHunger, catMorale, catEnergy, catHealth, catToilet;

    public enum STATTYPE { HUNGER, MORALE, ENERGY, HEALTH, TOILET };

    public void fncSetStat(float amount, STATTYPE getSTAT)
    {
        switch (getSTAT)
        {
            case STATTYPE.HUNGER: catHunger = amount; break;
            case STATTYPE.MORALE: catMorale = amount; break;
            case STATTYPE.ENERGY: catEnergy = amount; break;
            case STATTYPE.HEALTH: catHealth = amount; break;
            case STATTYPE.TOILET: catToilet = amount; break;
            default: break;
        }
    }
    public void fncAdjStat(float amount, STATTYPE getSTAT)
    {
        switch (getSTAT)
        {
            case STATTYPE.HUNGER:
                catHunger += amount;
                if (catHunger < 0) fncSetStat(0, STATTYPE.HUNGER);
                break;
            case STATTYPE.MORALE:
                catMorale += amount;
                if (catHunger < 0) fncSetStat(0, STATTYPE.MORALE);
                break;
            case STATTYPE.ENERGY:
                catEnergy += amount;
                if (catHunger < 0) fncSetStat(0, STATTYPE.ENERGY);
                break;
            case STATTYPE.HEALTH:
                catHealth += amount;
                if (catHunger < 0) fncSetStat(0, STATTYPE.HEALTH);
                break;
            case STATTYPE.TOILET:
                catToilet += amount;
                if (catHunger < 0) fncSetStat(0, STATTYPE.TOILET);
                break;
            default: break;
        }
    }
}
