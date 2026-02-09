VAR speakerIndex = -1
VAR canScan = false
VAR ticketRejected = false

~ speakerIndex = 0
Your ticket, please.

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

~ speakerIndex = 0
* [Why do you refuse to cross over?] -> unfinished_matters
* [Your ticket is valid. You have the right to continue] -> a_right_to_continue


=== a_right_to_continue ===
You do not yet grasp the gravity of your mistake.
Generations will remember this decision.
~ ticketRejected = true
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
~ ticketRejected = true
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
    -> END


=== train_will_wait ===
~ speakerIndex = 1
~ canScan = true
Then perhaps someone else will not repeat my mistake.
Thank you.
    -> END
