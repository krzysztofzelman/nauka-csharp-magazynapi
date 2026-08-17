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

---

# Dzień 4 (15.08.2026) — Blazor od podstaw (czysta teoria)

## Co się stało

Dzień zaczęliśmy od powtórki `foreach` (nadal nie siedzi — to normalne, wracamy do niego co sesję, małymi krokami). Ale potem padły ważne słowa:

> "po pierwsze to ja jakieś podstawy powinienem poznać z blazor a nie lecisz gdzieś w środku"

Słuszna uwaga! Zamiast wskakiwać w środek (foreach w tabeli), cofnęliśmy się do **samych podstaw Blazora**. Dzień skończył się teorią bez kodu — to był fundament, na którym jutro napiszemy formularz.

## Nowe koncepty (dzień 4)

### 1. Blazor = strony w C#
- Plik `.razor` = **jedna strona**: góra = wygląd (HTML), dół = logika (C# w `@code { }`)
- `@` = znak graniczny: "tu wchodzi C# do HTML-a"
- **C# liczy, HTML pokazuje**

### 2. Adres (URL) — fundament
- Adres = **do kogo** (`localhost:5008`) + **o co** (`/`)
- `http://` = protokół (język stron), `localhost` = ten komputer, `:port` = numer drzwi programu
- Port to **konfiguracja, nie default C#** — mieszka w `Properties/launchSettings.json`:
  - MagazynWeb = `5008`
  - MagazynApi z VS = `5109`, startowany ręcznie = `5000`
- `https` = drzwi z kłódką (szyfrowane); `;` w launchSettings = "i" (program słucha na obu naraz)

### 3. `@page "/"` — wizytówka strony
- Każda strona `.razor` ma swój adres: `@page "/"` = "kto poprosi o `/` (strona główna), dostaje TĘ stronę"
- Plik zna tylko SWOJĄ część adresu (numer wewnętrzny) — reszta (centrala = `localhost:port`) siedzi w `launchSettings.json`

### 4. `@code { }` — słowo po słowie
```csharp
private List<Przedmiot>? przedmioty;
// private = "tylko dla strony"
// List<Przedmiot> = lista przedmiotów (znane z konsoli: List<Item>)
// ? = "albo lista, albo nic" (na start półka pusta)

protected override async Task OnInitializedAsync()
// "gdy strona się otworzy" (OnInitialized = zainicjowana)
// async = "ta metoda może czekać", Task = obietnica, że skończy

{
    przedmioty = await Http.GetFromJsonAsync<List<Przedmiot>>("http://localhost:5000/api/przedmioty");
    // await = "czekaj, aż..." (jak na paczkę)
    // Http = telefon do API (dostajesz przez @inject na górze pliku)
    // GetFromJsonAsync = "zapytaj API i zamień JSON na listę przedmiotów"
}
```
Całość: **strona się otwiera → dzwoni do API → wkłada dane na półkę → HTML rysuje tabelę**

### 5. `var` — skrót od "variable" (zmienna)
- `var` = "zmienna, typ zgadnij za mnie" — C# sam ustala typ, patrząc na to, co jest po prawej
- W kodzie: `@foreach (var przedmiot in przedmioty)` — półka `przedmioty` trzyma `Przedmiot`, więc `przedmiot` JEST typu `Przedmiot`
- `var przedmiot` = to samo, co napisanie `Przedmiot przedmiot` (tylko mniej pisania)
- `var` to NIE "typ dowolny" — zmienna zawsze ma konkretny typ, tylko ty go nie wypisujesz

## Pułapki dnia

- `localhost:5000` to **NIE** "default dla C#/.NET" — to adres, który program dostaje przy starcie (z konfiguracji)
- Porównanie `@code` do klasy `Enemy` **nie trafiło** (za bardzo odjeżdżało od sedna) — zamiast tego: "kawałek C# przyklejony do strony"
- Notatnik jako miejsce ćwiczeń — odrzucone ("nie będę klepał w notatniku") — ćwiczymy prosto w kodzie

## Następna sesja (Dzień 5) — PISZEMY KOD! 💪

1. **Formularz dodawania na stronie** — pola Nazwa/Ilość/Cena + przycisk "Dodaj" → POST do API (prawdziwa aplikacja!)
2. Przyciski Edytuj/Usuń (PUT/DELETE z frontendu)
3. `appsettings.json` — connection string poza kodem
4. Wracamy do `foreach` — małymi krokami, na własnym kodzie

---

# Dzień 5 (16.08.2026) — FORMULARZ DODAWANIA DZIAŁA ✅ (prawdziwa aplikacja!)

## Co zrobiliśmy

1. **Powtórka dnia 4 (2/2 z pamięci!)** 🎉 — porty (5008/7052 w `launchSettings.json`) i @ (granica C#) — wczorajsze słabe punkty dziś poszły gładko
2. **Formularz dodawania na stronie** — 3 pola + niebieski przycisk "Dodaj" → nowy przedmiot w bazie **kliknięciem w przeglądarce**
3. **TEST NA ŻYWO ✅** — wpisane "Test z formularza" (2×99) → curl pokazał `id 12` w prawdziwej bazie. Pełne koło zamknięte:

```
przeglądarka (wpisujesz + klik)
  → @bind łapie wartości do zmiennych
  → @onclick woła metodę Dodaj()
  → Http.PostAsJsonAsync wysyła pudełko do API
  → API robi INSERT do SQLite (opcja 1 konsoli!)
  → odświeżenie listy → nowy wiersz w tabeli
```

**Dwa ekrany, jedna baza — teraz z obu stron:** dodawanie działało już z konsoli i z curl, dziś po raz pierwszy kliknięciem. To już "prawdziwa aplikacja", nie wypluty JSON.

## Nowe koncepty (dzień 5)

### 1. `@rendermode InteractiveServer` — przełącznik interaktywności
- Bez niego strona jest **statyczna** (jak gazeta — przeczytasz i koniec): pokazuje dane, ale nie reaguje na kliknięcia
- Z nim strona dostaje linię telefoniczną do serwera i **reaguje na kliknięcia** (a formularz to przecież kliknięcia)
- 1 linijka pod `@page "/"`

### 2. `@bind` — klej pole ↔ zmienna (w DWIE strony)
- `@bind="nazwa"` = wpisujesz "Młotek" w pole → zmienna `nazwa` staje się "Młotek"
- (i odwrotnie: jak zmienna się zmieni, pole pokaże nową wartość)
- Jak `Console.ReadLine()` z konsoli, tylko na żywo przy każdym stuknięciu klawisza
- `placeholder` = szara podpowiedź w polu (kosmetyka, znika przy pisaniu)

### 3. `@onclick` — zdarzenie
- `@onclick="Dodaj"` = "jak ktoś kliknie przycisk, wywołaj metodę `Dodaj`"
- Jak Enter w konsoli, tylko kliknięcie myszką
- `btn btn-primary` = klasy Bootstrapa (styl): `btn` = kształt przycisku, `btn-primary` = niebieski kolor

### 4. `Http.PostAsJsonAsync(adres, pudełko)` — wysyłka POST
- `new Przedmiot { Nazwa = nazwa, Ilosc = ilosc, Cena = cena }` = składanie pudełka z pól (obiekt = egzemplarz!)
- `PostAsJsonAsync` = "wyślij pudełko do API" — ta sama operacja co opcja 1 konsoli, tylko pudełko jedzie z przeglądarki
- Potem odświeżenie: `GetFromJsonAsync` jeszcze raz, żeby nowy wiersz od razu był w tabeli

## Błędy dnia (każdy = lekcja)

- **Zmienne wpisane POZA `@code`** (między HTML-em) — cały C# mieszka w kuchni `@code { }`, nie na wystawie. Bez tego strona się nie skompiluje
- **`<button>` wpadł do `@code`** — HTML nie może mieszkać w kuchni C# (odwrotna strona tego samego błędu)
- **Jedna zmienna, dwie nazwy** — `var nowyPrzedmiot = ...` a potem wysyłka `nowy` → zmienna `nowy` nie istnieje → błąd. Nazwa zmiennej musi być spójna w całej metodzie (dobre: user sam nadał bardziej opisową nazwę `nowyPrzedmiot`!)
- **Brakująca klamra `}`** na końcu `@code` — każdy blok otwarty `{` musi się domknąć `}`

**Wniosek ogólny:** "pogmatwane" to zwykle nie błąd logiki, tylko 1–2 linijki w złym miejscu. Blazor jest wybredny co do miejsca: HTML na wystawie, C# w kuchni.

## Rozróżnienie: Razor vs Blazor (pytanie usera)

- **Razor = składnia** (sposób pisania HTML+C# w jednym pliku; rozumie `@`; plik `.razor`)
- **Blazor = cały framework** (strony, komponenty, kliknięcia, Http...); Blazor UŻYWA Razora do rysowania
- Analogia: **Razor = silnik, Blazor = cały samochód**

## Przycisk Usuń — ZROBIONE ✅ (ta sama sesja)

User wpisał kod **sam, z notatek** (własny wariant: `@onclick="@(() => Usun(przedmiot.Id))"` — dodatkowe `@` przed nawiasem też działa, jest zbędne). Build OK (0 błędów). **TEST NA ŻYWO ✅:** klik w Usuń przy "Test z formularza" (id 12) → wiersz zniknął z tabeli, curl potwierdził brak id 12 w bazie. Koło: klik → `() =>` przekazuje id → `Usun(id)` → `Http.DeleteAsync(".../{id}")` → API robi `DELETE WHERE Id` (opcja 4) → odświeżenie listy.

- `<th>Akcje</th>` — nowa kolumna
- `<button class="btn btn-danger" @onclick="() => Usun(przedmiot.Id)">Usuń</button>` — w każdym wierszu
  - **`() =>` (strzałka/lambda)** = "gdy klikniesz, WTEDY wywołaj metodę z argumentem" — bez niej Blazor wywołałby od razu przy rysowaniu strony
- `private async Task Usun(int id)` — parametr z adresu; `Http.DeleteAsync($".../{id}")` (interpolacja znana z konsoli)

---

# Dzień 6 (16.08.2026, wieczór) — PRZYCISK EDYTUJ DZIAŁA ✅ (CRUD KOMPLETNY z przeglądarki!)

## Co zrobiliśmy

1. **Przycisk Edytuj w wierszu** — klik → formularz **wypełnia się danymi wiersza** (w tym cała magia `@bind` w DWIE strony!)
2. **Zielony "Zapisz"** — tryb edycji: zamiast "Dodaj" przycisk robi się zielony, a metoda wysyła **PUT** zamiast POST
3. **TEST NA ŻYWO ✅** — Kosiarka: Edytuj → Ilość 5 → 6 → Zapisz → tabela odświeżona, nowa wartość w bazie

**CRUD z przeglądarki zamknięty:** Dodaj ✅ (dzień 5), Usuń ✅ (dzień 5), **Edytuj ✅ (dziś)**. Cały magazyn obsługiwany kliknięciami — prawdziwa aplikacja.

## Nowe koncepty (dzień 6)

### 1. `edytowanyId` — zmienna-pamięć
- `private int edytowanyId = 0;` — zapamiętuje, **który wiersz właśnie edytujemy**
- `0` = żaden (tryb dodawania); baza numeruje id od 1, więc 0 to bezpieczny znak "brak edycji"
- Po zapisaniu zmian wraca do 0 (przycisk znów "Dodaj")

### 2. `@bind` działa w DWIE strony
- Do tej pory znaliśmy: wpisujesz w pole → zmienna
- Dziś odwrotnie: `nazwa = przedmiot.Nazwa;` → **pole samo pokazuje nową wartość**
- To dlatego klik Edytuj "wypełnia" formularz — nie ma żadnego specjalnego kodu do tego, tylko przypisanie do zmiennej!

### 3. Wzorzec "szukanie po liście": `foreach` + `if`
```csharp
foreach (var przedmiot in przedmioty)   // dla każdego wiersza
{
    if (przedmiot.Id == id)             // jeśli to ten kliknięty
    {
        nazwa = przedmiot.Nazwa;        // wypełnij formularz
        ilosc = przedmiot.Ilosc;
        cena = przedmiot.Cena;
        edytowanyId = id;               // zapamiętaj, który edytujemy
    }
}
```
Zarazem **powtórka foreach** (słaby punkt) — tu foreach robi coś nowego: SZUKA, nie tylko wyświetla.

### 4. `PutAsJsonAsync` — bliźniak `PostAsJsonAsync`
- Różnica tylko w adresie: POST idzie na listę (`/api/przedmioty`), PUT na konkretny wiersz (`/api/przedmioty/{edytowanyId}` — interpolacja znana z konsoli)
- w API to ta sama opcja 3 konsoli: `UPDATE ... WHERE Id = @id`

### 5. `@if` na wystawie — przycisk zmienia się w zależności od trybu
```razor
@if (edytowanyId == 0)
{
    <button class="btn btn-primary" @onclick="Dodaj">Dodaj</button>
}
else
{
    <button class="btn btn-success" @onclick="Dodaj">Zapisz</button>
}
```
- `@if` znamy już z tabeli (ładowanie) — tu robi coś nowego: wybiera WARIANT przycisku
- zawsze woła tę samą metodę `Dodaj` — to ona decyduje wewnątrz

### 6. Gdzie stoją przyciski = skąd biorą dane
- **Dodaj** stoi przy polach, bo czyta **pola** (3 zmienne przez `@bind`) — tworzy nowy przedmiot, tabeli nie potrzebuje
- **Edytuj/Usuń** stoją w wierszach, bo potrzebują `przedmiot.Id` — a zmienna `przedmiot` istnieje TYLKO wewnątrz `@foreach`. Poza pętlą nie da się powiedzieć, KTÓRY wiersz
- Analogia zakupów: "dodaj do listy" u góry kartki, "skreśl tę pozycję" przy każdej pozycji

### 7. Metoda Dodaj — jedna metoda, dwie ścieżki (if/else)
```csharp
private async Task Dodaj()
{
    if (edytowanyId == 0)      // pytanie: czy edytujemy?
    {
        // NIE → POST (dodaj nowy — jak wczoraj)
        var nowyPrzedmiot = new Przedmiot { Nazwa = nazwa, Ilosc = ilosc, Cena = cena };
        await Http.PostAsJsonAsync("http://localhost:5000/api/przedmioty", nowyPrzedmiot);
    }
    else                       // TAK → PUT (zapisz zmiany)
    {
        var edytowany = new Przedmiot { Nazwa = nazwa, Ilosc = ilosc, Cena = cena };
        await Http.PutAsJsonAsync($"http://localhost:5000/api/przedmioty/{edytowanyId}", edytowany);
        edytowanyId = 0;       // koniec edycji → tryb dodawania
    }
    przedmioty = await Http.GetFromJsonAsync<List<Przedmiot>>("http://localhost:5000/api/przedmioty"); // odświeżenie
}
```
- Logika = `if/else`, mocny punkt usera
- Oba kawałki składają to samo pudełko — różni się tylko adres wysyłki

## Błędy dnia (lekcje)

- **User napisał własny wariant metody (dobrze!)** — logika poprawna (odwrócił `if` na `!= 0` → PUT — w pełni równoważne), ale posypały się literówki: brak `@` przed `if` na wystawie, `}` zamiast `{`, **dwie metody** (`Dodaj` i `dodaj` małą literą — w C# wielkość liter = inne nazwy!), brak `{` po sygnaturze, `varedytowany` (brak spacji), `nazwa = nazwa. Ilosc` (kropka zamiast `;`), adres PUT **bez `{edytowanyId}`** (PUT musi celować w wiersz!), brak `;` na końcu
- **User przytłoczony** ("za dużo napierdolone, nie ogarniam") → delegował fix asystentowi — poprawione 3 edytami: przycisk, metoda, usunięcie duplikatu metody
- `viud` → `void` (literówka, ta sama lekcja co `Void` w Dzien 3)
- **Ostrzeżenie CS8602** na `foreach (... in przedmioty)` — kompilator marudzi, że lista "może być null". Nieszkodliwe: przycisk istnieje tylko gdy tabela widoczna = lista na pewno załadowana. Zostawione.

## Proces dnia

- Oba serwery odpalane w tle z poziomu asystenta: API (`dotnet MagazynApi.dll` z bin → port 5000), strona (`dotnet run --launch-profile http` → port 5008)
- Przed buildem ZATRZYMAĆ serwer strony (blokada DLL) — asystent robi task_stop, build, start od nowa
- Testy na żywo w przeglądarce na **http://localhost:5008**

---

# Dzień 7 (17.08.2026) — tłumaczenie Home.razor linia po linii + 2 NOWE FUNKCJE

## NOWE FUNKCJE (dopisane pod koniec dnia — bounce-back po przytłoczeniu!)

User po tłumaczeniu powiedział "chcę dopisać coś do kodu" → wybrał **"Wartość magazynu na dole"**, potem sam zaproponował drugą: **"Liczba sztuk"**.

**WZORZEC "4 KLOCKI" (nauka: nowy wariant foreach = SUMOWANIE):**

| Klocek | Kod | Rola |
|---|---|---|
| Pudełko | `private decimal wartoscMagazynu;` / `private int iloscSztuk;` | szuflada na wynik |
| Licznik | `ObliczWartosc()` / `ObliczIlosc()` — foreach + `= + ` | pracownik dokłada do szuflady |
| Włącznik | `ObliczWartosc();` + `ObliczIlosc();` po KAŻDYM odświeżeniu (3×) | "policz!" |
| Witryna | `<p>Wartość magazynu: @wartoscMagazynu zł</p>` | pokazuje szufladę |

- `ObliczIlosc()` = bliźniak `ObliczWartosc()` — tylko `+ przedmiot.Ilosc` zamiast `* przedmiot.Cena`
- **Akumulator:** `wartoscMagazynu = wartoscMagazynu + ...` — pudełko rośnie; **`= 0` na starcie** (bez tego sumowanie dokładałoby do starej sumy!)
- Włącznik stoi w 3 miejscach = tam, gdzie jest `przedmioty = await GetFromJsonAsync(...)` (start strony, Dodaj, Usun) — "pobierz świeżą listę → policz"
- Włącznik = "() = spust 🔫": metoda bez `()` tylko istnieje, z `()` wykonuje się
- `var` = "C# sam zgadnij typ" (po prawej stronie); przy pustych pudełkach typ trzeba podać jawnie
- `private` ≠ szyfrowanie! = "tylko dla nas w tym pliku"

**Błędy usera (typowe):** `on` zamiast `in` (foreach), spacja w nazwie (`ilosc sztuk`), mała litera właściwości (`przedmiot.ilosc` vs `Ilosc`), wielkość liter (`iloscsztuk` vs `iloscSztuk` — C# rozróżnia!), **2× wklejony CAŁY blok zamiast jednej linijki** (sierocy `{ }` — build CS1520) → delegacja fixu asystentowi (wzorzec z Dnia 6)

**TEST:** dodawanie/edycja/usuwanie → obie sumy przeliczają się na żywo ✅ (dowód: 2 Łopaty w bazie → po Usuń jednej sumy spadły)

## Co zrobiliśmy

Cały `MagazynWeb\Components\Pages\Home.razor` wytłumaczony linia po linii. **Mapa pliku:** 2 części — HTML (co widać) + `@code` (logika C#). Łączą je `@bind` i `@onclick`.

**Cały plik = 4 narzędzia w kółko:**

| Narzędzie | Co robi |
|---|---|
| `@bind` | most pole ↔ zmienna (w obie strony) |
| `@if` | pokaż to ALBO tamto (Ładowanie/tabela, Dodaj/Zapisz) |
| `@foreach` | powtórz wiersz dla każdego przedmiotu |
| `@onclick` | klik → metoda C# |

**Metody:** `OnInitializedAsync` = start strony (pobierz listę), `Dodaj()` = POST albo PUT + odświeżenie, `Usun()` = DELETE + odświeżenie, `Edytuj()` = szukanie po liście (foreach+if).

## Trudny moment 1: sentynek `edytowanyId == 0` (3 podejścia!)

1. **"Po co to pisać? 0 = nic nie robi"** — user czytał `if` jak polecenie dla SIEBIE. Odblokowane: `if` to pytanie, które **program zadaje sobie**; program nie ma oczu — czyta zmienne.
2. **"To kłamstwo, bo każdy wiersz ma ID"** — mylił ID wierszy ze zmienną. Odblokowane: 2 światy — ID wierszy (1, 5, 8...) vs zmienna startująca pusto. **0 = symbol "nic", nie numer wiersza** — a obserwacja usera ("zero nie pasuje") jest DOWODEM, że 0 jest bezpieczne jako "pusto".
3. **"Skoro nic nie robię, to wiadomo, że 0"** — zgadza się! I właśnie to `if` sprawdza. Program nie zgaduje — czyta zmienną.

**Werdykt:** "nooo powiedzmy... 0, bo to tak jest" — konwencji sam by nie wydedukował. **Lekcja: konwencje (0 = pusto) podawać WPROST jako umowę ("czerwone światło = stój"), nie liczyć na dedukcję.** Analogia, która zadziałała: karteczka w kieszeni (pusta = dodaj, z numerem 5 = popraw przedmiot 5).

## Trudny moment 2: `if (przedmiot.Id == id)`

"To jakby kolumna i wiersz to to samo" → dwie RÓŻNE liczby o podobnych nazwach:
- `id` (małe) = numer z **kliknięcia** (lambda podchwyciła z wiersza) — "czego szukam"
- `przedmiot.Id` (duże) = **etykieta w każdym pudełku** z listy — "co ma każde pudełko"
- pętla = szukanie pudełka po etykiecie (karteczka "5" → pudełko z etykietą "5")

## Werdykt dnia

- "frontend pojebany, backend łatwiejszy" → backend = jedna droga (prośba → SQL → odpowiedź), frontend = wszystko naraz (pola + tabela + zdarzenia). Nie trudniejszy — **gęstszy**. Cel usera = backend; frontend ma działać, nie być wyrecytowanym.
- Koniec dnia: "strasznie to mętne" → zwolnienie z rozumienia detali (lambda, `List<Przedmiot>?`, `await`) — wrócą przy powtórkach.
- **Dowód postępu:** `foreach` (tygodniowy słaby punkt) siedzi w działającym kodzie usera — "najpierw używasz, potem rozumiesz".

## Następna sesja (Dzień 8)

1. **NOWA FUNKCJA (user sam zaproponował): "Łopata do Łopaty"** — dodawanie przedmiotu, który JUŻ istnieje, ma zwiększać ilość zamiast tworzyć duplikat. Plan: w `Dodaj()` (gałąź POST) — foreach+if po liście (wzorzec "szukanie" z Edytuj): jak `przedmiot.Nazwa == nazwa` → **PUT** z ilością `przedmiot.Ilosc + ilosc` (zamiast POST). Nowy koncept: **flaga** (`bool znaleziony` — "czy coś znalazłem?"). Uwaga: user był zmęczony — robić na spokojnie, małe kroki.
2. **Powtórka z pamięci** (recall drill) — koncepty z Dni 5–7 (wzorzec 4 klocki, sentynek, var)
3. `appsettings.json` — connection string poza kodem (duplikat w GET/POST/PUT/DELETE kontrolera)
4. Powtórka `foreach` — wzorzec "szukanie po liście" z Edytuj
5. Serwery do testów: API 5000, strona 5008 (odpalać w tle)
