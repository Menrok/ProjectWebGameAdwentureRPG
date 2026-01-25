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