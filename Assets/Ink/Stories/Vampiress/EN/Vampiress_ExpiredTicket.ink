VAR speakerIndex = -1
VAR canScan = false
VAR ticketRejected = false

~ speakerIndex = 0
Good evening, madam. Your ticket, please.

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

~ speakerIndex = 0
* [A ‘brighter morrow’?]
~ speakerIndex = 1
        -> a_brighter_morrow
        
* [Forgive me, milady, but with an expired ticket, I cannot allow you to proceed with your journey (refuse)]
    ~ speakerIndex = 1
    …Time is but a fragile notion.
    I possess too much of it.
    Others, far too little.
    I shall alight at the next station…
    ~ ticketRejected = true
        -> END
        
=== a_brighter_morrow ===
~ speakerIndex = 1
That ticket was bought when these very rails were still being forged, young man.
Back then, my dreams had yet to be buried beneath dust and silence.

~ speakerIndex = 0
* [Your dreams, milady? Could you be more specific?]
    -> regret

* [Forgive me, milady, but with an expired ticket, I cannot allow you to proceed with your journey (refuse)]
    ~ speakerIndex = 1
    … Perhaps they were right after all.
    One cannot live upon dreams alone.
    Thank you for reminding me of that truth, just as I began to doubt it.
    ~ ticketRejected = true
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

~ speakerIndex = 0
* [What is it that you regret, madam?]
    -> love
* [Why such remorse?]
    -> remorse

* [Forgive me, but regardless of your reason - with an expired ticket, I cannot let you pass (refuse)]
    ~ speakerIndex = 1
    … And yet nothing has changed…
    Men still despise and fear the dreams and wishes of other beings.
    ~ ticketRejected = true
    -> END

=== love ===
~ speakerIndex = 1
Love.
Or rather, my failure to follow it. 
Not chasing after it.
Not daring to reach for it.
I regret that I lacked the courage. The courage to defy the elders. The courage to say “no”.
~ speakerIndex = 0
* [Your husband?]
    -> beloved_man
* [Olivier?]
    -> is_dying
* [Forgive me, but regardless of your reason - with an expired ticket, I cannot let you pass (refuse)]
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
Had I but possessed courage… had I only dared… 
My life might have taken another course.
And yet… I…
I lacked the voice to speak his name aloud - to tell the world how deeply I loved my Olivier.
~ speakerIndex = 0
* [Your husband?]
    -> beloved_man
* [Olivier?]
    -> olivier
* [Forgive me, but regardless of your reason - with an expired ticket, I cannot let you pass (refuse)]
    ~ speakerIndex = 1
    …I should have done it long ago.
    When there was still time.
    But now…
    … Forget it.
    ~ ticketRejected = true
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
* [I wish I knew the answer to your question, madam. Still… your ticket has expired. You must alight at the next station (refuse)]
    ~ speakerIndex = 1
    …And yet, I was wrong.
    We were never meant for one another. For had we been, fate would have guided us together…
    Would it not..?
    ~ ticketRejected = true
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
* [I wish I knew the answer to your question, madam. Still… your ticket has expired. You must alight at the next station (refuse)]
    ~ speakerIndex = 1
    …And yet, I was wrong.
    We were never meant for one another. For had we been, fate would have guided us together…
    Would it not..?
    ~ ticketRejected = true
    -> END


=== final_decision ===
~ speakerIndex = 0
* [… Very well. Upon mine own responsibility, you may proceed to your destination (accept)]
    ~ speakerIndex = 1
    ~ canScan = true
    Thank you, young man. You have my eternal gratitude.
    But remember, never ignore your heart.
    It alone knows the way.
    -> END

* [I cannot do that, madam. I must obey the rules (refuse)]
    ~ speakerIndex = 1
    … Rules…
    We all have them.
    We all chase unseen purposes.
    But remember, young man. 
    Never let them silence you. Never let them shape you. Be the master of your own fate.
    Unlike me…
    ~ ticketRejected = true
    -> END
