Problem:        <tr> elementen i mitt checkout table (CheckOut.razor) hamnade längst ner när jag ändrade kvantitet på en produkt ibland

Felsökning 1:   Satte en breakpoint på UpdateProductQuantity()
Resultat:       Blev inte klokare

Felsökning 2:   Jag hade tidigare lärt mig att IJSRuntime.InvokeAsyc inte lirade bra med OnInitializedAsync() och använde därför OnAfterRenderAsync()
                Satte en breakpoint på OnAfterRenderAsync() där jag hämtade varukorgen från LocalStorage

Hittat fel:     Insåg att eftersom jag inte hade specificerat om InvokeAsync bara skulle köras vid firstRender så kördes metoden varje gång jag interagerade med + - och x.
                Metoden skrev sådeles över _boardgames listan hela tiden och ordningen på objekten ändrades.

Lösning:        Lade till villkoret if(firstRender) så metoden bara körs en gång
                Såg till att logik som interagerar med local storage sker när UpdateProductQuantity() körs

# ----------------
# ----------------

Problem:        Fick följande fel i WebshopBackend.Services.UserService.GetUserDetailsAsync när jag försökte lägga en order, men bara ibland.

                    "A second operation started on this context before a previous operation completed.
                    This is usually caused by different threads using the same instance of DbContext,
                    however instance members are not guaranteed to be thread safe."

Felsökning 1:   Kollade först länken (https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#avoiding-dbcontext-threading-issues) som nämndes i felet.
                Såg att detta kunde uppstå om man inte awaitade dBcontext operationer då dBcontext inte är thread-safe.
                Kollade så jag använt await på alla operationer i metoden och endpointen som använde den.

Resultat:       Jag awaitade allt, så felsökte vidare

Felsökning 2:   Kollade i debugloggen vilka operationer som utfördes på databasen när felet uppstod,
                och det var både när jag hämtade UserDetails eller när jag insertade en Order.
                Satte breakpoints på alla metoder i Frontend som hämtade UserDetails och postade OrderDto.

Insikt 1:       Komponenter som renderas tar inte hänsyn till varandra, t.ex LogIn() metoden startas i LogInBox kompotenten.
                Samtidigt startar Cart komponetenen osv.
                Jag ser inget sätt, eller vet inget sätt att awaita komponenter.

Lösning 1:      Jag skrev om lite kod för att minska anrop till backend och problemet försvann. (Trodde jag)
                Men känns fel att lösa ett backendproblem i frontend.

Insikt 2:       Problemet försvann inte

Felsökning 3:   Jag googlade, chatgptade och hittade IDbContextFactory.
                Det funkar lite som IHttpClientFactory och jag kan skapa och disposa dBcontexts för varje databas-query

Lösning 2:      Implementerade IDbContextFactory i samtliga queries och har sedan dess inte sett felt igen.








