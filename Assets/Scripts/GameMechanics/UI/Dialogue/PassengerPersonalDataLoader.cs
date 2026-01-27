using UnityEngine;
using TMPro;
using Core.Scriptable_Objects;

public class PassengerPersonalDataLoader : MonoBehaviour
{
    [SerializeField] private TMP_Text fullName, species, causeOfDeath, biography;

    public void UpdatePassenderID(PassengerData data)
    {
        fullName.text = data.GetFullName();
        species.text = data.species;
        causeOfDeath.text = data.causeOfDeath;
        biography.text = data.biography;
    }
}
