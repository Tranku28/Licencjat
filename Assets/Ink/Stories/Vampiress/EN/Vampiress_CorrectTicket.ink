VAR speakerIndex = -1
VAR canScan = false
VAR ticketRejected = false
VAR harmony = 0
VAR passengerAction = ""
VAR decision = ""
VAR brokenRule = ""
VAR generalEntry = ""
VAR encounterEntry = ""

~ speakerIndex = 0
~ generalEntry = "Tonight, a woman boarded the train. Young, at first glance, yet her eyes held something... ancient. I felt that this train had been waiting for her all along This train carries more than passengers."
Good evening, madam… Your ticket, please.

~ speakerIndex = 1
Charlotte Bloodrose.

~ speakerIndex = 0
Good evening, Miss Bloodrose. Your ticket, please.

~ speakerIndex = 1
Here it is. Station Maplewood, one-way ticket. No return.

~ speakerIndex = 0
Understood. And may I ask the purpose of your journey?

~ speakerIndex = 1
I am bound for a meeting… though, in truth, it comes many years too late.

* [Very well, Miss Bloodrose. Have a safe trip (stamp the ticket)] -> accept
* [I'm sorry Miss, I cannot let you pass (refuse)] -> refuse

=== accept ===
Thank you, kind man. I hope everything goes well for you.
    ~ harmony = 25
    ~ decision = "Approved"
    ~ passengerAction = "Charlotte reached the designated destination"
    ~ canScan = true
    ~ passengerAction = "Charlotte reached the designated destination"
    -> END

=== refuse ===
W-what? Why not?

~ speakerIndex = 0
The protocols. Please leave at the nearest station.
    ~ harmony = -25
    ~ decision = "Disapproved"
    ~ passengerAction = "Charlotte didn’t reached the designated destination, despite having a valid ticket"
    ~ ticketRejected = true
    ~ brokenRule = "Unjustified refusal"
    ~ encounterEntry = "I did not let her continue. Perhaps that is why I am here, to decide who arrives in time, and who remains behind."
    ~ passengerAction = "Charlotte didn’t reached the designated destination, despite having a valid ticket."
    -> END
