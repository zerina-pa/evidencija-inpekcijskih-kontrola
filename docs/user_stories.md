# User Stories

| ID | User story | Zahtjev |
|---|---|---|
| US-01 | Kao službenik, želim unositi, mijenjati i brisati proizvode, kako bih nad njima mogao evidentirati kontrole. | FZ-01 |
| US-02 | Kao službenik, želim unositi, mijenjati i brisati inspekcijska tijela, kako bih znao koje tijelo je izvršilo kontrolu. | FZ-02 |
| US-03 | Kao službenik, želim evidentirati inspekcijsku kontrolu, kako bi rezultat kontrole bio trajno zabilježen. | FZ-03 |
| US-04 | Kao službenik, želim da sistem ne dozvoli datum kontrole u budućnosti, kako evidencija ne bi sadržavala nemoguće podatke. | FZ-03, PP-01 |
| US-05 | Kao službenik, želim izabrati tijelo i period prije pokretanja izvještaja, kako bih dobio samo podatke koji me zanimaju. | FZ-04 |
| US-06 | Kao službenik, želim pregled svih kontrola tijela u periodu sortiran po datumu, kako bih imao uvid u rad tijela. | FZ-04 |
| US-07 | Kao službenik, želim vidjeti detalje izabrane kontrole, kako bih imao zapisnik o kontroli. | FZ-05 |

## Kriteriji prihvatanja

**US-01 / US-02**
- Obavezna polja se provjeravaju, a greška se prikazuje uz polje.
- Ne može se obrisati proizvod ili tijelo koje ima kontrole.

**US-04** (trenutno vrijeme: 26.11.2026. 10:00)
- 27.11.2026. 09:00 → odbijeno
- 26.11.2026. 10:30 → odbijeno (isti dan, kasnije vrijeme)
- 26.11.2026. 10:00 → prihvaćeno
- Pravilo važi i pri izmjeni kontrole i provjerava se na serveru.

**US-05 / US-06**
- Izvještaj se ne prikazuje dok parametri nisu izabrani.
- Datum od poslije datuma do → poruka o grešci.
- Kontrole na sam dan „datum do” ulaze u izvještaj.
- Prazan rezultat → „Nema kontrola za izabrani period.”

**US-07**
- Prikazani su serijski broj (ili „–”), naziv, zemlja porijekla, datum i vrijeme, rezultati.
- Nepostojeća kontrola → stranica „Nije pronađeno”.
