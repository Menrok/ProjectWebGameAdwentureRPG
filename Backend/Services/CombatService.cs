using Backend.Models.Game;

namespace Backend.Services;

public class CombatService
{
    private readonly Random _random = new();

    public int CalculateAttackDamage(int minAttack, int maxAttack)
    {
        if (minAttack < 0) minAttack = 0;
        if (maxAttack < minAttack) maxAttack = minAttack;

        return _random.Next(minAttack, maxAttack + 1);
    }

    public int ApplyDefense(int damage, int defense)
    {
        var finalDamage = damage - defense;
        return finalDamage < 0 ? 0 : finalDamage;
    }

    public void ApplyDamage(Player target, int damage)
    {
        if (damage <= 0)
            return;

        target.Health -= damage;

        if (target.Health < 0)
            target.Health = 0;
    }

    public int PerformAttack(int attackerMinAttack, int attackerMaxAttack, int defenderDefense, Player defender)
    {
        var rawDamage = CalculateAttackDamage(attackerMinAttack, attackerMaxAttack);
        var finalDamage = ApplyDefense(rawDamage, defenderDefense);

        ApplyDamage(defender, finalDamage);

        return finalDamage;
    }

    public void ApplyEnvironmentalDamage(Player player, int damage)
    {
        if (damage <= 0)
            return;

        player.Health -= damage;

        if (player.Health < 0)
            player.Health = 0;
    }
}
