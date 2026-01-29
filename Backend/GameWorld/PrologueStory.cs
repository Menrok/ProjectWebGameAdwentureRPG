using Backend.Models.Game;

namespace Backend.GameWorld;

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
                "Każdy oddech pali w płucach.\n\n" +
                "Gdy próbujesz się poruszyć, przeszywa cię ból w ramieniu.\n" +
                "Nad tobą rozciąga się szare, ciężkie niebo,\n" +
                "a fale spokojnie muskają brzeg,\n" +
                "jakby nocna burza nigdy nie miała miejsca.",
            Choices = new()
            {
                new StoryChoice
                {
                    Text = "Spróbuj wstać",
                    NextNodeId = "prologue_alone_beach"
                }
            }
        },

        new StoryNode
        {
            Id = "prologue_alone_beach",
            Text =
                "Podnosisz się powoli.\n\n" +
                "Plaża jest pusta.\n" +
                "Nie widać nikogo. Nie słychać głosów.\n\n" +
                "Przy linii wody leżą pojedyncze szczątki —\n" +
                "zbyt małe, by dawać nadzieję.\n\n" +
                "Ramię boli coraz mocniej.",
            Choices = new()
            {
                new StoryChoice
                {
                    Text = "Rozejrzyj się",
                    NextNodeId = "prologue_end_scene"
                }
            }
        },

        new StoryNode
        {
            Id = "prologue_end_scene",
            Text =
                "Odwracasz się plecami do morza.\n\n" +
                "Za tobą rozciąga się gęsty las.\n" +
                "Ciemny, cichy, nieruchomy.\n\n" +
                "Nie masz pojęcia, gdzie jesteś.\n" +
                "Wiesz tylko jedno — nie możesz tu zostać.",
            Choices = new()
            {
                new StoryChoice
                {
                    Text = "Zrób pierwszy krok",
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
