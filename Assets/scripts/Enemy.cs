// zoe - 2023
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zoe
{
    public class Enemy : MonoBehaviour
    {
        public static Enemy instance; // Singleton instance to hold the currently selected enemy type

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
            SetEnemyProperties();

            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            unitComponent = GetComponent<unit>(); // Assuming you have a unit script attached to the same GameObject
            CheckEnemyType();
            UpdateSprites();
        }

        private void Update()
        {

            SetEnemyProperties();
            CheckEnemyType();
            UpdateSprites();
        }

        public void SetEnemyProperties()
        {
            isGnome = PlayerPrefs.GetInt("IsGnome", 0) == 1;
            isFairy = PlayerPrefs.GetInt("IsFairy", 0) == 1;
            isFrog = PlayerPrefs.GetInt("IsFrog", 0) == 1;
            isBee = PlayerPrefs.GetInt("IsBee", 0) == 1;
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
