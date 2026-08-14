using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using NEXT.Events;
using NEXT.Options.Roles.Neutral;

namespace NEXT.Modifiers;

public sealed class SurvivorVestModifier : TimedModifier
{
    public override float Duration => OptionGroupSingleton<SurvivorOptions>.Instance.VestDuration;
    public override string ModifierName => "Vested";
    public override bool AutoStart => true;
    public override bool HideOnUi => true;

    public override void OnActivate()
    {
        base.OnActivate();

        var abilityEvent = new AbilityEvent(AbilityType.SurvivorVest, Player);
        MiraEventManager.InvokeEvent(abilityEvent);
    }

    public override void OnMeetingStart()
    {
        ModifierComponent?.RemoveModifier(this);
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent?.RemoveModifier(this);
    }
}