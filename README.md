# DiabloSimulator
A text-based WPF (C#) program with lots of RNG that simulates the "kill monsters, get loot" core loop of Diablo. The player will be able to perform the following actions:

* Explore areas in the world of Sanctuary, both overworld and dungeons. Colorful descriptions will be used to convey the world around them.
* Encounter NPCs, monsters, and other events. Encounters are randomly generated, monster power can scale with the player's level.
* Fight monsters. In the interest of conserving scope, there is no plan to incorporate custom class skills; combat will consist only of attacks with the currently equipped weapon, and defending, which has a chance to prevent the monster from dealing any damage and regenerating some health.
* Acquire items of varying rarities with randomly generated attributes. The player character has numerous equipment slots, all of which can host items that benefit their various stats and attributes.
* Sell items, heal, and gamble in town. The player has a fixed number of items they can carry, health regeneration is limited, as are potions, so returning to town will be a necessity.

## Visual Preview
Below is a GIF that shows the current features of the project:

![Diablo Simulator Game Screen Mockup](/Images/DiabloSimulatorPreview.gif)

## Class Diagram
The elements and interactions of the game (i.e., the model, separate from the view) will be something like this. This is already somewhat divorced from the model currently used in the game, but should give you a rough idea of the scope and number of moving parts.

![Diablo Simulator Class Diagram](/Images/Class_Diagram.png)
