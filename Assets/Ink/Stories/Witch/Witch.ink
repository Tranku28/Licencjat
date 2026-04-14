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
Ticket, please.
~ generalEntry = "Delliah. For years, she healed with the trust of others more than with herbs. She knew the names of plants, but she didn't know the limits of her own knowledge. When the real illness came, she was left all alone, with empty hands and overly proud tongue."

~ speakerIndex = 1
Of course, dear.
I always keep what I borrow close to my heart.

~ speakerIndex = 0
This ticket does not belong to you.
The name, the time. Your life record does not match with it.

~ speakerIndex = 1
Names change.
Lives blur.
What matters is where I’m going... And that I deserve to get there.

*[You stole this ticket]
    ~ speakerIndex = 0
    You stole this ticket.
    -> option1_1
    
*[Your Life Card marks you as a village herbalist. Is that who you truly were?]
    ~ speakerIndex = 0
    Your Life Card marks you as a village herbalist. Is that who you truly were?
    -> option1_2
    
*[I’m sorry. But it’s a theft. I cannot let you pass. Please leave the train. (refuse)]
    ~ speakerIndex = 0
    I’m sorry. But it’s a theft. I cannot let you pass. Please leave the train.
    -> option1_3

=== option1_1 ===
~ speakerIndex = 1
Borrowed.
From someone who didn’t deserved it the way I do.

~ speakerIndex = 0
Your soul hasn’t paid for what it did.
You should not be here.

~ speakerIndex = 1
And yet.. Here I am.
Clever enough to find my way even now, between the worlds.

*[Your Life Card marks you as a village herbalist. Is that who you truly were?]
    ~ speakerIndex = 0
    Your Life Card marks you as a village herbalist. Is that who you truly were?
        -> option1_1
*[I’m sorry. But it’s a theft. I cannot let you pass. Please leave the train. (refuse)]
    ~ speakerIndex = 0
    I’m sorry. But it’s a theft. I cannot let you pass. Please leave the train.
        -> option1_3

=== option1_2 ===
~ speakerIndex = 1
For many years I was one.
I knew herbs, roots and even old recipes whispered from mouth to mouth.
They came to me whenever they were sick, frightened... desperate.

~ speakerIndex = 0
Your remedies didn’t always work.

~ speakerIndex = 1
No remedy always works.
People want miracles. I gave them hope.
Sometimes that had to be enough.

~ speakerIndex = 0
Until it wasn’t.

~ speakerIndex = 1
Until the sickness settled.
Until they started dying.
And suddenly...
Hope turned into blame.

~ speakerIndex = 0
The village accused you of lying.

~ speakerIndex = 1
They called me a fraud.
Said I pretended to be wiser than I was.
They forgot every night I stayed awake mixing salve.
Every prayer whispered over boiling water.

~ speakerIndex = 0
They forced you to leave.

~ speakerIndex = 1
Yes.
And before I did..
I made sure they would never forget me.

~ speakerIndex = 0
The water source.

~ speakerIndex = 1
The only one they had.
Poison is just another kind of medicine.
Just used with different intent.

*[You used them. For your own pleasure]
    ~ speakerIndex = 0
    You used them. For your own pleasure
        -> option2_1
*[Your soul hasn’t paid for what you did. And I do believe that since you are here, you do know the rules. You must leave the train. (refuse)]
    ~ speakerIndex = 0
    Your soul hasn’t paid for what you did. And I do believe that since you are here, you do know the rules. You must leave the train.
        -> option2_2


=== option1_3 ===
Tsk...
So everyone still believes in purity...
Remember this, Conductor.
The righteous die just as easily as liars.
The difference is that liars are better prepared.
~ harmony = 25
~ dissapear = true
~ decision = "Disapproved"
~ passengerAction = "Deliah was ordered to leave the train due to invalid ticket"
~ allowQuitDialogue = true
    -> END
    

=== option2_1 ===
~ speakerIndex = 1
I used them just as much as they used me.
They wanted miracles.
I wanted attention.

~ speakerIndex = 0
And when you didn’t got what you wanted?

~ speakerIndex = 1
They didn’t either.
If I couldn’t have what I wanted, why would they get anything?

*[Why should I let you stay here? Get the redemption that many seek?]
    ~ speakerIndex = 0
    Why should I let you stay here? Get the redemption that many seek?
        -> option3_1
*[You know the rules. I am the one who decides who continues the journey. And yours ends here. (refuse)]
    ~ speakerIndex = 0
    You know the rules. I am the one who decides who continues the journey. And yours ends here.
        -> option3_2
    

=== option2_2 ===
~ speakerIndex = 1
You think sending me away restores balance?
Balance is already broken.
You are just choosing which cracks you prefer not to see.
~ harmony = 25
~ dissapear = true
~ decision = "Disapproved"
~ passengerAction = "Deliah was ordered to leave the train due to invalid ticket"
~ allowQuitDialogue = true
    -> END
    
    
=== option3_1 ===
~ speakerIndex = 1
Because we are similar, aren’t we?
You also want to be acknowledged.
You want to matter.
Just like i do.
So let me pass, and I will help you keep everything neat here.

*[... I’ll allow it. (let her travel)]
    ~ speakerIndex = 0
    ... I’ll allow it.
        -> option4_1
*[We are not the same. Your journey ends here. (refuse)]
    ~ speakerIndex = 1
    We are not the same. Your journey ends here.
        -> option1_3


=== option3_2 ===
~ speakerIndex = 0
Journeys don't end when you say they do.
They just stop bothering you.
~ harmony = 25
~ dissapear = true
~ decision = "Disapproved"
~ passengerAction = "Deliah was ordered to leave the train due to invalid ticket"
~ allowQuitDialogue = true
    -> END
    
=== option4_1 ===
I knew you would understand.
Rules bend for those who know where to press.
I know my ways. I always knew them.
That’s why, expect a gift from me.
A little charm, for you, to bend more rules.
~ canScan = true
~ harmony = -25
~ encounterEntry = "I let her stay. She spoke of knowledge as if it were something owned, not earned. Her hands knew poisons as well as cures, yet she called both wisdom. She believed herself clever until the very end. Perhaps that belief is what kept her standing when everyone else fell. The Train feels heavier tonight."
~ brokenRule = "Passenger data does not match with the ticket"
~ decision = "Approved"
~ passengerAction = "Deliah continued her journey despite an invalid ticket"
~ gift = true
    -> END
