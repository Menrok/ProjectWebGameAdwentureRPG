using Backend.Models.Game;
using Backend.DTOs.Game;

namespace Backend.Story;

public static class PrologueStory
{
    public static readonly List<StoryNode> Nodes = new()
    {
        new StoryNode
        {
            Id = "prologue_intro",
            Text =
                "Sztorm jest silniejszy, niż ktokolwiek przewidywał.\n\n" +
                "Niebo rozdzierają błyskawice. Fale uderzają z pełną siłą, jedna po drugiej.\n" +
                "Metalowy statek przechyla się gwałtownie. Ludzie tracą równowagę.\n\n" +
                "Ktoś krzyczy. Wiatr natychmiast zagłusza ten dźwięk.\n\n" +
                "Woda zalewa pokład. Światła gasną.\n\n" +
                "A potem —\n" +
                "ciemność.",
            Choices = new()
            {
                new StoryChoice
                {
                    Text = "Dalej",
                    NextNodeId = "prologue_wake"
                }
            }
        },

        new StoryNode
        {
            Id = "prologue_wake",
            Text =
                "Budzisz się na wilgotnym, zimnym piasku.\n" +
                "Każdy oddech pali w płucach, a ciało boli tak, jakbyś spadła z klifu.\n\n" +
                "Nad tobą rozciąga się szare, ciężkie niebo,\n" +
                "a fale spokojnie muskają brzeg,\n" +
                "jakby nocna burza nigdy nie miała miejsca.\n\n" +
                "Wokół nie widać nikogo.",
            Choices = new()
            {
                new StoryChoice
                {
                    Text = "Spróbuj wstać",
                    NextNodeId = "prologue_npc_appears"
                }
            }
        },

        new StoryNode
        {
            Id = "prologue_npc_appears",
            Text =
                "Gdy próbujesz się podnieść, zauważasz sylwetkę nadchodzącą z oddali.\n" +
                "To mężczyzna z plecakiem przewieszonym przez ramię.\n" +
                "Zatrzymuje się kilka kroków od ciebie i przygląda uważnie.\n\n" +
                "— Cholera… żyjesz.\n\n" +
                "Jego wzrok natychmiast pada na twoje ramię.\n\n" +
                "— Jesteś ranna. To trzeba opatrzyć.",
            Choices = new()
            {
                new StoryChoice
                {
                    Text = "Pozwól mu pomóc",
                    NextNodeId = "prologue_give_bandage"
                }
            }
        },

        new StoryNode
        {
            Id = "prologue_give_bandage",
            Text =
                "Mężczyzna szybko sięga do plecaka i wyciąga prowizoryczny bandaż.\n\n" +
                "— Nie jest sterylny, ale zatrzyma krwawienie.\n\n" +
                "Pomaga ci owinąć rozcięte ramię.\n\n" +
                "— Już. Teraz oddychaj spokojnie.",
            Choices = new()
            {
                new StoryChoice
                {
                    Text = "Spróbuj się podnieść",
                    NextNodeId = "prologue_dialog"
                }
            }
        },

        new StoryNode
        {
            Id = "prologue_dialog",
            IsDialog = true,
            Text = "Mężczyzna patrzy na ciebie uważnie.",
            Choices = new()
            {
                new StoryChoice
                {
                    Id = "ask_who",
                    Text = "Kim jesteś?",
                    ResponseText =
                        "— Jestem Leon. Mieszkam w niewielkiej wiosce po drugiej stronie lasu.",
                },
                new StoryChoice
                {
                    Id = "ask_where",
                    Text = "Gdzie jestem?",
                    ResponseText =
                        "— Na wyspie. Nie ma jej na mapach. Statki tu nie przypływają.",
                },
                new StoryChoice
                {
                    Text = "Zakończ rozmowę",
                    NextNodeId = "prologue_free_mode"
                }
            }
        },

        new StoryNode
        {
            Id = "prologue_free_mode",
            IsFreeMode = true,
            Text =
                "Leon odchodzi w stronę lasu.\n\n" +
                "Zostajesz sama na plaży.\n\n" +
                "Morze jest spokojne.\n" +
                "Za plecami rozciąga się gęsty las.\n\n" +
                "Możesz ruszyć w głąb wyspy albo chwilę odpocząć."
        }
    };
}
