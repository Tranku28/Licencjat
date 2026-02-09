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
            public string PassengerAction, Decision, HarmonyValue, RuleBreak;

            public CachedSummary(string passengerAction, string decision, string harmony, string ruleBreak = null)
            {
                PassengerAction = passengerAction;
                Decision = decision;
                HarmonyValue = harmony;
                RuleBreak = ruleBreak;
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
        }

        public void DisplaySummary()
        {
            if (string.IsNullOrEmpty(_summary.RuleBreak))
                ruleBreak.enabled = false;
            else
                ruleBreak.enabled = true;

            _textPrinter
                .BeginChain()
                .ThenPrint(passengerAction, _summary.PassengerAction)
                .ThenPrint(decision, $"Decision: {_summary.Decision}")
                .ThenPrint(harmonyValue, $"Harmony Status: (+{_summary.HarmonyValue})")
                .ThenPrint(ruleBreak, string.IsNullOrEmpty(_summary.RuleBreak) ? "" : _summary.RuleBreak)
                .OnChainComplete(() =>
                {
                    Debug.Log("Summary Finished");
                })
                .PlayChain();
        }
    }
}
