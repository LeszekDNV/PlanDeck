## **Struktura rozwiązań (Solution)**

Nazwa pliku rozwiązania:  
```
PlanDeck.sln
```

**Projekty** w obrębie solucji:

1. **PlanDeck.Domain**  
2. **PlanDeck.Application**  
3. **PlanDeck.Infrastructure**  
4. **PlanDeck.App** (ASP.NET Core – serwerowe API)  
5. **PlanDeck.Client** (Blazor WebAssembly – frontend)

Struktura projektów.

```
PlanDeck.sln
│
├── PlanDeck.Domain
├── PlanDeck.Application
├── PlanDeck.Infrastructure
├── PlanDeck.App
└── PlanDeck.Client
```

---

## **1. PlanDeck.Domain**

Zawiera wyłącznie **rdzeń** logiki biznesowej i modeli domenowych.  

**Proponowana struktura katalogów**:

```
PlanDeck.Domain
│
├── Entities
│   ├── Room.cs
│   ├── Issue.cs
│   ├── Vote.cs
│   └── ...
├── ValueObjects
│   └── (np. VotingSystem.cs, CardValue.cs, itp.)
├── Services (opcjonalnie)
│   └── (np. klasa RoomService z metodami typowo domenowymi)
├── Exceptions
│   └── (np. DomainException.cs)
└── Interfaces
    └── (tylko jeśli są ściśle domenowe, np. IAggregateRoot – często zbędne w uproszczonej wersji)
```

- **Entities** – obiekty posiadające własną tożsamość (np. `Room`, `Issue`), zawierające reguły biznesowe.  
- **ValueObjects** – obiekty reprezentujące wartości (nie mają ID), np. `VotingSystem`.  
- **Services** – jeżeli masz logikę domenową, która dotyczy wielu encji, możesz tu umieścić proste klasy lub metody.  
- **Exceptions** – wyjątki charakterystyczne dla domeny.  

**Zasada**: Żadnych zależności na zewnątrz (brak odwołań do EF Core, brak serwisów zewnętrznych).  

---

## **2. PlanDeck.Application**

W warstwie **Application** umieszczamy **logikę aplikacyjną** i **przypadki użycia** (Use Cases). Tu definiujemy operacje, które chcemy umożliwić z poziomu API – takie jak tworzenie pokoju, zarządzanie głosowaniem, itp.  

Ponieważ **nie** używasz MediatR (czyli nie ma wzorca Command/Query z Handlerami), możesz zastosować **zwykłe serwisy** lub **klasy menedżerów** (tzw. Application Services).  

**Proponowana struktura**:

```
PlanDeck.Application
│
├── Interfaces
│   ├── IRoomService.cs
│   ├── IIssueService.cs
│   └── ...
├── Services
│   ├── RoomService.cs
│   ├── IssueService.cs
│   └── ...
├── Dtos
│   ├── RoomDto.cs
│   ├── IssueDto.cs
│   └── ...
├── Mappers
│   └── (np. RoomMapper.cs, IssueMapper.cs)
└── Exceptions
    └── (np. ApplicationException.cs, NotFoundException.cs itp.)
```

- **Interfaces** – interfejsy serwisów (np. `IRoomService`) definiujące metody związane z operacjami na pokojach (tworzenie, pobieranie, itp.).  
- **Services** – konkretne implementacje tych interfejsów (np. `RoomService`) – tutaj będzie wywołanie metod repozytorium (dostarczonych w warstwie **Infrastructure**), oraz logika związana z łączeniem danych z Domeny.  
- **Dtos** – obiekty do transferu danych między warstwą Application a warstwą wyższą (API).  
- **Mappers** – klasy mapujące encje domenowe <-> Dto, jeśli jest to bardziej rozbudowane (możesz też użyć np. AutoMapper).  
- **Exceptions** – wyjątki specyficzne dla logiki aplikacyjnej (np. brak uprawnień, brak zasobu itp.).  

