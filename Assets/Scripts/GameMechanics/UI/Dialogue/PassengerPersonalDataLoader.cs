using UnityEngine;
using TMPro;
using Core.Scriptable_Objects;
using UnityEngine.UI;

public class PassengerPersonalDataLoader : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private TMP_Text fullName, species, causeOfDeath, biography;

    public void UpdatePassenderID(PassengerData data)
    {
        portrait.sprite = data.passengerPortrait;
        fullName.text = data.GetFullName();
        species.text = data.species;
        causeOfDeath.text = data.causeOfDeath;
        biography.text = data.biography;
    }
}
