# Product Vision
## Sistem za evidenciju inspekcijskih kontrola proizvoda (SEIK)


## 1. Svrha dokumenta
Dokument opisuje viziju, ciljeve, korisnike i obim sistema SEIK. Služi kao osnova za
product backlog, user stories te funkcionalne i nefunkcionalne zahtjeve.

## 2. Opis problema
Kontrolu kvaliteta i ispravnosti proizvoda na tržištu BiH provode inspekcijska tijela
tri inspektorata (FBiH, RS, Distrikt Brčko). Podaci o kontrolama se često vode u papirnim
zapisnicima ili nepovezanim tabelama. Posljedice su:

- teško je dobiti pregled svih kontrola jednog tijela za određeni period,
- pronalazak zapisnika o konkretnoj kontroli traje dugo,
- nema jedinstvene evidencije proizvoda ni inspekcijskih tijela,
- greške pri unosu (npr. datum kontrole u budućnosti) narušavaju vjerodostojnost evidencije.

## 3. Izjava o viziji
> **Za** službenike inspekcijskih tijela
> **koji** trebaju pouzdanu i preglednu evidenciju izvršenih kontrola proizvoda,
> **SEIK** je web aplikacija
> **koja** na jednom mjestu objedinjuje proizvode, inspekcijska tijela i kontrole
> i omogućava generisanje standardizovanih izvještaja.
> **Za razliku od** papirnih zapisnika i tabela,
> **naš proizvod** sprječava neispravan unos i daje izvještaje spremne za štampu.

## 4. Korisnici i zainteresovane strane

| Uloga | Opis | Interes |
|---|---|---|
| Službenik (primarni korisnik) | Evidentira proizvode, tijela i kontrole | Brz i ispravan unos, lahko pretraživanje |
| Rukovodilac inspekcije | Koristi izvještaje | Pregled rada tijela za period |
| Ocjenjivač zadatka | Ocjenjuje rješenje | Ispunjenost zahtjeva, kvalitet koda i dokumentacije |
| Potrošači (indirektno) | Krajnji korisnici proizvoda | Sigurni proizvodi na tržištu |

## 5. Ciljevi

### 5.1 Poslovni ciljevi
| ID | Cilj |
|---|---|
| C-01 | Jedinstvena evidencija proizvoda, inspekcijskih tijela i kontrola |
| C-02 | Pregled kontrola izabranog tijela za izabrani period, sortiran po datumu |
| C-03 | Brz pristup zapisniku o konkretnoj kontroli |
| C-04 | Sprječavanje logički neispravnih podataka (datum kontrole u budućnosti) |

### 5.2 Ciljevi projekta
| ID | Cilj |
|---|---|
| CP-01 | Ispuniti svih 5 tačaka projektnog zadatka i logičku kontrolu datuma |
| CP-02 | Realizovati rješenje kao web aplikaciju (prva preferirana platforma) |
| CP-03 | Priložiti tehničku dokumentaciju s dijagramima (ERD, use case) |
| CP-04 | Objaviti kod na javnom Git repozitoriju s jasnom historijom commitova |

## 6. Ključne funkcionalnosti

| # | Funkcionalnost | Tačka zadatka |
|---|---|---|
| 1 | Evidencija proizvoda (unos, izmjena, brisanje) | 1 |
| 2 | Evidencija inspekcijskih tijela | 2 |
| 3 | Evidencija inspekcijskih kontrola uz zabranu budućeg datuma | 3 |
| 4 | Statički izvještaj: kontrole po tijelu za period, s parametarskom formom | 4 |
| 5 | Statički izvještaj: detalji (zapisnik) izabrane kontrole | 5 |
| 6 | Štampa i izvoz izvještaja u PDF | dodatno |

## 7. Obim

**U obimu:** funkcionalnosti iz poglavlja 6, početni demonstracijski podaci, tehnička
dokumentacija, automatski testovi poslovnih pravila.

**Van obima (moguća unapređenja):**
- prijava korisnika i uloge,
- evidencija izmjena (audit log),
- REST API za druge sisteme,
- višejezičnost interfejsa,
- prilaganje fotografija i dokumenata uz kontrolu.

## 8. Pretpostavke i ograničenja
- Sistem koristi jedan tip korisnika (službenik). Autentifikacija nije tražena zadatkom.
- Liste vrijednosti za inspektorat i nadležnost su fiksne i definisane zadatkom.
- Vremenska zona sistema je Europe/Sarajevo.
- Rješenje mora biti pokretljivo lokalno, bez instalacije posebnog servera baze.
- Rok izrade je ograničen, pa prednost imaju obavezne funkcionalnosti (MoSCoW: Must).

## 9. Rizici

| Rizik | Vjerovatnoća | Uticaj | Mjera |
|---|---|---|---|
| Pogrešno tumačenje zahtjeva | Srednja | Visok | Matrica sljedivosti (zahtjev ↔ tačka zadatka), dokumentovane pretpostavke |
| Neispravan prikaz č, ć, š, ž, đ u PDF-u | Srednja | Srednji | Font s podrškom za dijakritike, ručni test |
| Problemi s vremenskom zonom pri provjeri datuma | Srednja | Visok | Apstrakcija sata (TimeProvider) i testovi graničnih slučajeva |
| Prekoračenje roka | Srednja | Visok | Iterativni rad, prvo Must stavke |

## 10. Kriteriji uspjeha
- Svih 5 tačaka zadatka je realizovano i demonstrirano na početnim podacima.
- Nije moguće snimiti kontrolu s datumom u budućnosti, ni zaobilaženjem forme.
- Izvještaj iz tačke 4 se pokreće tek nakon izbora parametara.
- Svi automatski testovi prolaze u CI-u.
- Aplikacija se pokreće prema uputstvu iz README-a.

## 11. Tehnološki okvir
ASP.NET Core MVC (.NET 8), Entity Framework Core, SQLite, Bootstrap 5,
QuestPDF (PDF izvještaji), xUnit (testovi), GitHub Actions (CI), PlantUML (dijagrami).
