using UnityEngine;

namespace SoContent
{
    [CreateAssetMenu(fileName = "NewSpacingConfig", menuName = "Configs/SpacingIngridientsConfig")]
    public class SpacingIngredientsConfig : ScriptableObject
    {
        public float GetSpacing(int ingredientCount)
        {
            Debug.Log("ingredientCount " + ingredientCount);

            return ingredientCount switch
            {
                <= 4 => -165f,
                <= 6 => -130f,
                <= 10 => -65f,
                _ => -65f
            };
        }
    }
}