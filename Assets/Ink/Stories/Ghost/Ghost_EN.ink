VAR speakerIndex = -1
VAR canScan = false
VAR ticketRejected = false
VAR forceScan = false
VAR harmony = 0
VAR passengerAction = ""
VAR decision = ""
VAR brokenRule = ""
VAR generalEntry = ""
VAR encounterEntry = ""

~ speakerIndex = 0
Your ticket, please.
~ generalEntry = "Today I met a doctor, who was late for his own solution. He said that the time wasn’t the one who let him down."

~ speakerIndex = 1
Of course.
I have always kept my papers in order.
As you see, even after death.

~ speakerIndex = 0
Your destination?

~ speakerIndex = 1
Anywhere but the Afterlife.
At least not yet.
I cannot go there now.


* [Why do you refuse to cross over?]
    ~ speakerIndex = 0
    Why do you refuse to cross over?
    -> unfinished_matters
* [Your ticket is valid. You have the right to continue] -> a_right_to_continue


=== a_right_to_continue ===

~ speakerIndex = 1
You do not yet grasp the gravity of your mistake.
Generations will remember this decision.
~ ticketRejected = true
~ encounterEntry = "I reported him. He broke the rules. He should have known better. He wanted to fix the world. And I wanted to keep everything in order."
~ passengerAction = "The Ghost reached the designated destination."
~ decision = "Approved"
~ harmony = -25
    -> END


=== unfinished_matters ===

~ speakerIndex = 1
I have…
Unfinished matters. You must understand.

~ speakerIndex = 0
Unfinished matters?

~ speakerIndex = 1
I was a doctor once.
In a small border town.
No one remembers its name now..
Perhaps that is for the best.

~ speakerIndex = 0
What happened there?

~ speakerIndex = 1
An epidemic,
At first, a handful of cases. Then entire households.
I ordered the gates sealed…
no one in, no one out.
I believed I was protecting the world…
Yet I condemned the town to death.

~ speakerIndex = 0
* [You could not cure them?] -> hopelessness
* [I am sorry, but there is nothing more you can do now. Your time has come.] -> my_time_has_come


=== my_time_has_come ===

~ speakerIndex = 1
…
Perhaps you are right.
Still, I hoped I might atone for my mistake.
If only I had more time…
~ ticketRejected = true
~ encounterEntry = "I reported him. He broke the rules. He should have known better. He wanted to fix the world. And I wanted to keep everything in order."
~ passengerAction = "The Ghost reached the designated destination."
~ decision = "Approved"
~ harmony = -25
    -> END

=== hopelessness ===
~ speakerIndex = 1
I tried everything.
Every remedy. Every mixture, every method recorded in old tomes.
Nothing worked. People died before my eyes, faster each day.
And then…
I fell ill myself.

~ speakerIndex = 0
So this is how your story ends?

~ speakerIndex = 1
No. Not at once.
When I was certain the end had come… I looked upon my notes
once more.
And then I saw it.
A flaw.
One. Small. Detail.
The proportions… if only they had been altared…
…
But the answer came too late.

~ speakerIndex = 0
* [Why do you wish to remain on this train?] -> a_purpose
* [I am sorry to hear this, but I must let you pass. These are the rules.] -> the_rules



=== the_rules ===
~ speakerIndex = 1
…
Rules are like diagnoses.
Not always fair, but always unavoidable.
~ encounterEntry = "I reported him. He broke the rules. He should have known better. He wanted to fix the world. And I wanted to keep everything in order."
~ ticketRejected = true
~ passengerAction = "The Ghost reached the designated destination."
~ decision = "Approved"
~ harmony = -25
    -> END



=== a_purpose ===
~ speakerIndex = 1
The Afterlife is silent.
There, I will help no one.
And I still wish to make amends.
If I cross over, that chance is lost forever.
Let me stay.
This train… it remembers, does it not?
My notes will find their place.
Please, allow me to remain.

~ speakerIndex = 0
* […This train will wait for you a little longer.] -> train_will_wait
* [You have already surrendered. One lives but once. And you have made your choice.] -> rest_in_peace



=== rest_in_peace ===
~ speakerIndex = 1
…
I understand.
I always knew that not every mistake can be undone.
Yet every choice remains with us forever…
Look after the Harmony.
But never forget. It, too, can be wrong.
~ ticketRejected = true
~ encounterEntry = "I reported him. He broke the rules. He should have known better. He wanted to fix the world. And I wanted to keep everything in order."
~ passengerAction = "The Ghost reached the designated destination."
~ decision = "Approved"
~ harmony = -25
~ canScan = true
    -> END


=== train_will_wait ===
~ speakerIndex = 1
~ forceScan = true
~ canScan = false
~ encounterEntry = "I broke the rules. I should have reported this incident. But I didn't. Why? Was it for him? Because I pity him? Or was it because I hoped that him breaking the rules would bring peace to others who are still alive?"
~ passengerAction = "The Ghost didn’t reached the designated destination, despite having a valid ticket"
~ brokenRule = "Unjustified Refusal"
~ decision = "You let the passenger to stay"
~ harmony = 25
Then perhaps someone else will not repeat my mistake.
Thank you.
    -> END
