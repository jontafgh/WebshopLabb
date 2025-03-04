# Labb 1 – Webshop
## Webbutveckling med .Net

### Laboration – Blazor
I den här laborationen kommer ni att bygga en Webshop med Blazor Web App utifrån följande krav:

#### För godkänt:
- [x] Appen ska ha en startsida (page) med en lista av produkter (komponenter)
- [x] Appen ska innehålla minst 10 produkter
- [x] En produkt innehåller information som:
  - [x] ID, Namn, Beskrivning, Bild-URL, Pris
- [ ] Produkterna ska visas upp med hjälp av Razor-komponenter på startsidan
- [x] Dessa komponenter ska inte visa all information om produkten, utan endast en överblick
- [x] Komponenten ska innehålla en knapp som lägger till produkten i en varukorg
- [x] Varukorgen ska lagras i localStorage (valfritt att använda i VG, inget krav).
- [x] Ett rest API ska användas för att få åtkomst till produkterna
  - [x] Produktdatan kan dock vara hårdkodad i APIet
- [x] Om man klickar på produkt-komponenten ska man komma till en produktsida (page) där all information om den specifika produkten visas
- [x] På denna sida ska det också vara möjligt att lägga till produkten i varukorgen
- [x] Man ska kunna navigera till produktsidan via sökfältet i webbläsaren (t.ex. localhost/product/1)
- [x] Man ska kunna navigera till varukorgen (page) via en knapp/länk
- [x] Sidan ska visa vad som finns i varukorgen
  - [x] Innehålla ett formulär för att fylla i adressuppgifter
- [ ] När formuläret skickas in ska användaren komma till en bekräftelsesida (page) där informationen om beställningen visas:
  - [ ] Vilka produkter som köpts
  - [ ] Namn och adress från formuläret
- [ ] När beställningen är klar ska varukorgen tömmas
- [x] Minst 2 komponenter (inte pages/Layout) ska användas
- [ ] All data i "godkänt"-scenariot kan vara statisk (hårdkodad)
- [ ] HTML ska användas på rätt sätt och valideras
  - [ ] Semantiska element där det finns möjlighet
- [ ] CSS ska vara tydligt strukturerad och bidra till GUI/UX
- [ ] Appen ska vara responsiv och anpassad till mobile & desktop
- [x] Inget CSS ramverk (bootstrap, tailwind etc.) får användas
- [x] Använd templatet Blazor Web App
  - [x] Man behöver inte kombinera olika rendermodes
  - [x] För godkänt räcker det med ett projekt i din solution

#### För Väl Godkänt:
- [ ] Alla kriterier för godkänt ska uppfyllas (localStorage kan väljas bort)
- [ ] Blazor Web App ska användas med både Server-rendering och Client-side rendering
- [x] Minst 4 komponenter (inte pages/Layout) ska designas och användas
- [ ] Produkternas kvantiteter ska hanteras:
  - [ ] Produkter ska kunna bli slutsålda
  - [ ] Komponenten för att visa upp produkter ska kunna ändra utseende beroende på om varan är slut eller om varan är på rea
- [ ] På produktsidan ska produktens pris kunna visas i olika valutor med hjälp av APIet [https://api-ninjas.com/api/exchangerate](https://api-ninjas.com/api/exchangerate)
  - [ ] APIet måste användas på ett sätt som gör att slutanvändaren inte kan komma åt API-nyckeln
- [x] En databas och ett rest API ska användas för att få åtkomst till produkterna
  - [x] Produktdatan ska lagras i databasen (OBS! Inte valutakurserna)
- [ ] Användaren ska kunna registrera ett användarkonto med användarnamn och lösenord
- [ ] En användare ska kunna logga in på sidan med hjälp av användarnamn och lösenord
- [ ] Sidan ska komma ihåg vad en användare har lagt i sin varukorg när den loggar in nästa gång
- [ ] Ett köp ska inte kunna slutföras utan att vara inloggad
- [ ] Informationen hämtas från Servern via ett HTTP-anrop till en API-endpoint
- [x] Delade klasser ska kunna användas från både Frontend och Backend
- [x] Dela upp din app i 3 projekt, exempelvis:
  - [x] WebshopFrontend (UI, sidor, komponenter)
  - [x] WebshopBackend (API-endpoints och affärslogik)
  - [x] WebshopShared (Ett separat bibliotek för exempelvis delade modeller, DTOs och gemensam valideringslogik)
- [x] I roten på ditt projekt (jämte .sln-filen) ska filen `Analysis.md` finnas. Där ska du dokumentera och demonstrera hur du använt .NET Core:s felsökningsverktyg och loggningssystem för att identifiera, analysera och åtgärda minst en specifik bugg i applikationen, inklusive en kort reflektion över bugghanteringsprocessen
- [x] Dessutom ska du ha filen `Assignment.md` där du kopierar in alla kriterier (G+VG) och bockar av varje utförd instruktion

### Inlämning
- [ ] Zippa/komprimera mappen med kodprojektet och lämna in under Inlämning – Labb 1 senast fredag den 21/3 2025 kl. 23:59.
