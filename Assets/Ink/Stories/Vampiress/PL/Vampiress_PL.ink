VAR speakerIndex = -1
VAR canScan = false
VAR ticketRejected = false

~ speakerIndex = 0
Dobry wieczór, Pani. Poproszę bilet do kontroli.

~ speakerIndex = 1
Proszę. Stacja Maplewood, bilet w jedną stronę. Bez powrotu.

~ speakerIndex = 0
Rozumiem, Pani cel podróży?

~ speakerIndex = 1
Udaję się na spotkanie. Jednak z perspektywy czasu, jest ono bardzo spóźnione.

~ speakerIndex = 0
Pani bilet przedawnił się… 70 lat temu..

~ speakerIndex = 1
Był on kupiony dawno. Wtedy, gdy jeszcze wierzyłam w lepsze jutro.

~ speakerIndex = 0
* [Lepsze jutro?]
        -> lepsze_jutro
        
* [Przepraszam, ale z przedawnionym biletem, nie mogę Pani pozwolić dalej jechać.]
    ~ speakerIndex = 1
    … Czas to tylko pojęcie względne.
Ja mam go aż za dużo.
Jednak inni mają go zdecydowanie za mało..
Wysiądę na następnej stacji…

    ~ ticketRejected = true
        -> END
        
=== lepsze_jutro ===
~ speakerIndex = 1
Bilet kupiłam gdy ta sama kolej powstawała lata temu, młody człowieku.
Gdy moje plany i marzenia nie zostały jeszcze zamiecione pod dywan.

~ speakerIndex = 0
* [Pani marzenia? Może być Pani bardziej precyzyjna?]
    -> regret

* [Przepraszam, ale z przedawnionym biletem, nie mogę Pani pozwolić dalej jechać.]
    ~ speakerIndex = 1
    … Jednak inni mieli rację.
Na marzeniach nie da się żyć.
Dziękuję za utwierdzenie mnie w tym przekonaniu, gdy zaczęłam w nie wątpić
    ~ ticketRejected = true
    -> END

=== regret ===
~ speakerIndex = 1
Widzisz młody człowieku, nie każde marzenia mogą się spełnić. Szczególnie gdy są….
Zakazane.
Niepoprawne.
Gdy marzenia przeszkadzają innym. Nie ma wtedy ucieczki. Chyba, że ma się na tyle odwagi w sobie.
Ja….
Ja nie miałam…
I teraz tego bardzo żałuję…

~ speakerIndex = 0
* [Czego Pani żałuje?]
    -> love
* [Dlaczego Pani tego żałuje?]
    -> remorse

* [Przepraszam, jednak jeśli bilet jest przedawniony, nie mogę Pani pozwolić dalej jechać. Nieważne z jakiego powodu.]
    ~ speakerIndex = 1
    … Jednak nic się nie zmieniło…
Ludziom dalej przeszkadzają marzenia i plany innych istot.
    ~ ticketRejected = true
    -> END

=== love ===
~ speakerIndex = 1
Miłości.
A raczej niepodążania za nią. Nieubiegania się o nią. Niegonienia za nią.
Żałuję, że nie miałam odwagi. Nie miałam odwagi przeciwstawić się starszyźnie. Nie miałam odwagi powiedzieć “nie”.

~ speakerIndex = 0
* [Pani męża?]
    -> beloved_man
* [Olivier?]
    -> is_dying
* [Przepraszam, jednak jeśli bilet jest przedawniony, nie mogę Pani pozwolić dalej jechać. Nieważne z jakiego powodu.]
    ~ speakerIndex = 1
    … And yet nothing has changed…
    Men still despise and fear the dreams and wishes of other beings.
    ~ ticketRejected = true
    -> END
    
=== is_dying ===
My beloved is dying. A mortal man, his time nearly spent - while I have far too much of mine.
Young man, please… have mercy. Allow me to see him, if only this once. Now that I have finally learned what truly matters.
    -> final_decision


=== remorse ===
~ speakerIndex = 1
Gdybym miała odwagę… Gdybym tylko ją miała…
To moje życie potoczyłoby się inaczej.
Jednak… Jednak ja…
Wampirzyca:
Nie miałam odwagi zabrać głosu, by przy wszystkich powiedzieć jak bardzo kocham Mojego Oliviera. 

