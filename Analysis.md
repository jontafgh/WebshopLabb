Problem:        <tr> elementen i mitt checkout table (CheckOut.razor) hamnade längst ner när jag ändrade kvantitet på en produkt ibland

Felsökning 1:   Satte en breakpoint på UpdateProductQuantity()
Resultat:       Blev inte klokare

Felsökning 2:   Jag hade tidigare lärt mig att IJSRuntime.InvokeAsyc inte lirade bra med OnInitializedAsync() och använde därför OnAfterRenderAsync()
                Satte en breakpoint på OnAfterRenderAsync() där jag hämtade varukorgen från LocalStorage

Hittat fel:     Insåg att eftersom jag inte hade specificerat om InvokeAsync bara skulle köras vid firstRender så kördes metoden varje gång jag interagerade med + - och x.
                Metoden skrev sådeles över _boardgames listan hela tiden och ordningen på objekten ändrades.

Lösning:        Lade till villkoret if(firstRender) så metoden bara körs en gång
                Såg till att logik som interagerar med local storage sker när UpdateProductQuantity() körs

#####################################
#####################################

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

Insikt:         På min checkout sida renderar jag olika komponenter beroende på vilkor.
                Innan man betalar ska man fylla i en Form med UserDetails,
                denna autofyller jag åt användaren om hen är inloggad och redan fyllt i tidigare.
                När formen submittas så skickar jag resultatet med en eventcallback till parenten som sedan postar till backend och postar sen ordern.

Resultat:       På form-komponenten så autofyllde jag med hjälp av OnParametersSetAsync()
                Såg att den renderades en till gång när jag tryckte på submit
                Detta antar jag (ska checka mina assumptions senare) den gjorde när jag invokade mitt EventCallback.
                Det som hände var att form-komponenten hämtade UserDetails igen, samtitigt som parenten postade UserDetails.

Lösning:        Jag provade använda OnInitializedAsync() istället för OnParametersSetAsync() och gjorde om lite logk.
                Nu renderas komponenten bara en gång och har inte fått felet igen
                Men det känns som att lösnigen borde ske i backend, men har inte listat ut hur än.
                Vill inte lösa problem som uppstår i backend genom att ändra i frontend.


