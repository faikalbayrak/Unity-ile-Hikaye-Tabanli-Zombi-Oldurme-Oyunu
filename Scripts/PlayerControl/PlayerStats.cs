using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats 
{
    public float health;
    public float poisonLevel;
    public int pistolAmmo;


    public PlayerStats()
    {
        health = 100;
        poisonLevel = 0;
        pistolAmmo = 10;
    }
}
