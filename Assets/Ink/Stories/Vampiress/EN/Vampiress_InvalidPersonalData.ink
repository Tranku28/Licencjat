VAR speakerIndex = -1
VAR canScan = false
VAR allowQuitDialogue = false
VAR dissapear = false
VAR harmony = 0
VAR passengerAction = ""
VAR decision = ""
VAR brokenRule = ""
VAR addEntry = ""
VAR gift = false
VAR staysNextDay = false

~ addEntry = "Tonight, a woman boarded the train. Young, at first glance, yet her eyes held something... ancient. Her ticket had a different name. When I told her, she smiled - as though time itself had long since lost meaning. And she started to talk about her life before all this. She should not have been here. And yet, I felt that this train had been waiting for her all along. This train carries more than passengers."
~ speakerIndex = 0
Good evening, madam. Your ticket, please.

~ speakerIndex = 1
Charlotte Bloodrose.

~ speakerIndex = 0
Good evening, Miss Bloodrose. Your ticket, please.

~ speakerIndex = 1
Here it is. Station Maplewood, one-way ticket. No return.

~ speakerIndex = 0
Understood. And may I ask the purpose of you journey?

~ speakerIndex = 1
I am bound for a meeting… though, in truth, it comes many years too late.

~ speakerIndex = 0
Madam, your name doesn’t match with the one on the ticket....

~ speakerIndex = 1
Aye. It is the nickname my beloved called me once… Till this day it feels like an unfulfilled dream.

* [Unfulfilled dream?]
    ~ speakerIndex = 0
    Unfulfilled dream?
    -> unfunfilled_dream
* [Forgive me, milady, but with an incorrect ticket, I cannot allow you to proceed with your journey (refuse)]
    ~ speakerIndex = 0
    Forgive me, milady, but with an incorrect ticket, I cannot allow you to proceed with your journey
    -> cannot_allow_1

=== cannot_allow_1 ===
~ speakerIndex = 1
…Time is but a fragile notion.
I possess too much of it.
Others, far too little.
I shall alight at the next station…
~ harmony = -25
~ dissapear = true
~ decision = "Disapproved"
~ passengerAction = "Charlotte didn’t reach the designated destination"
~ addEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
~ allowQuitDialogue = true
    -> END

=== unfunfilled_dream ===
~ speakerIndex = 1
Ah, yes…
Something that you want to chase so dearly… Yet none of your dreams did come true…

* [Your dreams, milady? Could you be more specific?]
    ~ speakerIndex = 0
    Your dreams, milady? Could you be more specific?
    -> dreams
* [Forgive me, milady, but with an incorrect ticket, I cannot allow you to proceed with your journey (refuse)]
    ~ speakerIndex = 0
    Forgive me, milady, but with an incorrect ticket, I cannot allow you to proceed with your journey
    -> cannot_allow_2

=== cannot_allow_2 ===
~ speakerIndex = 1
… Perhaps they were right after all.
One cannot live upon dreams alone.
Thank you for reminding me of that truth, just as I began to doubt it.
~ harmony = -25
~ dissapear = true
~ decision = "Disapproved"
~ passengerAction = "Charlotte didn’t reach the designated destination"
~ addEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
~ allowQuitDialogue = true
    -> END


=== dreams ===
~ speakerIndex = 1
You see, young man, not every dream is meant to come true.
Especially those that are….
Forbidden.
Improper.
When one’s dreams offend others, there is no escape…
Unless one possesses the courage to defy them.
I….
I lacked that courage…
And now I bear the weight of that regret…

* [What is it that you regret, madam?]
    ~ speakerIndex = 0
    What is it that you regret, madam?
    -> regret
* [Why such remorse?]
    ~ speakerIndex = 0
    Why such remorse?
    -> why_such_remorse
* [Forgive me, but regardless of your reason - with an incorrect ticket, I cannot let you pass (refuse)]
    ~ speakerIndex = 0
    Forgive me, but regardless of your reason - with an incorrect ticket, I cannot let you pass
    -> cannot_allow_3

=== cannot_allow_3 ===
… And yet nothing has changed…
Men still despise and fear the dreams and wishes of other beings.
~ harmony = -25
~ dissapear = true
~ decision = "Disapproved"
~ passengerAction = "Charlotte didn’t reach the designated destination"
~ addEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
~ allowQuitDialogue = true
    -> END

=== regret ===
~ speakerIndex = 1
Love.
Or rather, my failure to follow it. 
Not chasing after it.
Not daring to reach for it.
I regret that I lacked the courage. The courage to defy the elders. The courage to say “no”.
I lacked the voice to speak his name aloud - to tell the world how deeply I loved my Olivier.

* [Your husband?]
    ~ speakerIndex = 0
    Your husband?
    -> husband
* [Olivier]
    ~ speakerIndex = 0
    Olivier
    -> olivier
* [Forgive me, but regardless of your reason - with an incorrect ticket, I cannot let you pass (refuse)]
    ~ speakerIndex = 0
    Forgive me, but regardless of your reason - with an incorrect ticket, I cannot let you pass
    -> cannot_allow_4

=== cannot_allow_4 ===
~ speakerIndex = 1
…I should have done it long ago.
When there was still time.
But now…
… Forget it.
~ harmony = -25
~ dissapear = true
~ decision = "Disapproved"
~ passengerAction = "Charlotte didn’t reach the designated destination"
~ addEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
~ allowQuitDialogue = true
    -> END


