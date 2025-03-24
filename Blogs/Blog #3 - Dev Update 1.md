# Main menu
From the main menu, you can select how many teams you want to play with and how many players per team. You can also quit the game from the main menu.

# Skins
I am storing different skin sprites inside `ScriptableObject`'s. I have created a `PlayerSkinManager.cs` that has references to all of the sprite renders of the player and changes the sprits according to the `ScriptableObject`. Now I can have different skins stored inside `ScriptableObject`'s and load them at will.

# Split screen
I am using two different cameras and changing the viewport rect so both of them are rendered on the screen at the same time.

# Movement and camera
The input is handled by new new unity input system and I am using the input package for the arcade machine provided by the teacher. I am reading the input in the `PlayerController.cs` script and then applying that input on a `Rigidbody2D` inside a `FixedUpdate`. I am changing the velocity of the `Rigidbody2D` depending on the movement input vector. The camera movement/tracking is achieved with unity `Cinemachine`.

# Audio manager
I am using an audio manager from Brackey (referenced in the game design document). The audio manager is a singleton and I can call any sound from any script. The audio manager is persisting between scenes changes, so the music will not get cut off.