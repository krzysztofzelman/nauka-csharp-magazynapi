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

---

# Dzień 3 (14.08.2026) — CRUD KOMPLETNY ✅ + start frontendu (Blazor)

## Co zrobiliśmy

### Część 1: API — dokończenie CRUD (wszystko testowane curl-em na prawdziwej bazie)

| Endpoint | Odpowiednik konsoli | Test |
|---|---|---|
| `GET /api/przedmioty` (był z dnia 2) | opcja 2 | 200 → JSON z bazy ✅ |
| `POST /api/przedmioty` | opcja 1 | 200 → Sekator id 9 w bazie ✅ |
| `PUT /api/przedmioty/{id}` | opcja 3 | 200 → Sekator ilość 3→5 ✅ |
| `DELETE /api/przedmioty/{id}` | opcja 4 | 200 → Sekator usunięty ✅ |

**Dowód spójności:** Sekator dodany przez API → widoczny w konsoli; Łopata dodana w konsoli → widoczna na stronie. Jedno źródło prawdy.

### Część 2: Frontend — projekt MagazynWeb (Blazor)

- `dotnet new blazor -n MagazynWeb` — pierwsza STRONA zamiast konsoli
- `Pages/` = odpowiednik `Controllers/` w API; plik `.razor` = strona + adres (`@page "/"`)
- Strona główna pokazuje **tabelę przedmiotów z prawdziwego API** — klikasz F5 i widzisz magazyn
- Dodanie przedmiotu w konsoli → F5 na stronie → przedmiot jest! **Dwa ekrany, jedna baza**

## Nowe koncepty (dzień 3)

### API:
- **`[HttpPost]`** — "odpowiadam na prośbę: dodaj do bazy". Parametr `Przedmiot nowy` = **model binding**: framework sam rozpakowuje JSON z ciała żądania do pudełka `nowy`
- **`[HttpPut("{id}")]`** — "zmień przedmiot o numerze z adresu": `{id}` w trasie → framework wkłada liczbę z URL do parametru `int id`. Dwa wejścia: `id` (adres) + `nowy` (ciało)
- **`[HttpDelete("{id}")]`** — "usuń przedmiot o numerze". Tylko `id` — bez ciała, bo do usunięcia wystarczy wiedzieć CO
- **`WHERE Id = @id`** — kluczowe w UPDATE/DELETE! Bez niego SQL zmieniłby/skasowałby WSZYSTKIE wiersze
- **`ExecuteNonQuery()`** — "wykonaj bez wyników" dla INSERT/UPDATE/DELETE (SELECT używa `ExecuteReader`)
- **Zamek na pliku:** build nie może nadpisać pliku działającego serwera (MSB3026/27). Przed buildem: zatrzymaj serwer (Ctrl+C / taskkill)

### Blazor:
- **`.razor` = HTML + C# w jednym pliku** (HTML — user uczył się kiedyś, pamięta tagi!)
- **`@`** = przełącznik: "tu się kończy HTML, zaczyna C#"
- **`@code { }`** — blok C# na dole strony (zmienne, metody)
- **`@inject HttpClient Http`** — "daj mi telefon z rejestru"; rejestr = `Program.cs`, gdzie dodajemy `builder.Services.AddHttpClient();`
- **`await Http.GetFromJsonAsync<List<Przedmiot>>("...")`** — "zadzwoń do API i od razu rozpakuj JSON do listy pudełek"; `await` = "czekaj, aż wróci"
- **`@foreach (var przedmiot in przedmioty)`** — "dla każdego przedmiotu z listy → zrób wiersz tabeli" (pętla jak `while` z konsoli, tylko sama wie, kiedy skończyć)
- **`@if (x == null)`** — "jak danych nie ma, pokaż Ładowanie..."

## Błędy dnia (każdy = lekcja)

- `Void` → `void` (C# rozróżnia wielkość liter — słowa kluczowe małą)
- `Przededmiot` → `Przedmiot` (literówka)
- `[HttPut` → `[HttpPut` (brak "p")
- `buldier` → `builder` + **przecięcie łańcucha** — kropki łączą wywołania w "pociąg": nie wstawiaj nowej linii w środek pociągu, tylko po jego końcu
- **PowerShell zjada cudzysłowy** przy `curl -d` z JSON-em → 400 "invalid start of property name". Rozwiązanie: `-d @plik.json` (JSON w pliku)

## Pułapki dnia (ważne!)

- **Korzeń `localhost:5000/` = 404** — to normalne! API mieszka pod `/api/...` ("centrala vs numer wewnętrzny"). ZAWSZE pełny adres.
- **"To tylko JSON"** — backend = składniki na zapleczu, frontend = danie na talerzu. Tak działa Allegro. Frontend dopiero budujemy!
- **Build z VS (F5) też blokowany** przez działający serwer — Shift+F5 (zatrzymaj) przed F5
- **Serwer na 2 portach możliwych:** 5000 (ręczny start) / 5109 (profil VS). Sprawdzić curl-em przed testem.

## Następna sesja (Dzień 4)

1. **Formularz dodawania na stronie** — pola Nazwa/Ilość/Cena + przycisk "Dodaj" → POST do API (prawdziwa aplikacja!)
2. Przyciski Edytuj/Usuń na stronie (PUT/DELETE z frontendu)
3. `appsettings.json` — connection string poza kodem (obecnie duplikat w 4 metodach kontrolera!)
4. Opcjonalnie: `GET /api/przedmioty/wartosc` (opcja 5 z konsoli)
5. Przenieść notatki do pliku (ta notatka!) — już zrobione 😄
