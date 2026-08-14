using NEXT.Features;
using NEXT.Roles.Neutral;
using MiraAPI.GameEnd;
using MiraAPI.Utilities;

namespace NEXT.GameOver;

public sealed class ExecutionerGameOver : CustomGameOver
{
    public override bool VerifyCondition(PlayerControl playerControl, NetworkedPlayerInfo[] winners)
    {
        return winners is [{ Role: ExecutionerRole }];
    }

    public override void AfterEndGameSetup(EndGameManager endGameManager)
    {
        endGameManager.WinText.text = "Executioner Wins!";
        endGameManager.WinText.color = LaunchpadPalette.ExecutionerColor;
        endGameManager.BackgroundBar.material.SetColor(ShaderID.Color, LaunchpadPalette.ExecutionerColor);
    }
}