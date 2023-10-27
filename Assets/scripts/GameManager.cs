// zoe - 2023
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace zoe
{
    public class GameManager : MonoBehaviour
    {
        public GameObject slotA;
        public GameObject slotB;
        public Image resultImage; // Use the Image component for UI images

        private string currentItemA = "";
        private string currentItemB = "";

        public bool isDebugging = false;

        public string[] recipes; // Public array for recipes
        public Sprite[] results; // Public array for results

        bool recipeFound = false;

        void Start()
        {
            resultImage.gameObject.SetActive(false);
            // Add more initialization logic here if necessary
        }

        void Update()
        {
            UpdateResult();
        }

        public void UpdateResult()
        {
            UpdateCurrentItems(); // Update the current items in the slots

            string currentRecipe = currentItemA + currentItemB;

            if (isDebugging){Debug.Log("Current Recipe: " + currentRecipe);}

            for (int i = 0; i < recipes.Length; i++)
            {
                if (currentRecipe.Equals(recipes[i]))
                {
                    if (i < results.Length)
                    {
                        // Set the UI image sprite to the corresponding result sprite
                        resultImage.sprite = results[i];
                        recipeFound = true;
                        if (isDebugging){Debug.Log("Result updated.");}
                        break;
                    }
                }
                else
                {
                    recipeFound = false;
                }
            }

            resultImage.gameObject.SetActive(recipeFound);
        }

        public void UpdateCurrentItems()
        {
            if (slotA.transform.childCount > 0)
            {
                currentItemA = slotA.transform.GetChild(0).GetComponent<DraggableItem>().ItemName;
            }
            else
            {
                currentItemA = "";
            }

            if (slotB.transform.childCount > 0)
            {
                currentItemB = slotB.transform.GetChild(0).GetComponent<DraggableItem>().ItemName;
            }
            else
            {
                currentItemB = "";
            }
        }

        public string GetCurrentRecipe()
        {
            return currentItemA + currentItemB;
        }
    }
}
