using System;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("PuncherSound")] 
    [field: SerializeField] public EventReference puncherSound { get; set; }
    
    [field: Header("Step Sound")] 
    [field: SerializeField] public EventReference stepSound { get; set; }
    
    [field: Header("Diary Open Sound")] 
    [field: SerializeField] public EventReference diaryOpenSound { get; set; }
    
    [field: Header("Diary Close Sound")] 
    [field: SerializeField] public EventReference diaryCloseSound { get; set; }

    public static FMODEvents Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
