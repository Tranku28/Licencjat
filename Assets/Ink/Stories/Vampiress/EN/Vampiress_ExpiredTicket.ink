VAR speakerIndex = -1
VAR canScan = false
VAR allowQuitDialogue = false
VAR dissapear = false
VAR harmony = 0
VAR passengerAction = ""
VAR decision = ""
VAR brokenRule = ""
VAR generalEntry = ""
VAR encounterEntry = ""
VAR gift = false

~ speakerIndex = 0
Good evening, madam. Your ticket, please.
~ generalEntry = "Tonight, a woman boarded the train. Young, at first glance, yet her eyes held something... ancient. Her ticket had expired seventy years ago. When I told her, she merely smiled - as though time itself had long since lost meaning. She should not have been here. And yet, I felt that this train had been waiting for her all along. This train carries more than passengers."

~ speakerIndex = 1
Here it is. Station Maplewood, one-way ticket. No return.

~ speakerIndex = 0
Understood. And may I ask the purpose of your journey?

~ speakerIndex = 1
I am bound for a meeting… though, in truth, it comes many years too late.

~ speakerIndex = 0
Madam, your ticket expired… seventy years ago.

~ speakerIndex = 1
Aye. It was purchased long ago. Back when I still believed in a brighter morrow.

* [A ‘brighter morrow’?]
    ~ speakerIndex = 0
    A ‘brighter morrow’?
    -> a_brighter_morrow
        
* [Forgive me, milady, but with an expired ticket, I cannot allow you to proceed with your journey (refuse)]
    ~ speakerIndex = 0
    Forgive me, milady, but with an expired ticket, I cannot allow you to proceed with your journey
    ~ speakerIndex = 1
    …Time is but a fragile notion.
    I possess too much of it.
    Others, far too little.
    I shall alight at the next station…
    ~ dissapear = true
    ~ decision = "Disapproved"
    ~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
    ~ harmony = -25
    ~ encounterEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
        -> END
        
=== a_brighter_morrow ===
~ speakerIndex = 1
That ticket was bought when these very rails were still being forged, young man.
Back then, my dreams had yet to be buried beneath dust and silence.

* [Your dreams, milady? Could you be more specific?]
    ~ speakerIndex = 0
    Your dreams, milady? Could you be more specific?
    -> regret

* [Forgive me, milady, but with an expired ticket, I cannot allow you to proceed with your journey (refuse)]
    ~ speakerIndex = 0
    Forgive me, milady, but with an expired ticket, I cannot allow you to proceed with your journey
    ~ speakerIndex = 1
    … Perhaps they were right after all.
    One cannot live upon dreams alone.
    Thank you for reminding me of that truth, just as I began to doubt it.
    ~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
    ~ decision = "Disapproved"
    ~ dissapear = true
    ~ harmony = -25
    ~ encounterEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
    -> END

=== regret ===
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
    -> love
* [Why such remorse?]
    ~ speakerIndex = 0
    Why such remorse?
    -> remorse

* [Forgive me, but regardless of your reason - with an expired ticket, I cannot let you pass (refuse)]
    ~ speakerIndex = 0
    Forgive me, but regardless of your reason - with an expired ticket, I cannot let you pass
    ~ speakerIndex = 1
    … And yet nothing has changed…
    Men still despise and fear the dreams and wishes of other beings.
    ~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
    ~ decision = "Disapproved"
    ~ dissapear = true
    ~ harmony = -25
    ~ encounterEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
    -> END

=== love ===
~ speakerIndex = 1
Love.
Or rather, my failure to follow it. 
Not chasing after it.
Not daring to reach for it.
I regret that I lacked the courage. The courage to defy the elders. The courage to say “no”.
* [Your husband?]
    ~ speakerIndex = 0
    Your husband?
    -> beloved_man
    
* [Olivier?]
    ~ speakerIndex = 0
    Olivier?
    -> is_dying
    
* [Forgive me, but regardless of your reason - with an expired ticket, I cannot let you pass (refuse)]
    ~ speakerIndex = 0
    Forgive me, but regardless of your reason - with an expired ticket, I cannot let you pass
    ~ speakerIndex = 1
    … And yet nothing has changed…
    Men still despise and fear the dreams and wishes of other beings.
    ~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
    ~ decision = "Disapproved"
    ~ dissapear = true
    ~ harmony = -25
    ~ encounterEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
    -> END
    
=== is_dying ===
~ speakerIndex = 1
My beloved is dying. A mortal man, his time nearly spent - while I have far too much of mine.
Young man, please… have mercy. Allow me to see him, if only this once. Now that I have finally learned what truly matters.
    -> final_decision


=== remorse ===
~ speakerIndex = 1
Had I but possessed courage… had I only dared… 
My life might have taken another course.
And yet… I…
I lacked the voice to speak his name aloud - to tell the world how deeply I loved my Olivier.

