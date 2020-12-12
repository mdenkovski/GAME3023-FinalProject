using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public Slider slider;

    /// <summary>
    /// Retrieves data from the details to fill in appropriate ui elements.
    /// </summary>
    /// <param name="details"></param>
    public void FillUI(WaifuDetails details)
    {
        WaifuCreator baseDetails = details.waifu; ;

        nameText.text = baseDetails.CharacterName;

        string text = "" + details.Health.ToString() + "/" + baseDetails.HealthMax.ToString();
        healthText.SetText(text, true);

        //Debug.Log(slider.value);
        slider.maxValue = baseDetails.HealthMax;
        slider.value = details.Health;

    }

    /// <summary>
    /// Used to keep the BATTLE UI up-to-date on current health
    /// </summary>
    /// <param name="health"></param>
    public void UpdateHP(float health)
    {
        slider.value = health;
        healthText.text = "" + health.ToString() + "/" + slider.maxValue.ToString();
    }


}