~ speakerIndex = 0
* [Pani męża?]
    -> beloved_man
* [Olivier?]
    -> olivier
* [Przepraszam, jednak jeśli bilet jest przedawniony, nie mogę Pani pozwolić dalej jechać. Nieważne z jakiego powodu.]
    ~ speakerIndex = 1
    …I should have done it long ago.
    When there was still time.
    But now…
    … Forget it.
    ~ ticketRejected = true
    -> END
    

=== beloved_man ===
~ speakerIndex = 1
Mój ukochany.
Mężem nigdy nie mogłam go nazwać, przez popełnione przeze mnie błędy.

~ speakerIndex = 0
* [Chciałbym znać odpowiedzi na Pani pytania. Jednak wiem jedno, miłość nie wybiera.]
    … Nie wybiera…
Miłość jest okrutna.
Czy to my jesteśmy okrutni?

    -> final_decision

* [Niestety nie znam odpowiedzi na Pani pytania…]
    ~ speakerIndex = 1
     .. Nic się nie stało, młody człowieku. Niektóre pytania muszą pozostać bez odpowiedzi.
Lub czekać, aż sami do niej dojdziemy.
Mój ukochany umiera, to człowiek. Czas go goni. A ja mam go aż za mało.
Proszę, młody człowieku. Miej serce. Pozwól mi się spotkać z ukochanym, chociaż teraz, gdy wreszcie zmądrzałam.

    -> final_decision
* [Niestety nie potrafię odpowiedzieć na Pani pytania. Pani bilet jest przedawniony, proszę opuścić pociąg na najbliższej stacji.]
    ~ speakerIndex = 1
    …And yet, I was wrong.
    We were never meant for one another. For had we been, fate would have guided us together…
    Would it not..?
    ~ ticketRejected = true
    -> END


=== olivier ===
~ speakerIndex = 1
Olivier… Mój ukochany.
Poznaliśmy się za szczeniaka. Ja, młody wampir, ledwo kontrolujący swoje zachowania. On, głupi i naiwny człowiek. Był synem łowcy potworów, a ja szlachcianką.
Nasze rasy nigdy nie powinny były się spotkać. A jednak.
A jednak to przy nim kontrolowałam swoje moce najbardziej. To on uczył mnie opanowania, kontroli. A ja uczyłam go jak zachować zimną krew.
Pomimo tego, że byliśmy z dwóch różnych biegunów, dopełnialiśmy się jak dwa puzzle układanki.
Powiedz mi, jak to możliwe?

~ speakerIndex = 0
* [I wish I knew the answer to your question, madam. But I do know this, love chooses none of us.]
    … It chooses none of us…
    Love is cruel.
    -> final_decision

* [I wish I had the answers for your questions, milady…]
    ~ speakerIndex = 1
     .. It matters not, young man. Some questions are best left unanswered.
    Or wait until we find the courage to face them ourselves.
    My beloved is dying. A mortal man, his time nearly spent - while I have far too much of mine.
    Young man, please… have mercy. Allow me to see him, if only this once. Now that I have finally learned what truly matters.
    -> final_decision
* [I wish I knew the answer to your question, madam. Still… your ticket has expired. You must alight at the next station.]
    ~ speakerIndex = 1
    …And yet, I was wrong.
    We were never meant for one another. For had we been, fate would have guided us together…
    Would it not..?
    ~ ticketRejected = true
    -> END


=== final_decision ===
~ speakerIndex = 0
* [… Very well. Upon mine own responsibility, you may proceed to your destination.]
    ~ speakerIndex = 1
    ~ canScan = true
    Thank you, young man. You have my eternal gratitude.
    But remember, never ignore your heart.
    It alone knows the way.
    -> END

* [I cannot do that, madam. I must obey the rules.]
    ~ speakerIndex = 1
    … Rules…
    We all have them.
    We all chase unseen purposes.
    But remember, young man. 
    Never let them silence you. Never let them shape you. Be the master of your own fate.
    Unlike me…
    ~ ticketRejected = true
    -> END
