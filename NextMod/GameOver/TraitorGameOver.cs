using NEXT.Features;
using NEXT.Roles.Neutral;
using MiraAPI.GameEnd;
using MiraAPI.Utilities;

namespace NEXT.GameOver
{
	public sealed class TraitorGameOver : CustomGameOver
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x0000A0CC File Offset: 0x000082CC
		public override bool VerifyCondition(PlayerControl playerControl, NetworkedPlayerInfo[] winners)
		{
			return winners is [{ Role: TraitorRole }];
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000A0FF File Offset: 0x000082FF
		public override void AfterEndGameSetup(EndGameManager endGameManager)
		{
			endGameManager.WinText.text = "Traitor Wins!";
			endGameManager.WinText.color = LaunchpadPalette.TraitorColor;
			endGameManager.BackgroundBar.material.SetColor(ShaderID.Color, LaunchpadPalette.TraitorColor);
		}
	}
}
