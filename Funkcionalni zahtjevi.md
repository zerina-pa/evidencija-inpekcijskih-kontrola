# Funkcionalni zahtjevi

## 1. Uvod
Dokument opisuje funkcionalnosti sistema za evidenciju inspekcijskih kontrola proizvoda.
Svaki zahtjev ima jedinstvenu oznaku (FZ-xx) i povezan je s tačkom projektnog zadatka.

**Korisnik sistema:** službenik inspekcije. Sistem ima jednu ulogu, a prijava korisnika
nije dio zadatka.

## 2. Pretpostavke i tumačenja zadatka
| # | Tekst zadatka | Tumačenje |
|---|---|---|
| P-01 | Tačka 2 ne navodi izričito izmjenu i brisanje tijela | Tijela imaju unos, izmjenu i brisanje, kao proizvodi i kontrole |
| P-02 | Tačka 3 traži „datum”, tačka 5 „datum i vrijeme” kontrole | Čuva se datum **i vrijeme** kontrole |
| P-03 | Tačka 4: „sortiran po datumu kontrole” | Sortiranje rastuće (od najstarije prema najnovijoj) |
| P-04 | „Disktrit Brčko” | Štamparska greška, koristi se „Distrikt Brčko” |
| P-05 | „Statički izvještaj” | Izvještaj samo za čitanje, generisan na zahtjev, pogodan za štampu |
| P-06 | „Datum u budućnosti” | Datum i vrijeme poslije trenutnog vremena servera (Europe/Sarajevo) |

## 3. Pregled zahtjeva
| ID | Naziv | Tačka zadatka |
|---|---|---|
| FZ-01 | Evidencija proizvoda | 1 |
| FZ-02 | Evidencija inspekcijskih tijela | 2 |
| FZ-03 | Evidencija inspekcijskih kontrola | 3 |
| FZ-04 | Izvještaj: kontrole po inspekcijskom tijelu za period | 4 |
| FZ-05 | Izvještaj: detalji inspekcijske kontrole | 5 |
| FZ-06 | Početni demonstracijski podaci | dodatno |

## 4. Opis zahtjeva

### FZ-01 Evidencija proizvoda
Sistem omogućava pregled liste, unos, izmjenu i brisanje proizvoda.

| Polje | Obavezno | Ograničenje |
|---|---|---|
| Naziv proizvoda | DA | do 200 znakova |
| Proizvođač | DA | do 200 znakova |
| Serijski broj | NE | do 100 znakova, jedinstven ako je unesen |
| Zemlja porijekla | DA | do 100 znakova |
| Opis | NE | do 2000 znakova |

- Lista prikazuje naziv, proizvođača, serijski broj i zemlju porijekla, s pretragom po nazivu.
- Brisanje zahtijeva potvrdu i nije dozvoljeno ako proizvod ima evidentirane kontrole (PP-03).

### FZ-02 Evidencija inspekcijskih tijela
Sistem omogućava pregled liste, unos, izmjenu i brisanje inspekcijskih tijela.

| Polje | Obavezno | Ograničenje |
|---|---|---|
| Naziv inspekcijskog tijela | DA | do 200 znakova |
| Inspektorat | DA | lista: FBiH, RS, Distrikt Brčko |
| Nadležnost | DA | lista: Tržišna inspekcija, Zdravstveno-sanitarna inspekcija |
| Kontakt osoba | DA | do 200 znakova |

- Brisanje zahtijeva potvrdu i nije dozvoljeno ako tijelo ima evidentirane kontrole (PP-03).

### FZ-03 Evidencija inspekcijskih kontrola
Sistem omogućava pregled liste, unos, izmjenu i brisanje inspekcijskih kontrola.

| Polje | Obavezno | Ograničenje |
|---|---|---|
| Datum i vrijeme kontrole | DA | ne smije biti u budućnosti (PP-01) |
| Nadležno inspekcijsko tijelo | DA | izbor iz liste evidentiranih tijela |
| Kontrolisani proizvod | DA | izbor iz liste evidentiranih proizvoda |
| Rezultati kontrole | DA | tekst, do 2000 znakova |
| Proizvod siguran | DA | DA / NE |

- Lista kontrola je sortirana po datumu opadajuće i ima link na detalje (FZ-05).
- Pri pokušaju unosa datuma u budućnosti sistem prikazuje poruku
  „Datum kontrole ne može biti u budućnosti.” i ne snima kontrolu.

### FZ-04 Izvještaj: kontrole po inspekcijskom tijelu za period
Statički izvještaj s parametarskom formom.

**Parametarska forma** (prikazuje se prije pokretanja izvještaja):
- Inspekcijsko tijelo: izbor iz liste, obavezno
- Datum od: obavezno
- Datum do: obavezno, ne smije biti prije datuma od (PP-04)

**Sadržaj izvještaja:**
- zaglavlje: naziv tijela, inspektorat, nadležnost, period, datum generisanja,
- tabela kontrola sortirana po datumu kontrole rastuće: datum i vrijeme, naziv proizvoda,
  serijski broj, proizvod siguran (DA/NE),
- ukupan broj kontrola,
- poruka „Nema kontrola za izabrani period.” kada nema podataka.

Period je uključiv: obuhvata kontrole od 00:00 prvog do 23:59 posljednjeg dana.

### FZ-05 Izvještaj: detalji inspekcijske kontrole
Statički izvještaj (zapisnik) za izabranu kontrolu. Prikazuje:
- serijski broj proizvoda (ili „–” ako nije unesen),
- naziv proizvoda,
- zemlju porijekla,
- datum i vrijeme kontrole,
- rezultate kontrole.

Dodatno (nije traženo, ali je korisno): inspekcijsko tijelo i status „Proizvod siguran”.

### FZ-06 Početni demonstracijski podaci
Pri prvom pokretanju s praznom bazom sistem upisuje primjere tijela, proizvoda i kontrola,
kako bi se izvještaji mogli odmah isprobati.

## 5. Poslovna pravila
| ID | Pravilo |
|---|---|
| PP-01 | Datum i vrijeme kontrole ne smiju biti poslije trenutnog vremena. Provjera se vrši na serveru, pri unosu i pri izmjeni. |
| PP-02 | Serijski broj proizvoda, ako je unesen, mora biti jedinstven. |
| PP-03 | Proizvod ili inspekcijsko tijelo koji imaju evidentirane kontrole ne mogu se obrisati. |
| PP-04 | U izvještaju FZ-04 datum od mora biti manji ili jednak datumu do. |
