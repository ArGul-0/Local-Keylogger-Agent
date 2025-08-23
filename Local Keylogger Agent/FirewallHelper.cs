using System;

public static class FirewallHelper
{
    private const int pritocolTCP = 6;
    private const int pritocolUDP = 17;

    private const int DirectionIn = 1;

    private const int AcctionAllow = 1;

    public static void EnsurePortRule(string ruleName, string description, int protocolNumber, string localPorts)
    {
        try
        {
            var policyType = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            dynamic policy = Activator.CreateInstance(policyType);
            dynamic rules = policy.Rules;

            foreach (dynamic r in rules)
            {
                try
                {
                    if (r.Name == ruleName)
                    {
                        return;
                    }
                }
                catch {}
            }

            var ruleType = Type.GetTypeFromProgID("HNetCfg.FwRule");
            dynamic newRule = Activator.CreateInstance(ruleType);

            newRule.Name = ruleName;
            newRule.Description = description ?? ruleName;
            newRule.Protocol = protocolNumber;
            newRule.LocalPorts = localPorts;   
            newRule.Direction = DirectionIn;
            newRule.Enabled = true;
            newRule.Action = AcctionAllow;
            newRule.Profiles = 1 | 2 | 4;

            rules.Add(newRule);
        }
        catch (Exception)
        {   }
    }

    public static void EnsureUdpPort(string ruleName, int port, string desc = null)
        => EnsurePortRule(ruleName, desc, pritocolUDP, port.ToString());

    public static void EnsureTcpPort(string ruleName, int port, string desc = null)
        => EnsurePortRule(ruleName, desc, pritocolTCP, port.ToString());
}
