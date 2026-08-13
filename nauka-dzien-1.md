# Nauka C# — MagazynApi, dzień 1 (12.08.2026) — decyzja o kierunku

## Co się stało

Konsolowy Magazyn skończony (opcje 1–5 w pełni na SQLite, jedyne źródło prawdy). Podjęta decyzja o następnym kroku: **ASP.NET Core (backend)**.

Utworzony projekt: `D:\Dane\Projekty\NaukaCSharp\MagazynApi`
- `dotnet new webapi --use-controllers -n MagazynApi`
- Szablon z kontrolerami (blisko Springa, który jest celem usera)

## Dlaczego ASP.NET Core (a nie od razu React+TS)

- **ASP.NET Core dla C# = Spring Boot dla Javy** — kontrolery, endpointy, JSON — ten sam pomysł
- **.NET to nie nowość** — Magazyn to już `net10.0`; dokładamy tylko narzędzia webowe
- **React+TS teraz = za duży skok** — drugi język + ekosystem (npm, Vite, hooks), a C# jeszcze nie "wsiadł"
- **CRUD** — user już to umie! Skrót: Create/Read/Update/Delete = opcje 1–4 (INSERT/SELECT/UPDATE/DELETE). To tylko nazwa dla znanych rzeczy.

## Plan (kolejność!)

1. **Backend (silnik)** — ASP.NET Core API: każda opcja konsoli → endpoint:
   | Konsola | API |
   |---|---|
   | Opcja 1 (dodaj) | `POST /api/przedmioty` |
   | Opcja 2 (pokaż) | `GET /api/przedmioty` |
   | Opcja 3 (edytuj) | `PUT /api/przedmioty/{id}` |
   | Opcja 4 (usuń) | `DELETE /api/przedmioty/{id}` |
   | Opcja 5 (wartość) | `GET /api/przedmioty/wartosc` |
   - Testowanie: Postman / plik `.http` zamiast konsoli
   - SQL zostaje (INSERT/SELECT/UPDATE/DELETE + parametry @)
2. **Frontend** — dopiero PO API. Na start **Blazor (w C#)** — zero nowego języka, Magazyn na stronie kawałek po kawałku. TS/React = opcja na później (składnia TS podobna do C# — wejdzie łatwiej).
3. **Łączenie** — frontend ↔ backend przez HTTP (GET/POST/PUT/DELETE). Osobna robota (CORS, formaty), ale wtedy obie strony będą znane.

## Struktura projektu (mapa)

| Plik | Co to jest | Odpowiednik z Magazynu |
|---|---|---|
| `Program.cs` | Start aplikacji | Start Magazynu (nowa piosenka) |
| `Controllers/` | Folder na kontrolery | Kontrolery w Spring |
| `WeatherForecastController.cs` | Przykład z szablonu | **Martwy kod — do usunięcia** |
| `WeatherForecast.cs` | Przykładowy model | `Item.cs` |
| `appsettings.json` | Konfiguracja | `application.properties` w Spring |
| `MagazynApi.csproj` | Plik projektu | `Magazyn.csproj` |

## Następna sesja

- Usunąć `WeatherForecastController.cs` + `WeatherForecast.cs` (martwy kod)
- Pierwszy prawdziwy endpoint: `GET /api/przedmioty` — lista przedmiotów z bazy (SELECT + SQLite — to samo co opcja 2!)
- Nowe koncepty do nauki: atrybuty (`[ApiController]`, `[HttpGet]`), JSON, async/await, Postman

---

# Dzień 2 (13.08.2026) — pierwszy endpoint DZIAŁA ✅

## Co zrobiliśmy

1. **Sprzątanie** — usunięte `WeatherForecastController.cs` + `WeatherForecast.cs` (martwy kod z szablonu, jak `Funkcje.cs` w konsoli)
2. **Paczka** — dodana `Microsoft.Data.Sqlite` 10.0.11 (ta sama, którą używa konsolowy Magazyn)
3. **Model** — `Przedmiot.cs`: property `Id`, `Nazwa`, `Ilosc`, `Cena` (1:1 z kolumnami tabeli)
4. **Kontroler** — `Controllers/PrzedmiotyController.cs` z endpointem `GET /api/przedmioty`
5. **TEST NA ŻYWO** — przeglądarka pokazała JSON z 7 przedmiotami z bazy! Dowody, że to ta sama baza: brak `id 7` (usunięty w konsoli) i `" Piła łańcuchowa"` ze spacją.

## Nowe koncepty

- **Atrybuty** `[ ]` = znaczniki dla frameworka: `[ApiController]` (to kontroler API), `[Route("api/[controller]")]` (adres; `[controller]` = nazwa klasy bez "Controller" → `/api/przedmioty`), `[HttpGet]` (metoda odpowiada na GET = prośbę o dane, jak opcja 2)
- **Property** `{ get; set; }` vs pole — bramki odczytu/zapisu; JSON czyta przez `get`; konstruktor zbędny, bo `set` sam wstawia wartości
- **Inicjalizator obiektu** — `new Przedmiot { Id = ..., Nazwa = ... }` zamiast konstruktora
- **`@` przed stringiem** — tekst dosłowny, bez podwajania `\\`
- **JSON** — zwykły tekst: `{ }` = jeden obiekt, `[ ]` = lista, pola po przecinku; framework zamienia pierwszą literę na małą (camelCase: `id` zamiast `Id`)
- **Katalog roboczy** — VS uruchamia program z `bin\Debug\net10.0` (NIE z folderu projektu!) → ścieżki względne typu `magazyn.db` trafiają w pustkę. Stąd "Magazyn jest pusty" po F5 (utworzyła się pusta baza w bin!) i błąd 500 przy starcie z bin. Dane w `Magazyn\magazyn.db` były cały czas bezpieczne.
- **Endpoint** = adres, na którym program odpowiada. GET = chcę dane. (POST = chcę dodać — jutro!)

## Ważne decyzje / pułapki

- **Connection string TYMCZASOWO absolutny**: `@"Data Source=D:\Dane\Projekty\NaukaCSharp\Magazyn\magazyn.db"` — do **przeniesienia do `appsettings.json`** (plik konfiguracji, jak `application.properties` w Springu)
- **F5 w VS** uruchamia otwarty projekt — w VS trzeba otworzyć `MagazynApi.csproj` (Plik → Otwórz → Projekt/Rozwiązanie), żeby F5 odpalił API, a nie konsolowy Magazyn
- Plik testowy `MagazynApi.http` = "frontend do testów" (klikasz Send Request) — przeglądarka też działa jako klient

## Następna sesja (Dzień 3)

1. `appsettings.json` — przeniesienie connection string do konfiguracji (i czytanie go z `IConfiguration`)
2. `POST /api/przedmioty` — dodawanie przedmiotu (odpowiednik opcji 1 konsoli; dane przyjdą w JSON-ie zamiast z klawiatury)
3. Potem: `PUT /api/przedmioty/{id}` (opcja 3), `DELETE /api/przedmioty/{id}` (opcja 4), `GET /api/przedmioty/wartosc` (opcja 5)
4. Na koniec CRUD w całości = to samo co konsola, tylko przez HTTP
