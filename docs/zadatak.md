# Projektni zadatak

## Opis zahtjeva

Za potrebe kontrole kvaliteta i ispravnosti proizvoda na tržištu BiH potrebno je dizajnirati sistem koji će omogućiti evidenciju i pregled izvršenih **inspekcijskih kontrola** registrovanih **proizvoda** od strane **inspekcijskih tijela** evidentiranih u sistemu.

U tu svrhu potrebno je realizirati sljedeće:

1. Evidenciju (unos, izmjenu i brisanje) proizvoljnog broja proizvoda, pri čemu je za svaki proizvod neophodno unijeti:
   - Naziv proizvoda
   - Proizvođač
   - Serijski broj (opcionalno)
   - Zemlja porijekla
   - Opis (opcionalno)

2. Evidenciju proizvoljnog broja inspekcijskih tijela, pri čemu je za svako inspekcijsko tijelo neophodno unijeti:
   - Naziv inspekcijskog tijela
   - Inspektorat (lista vrijednosti: FBiH, RS, Distrikt Brčko)
   - Nadležnost (lista vrijednosti: Tržišna inspekcija, Zdravstveno-sanitarna inspekcija)
   - Kontakt osoba

3. Evidenciju (unos, izmjenu i brisanje) proizvoljnog broja inspekcijskih kontrola, pri čemu je za svaku inspekcijsku kontrolu neophodno navesti:
   - Datum inspekcijske kontrole
   - Nadležno inspekcijsko tijelo (vrijednost izabrana iz liste evidentiranih inspekcijskih tijela)
   - Kontrolisani proizvod (vrijednost izabrana iz liste evidentiranih proizvoda)
   - Rezultati kontrole (tekstualni opis)
   - Proizvod siguran (True/False vrijednost)

4. Pregled svih izvršenih inspekcijskih kontrola za izabrano inspekcijsko tijelo za izabrani vremenski period, sortiran po datumu kontrole.

5. Pregled detalja izabrane inspekcijske kontrole, tačnije:
   - Detalji kontrolisanog proizvoda (serijski broj, naziv, zemlja porijekla)
   - Datum i vrijeme kontrole
   - Rezultati kontrole

Postaviti logičku kontrolu za zahtjev 3. tako da nije moguće unijeti datum inspekcijske kontrole u budućnosti.

Zahtjevi iz 4. i 5. se trebaju realizirati u obliku statičkog izvještaja. Zahtjev iz 4. je potrebno realizirati sa parametarskom formom koja omogućava odabir parametara izvještaja prije pokretanja.

## Napomene

Zadatak je moguće realizirati koristeći bilo koji moderni programski jezik / framework.

Preferirana platforma (u redoslijedu preferencije) su:

1. **Web aplikacija**
2. Desktop aplikacija
3. Komandnolinijska aplikacija

Uz implementaciju (arhiva izvornog koda ili link na javni GIT repozitorij) priložiti i funkcionalni opis predloženog rješenja (tehnička dokumentacija) zajedno sa pratećim dijagramima i/ili ostalom tehničkom dokumentacijom koju smatrate korisnom za razumijevanje rješenja.