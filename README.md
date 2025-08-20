# Pluggable AI & FPS Engine Framework

This project demonstrates a modular AI system using a **Pluggable ScriptableObject-based Finite State Machine (FSM)** in Unity, along with a **Weapon System** and a **2.5D/2.5 FPS Engine Controller**.

Watch the demos here: 

[Showcase 1](https://www.youtube.com/watch?v=Z07Y1964oMY)

[Showcase 2](https://youtu.be/szbcQCaNNjU)

[Showcase 3](https://youtu.be/3CIl0Lq6StM)

## Features

### Pluggable AI FSM

- **ScriptableObject-based actions**: Each AI behavior is encapsulated in a `SM_Action` ScriptableObject.
- **Interface Segregation**: Behaviors implement small, focused interfaces (e.g., `IEnemyChase`, `IEnemyWalk`, `IEnemyAttack`) to allow modularity.
- **Generic behavior management**: The `EnemyController` caches components implementing behavior interfaces and exposes generic `Init<T>()` and `Tick<T>()` methods to avoid boilerplate.
- **Modular & extendable**: Adding new behaviors does not require modifying the controller logic.
- **State Machine integration**: States can call any behavior generically, keeping the FSM clean and decoupled from specific implementations.

```csharp
// Example: Generic action call
stateController.Enemy.InjectTarget<IEnemyChase>();
stateController.Enemy.Tick<IEnemyChase>();
```

### Weapon System

- **Component-based**: Weapons are managed as separate components that can be attached to player or enemy entities.
- **Flexible firing mechanics**: Supports hitscan, projectiles, or melee depending on weapon component.
- **Pluggable**: Weapons can be swapped or extended without modifying the core character controller.**

### 2.5D Engine Controller

- **Custom character controller**: Physics-based movement using raycasts (`RaycastMotor2D` and `PlatformMotor2D`) to handle slopes, collisions, and wall jumps.
- **Smooth movement & jumping**: Acceleration and deceleration differ between air and ground for responsive controls.
- **Camera flexibility**: Works for top-down, 3/4, or first-person-like perspectives.
- **Input decoupling**: `PlayerInput` class handles input, allowing the controller to remain modular and testable.

### Architecture Highlights

- **EnemyController**: Central hub for AI behaviors.
- **Behavior Interfaces**: Minimal, focused interfaces allow clear segregation of responsibilities.
- **Pluggable Actions**: ScriptableObjects define AI actions (`WalkAction`, `ChaseAction`, `AttackAction`, etc.).
- **Generics for simplicity**: Generic methods replace repetitive `InitWalk()`, `TickWalk()`, `InitChase()`, etc., while preserving modularity.

### Enemy Controller Used By The FSM Highlight
```csharp
    // Enemy Controller bla bla bla...
    private Transform playerTransform;
    private Dictionary<Type, object> behaviors = new();

    #region Properties
    public int ScoreValue { get => scoreValue; set => scoreValue = value; }
    public float IdleDuration { get => idleDuration; set => idleDuration = value; }
    public float WalkDuration { get => walkDuration; set => walkDuration = value; }
    public float SearchDuration { get => searchDuration; set => searchDuration = value; }
    #endregion

    private void Awake()
    {
        CacheBehavior<IEnemySearch>();
        CacheBehavior<IEnemyPatrol>();
        CacheBehavior<IEnemyWalk>();
        CacheBehavior<IEnemyChase>();
        CacheBehavior<IEnemyLook>();
        CacheBehavior<IEnemyAttack>();
        InjectTargets();
    }
    
    public void Tick<T>() where T : class, IEnemyTickable
    {
        GetBehavior<T>()?.Tick();
    }

    public bool IsLooking()
    {
        return GetBehavior<IEnemyLook>()?.IsLooking() ?? false;
    }
    
    private void CacheBehavior<T>() where T : class
    {
        var behavior = GetComponent<T>();
        if (behavior != null)
        {
            behaviors[typeof(T)] = behavior;
        }
    }

    private void InjectTargets()
    {
        List<IEnemyTargetable> targetables = GetComponents<IEnemyTargetable>().ToList();

        foreach (var t in targetables)
        {
            t.InjectTarget(playerTransform);
        }
    }
    
    private T GetBehavior<T>() where T : class
    {
        behaviors.TryGetValue(typeof(T), out var behavior);
        return behavior as T;
    }
```
### Example Pluggable FSM Action Usage

```csharp
// Pluggable FSM Action example
[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Chase")]
public class ChaseAction : SM_Action
{
    public override void Initialize(EnemyStateMachine stateController)
    {
        stateController.Enemy.Init<IEnemyChase>();
    }

    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Tick<IEnemyChase>();
    }
}
```
### Benefits

- **Modularity**: Add or remove behaviors without touching core code.
- **Maintainability**: Clear separation of concerns.
- **Scalability**: Easily extendable for new enemy types, weapons, or input systems.
- **Clean architecture**: Avoids boilerplate while keeping the FSM decoupled from specific implementations.
  
### Future Improvements

- Network synchronization for AI and player.
- Expand weapon system with status effects and ammo management.
- Add more pluggable AI states like `Flee`, `Patrol`, `Search`, etc.
- Maybe more parkour and maps? hehe.

### Unity Version

- Updated to `2021.3.16f1`.
