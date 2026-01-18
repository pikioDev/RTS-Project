namespace Xenocode.Features.MatchStatus.Scripts.Domain.Model
{
    public interface IMatchStatusView
    {
        void UpdateIndividualBar(int teamIndex, float currentHealth, float maxHealth);
        void UpdateSharedBar(float teamAHealth, float teamBHealth);
        void ShowVictory();
    }
}