* [Your husband?]
    ~ speakerIndex = 0
    Your husband?
    -> beloved_man
* [Olivier?]
    ~ speakerIndex = 0
    Olivier?
    -> olivier
* [Forgive me, but regardless of your reason - with an expired ticket, I cannot let you pass (refuse)]
    ~ speakerIndex = 0
    Forgive me, but regardless of your reason - with an expired ticket, I cannot let you pass
    ~ speakerIndex = 1
    …I should have done it long ago.
    When there was still time.
    But now…
    … Forget it.
    ~ decision = "Disapproved"
    ~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
    ~ dissapear = true
    ~ harmony = -25
    ~ encounterEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
    -> END
    

=== beloved_man ===
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
    … It chooses none of us…
    Love is cruel.
    -> final_decision

* [I wish I had the answers for your questions, milady…]
    ~ speakerIndex = 0
    I wish I had the answers for your questions, milady…
    ~ speakerIndex = 1
     .. It matters not, young man. Some questions are best left unanswered.
    Or wait until we find the courage to face them ourselves.
    My beloved is dying. A mortal man, his time nearly spent - while I have far too much of mine.
    Young man, please… have mercy. Allow me to see him, if only this once. Now that I have finally learned what truly matters.
    -> final_decision
* [I wish I knew the answer to your question, madam. Still… your ticket has expired. You must alight at the next station (refuse)]
    ~ speakerIndex = 0
    I wish I knew the answer to your question, madam. Still… your ticket has expired. You must alight at the next station
    ~ speakerIndex = 1
    …And yet, I was wrong.
    We were never meant for one another. For had we been, fate would have guided us together…
    Would it not..?
    ~ decision = "Disapproved"
    ~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
    ~ harmony = -25
    ~ dissapear = true
    ~ encounterEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
    -> END


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
    ~ speakerIndex = 1
    … It chooses none of us…
    Love is cruel.
    -> final_decision

* [I wish I had the answers for your questions, milady…]
    ~ speakerIndex = 0
    I wish I had the answers for your questions, milady…
    ~ speakerIndex = 1
     .. It matters not, young man. Some questions are best left unanswered.
    Or wait until we find the courage to face them ourselves.
    My beloved is dying. A mortal man, his time nearly spent - while I have far too much of mine.
    Young man, please… have mercy. Allow me to see him, if only this once. Now that I have finally learned what truly matters.
    -> final_decision
* [I wish I knew the answer to your question, madam. Still… your ticket has expired. You must alight at the next station (refuse)]
    ~ speakerIndex = 0
    I wish I knew the answer to your question, madam. Still… your ticket has expired. You must alight at the next station
    ~ speakerIndex = 1
    …And yet, I was wrong.
    We were never meant for one another. For had we been, fate would have guided us together…
    Would it not..?
    ~ decision = "Disapproved"
    ~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
    ~ harmony = -25
    ~ dissapear = true
    ~ encounterEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
    -> END


=== final_decision ===
* [… Very well. Upon mine own responsibility, you may proceed to your destination (accept)]
    ~ speakerIndex = 0
    … Very well. Upon mine own responsibility, you may proceed to your destination
    ~ speakerIndex = 1
    ~ canScan = true
    Thank you, young man. You have my eternal gratitude.
    But remember, never ignore your heart.
    It alone knows the way.
    ~ decision = "Approved"
    ~ passengerAction = "Charlotte personal data was incorrect, but you let her pass."
    ~ harmony = 25
    ~ encounterEntry = "I let her pass. I do not know why. Perhaps because in her eyes I saw something I have long forgotten - hope. She said she was going to see her beloved, one last time. A mortal man, dying. They should never have existed in the same world and yet, there was more life in her voice than in many hearts I’ve met along this endless route. Little did she know that she's already dead herself."
    ~ brokenRule = "Passenger ticket was expired"
    ~ gift = true
    -> END

* [I cannot do that, madam. I must obey the rules (refuse)]
    ~ speakerIndex = 0
    I cannot do that, madam. I must obey the rules
    ~ speakerIndex = 1
    … Rules…
    We all have them.
    We all chase unseen purposes.
    But remember, young man. 
    Never let them silence you. Never let them shape you. Be the master of your own fate.
    Unlike me…
    ~ decision = "Disapproved"
    ~ dissapear = true
    ~ passengerAction = "Charlotte personal data was incorrect and you didn’t let her pass."
    ~ harmony = -25
    ~ encounterEntry = "I did not let her continue. I said, those are the rules. She did not protest. She merely bowed her head and vanished before the next station. I cannot stop thinking about her. That perhaps she only wished to say goodbye. That what I saw as breach of order, was, for her, a final chance. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
    -> END
