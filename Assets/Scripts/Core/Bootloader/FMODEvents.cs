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
    [field: Header("Save Board Hover")] 
    [field: SerializeField] public EventReference saveBoardHover { get; set; }
    [field: Header("Save Board Open")] 
    [field: SerializeField] public EventReference saveBoardOpen { get; set; }
    [field: Header("Watch Hover")] 
    [field: SerializeField] public EventReference watchHover { get; set; }
    [field: Header("Watch Open")] 
    [field: SerializeField] public EventReference watchOpen { get; set; }
    [field: Header("Credits Hover")] 
    [field: SerializeField] public EventReference creditsHover { get; set; }
    [field: Header("Credits Open")] 
    [field: SerializeField] public EventReference creditsOpen { get; set; }
    [field: Header("Candle Blow")] 
    [field: SerializeField] public EventReference candleBlow { get; set; }
    [field: Header("Candle Hover")] 
    [field: SerializeField] public EventReference candleHover { get; set; }
    [field: Header("Candle Put Down")]
    [field: SerializeField] public EventReference candlePutDown { get; set; }
    [field: Header("Door Open")]
    [field: SerializeField] public EventReference doorOpen { get; set; }
    [field: Header("Door Close")]
    [field: SerializeField] public EventReference doorClose { get; set; }
    [field: Header("Rewind")]
    [field: SerializeField] public EventReference rewind { get; set; }
    [field: Header("Harmony")]
    [field: SerializeField] public EventReference harmonyDisorder { get; set; }
    [field: Header("Notification")]
    [field: SerializeField] public EventReference notification { get; set; }
    [field: Header("Diary Entry")]
    [field: SerializeField] public EventReference diaryEntry { get; set; }
    [field: Header("Typewriter")]
    [field: SerializeField] public EventReference dayEndTypewriter { get; set; }
    [field: Header("Potion")]
    [field: SerializeField] public EventReference potionUse { get; set; }

    [field: Header("====== Music & Ambient ======")]
    [field: SerializeField] public EventReference ambient {get; set;}
    [field: SerializeField] public EventReference music {get; set;}


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
