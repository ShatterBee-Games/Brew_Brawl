// zoe - 2023
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zoe {
public class unit : MonoBehaviour
{
   public string unitName;
   public int unitLevel;
   public int damage;
   public int maxHP; 
   public int currentHP;

   public bool takeDamage(int dmg) => (currentHP -= dmg) <= 0;

   public void heal(int amount)
   {
        currentHP += amount; 
        currentHP = currentHP > maxHP ? maxHP : currentHP; 
   }
}
}