**Zasada**: Warstwa **Application** korzysta z **Domain** (encje, value objects), ale sama nie jest zależna od **Infrastructure**. Zamiast tego, w Services odwołujesz się do interfejsów repozytoriów (zdefiniowanych w Application lub Domain), a implementacje tych repozytoriów znajdują się w **Infrastructure**.  

---

## **3. PlanDeck.Infrastructure**

Zawiera implementacje szczegółowe dostępu do zasobów i technologii: **baza danych (EF Core, MSSQL)**, **integracje z usługami zewnętrznymi** (np. Azure DevOps, CSV) oraz **repozytoria**.  

**Przykładowa struktura**:

```
PlanDeck.Infrastructure
│
├── Persistence
│   ├── PlanDeckDbContext.cs
│   ├── Configurations
│   │   ├── RoomConfiguration.cs
│   │   ├── IssueConfiguration.cs
│   │   └── ...
│   ├── Repositories
│   │   ├── RoomRepository.cs
│   │   ├── IssueRepository.cs
│   │   └── ...
│   ├── Migrations
│   └── (inne pliki związane z bazą)
├── Services
│   ├── AzureDevOpsImporter.cs
│   ├── CsvImporter.cs
│   └── ...
├── DependencyInjection
│   └── InfrastructureModule.cs
└── (opcjonalnie inne foldery np. "gRPC" jeśli używasz gRPC)
```

- **Persistence** – klasa kontekstu EF Core (`PlanDeckDbContext`) i konfiguracje encji.  
- **Repositories** – implementacje repozytoriów, które będą używane w warstwie **Application** (np. `IRoomRepository`, `IIssueRepository` jeśli takie interfejsy zdefiniujesz w Application/Domain).  
- **Services** – implementacje usług dostępu do zewnętrznych API (np. Azure DevOps, CSV) jeśli w warstwie **Application** mamy interfejsy `IAzureDevOpsService`, `ICsvImporterService`.  
- **DependencyInjection** – metody rozszerzające (np. `AddInfrastructureServices`) rejestrujące w kontenerze IoC wszystkie serwisy i repozytoria.  

**Zasada**: Warstwa **Infrastructure** odwołuje się do **Application** (żeby zaimplementować tamtejsze interfejsy) i do **Domain** (żeby mapować encje). Natomiast ani Domain, ani Application nie odwołują się wprost do Infrastructure.  

---

## **4. PlanDeck.App** – ASP.NET Core (Serwerowe API)

To projekt odpowiedzialny za:

- Hostowanie aplikacji (konfiguracja `Program.cs`, `Startup.cs`/`builder` w .NET 6/7/8/9).  
- Udostępnianie gRPC endpoints.  
- Rejestrowanie zależności z warstw **Application** i **Infrastructure**.  
- Obsługę całego ruchu HTTP (autoryzacja, routing, swagger, itd.).  

**Możliwa struktura**:

```
PlanDeck.App
│
├── Controllers
│   ├── RoomController.cs
│   ├── IssueController.cs
│   └── ...
├── Program.cs (lub Startup.cs w zależności od wersji .NET)
├── Properties
│   └── launchSettings.json
└── appsettings.json (konfiguracja)
```

### **Flow wywołań**  
1. **Klient** (PlanDeck.Client) wywołuje `GET /api/rooms` lub `POST /api/rooms`.  
2. **Controller** (np. `RoomController`) odwołuje się do serwisu z warstwy Application (np. `IRoomService`), przekazując dane.  
3. **IRoomService** (wdrożone w PlanDeck.Application) wykonuje akcję logiki aplikacyjnej, wywołuje repozytorium z **Infrastructure** (implementacja `IRoomRepository`).  
4. **Repository** korzysta z `PlanDeckDbContext` do zapisu/odczytu w bazie MSSQL.  
5. Wynik (np. `RoomDto`) trafia z powrotem do kontrolera, który zwraca go klientowi w formacie JSON.

---

## **5. PlanDeck.Client** – Blazor WebAssembly

To oddzielna aplikacja front-end hostowana w przeglądarce, która komunikuje się z `PlanDeck.App` poprzez gRPC.  

**Proponowana struktura**:

