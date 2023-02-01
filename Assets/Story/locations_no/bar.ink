=== Bar
{not first:
    ->first
}

SETTINGS #mode:overworld
+ [BARTENDER]
->marianne->
+ [RONNY]
->ronny->
+ [TELEFON] ->telefon
+ [BOKHYLLE] ->bokhylle

- ->Bar

= first
CUTSCENE #mode:conversation #cutscene:enter_bar
ANITA: Phew. For ein dag. No skal det bli godt med ein pause.
RONNY: Hei, Anita! Vi sit her borte!
ANITA: Hei! Eg skal berre kjøpe noko å drikke, så kjem eg!
->Bar

= telefon
ANITA: Eg treng ikkje å bruke telefonkiosken akkurat no. #mode:conversation
Alle venene mine er i dette rommet allereie, liksom?
-> Bar

= bokhylle
ANITA: Eg liker at det står ei lita bokhylle her.  #mode:conversation
Set inn vits om ei bok som var aktuell i 2003 her, liksom.
Kanskje til og med fleire vitsar og så plukkast det ein tilfeldig.
Det får du til, Olaf.
-> Bar