using TMPro;
using UnityEngine;

namespace GameMechanics.UI
{
    public class DaySummaryHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text passengerAction, decision, harmonyValue, ruleBreak;

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
        }

        private void OnDestroy()
        {
            DialogueManager.OnDialogueQuitEvent -= CacheSummaryData;
            BlinkPanelUI.OnSummaryDisplay -= DisplaySummary;
        }

        private TextPrinter _textPrinter = new TextPrinter();

        private void CacheSummaryData(object sender, DialogueEndEventArgs e)
        {
            _summary = new CachedSummary(e.PassengerAction, e.Decision, e.HarmonyValue, e.RuleBroken);
            Debug.Log(
                $"Harmony: {e.HarmonyValue}"
                );
        }

        public void DisplaySummary()
        {
            string harmonyFormat = _summary.HarmonyValue > 0 ? $"+{_summary.HarmonyValue}" : $"{_summary.HarmonyValue}";
            if (string.IsNullOrEmpty(_summary.RuleBroken))
                ruleBreak.enabled = false;
            else
                ruleBreak.enabled = true;

            _textPrinter
                .BeginChain()
                .ThenPrint(passengerAction, _summary.PassengerAction)
                .ThenPrint(decision, $"Decision: {_summary.Decision}")
                .ThenPrint(harmonyValue, $"Harmony Status: {harmonyFormat}")
                .ThenPrint(ruleBreak, string.IsNullOrEmpty(_summary.RuleBroken) ? "" : _summary.RuleBroken)
                .OnChainComplete(() =>
                {
                    Debug.Log("Summary Finished");
                })
                .PlayChain();
        }
    }
}
