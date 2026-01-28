# Historia wersji

## v0.0.1
- Utworzono repozytorium projektu.
- Dodano podstawową dokumentację techniczną projektu.
- Skonfigurowano komunikację frontend–backend (HTTP, JSON).
- Dodano system rejestracji i logowania użytkowników.
- Zaimplementowano uwierzytelnianie JWT po stronie backendu.

## v0.0.2
- Zaimplementowano system ekwipunku gracza (plecak z limitem 10 przedmiotów).
- Dodano bazę przedmiotów gry (Items) wraz z mechanizmem seedowania danych.
- Wprowadzono obsługę przedmiotów jednorazowych (consumable) oraz ekwipunku.
- Zaimplementowano dwa sloty ekwipunku: broń oraz ubranie.
- Dodano logikę zakładania i zdejmowania przedmiotów.
- Wprowadzono bazowe statystyki gracza (atak, obrona, zdrowie).
- Zaimplementowano dynamiczne liczenie statystyk na podstawie założonych przedmiotów.
- Dodano serwis zarządzania ekwipunkiem (InventoryService).
- Dodano kontroler API do obsługi ekwipunku i stanu gracza.
- Udostępniono endpoint prezentujący aktualny stan gracza (HP, statystyki, ekwipunek).

## v0.0.3
- Zaimplementowano system fabularny oparty o węzły historii (StoryNode).
- Dodano prolog gry wraz z dialogami i wyborami gracza.
- Wprowadzono efekty fabularne wpływające na stan gracza (nagrody, zmiana rozdziału).
- Zintegrowano fabułę z systemem ekwipunku (nagroda: bandaż).
- Dodano stabilne identyfikatory gameplayowe przedmiotów (Item.Code).
- Zaktualizowano mechanizm seedowania przedmiotów o kody przedmiotów.
- Zrefaktoryzowano system ekwipunku (IsEquipped, przygotowanie pod stackowanie).
- Wprowadzono system lokacji świata gry (plaża, las, wioska).
- Dodano połączenia pomiędzy lokacjami oraz możliwość powrotu.
- Zaimplementowano tryb swobodnej eksploracji świata (free mode).
- Utworzono centralne definicje świata gry (LocationsData).
- Zaimplementowano system akcji zależnych od lokacji.
- Dodano akcje dostępne na plaży (przeszukiwanie, odpoczynek).
- Wprowadzono flagi świata gry (jednorazowe interakcje).
- Zaimplementowano serwisy: StoryService, LocationService, LocationActionService.
- Zintegrowano system fabularny, lokacje i akcje w jeden spójny flow gry.
- Dodano centralny GameController zwracający pełny stan gry.
- Backend decyduje o aktualnym trybie gry (Story / World).

## v0.0.4
- Połączono logowanie i rejestrację w jeden widok autoryzacji (AuthView).
- Dodano animowane przełączanie trybu logowanie/rejestracja.
- Dodano pole „Powtórz hasło” wraz z walidacją frontend i backend.
- Ujednolicono komunikaty błędów i sukcesu w formularzu.
- Dodano bezpieczne uwierzytelnianie rejestracji po stronie backendu (ConfirmPassword).
- Uporządkowano repozytorium: dodano `.gitignore` dla SQLite i migracji EF Core.

## v0.0.5
- Zaimplementowano spójny core gameplay loop: lokacja → akcja → rezultat → aktualizacja stanu.
- Backend przejął pełną kontrolę nad walidacją dostępności i wykonywania akcji.
- Ujednolicono obsługę akcji świata i fabuły w jeden przewidywalny flow gry.
- Dodano centralny model wyniku akcji (ActionResult) zawierający komunikaty, nagrody i zmiany stanu.
- Uporządkowano serwisy gameplayowe i ich odpowiedzialności.
- Przygotowano stabilne kontrakty API pod dalszy rozwój frontendu.

## v0.0.6
- Dodano podstawowy interfejs użytkownika gry po stronie frontendu.
- Zaimplementowano główny widok gry (GameView) integrujący stan gry.
- Dodano widoki fabularne i świata gry (StoryView, WorldView).
- Zaimplementowano widok akcji gracza (ActionView).
- Dodano modalne okna:
  - ekwipunku gracza,
  - dziennika wydarzeń,
  - profilu gracza.
- Dodano górny i dolny pasek nawigacji (TopBar, BottomBar).
- Wprowadzono system ikon UI dla przedmiotów i elementów interfejsu.
- Frontend umożliwia pełną interakcję z backendowym gameplay loopem.
