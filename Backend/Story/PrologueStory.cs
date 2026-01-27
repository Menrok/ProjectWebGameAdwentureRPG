using Backend.Models.Game;

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
                "Statek przechyla się gwałtownie. Ludzie tracą równowagę.\n\n" +
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
                "Mężczyzna szybko sięga do plecaka i wyciąga bandaż.\n\n" +
                "— Powinien zatrzymać krwawienie.\n\n" +
                "Podaje ci bandaż.\n\n" +
                "— Przyda się później.",
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
                    NextNodeId = "prologue_ask_name"
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
            Id = "prologue_ask_name",
            IsDialog = true,
            Text =
                "— Jestem Leon. Mieszkam w niewielkiej wiosce po drugiej stronie lasu.\n\n" +
                "— A ty?",
            Choices = new()
            {
                new StoryChoice
                {
                    Text = "Przedstaw się",
                    NextNodeId = "prologue_after_name"
                }
            }
        },

        new StoryNode
        {
            Id = "prologue_after_name",
            IsDialog = true,
            Text = 
                "— Lia.\n\n" +
                "Leon kiwa głową, jakby zapamiętał to imię.",
            Choices = new()
            {
                new StoryChoice
                {
                    Id = "ask_where_after_name",
                    Text = "Gdzie jestem?",
                    ResponseText =
                        "— Na wyspie. Nie ma jej na mapach. Statki tu nie przypływają.",
                },
                new StoryChoice
                {
                    Text = "Zakończ rozmowę",
                    NextNodeId = "prologue_end_scene"
                }
            }
        },

        new StoryNode
        {
            Id = "prologue_end_scene",
            Text =
                "Leon odchodzi w stronę lasu.\n\n" +
                "Zostajesz sama na plaży.\n\n" +
                "Morze jest spokojne.\n" +
                "Za plecami rozciąga się gęsty las.",
            Choices = new()
            {
                new StoryChoice
                {
                    Text = "Rozejrzyj się",
                    NextNodeId = "prologue_free_mode"
                }
            }
        },

        new StoryNode
        {
            Id = "prologue_free_mode",
            IsFreeMode = true,
        },
    };
}
