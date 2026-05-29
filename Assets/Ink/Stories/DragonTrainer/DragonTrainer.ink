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

~ addEntry = "Tonight, a man boarded the train.\\nHe carried himself like someone who once commanded respect... and lost it.\\nHis hands were steady, yet there was something in his gaze. Not fear, not quite regret.\\nSomething heavier.\\nWhen I took his ticket, he did not hesitate. Yet when I asked about his purpose, his voice changed.\\nHe said he was looking for someone. Not a person. He told me everything. About the dragons. About the war. About the choice he made.\\n"
~ addEntry = "He spoke of them not as beasts, but as something... closer.\\nAs if, somewhere along the way, the line between master and companion had shattered.\\n\\nAnd yet, I could still see it.\\nThe weight of all the creatures he had sent to their deaths before that final act.\\n\\nHe opened the gates.\\nToo late for many.\\n\\nI wonder... Does one act of defiance erase a lifetime of obedience?\\nOr does it only make the silence louder?"

~ speakerIndex = 0
Ticket, please.

~ speakerIndex = 1
...
Of course.
I suppose even here, some rules still remain.

~ speakerIndex = 0
Name?

~ speakerIndex = 1
They used to call me a royal dragon trainer.

~ speakerIndex = 0
Real name

~ speakerIndex = 1
...
Kaelen Ashenwing.

~ speakerIndex = 0
Purpose of your journey?

~ speakerIndex = 1
To find him.
To make sure he is safe.

* [‘Him’? Who are you talking about?]
    ~ speakerIndex = 0
    ‘Him’? Who are you talking about?
    -> 1_1
* [You said you were a ‘royal dragon trainer’?]
    ~ speakerIndex = 0
    You said you were a ‘royal dragon trainer’?
    -> 1_2

=== 1_1 ===
~ speakerIndex = 1
A dragon.
Not a weapon. Not a beast.
Just... a friend.

    * [Your story doesn’t match. They weren’t friends. None of them. You trained them in harsh conditions, only to send them to die in the war.]
    ~ speakerIndex = 0
    Your story doesn’t match. They weren’t friends. None of them. You trained them in harsh conditions, only to send them to die in the war.
        -> 2_1
    * [Your ticket is valid. You can pass now. And good luck in finding your ‘friend’. (scan the ticket)]
    ~ speakerIndex = 0
    Your ticket is valid. You can pass now. And good luck in finding your ‘friend’.
        -> 2_2
    * [Your story doesn’t match. I do not see any remorse in your eyes.]
    ~ speakerIndex = 0
    Your story doesn’t match. I do not see any remorse in your eyes.
        -> 2_3

=== 1_2 ===
~ speakerIndex = 1
Yes...
I trained dragons. For the Kingdom.
They were... our friends.

    * [Your story doesn’t match. They weren’t friends. None of them. You trained them in harsh conditions, only to send them to die in the war.]
    ~ speakerIndex = 0
    Your story doesn’t match. They weren’t friends. None of them. You trained them in harsh conditions, only to send them to die in the war.
        -> 2_1
    * [Your ticket is valid. You can pass now. And good luck in finding your ‘friend’. (let him pass)]
    ~ speakerIndex = 0
    Your ticket is valid. You can pass now. And good luck in finding your ‘friend’
        -> 2_2
    * [Your story doesn’t match. I do not see any remorse in your eyes.]
    ~ speakerIndex = 0
    Your story doesn’t match. I do not see any remorse in your eyes.
        -> 2_3
    
=== 2_1 ===
~ speakerIndex = 1
You don't understand. I’ve never wanted this.
I raised him from an egg.
He wasn’t meant for war.. none of them were.
But he listened to me. He trusted me.
Probably more than he ever should have.

    * [What happened?]
        ~ speakerIndex = 0
        What happened?
        -> 3_1
    * [I’m sorry. But I cannot let you pass (refuse).]
        ~ speakerIndex = 0
        I’m sorry. But I cannot let you pass.
        ~ gift = false
        ~ allowQuitDialogue = true
        ~ harmony = -25
        ~ dissapear = true
        ~ decision = "Disapproved"
        ~ passengerAction = "Dragon Trainer was refused to pass"
        -> END

