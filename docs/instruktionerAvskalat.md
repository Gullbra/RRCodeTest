
## Tech

### FE

* Angular 18
* Font Awesomw
* Responsive
* Bootstrap


### Backed

* tokenhantering (JWT)
* .net 8
* Rest API i C#


### DevOps

* Valfri gratis hostingsite
* GitHub repo

## Specifikt


### FE

Implementera en webbapplikation med en sida som visar en lista över alla böcker.
Skapa en startsida med en knapp för att lägga till en ny bok.
Om du klickar på knappen "Lägg till ny bok" bör användaren omdirigeras till ett formulär där de kan ange information om en ny bok (t.ex. titel, författare, publiceringsdatum).
Efter att ha skickat in formuläret ska användaren omdirigeras tillbaka till startsidan, där de kan se den nya boken som lagts till i listan.
Varje bok i listan bör ha en "Redigera"-knapp som tar användaren till ett formulär där de kan redigera detaljerna i boken.
Efter att ha skickat in formuläret ska användaren omdirigeras tillbaka till startsidan, där de kan se de uppdaterade bokdetaljerna i listan.
Varje bok i listan bör ha en "Radera"-knapp som låter användaren ta bort boken.
Efter att ha tagit bort en bok bör användaren se boken borttagen från listan.

#### Mina Citat

Skapa en separat vy som heter "Mina citat".
Visa listan på 5 citat du gillar.
Det ska finna en meny så att man kan gå mellan bok vyn och citat vyn

#### Dark vs Light mode

Implementera en knapp som gör att användaren kan växla mellan ljusa och mörka UX-design för applikationen.


### Auth

Implementera användarautentisering med JWT (JSON Web Tokens).
Skapa en enkel inloggningssida där användare kan ange sina referenser (t.ex. användarnamn och lösenord).
Efter lyckad inloggning bör back-end generera en token och skicka tillbaka den till front-end.
Front-end bör lagra token säkert (t.ex. i lokal lagring eller en cookie) och använda den för efterföljande API-förfrågningar till back-end.
Implementera token-validering på back-end för att säkerställa att endast autentiserade användare kan komma åt CRUD-operationerna.