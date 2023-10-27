// zoe - 2023
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zoe
{
    public class Enemy : MonoBehaviour
    {
        public bool isGnome;
        public bool isFairy;
        public bool isFrog;
        public bool isBee;

        public GameObject gnomeSprite;
        public GameObject fairySprite;
        public GameObject frogSprite;
        public GameObject beeSprite;

        private unit unitComponent;

        private void Awake()
        {
            unitComponent = GetComponent<unit>(); // Assuming you have a unit script attached to the same GameObject
            CheckEnemyType();
            UpdateSprites();
        }

        private void Update()
        {
            CheckEnemyType();
            UpdateSprites();
        }

        private void CheckEnemyType()
        {
            if (isGnome)
            {
                unitComponent.unitName = "Gnome";
            }
            else if (isFairy)
            {
                unitComponent.unitName = "Fairy";
            }
            else if (isFrog)
            {
                unitComponent.unitName = "Frog";
            }
            else if (isBee)
            {
                unitComponent.unitName = "Bee";
            }
            else
            {
                Debug.LogError("No enemy type selected.");
            }
        }

        private void UpdateSprites()
        {
            gnomeSprite.SetActive(isGnome);
            fairySprite.SetActive(isFairy);
            frogSprite.SetActive(isFrog);
            beeSprite.SetActive(isBee);
        }
    }
}
