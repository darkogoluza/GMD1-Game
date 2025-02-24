# Introduction

The first blog is to show my process of following the official unities tutorial about creating a "Roll a Ball" game. The tutorial was straight forward and easy to follow, and has videos in case if you get stock in one of the steps. The tutorial is coverings basics about Unities new input system, how to structure your project, and how to make simple behaviors whit scripting. Also there is a small focus on unties editor navigation, and how to set up simple physis with ridgidbody component and collider components. 

# Process

First I have followed instructions for setting up the project, that is creating a new project giving it a name and using the correct rendering pipeline. After that I have continued to follow the steps of creating the player and making a simple movement script. I have had to install a package for the unities new Input system, which I remember it was not a thing in the past for unity. The new system already had configured (WASD) keys for movement so it was easy to integrate with the script. 

After that the tutorial wanted us to make a script that makes the camera follow the player. This logic was placed in a `LateUpdate` method that is fired after the normal `Update` method tickrate. This is to ensure that we do not get any jittering, we want to update the cameras position after the players position has been updated. 

Then I have created some objects that can be pickup and they give you points. The pickup objects are also spinning and they are a bright yellow color to draw the players attention, and to tell the player that they can be interacted with. The logic for spinning looks like this

```C#
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
```

Key thing to point out is that we are multiplying the spin direction with `Time.deltaTime` so that the spin speed will not depend on the framerate, but instead stay constant no matter what framerate we have.

Every game has to have some sort of a challenge so the next step was to introduce an enemy that is following you around and when it touches you, you lose the game. To add even more of a challenge the tutorial explained how to create obstacles, so you can hide around them when running away from the enemy, making the GamePlay more interesting. 

The AI logic was made using the unities AI navigation system which had to be added as an extra package. There we can define all of the walkable area for the enemy. We give the enemy a navigation agent which we can tell to always follow the player.  

# Extra feature
To have little bit fun and to experiment, I have added an extra feature where the player can make a jump. This was achieved by adding a new input action, that is listening for the "space" key. And then applying a force in up direction to the ball when the key is pressed. 

# Conclusion 

The experience was a great refresher to Unity, and you can take the tutorial no mater what experience level you are at.

[Click here](https://github.com/darkogoluza/GMD-Roll-A-Ball-tutorial) to go to the game.