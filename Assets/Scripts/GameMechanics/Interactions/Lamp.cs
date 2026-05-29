using System.Collections.Generic;
using Core;
using GameMechanics.Interactions;
using UnityEngine;

public class Lamp : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Light> lights = new();
    public string GetName()
    {
        return "Lamp";
    }

    public void Interact()
    {
        foreach (Light light in lights)
        {
            light.enabled = !light.enabled;
        }

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.lampInteracted, transform.position);
    }
}
