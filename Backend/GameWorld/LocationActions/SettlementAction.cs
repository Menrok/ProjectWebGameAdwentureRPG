using Backend.Data;
using Backend.Models.Game;
using Backend.DTOs.Game;
using Backend.Services;
using Backend.GameWorld.Quests;

namespace Backend.GameWorld.LocationActions;

public class SettlementAction
{
    private readonly GameDbContext _context;
    private readonly InventoryService _inventoryService;
    private readonly QuestService _questService;

    public SettlementAction(GameDbContext context, InventoryService inventoryService, QuestService questService)
    {
        _context = context;
        _inventoryService = inventoryService;
        _questService = questService;
    }

    public ActionResultDto Execute(Player player, string actionId)
    {
        return actionId switch
        {
            "enter_settlement" => EnterSettlement(player),
            "look_around" => LookAround(),

            "talk_trader" => TalkTrader(player),
            "talk_survivor" => TalkSurvivor(player),
            "talk_guard" => TalkGuard(player),

            "trader_settlement" => TraderAskSettlement(),
            "trader_island" => TraderAskIsland(),
            "trader_mountain" => TraderAskMountain(),
            "trader_cave" => TraderAskCave(player),
            "trader_trade" => TraderAskTrade(player),
            "open_trade" => OpenTrade(),

            "survivor_who" => SurvivorWho(),
            "survivor_camp" => SurvivorCamp(player),
            "survivor_warning" => SurvivorWarning(),

            "guard_warning" => GuardWarning(),
            "guard_city" => GuardCity(player),

            _ => new ActionResultDto { Text = "Nie możesz tego teraz zrobić." }
        };
    }

    public List<LocationAction> GetAvailableActions(Player player)
    {
        var actions = new List<LocationAction>
        {
            new() { Id = "enter_settlement", Text = "Wejdź do osady" },
            new() { Id = "look_around", Text = "Rozejrzyj się" },
            new() { Id = "talk_trader", Text = "Porozmawiaj z handlarzem" },
            new() { Id = "talk_survivor", Text = "Porozmawiaj z ocalałą" },
            new() { Id = "talk_guard", Text = "Podejdź do strażnika" }
        };

        if (player.HasFlag("talking_to_trader"))
        {
            actions.AddRange(new[]
            {
                new LocationAction { Id = "trader_settlement", Text = "Zapytaj o osadę" },
                new LocationAction { Id = "trader_island", Text = "Zapytaj o wyspę" },
                new LocationAction { Id = "trader_mountain", Text = "Zapytaj o górę" },
                new LocationAction { Id = "trader_cave", Text = "Zapytaj o jaskinię" },
                new LocationAction { Id = "trader_trade", Text = "Zapytaj o handel" }
            });

            if (player.HasFlag("trader_trade_unlocked") && !player.HasFlag("flashlight_bought"))
            {
                actions.Add(new LocationAction
                {
                    Id = "open_trade",
                    Text = "Handel"
                });
            }
        }

        if (player.HasFlag("talking_to_survivor"))
        {
            actions.AddRange(new[]
            {
                new LocationAction { Id = "survivor_who", Text = "Kim jesteś?" },
                new LocationAction { Id = "survivor_camp", Text = "Zapytaj o obóz" },
                new LocationAction { Id = "survivor_warning", Text = "Czy to miejsce jest bezpieczne?" }
            });
        }

        if (player.HasFlag("talking_to_guard"))
        {
            actions.AddRange(new[]
            {
                new LocationAction { Id = "guard_warning", Text = "Zapytaj o zagrożenia" },
                new LocationAction { Id = "guard_city", Text = "Zapytaj o miasto" }
            });
        }

        return actions;
    }

    private ActionResultDto EnterSettlement(Player player)
    {
        if (!player.HasFlag("settlement_entered"))
        {
            player.AddFlag("settlement_entered");

            _questService.AdvanceQuest(
                player,
                QuestIds.EscapeIsland,
                1,
                QuestDefinitions.EscapeIslandStage
            );
        }

        return new()
        {
            Text =
                "Docierasz do niewielkiej osady.\n\n" +
                "Ludzie patrzą nieufnie, ale nikt nie zadaje pytań.\n\n" +
                "To nie jest miejsce ratunku. To miejsce oczekiwania."
        };
    }

    private ActionResultDto LookAround() =>
        new()
        {
            Text =
                "Osada wygląda na tymczasową.\n" +
                "Jakby wszyscy byli tu tylko przejazdem."
        };

    private ActionResultDto TalkTrader(Player player)
    {
        player.RemoveFlagsByPrefix("talking_to_");
        player.AddFlag("talking_to_trader");

        return new()
        {
            Text = "Handlarz mierzy cię wzrokiem.\n\n\"Czego potrzebujesz?\""
        };
    }

    private ActionResultDto TalkSurvivor(Player player)
    {
        player.RemoveFlagsByPrefix("talking_to_");
        player.AddFlag("talking_to_survivor");

        return new()
        {
            Text = "Kobieta podnosi wzrok.\n\n\"Jeśli szukasz odpowiedzi… to źle trafiłaś.\""
        };
    }

    private ActionResultDto TalkGuard(Player player)
    {
        player.RemoveFlagsByPrefix("talking_to_");
        player.AddFlag("talking_to_guard");

        return new()
        {
            Text = "Strażnik stoi nieruchomo.\n\n\"Nie wychodź po zmroku.\""
        };
    }

    private ActionResultDto TraderAskSettlement() => new() { Text = "\"Osada? Tymczasowa. Jak wszystko tutaj.\"" };
    private ActionResultDto TraderAskIsland() => new() { Text = "\"Wyspa nie lubi ciekawskich.\"" };
    private ActionResultDto TraderAskMountain() => new() { Text = "\"Góra zabiera więcej, niż oddaje.\"" };

    private ActionResultDto TraderAskCave(Player player)
    {
        player.AddFlag("cave_hint_received");
        return new() { Text = "\"Bez światła tam nie wchodź.\"" };
    }

    private ActionResultDto TraderAskTrade(Player player)
    {
        if (!player.HasFlag("trader_trade_unlocked"))
        {
            player.AddFlag("trader_trade_unlocked");

            _questService.AdvanceQuest(
                player,
                QuestIds.CaveMystery,
                1,
                QuestDefinitions.CaveStage
            );
        }

        return new()
        {
            Text = "\"Zajrzyj do mojej oferty. Bez światła nie wrócisz stamtąd żywa.\""
        };
    }

    private ActionResultDto OpenTrade() =>
        new()
        {
            Text = "Handlarz rozkłada swoje towary.",
            OpenTradeId = "settlement_trader"
        };

    private ActionResultDto SurvivorWho() => new() { Text = "\"Byłam badaczką. Teraz… tylko czekam.\"" };

    private ActionResultDto SurvivorCamp(Player player)
    {
        player.AddFlag("camp_hint_received");
        return new() { Text = "\"Obóz w lesie. Nie wszyscy wrócili.\"" };
    }

    private ActionResultDto SurvivorWarning() => new() { Text = "\"Nie ufaj ciszy. Ona tu kłamie.\"" };

    private ActionResultDto GuardWarning() => new() { Text = "\"Po zmroku coś się budzi.\"" };

    private ActionResultDto GuardCity(Player player)
    {
        player.AddFlag("city_locked");
        return new() { Text = "\"Miasto? Jeszcze nie dla ciebie.\"" };
    }
}