=== 2_2 ===
~ speakerIndex = 1
...
So that’s how it ends? No questions? Nothing?
After everything that had happened? I thought-...
Nevermind.
~ addEntry = "I let him pass.\\nThere was something in his voice when he spoke of the dragon. Not pride. Not even redemption.\\nHope.\\nA fragile, desperate hope that somewhere beyond this journey, something he once set free still lives."
~ passengerAction = "Dragon Trainer was allowed to pass, but he still wanted to tell something."
~ canScan = true
~ gift = true
~ harmony = -25
~ decision = "Approved"
    -> END

=== 2_3 ===
~ speakerIndex = 1
I lied. Yes.
But there’s more story to be told.

    * [What happened?]
        ~ speakerIndex = 0
        What happened?
        -> 3_1
    * [I’m sorry. But I cannot let you pass (refuse).]
        ~ speakerIndex = 0
        I’m sorry. But I cannot let you pass.
        ~ gift = false
        ~ allowQuitDialogue = true
        ~ harmony = -25
        ~ dissapear = true
        ~ addEntry = "I did not let him continue.\\n\\nHis story was filled with sorrow, yes. But also with choices. Too many of them made too late.\\n\\nHe did not argue. He only nodded, as if he had been expecting this all along."
        ~ addEntry = "Before he vanished, he said something I cannot forget. That sometimes duty is nothing morethan fear wearing a uniform.\\n\\nSince then, I find myself hesitating more often. Because I begin to wonder...\\nHow many of my decisions are truly mine."
        ~ passengerAction = "Dragon Trainer was refused to pass"
        ~ decision = "Disapproved"
        -> END



=== 3_1 ===
~ speakerIndex = 1
There was one...
One that was different from the rest..
The others obeyed. And that one... that one chose to stay.
And the King. He heard about it.
And Kings have a habit of turning wonders into weapons.
He ordered all of the dragons to the war. And I knew what that meant.

    * [Dragons don’t return from battle.]
        ~ speakerIndex = 0
        Dragons don’t return from battle.
        -> 4_1
    * [Your ticket is valid. You can go now. Nothing is keeping you here anymore. (scan the ticket)]
        ~ speakerIndex = 0
        Your ticket is valid. You can go now. Nothing is keeping you here anymore.
        -> 4_2

=== 3_2 ===
Everything is keeping me here.
...
But if this is it...
~ speakerIndex = 1
~ canScan = true
~ gift = true
~ harmony = -25
~ passengerAction = "Dragon Trainer was allowed to pass, but he still wanted to tell something."
~ addEntry = "I let him pass.\\nThere was something in his voice when he spoke of the dragon. Not pride. Not even redemption.\\nHope.\\nA fragile, desperate hope that somewhere beyond this journey, something he once set free still lives."
~ decision = "Approved"
    -> END


=== 4_1 ===
~ speakerIndex = 1
No.
They burn... and are forgotten.

~ speakerIndex = 0
So what did you do?

~ speakerIndex = 1
I opened the gates.
Just like that. I opened the gates. Without any hesitation. I knew what I was supposed to do.
I choosed them. Over everything. Over the crown. Over my own life.

    * [You set them free. And you paid for it.]
        ~ speakerIndex = 0
        You set them free. And you paid for it.
            -> 5_1
            
    * [... I can hear the remorse in your voice. You may pass now. Try to find your ‘friend’. (let him pass)]
        ~ speakerIndex = 0
    ... I can hear the remorse in your voice. You may pass now. Try to find your ‘friend’.
            -> 5_2

