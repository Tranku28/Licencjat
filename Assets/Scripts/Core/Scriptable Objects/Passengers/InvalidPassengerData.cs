using UnityEngine;

namespace Core.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "InvalidPassengerData", menuName = "Scriptable Objects/InvalidPassengerData")]
    public class InvalidPassengerData : PassengerData
    {
        [field: SerializeField] public string invalidName;
        [field: SerializeField] public string invalidSurname;
    }
}
