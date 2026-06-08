using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanics.UI
{
    public class DaySummaryHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text passengerAction, decision, harmonyValue, ruleBreak, finalNote;

        private CachedSummary _summary;
        private struct CachedSummary
        {
            public string PassengerAction, Decision, RuleBroken;
            public int HarmonyValue;

            public CachedSummary(string passengerAction, string decision, int harmony, string ruleBreak)
            {
                PassengerAction = passengerAction;
                Decision = decision;
                HarmonyValue = harmony;
                RuleBroken = ruleBreak;
            }
        }

        private void Start()
        {
            DialogueManager.OnDialogueQuitEvent += CacheSummaryData;
            BlinkPanelUI.OnSummaryDisplay += DisplaySummary;
            BlinkPanelUI.OnSummaryDisplayFinalDay += DisplayFifthDaySummary;
        }

        private void OnDestroy()
        {
            DialogueManager.OnDialogueQuitEvent -= CacheSummaryData;
            BlinkPanelUI.OnSummaryDisplay -= DisplaySummary;
            BlinkPanelUI.OnSummaryDisplayFinalDay -= DisplayFifthDaySummary;
        }

        private TextPrinter _textPrinter = new TextPrinter();

        private void CacheSummaryData(object sender, DialogueEndEventArgs e)
        {
            _summary = new CachedSummary(e.PassengerAction, e.Decision, e.HarmonyValue, e.RuleBroken);
        }

        public void DisplaySummary()
        {
            finalNote.enabled = false;
            DependencyResolver.Instance.GetType<ApplicationGlobalSettings>().CursorActive(true);

            string harmonyFormat = _summary.HarmonyValue > 0 ? $"+{_summary.HarmonyValue}" : $"{_summary.HarmonyValue}";
            if (string.IsNullOrEmpty(_summary.RuleBroken))
                ruleBreak.enabled = false;
            else
                ruleBreak.enabled = true;
 
            _textPrinter
                .BeginChain()
                .ThenPrint(passengerAction, _summary.PassengerAction, this)
                .ThenPrint(decision, $"Decision: {_summary.Decision}", this)
                .ThenPrint(harmonyValue, $"Harmony Status: {harmonyFormat}", this)
                .ThenPrint(ruleBreak, string.IsNullOrEmpty(_summary.RuleBroken) ? "" : _summary.RuleBroken, this)
                .OnChainComplete(() =>
                {
                    Debug.Log("Summary Finished");
                })
                .PlayChain();
        }

        public void DisplayFifthDaySummary()
        {
            passengerAction.enabled = false;
            decision.enabled = false;
            harmonyValue.enabled = false;
            ruleBreak.enabled = false;

            _textPrinter.Print(finalNote, "You've reached day five of your journey.\nThis is the last day you can review everything you've done so far.\nThank you for playing The Enchanted Express. We hope you liked the journey. Take care \n\n ~ The Enchanted Express crew");

            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.dayEndTypewriter, transform.position);
        }
    }
}
