# Ink global rules
`speakerIndex` indicates which character is currently speaking
- `0` for the player
- `1` for the NPC

`canScan` indicates whether a ticket can be scaned
- **true** if yes
- **false** if not

`closeDialogue` adjudicates whether player may close dialogue

`harmony` a value to subtract or add to harmony after dialogue
- **int** value to add/subtract

`disappear` whether passenger dissapears after dialogue close
- **true** will dissapear
- **false** will stay in the train to the end of the day

`gift` whether player receive gift from the passenger
- **true** if yes
- **false** if no

`generalEntry` is an entry that appears in diary at the start of the dialogue

`afterEntry` is an entry that appears in diary after dialogue end.

`passengerAction` tells what was the behaviour of passenger after out decision

`decision` whether player decision approves passenger wish od dissaproves it

`allowQuitDialogue` lets player quit the dialogue from ink script. Use it when ticket scan is not required to proceed.