```
PlanDeck.Client
│
├── Pages
│   ├── Index.razor
│   ├── CreateRoom.razor
│   ├── RoomDetails.razor
│   ├── ...
├── Services
│   └── (np. RoomApiClient.cs - klasa odwołująca się do HTTP endpoints w PlanDeck.App)
├── Shared
│   └── MainLayout.razor
├── wwwroot
│   ├── css
│   ├── js
│   └── ...
├── Program.cs
└── App.razor
```

- **Pages** – poszczególne strony Blazor.  
- **Services** – klasy (API client) wywołujące endpointy HTTP (np. `HttpClient` + `RoomApiClient`).  
- **wwwroot** – pliki statyczne.  
- **App.razor / Program.cs** – konfiguracja samej aplikacji Blazor WebAssembly.  

### **Komunikacja Client ↔ Server**  
- Najprościej: standardowy **REST** (kontrolery w `PlanDeck.App`).  
- Możesz też użyć **gRPC-Web**, ale wymaga to nieco dodatkowej konfiguracji w ASP.NET i w Blazor.  

---

## **Wskazówki konfiguracyjne**

1. **Referencje projektów**  
   - `PlanDeck.App` ma referencje do:
     - `PlanDeck.Application` (żeby wstrzyknąć serwisy Application)  
     - `PlanDeck.Infrastructure` (żeby wstępnie skonfigurować EF Core i repozytoria)  
   - `PlanDeck.Application` ma referencję do `PlanDeck.Domain`.  
   - `PlanDeck.Infrastructure` ma referencję do `PlanDeck.Application` (w celu implementacji interfejsów) i do `PlanDeck.Domain`.  
   - `PlanDeck.Client` jest niezależny, wytwarza front-end – na poziomie kodu .NET nie musi referować pozostałych projektów (komunikuje się po HTTP).  

2. **IoC / Dependency Injection**  
   - W `Program.cs` (w `PlanDeck.App`) wywołujesz np. `services.AddApplicationServices()` i `services.AddInfrastructureServices(connectionString)`, co rejestruje potrzebne serwisy i repozytoria.  
   - W **Application** i **Infrastructure** możesz trzymać extension methods do rejestracji odpowiednich serwisów (np. `public static IServiceCollection AddInfrastructureServices(...)`).

3. Np. `RoomService` (w **PlanDeck.Application/Services**) używa repozytorium do zapisu i zwraca np. `RoomDto`.  

4. **Brak Domain Events**  
   - Jeśli nie potrzebujesz powiadomień wewnątrz domeny o zmianach stanu, nie implementujesz mechanizmu eventów domenowych. Wystarczą proste wywołania metod w serwisach i repozytoriach.

---

## **Podsumowanie**

1. **PlanDeck.Domain**  
   - Encje domenowe (np. `Room`, `Issue`), proste reguły biznesowe bez zależności zewnętrznych.  

2. **PlanDeck.Application**  
   - Klasy serwisowe (np. `RoomService`, `IssueService`), które implementują przypadki użycia.  
   - Interfejsy definiujące repozytoria i serwisy pomocnicze, DTO, mapowania.  

3. **PlanDeck.Infrastructure**  
   - Implementacja repozytoriów (EF Core, MSSQL), integracje (CSV, Azure DevOps), definicje kontekstu bazy, rejestracja w kontenerze IoC.  

4. **PlanDeck.App** (Serwer)  
   - ASP.NET Core.  
   - Kontrolery Web API (lub gRPC) wywołujące warstwę Application.  
   - Główne miejsce konfiguracji Dependency Injection i hostowania.  

5. **PlanDeck.Client** (Blazor WebAssembly)  
   - Frontend, komunikacja z `PlanDeck.App` przez HTTP/REST lub gRPC-Web.  
   - Strony (.razor), usługi do wywoływania API, layouty i zasoby statyczne.  

**Najważniejsze cechy**  
- Pełny rozdział warstw.  
- Warstwa Application nie musi mieć „command/query” – wystarczą serwisy z metodami łączącymi logikę domenową i repozytoria.  
- Warstwa Infrastructure jest wywoływana tylko przez Application, nigdy odwrotnie.  

