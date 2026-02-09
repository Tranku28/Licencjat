public class RulesValidator
{
    public string GetBrokenRule(RuleBreak rule)
    {
        return rule switch
        {
            RuleBreak.InvalidPersonalData => "Invalid personal data",
            RuleBreak.ExpiredTicket => "Ticket expired",
            _ => "No rules broken",
        };
    }
}


