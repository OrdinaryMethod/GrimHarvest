using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    [Range(0.0f, 50.0f)] //Measured in units
    public int playerHealth;
    [Range(0.0f, 100.0f)] //Measured in units
    public int playerSanity;
    [Range(0.0f, 100.0f)]
    public int playerSpeed;

    [Header("Combat")]
    [Range(0.0f, 1000.0f)]
    public int shootingDamage;
    [Range(0.0f, 1000.0f)]
    public int damage;
}