=== 4_2 ===
~ speakerIndex = 1
Everything is keeping me here.
...
But if this is it...
~ addEntry = "I let him pass.\\nThere was something in his voice when he spoke of the dragon. Not pride. Not even redemption.\\nHope.\\nA fragile, desperate hope that somewhere beyond this journey, something he once set free still lives."
~ passengerAction = "Dragon Trainer was allowed to pass, but he still wanted to tell something."
~ canScan = true
~ gift = true
~ harmony = -25
~ decision = "Approved"
    -> END
    
=== 5_1 ===
~ speakerIndex = 1
I did.
Publicly. They needed to make an example.

    * [Do you regret it?]
        ~ speakerIndex = 0
        Do you regret it?
            -> 6_1
    * [... I can hear the remorse in your voice. You may pass now. Try to find your ‘friend’. (let him pass)]
        ~ speakerIndex = 0
        ... I can hear the remorse in your voice. You may pass now. Try to find your ‘friend’.
            -> 6_2


=== 5_2 ===
~ speakerIndex = 1
I... I regret it.
I regret not seeing it sooner. I regret all the pain I caused them...
~ addEntry = "I let him pass.\\nThere was something in his voice when he spoke of the dragon. Not pride. Not even redemption.\\nHope.\\nA fragile, desperate hope that somewhere beyond this journey, something he once set free still lives."
~ passengerAction = "Dragon Trainer was allowed to pass, but he still wanted to tell something."
~ canScan = true
~ harmony = -25
~ gift = true
~ decision = "Approved"
    -> END
    
    
=== 6_1 ===
~ speakerIndex = 1
Regret?
No. Even now, I would do it again.

    * [Your ticket is in order. And you do look like someone who regrets their decisions. You may proceed. (let him pass)]
    ~ speakerIndex = 0
    Your ticket is in order. And you do look like someone who regrets their decisions. You may proceed.
        -> 7_1
        
    * [All your life, you ordered dragons to death. You treated them horribly. I don’t feel that you changed. (refuse)]
    ~ speakerIndex = 0
    All your life, you ordered dragons to death. You treated them horribly. I don’t feel that you changed.
        -> 7_2

=== 6_2 ===
~ speakerIndex = 1
Thank you...
For seeing the real me.
~ addEntry = "I let him pass.\\nThere was something in his voice when he spoke of the dragon. Not pride. Not even redemption.\\nHope.\\nA fragile, desperate hope that somewhere beyond this journey, something he once set free still lives."
~ passengerAction = "Dragon Trainer was let through after his confession"
~ canScan = true
~ harmony = 25
~ gift = true
~ decision = "Approved"
    -> END


=== 7_1 ===
~ speakerIndex = 1
If there is something beyond this..
I hope it is a sky wide enough for every dragon.
Thank you.
~ addEntry = "I let him pass.\\nThere was something in his voice when he spoke of the dragon. Not pride. Not even redemption.\\nHope.\\nA fragile, desperate hope that somewhere beyond this journey, something he once set free still lives."
~ passengerAction = "Dragon Trainer was let through after his confession"
~ gift = true
~ harmony = 25
~ canScan = true
~ decision = "Approved"
    -> END


=== 7_2 ===
~ speakerIndex = 1
...
I see.
I spent my life deciding the fate of the creatures who trusted me.
I guess it is only fair for you to decide my fate.
...
Just remember, Conductor. Sometimes what we call duty, is nothing more than fear wearing
a uniform.
~ addEntry = "I did not let him continue.\\n\\nHis story was filled with sorrow, yes. But also with choices. Too many of them made too late.\\n\\nHe did not argue. He only nodded, as if he had been expecting this all along."
~ addEntry = "Before he vanished, he said something I cannot forget. That sometimes duty is nothing morethan fear wearing a uniform.\\n\\nSince then, I find myself hesitating more often. Because I begin to wonder...\\nHow many of my decisions are truly mine."
~ passengerAction = "Dragon Trainer was refused to pass"
~ harmony = -25
~ allowQuitDialogue = true
~ dissapear = true
~ decision = "Disapproved"
    -> END