# Capture the flag system
The main objective is to capture the flags around the map. More captured flags is going to grand the team faster progress of points. Team who reaches score of 100 points wins the game. The more flags captured the faster the gain of points. Each team has a motive to continuously move to different parts of the map capturing or defending flags. `FlagManager.cs` is responsible for controlling the logic for this feature taking care which team owns the flag, and making all of the necessary animations giving the player feedback what team is capturing the flag or what flags are being conquered. 

# AI
AI logic is made with a behavior tree. A behavior tree is construed using code. The basic building blocks consist from `NodeStatus.cs` which tells is an action a **Success** or **Failure**. `SequenceNode.cs` node allow actions to run in a sequence, one node failing will result in the next actions not to run terminating the flow. `SelectorNode.cs` is similar to the sequence node but if a action fails it will skip to the next action. Last construction node is `ParallelNode.cs` which allow actions to run in parallel. Execution is defined via ticks that is controlled by the `AIManager.cs`. For each specific behavior a node is made such as: shooting behavior, searching behavior etc... behavior can take parameters or callbacks making the modular. The final result is having a modular AI, example is in a following code snippet:
```cs
  var combatLogic = new SequenceNode(
	onFailure: () => shootNode.StopFiring(),
	lookAtNode,
	follow,
	new ParallelNode(
		succeedOnAny: true,
		failOnAny: false,
		shootNode,
		new SequenceNode(
			checkAmmoNode,
			reloadNode
		)
	)
);

var root = new SelectorNode(
	new SequenceNode(
		searchNode,
		new SequenceNode(hasTargetNode, combatLogic)
	),
	wanderNode
);
```
The behavior tree try sto find a target first. If it finds one, it enters in combat mode by looking at the target and following it and shooting the target while reloading when needed. If no target is found, the AI switches to wandering between predefined points. The points are flag areas making the AI capture or defend flags.

# Game manager 
Each game has to be set up according to the setting the user has entered in the main menu, e.g. number of teams or number of players per team. `GameManager.cs` based on these values spawns the players, spawns the bots. Assigns each team skins and to which team each player/bot belongs to. It also handles re-spawning of players and bots, and makes sure they are spawned randomly on the map.


# Post processing
To make the game stand out, post processing is applied. Main notes are simple color grading, making the colors pop and bloom making the explosions and particle effects have glow to them giving of more impact.

# Mini map
Another camera is rendering the whole map to a raw image. This raw image is then displayed via UI element achieving a minimap effect. This gives the both players overview about the map, other player and other bots. As well as where are the Flags and which flag is captured by which team. The second camera only renders given layers which are Environment and Mini Map layers. The Mini Map layers is manly responsible for icons that only have to be seen by the minimap and the layer mask is excluded in the main cameras.