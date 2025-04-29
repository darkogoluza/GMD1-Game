# Weapons
There are three weapons implemented, ak-74, pistol and a shotgun. Each of them have different stats. Each weapon have accuracy, bullets in clip and total ammunition to reload. Since there are multiple weapons, I needed a way to connect each weapon with the player, an interface came in handy. I created a `IWeapon` interface that has all of the necessary methods to tell the weapon implementation what to do. Here is an example:

``` c#
public interface IWeapon
{
    public void StartFire();
    public void EndFire();
    public void Reload();
    public void SetPlayer(bool isPlayerOne); // is it the player one or player two, important for events
    public void SetTargetMasks(LayerMask[] targetMasks); // What layer masks the bullets should target
    public void ReplenishAmmo();
}
```

# Bullets
The bullets are a physical game object, but as well they use raycasting to ensure every bullet hits the target.

![alt text](./GMD%20bullet%20example.png)

The two key components are checking for collision and moving. So the Update function looks clean.

```c#
void FixedUpdate()
{
	CheckForCollision();
	Move();
}
```

# Health
Each entity that can have health and take damage must have the `IDamageable` interface.

``` c#
public interface IDamageable
{
    void TakeDamage(float damage);
}
```

In the bullet implementation I am checking if the collided game object has the interface and then apply the damage. The players implementation of health supports health regeneration. After a given amount of the player not taking damage, the health will start to generate.

# Ammo
Each weapon has its on ammo amount, and to replenish this ammo the player must pick up an ammo pick up that replenishes the ammo to the full amount again. The player can reload as long there is still ammo left. When the player is reloading they are unable to shot until the reload is finished.

# UI
Each player has the same identical UI that shows, current health, current ammo, is the player in process of reloading and damage indicator. The UI is decuple from the player logic, and they communicate via events. Example of the global event manager:

``` c#
public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance; // Singleton pattern

    private void Awake()
    {
        Instance = this;
    }

    public event Action<int, int, bool> onAmmoChange; // Used to subscribe to the event

	// ...

    public void AmmoChange(int newAmmo, int maxAmmo, bool isPlayerOne) // Used to fire the event
    {
        if (onAmmoChange != null)
            onAmmoChange(newAmmo, maxAmmo, isPlayerOne);
    }

	// ...

```

On each change of ammo, health etc, the player is calling the events and the UI is being updated. When the health changes the UI is displaying a red ring over the players screen indicating the health has been reduced. The indicator depends on the damage amount. As soon as the player starts regenerating the health the indication goes away. This is purely done in the UI layer.