=== why_such_remorse ===
~ speakerIndex = 1
Had I but possessed courage… had I only dared… 
My life might have taken another course.
And yet… I…
I lacked the voice to speak his name aloud - to tell the world how deeply I loved my Olivier.

* [Your husband?]
    ~ speakerIndex = 0
    Your husband?
    -> husband
* [Olivier]
    ~ speakerIndex = 0
    Olivier
    -> olivier
* [Forgive me, but regardless of your reason - with an incorrect ticket, I cannot let you pass (refuse)]
    ~ speakerIndex = 0
    Forgive me, but regardless of your reason - with an incorrect ticket, I cannot let you pass
    -> cannot_allow_4



=== husband ===
~ speakerIndex = 1
My beloved.
Husband he never was - my mistakes forbade it.
We met in our youth. I, a fledgling vampire, struggling to master my instincts. He, a tender-hearted human boy.
The son of a hunter… And I, a noble’s daughter.
Our kinds were never meant to meet. And yet…
And yet, with him, my hunger stilled and my power obeyed. He taught me restraint, I taught him composure.
Though born of two opposing worlds, we fit together like pieces of the same design.
Tell me, how could such a thing be?

* [I wish I knew the answer to your question, madam. But I do know this, love chooses none of us.]
    ~ speakerIndex = 0
    I wish I knew the answer to your question, madam. But I do know this, love chooses none of us.
    -> love_chooses
* [I wish I had the answers for your questions, milady…]
    ~ speakerIndex = 0
    I wish I had the answers for your questions, milady…
    -> it_matters_not
* [I wish I knew the answer to your question, madam. Still… your ticket is incorrect. You must alight at the next station (refuse)]
    ~ speakerIndex = 0
    I wish I knew the answer to your question, madam. Still… your ticket is incorrect. You must alight at the next station
    -> cannot_allow_5


=== olivier ===
~ speakerIndex = 1
Olivier… My dearest heart.
We met in our youth. I, a fledgling vampire, struggling to master my instincts. He, a tender-hearted human boy.
The son of a hunter… And I, a noble’s daughter.
Our kinds were never meant to meet. And yet…
And yet, with him, my hunger stilled and my power obeyed. He taught me restraint, I taught him composure.
Though born of two opposing worlds, we fit together like pieces of the same design.
Tell me, how could such a thing be?

* [I wish I knew the answer to your question, madam. But I do know this, love chooses none of us.]
    ~ speakerIndex = 0
    I wish I knew the answer to your question, madam. But I do know this, love chooses none of us.
    -> love_chooses
* [I wish I had the answers for your questions, milady…]
    ~ speakerIndex = 0
    I wish I had the answers for your questions, milady…
    -> it_matters_not
* [I wish I knew the answer to your question, madam. Still… your ticket is incorrect. You must alight at the next station (refuse)]
    ~ speakerIndex = 0
    I wish I knew the answer to your question, madam. Still… your ticket is incorrect. You must alight at the next station
    -> cannot_allow_5

=== cannot_allow_5 ===
~ speakerIndex = 1
…And yet, I was wrong.
We were never meant for one another. For had we been, fate would have guided us together…
Would it not..?
~ harmony = -25
~ dissapear = true
~ decision = "Disapproved"
~ passengerAction = "Charlotte didn’t reach the designated destination"
~ addEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
~ allowQuitDialogue = true
    -> END

=== love_chooses ===
… It chooses none of us…
Love is cruel.
Or perhaps… it is we who are cruel?

* [… Very well. Upon mine own responsibility, you may proceed to your destination (accept)]
    ~ speakerIndex = 0
    … Very well. Upon mine own responsibility, you may proceed to your destination
    -> accept
* [I cannot do that, madam. I must obey the rules (refuse)]
    ~ speakerIndex = 0
    I cannot do that, madam. I must obey the rules
    -> cannot_allow_6

=== it_matters_not ===
    ~ speakerIndex = 1
.. It matters not, young man. Some questions are best left unanswered.
Or wait until we find the courage to face them ourselves.
My beloved is dying. A mortal man, his time nearly spent - while I have far too much of mine.
Young man, please… have mercy. Allow me to see him, if only this once. Now that I have finally learned what truly matters.

* [… Very well. Upon mine own responsibility, you may proceed to your destination (accept)]
    ~ speakerIndex = 0
    … Very well. Upon mine own responsibility, you may proceed to your destination
    -> accept
* [I cannot do that, madam. I must obey the rules (refuse)]
    ~ speakerIndex = 0
    I cannot do that, madam. I must obey the rules
    -> cannot_allow_6

=== cannot_allow_6 ===
… Rules…
We all have them.
We all chase unseen purposes.
But remember, young man.
~ harmony = -25
~ decision = "Disapproved"
~ passengerAction = "Charlotte didn’t reach the designated destination"
~ addEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
~ dissapear = true
~ allowQuitDialogue = true
    -> END

=== accept ===
Thank you, young man. You have my eternal gratitude.
But remember, never ignore your heart.
It alone knows the way.
~ canScan = true
~ harmony = 25
~ decision = "Approved"
~ passengerAction = "Charlotte reached the designated destination"
~ addEntry = "I let her pass. I do not know why. Perhaps because in her eyes I saw something I have long forgotten - hope. She said she was going to see her beloved, one last time. A mortal man, dying. They should never have existed in the same world and yet, there was more life in her voice than in many hearts I’ve met along this endless route. Little did she know that she's already dead herself."
~ passengerAction = "Charlotte personal data was incorrect, but you let her pass"
~ brokenRule = "Invalid Personal Data."
~ gift = true
    -> END



