Problem:        <tr> elementen i mitt checkout table (CheckOut.razor) hamnade längst ner när jag ändrade kvantitet på en produkt ibland

Felsökning 1:   Satte en breakpoint på UpdateProductQuantity()
Resultat:       Blev inte klokare

Felsökning 2:   Jag hade tidigare lärt mig att IJSRuntime.InvokeAsyc inte lirade bra med OnInitializedAsync() och använde därför OnAfterRenderAsync()
                Satte en breakpoint på OnAfterRenderAsync() där jag hämtade varukorgen från LocalStorage

Hittat fel:     Insåg att eftersom jag inte hade specificerat om InvokeAsync bara skulle köras vid firstRender så kördes metoden varje gång jag interagerade med + - och x.
                Metoden skrev sådeles över _boardgames dictionaryn hela tiden och ordningen på objekten ändrades.

Lösning:        Lade till villkoret if(firstRender) så metoden bara körs en gång
                Såg till att logik som interagerar med local storage sker när UpdateProductQuantity() körs

