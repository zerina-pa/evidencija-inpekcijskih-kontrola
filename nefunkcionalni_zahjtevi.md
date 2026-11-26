# Nefunkcionalni zahtjevi

| ID | Kategorija | Zahtjev |
|---|---|---|
| NFZ-01 | Platforma | Sistem je web aplikacija koja radi u aktuelnim verzijama Chrome, Firefox i Edge preglednika. |
| NFZ-02 | Pouzdanost validacije | Sva poslovna pravila provjeravaju se na serveru, pa ih nije moguće zaobići slanjem zahtjeva mimo forme. |
| NFZ-03 | Integritet podataka | Baza podataka osigurava obavezna polja, strane ključeve (bez kaskadnog brisanja) i jedinstvenost serijskog broja. |
| NFZ-04 | Sigurnost | Aplikacija je zaštićena od SQL injection, XSS i CSRF napada (parametrizovani upiti, enkodiranje prikaza, anti-forgery token). |
| NFZ-05 | Upotrebljivost | Interfejs je na bosanskom jeziku. Poruke o greškama prikazuju se uz odgovarajuće polje, a uneseni podaci se ne gube. |
| NFZ-06 | Lokalizacija | Datumi se prikazuju u formatu `dd.MM.yyyy. HH:mm`, a slova č, ć, š, ž, đ ispravno se prikazuju i pri štampi. |
| NFZ-07 | Izvještaji | Izvještaji su pogodni za štampu iz preglednika (bez navigacije i dugmadi na odštampanoj strani). |
| NFZ-08 | Prenosivost | Aplikacija se pokreće lokalno, bez instalacije posebnog servera baze podataka. |
| NFZ-09 | Održivost | Kod je podijeljen na slojeve (kontroleri, servisi, pristup podacima), a poslovna logika nije u kontrolerima. |
| NFZ-10 | Testabilnost | Poslovna pravila PP-01 do PP-04 pokrivena su automatskim testovima. |

## Ograničenja
- Sistem ima jednu korisničku ulogu, bez prijave.
- Liste vrijednosti za inspektorat i nadležnost su fiksne.

## Moguća unapređenja (van obima)
- Prijava korisnika i uloge
- Evidencija izmjena (audit log)
- Izvoz izvještaja u PDF
- Prilaganje dokumenata i fotografija uz kontrolu
