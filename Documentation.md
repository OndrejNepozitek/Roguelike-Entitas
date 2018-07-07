# Documentation

## Table of contents

- [Introduction](#introduction)
- [How to play](#how-to-play)
- [Game features](#game-features)
- [Multiplayer](#multiplayer)
- [Challenges](#challenges)

# Introduction
The goal of this work is to create a 2D topdown rogue-lite game with both singleplayer and multiplayer experience. Main features include, among others, procedurally generated content, challenging combat, equipment and items to upgrade your character. Some features are not yet implemented as the current state is something like **technical beta preview**.


### Used terms
- [**Entity-Component-System (ECS)**](https://en.wikipedia.org/wiki/Entity%E2%80%93component%E2%80%93system) - An architectural pattern that is mostly used in game development. It prefers **composition** over inheritance.
- **LLAPI** - Low level api for network communication in Unity

### Used technologies
 - [**Unity**](https://unity3d.com/) - a platform used for developing 2D and 3D games
 - [**Entitas**](https://github.com/sschmid/Entitas-CSharp) - ECS framework for Unity and C#
 - [**protobuf-net**](https://github.com/mgravell/protobuf-net) - a library implementing protocol buffer by Google

### Code structure
All code is located in the *Assets* folder. It is divided into following folders:

- *Assets/Scripts* - Scripts that are directly attached to Unity's GameObjects. Most of them use Unity's hooks like *Update*, etc.. They do not directly change game's state but rather help with managing UIs, network communacation, etc..
- *Assets/Scripts/**GameController*** - The main entry point of the game. All systems (see below) are registered here and are then executed in Unity's *Update* hook.
- *Assets/Sources/Extensions* - Classes with useful extension methods.
- *Assets/Sources/Features* - All *ECS* systems and components. They are grouped by specific game features. Majority of game's logic happens here.
- *Assets/Sources/Generated* - *Entitas* uses code generation to generate code based on game's configuration and registered components.
- *Assets/Sources/Helpers* - Code that should make implementing game's logic easier.

### ECS and Entitas
The game is built on *Entitas* framework. Games running *Entitas* consist of 3 main building blocks:

#### Entities
An entity is something that exists in the game world. It usually has one or more components attached to it.

> EXAMPLE: Entities are for example players, monsters, chests with loot, items on the ground, map tiles, etc..

#### Components
Each component describes a certain aspect of an entity and its parameters. They should contain only raw data. 

> EXAMPLE:  Everything that is rendered on the game map must have a position component. It simply consists of a position vector and a boolean flag which tells whether the entity should move smoothly when the position is changed.

#### Systems
Systems contain all the game logic. They change components of entities and create new entities. There are currently 5 types of systems in *Entitas*:

- **Initialize systems** - Run once when the game is started.
- **Execute systems** - Run in every update cycle.
- **Reactive systems** - *Reactive systems* are just a special case of *Execute systems*. They watch entities for changes in specified components and run only when there is at least on such entity.
- **Cleanup systems** - Run in every update cycle after all *execute* and *reactive* systems are run.
- **Teardown systems** - Run usually when the game ends.

> EXAMPLE: *SetPositionSystem* is a *reactive systems* that handles entities with changed position. It checks if the entity should move smoothly and communicates with Unity to change the actual position of Unity's transform component.

> EXAMPLE: *ShouldDieSystem* is a *cleanup system*. It waits for all *execute systems* to change entities and then collects all entities with "DeadComponent" and removes them from the game.



# How to play

### Controls
- **Movement** - *wasd* or *arrows* to move around (in a grid)
- **Combat** - bump to an enemy (same as the basic movement) next to you to make an attack
- **Chests** - open chests by bumping to them
- **Inventory** - press *"I"* to access your inventory
- **Items** - equip an item by pressing *"E"* when standing on a tile with the item

### Goal
There is currently **no goal** in this stage of development. The purpose of this version is to showcase what the game is capable to do.


# Game features

> NOTE: The game operates on a simple 2D grid so I found it unnecessary to use most of Unity's functions like lights, collisions, etc.. Majority of features are tailored specifically for this game's needs and Unity could be quite easily replaced with a different game engine. Unity still saved me a lot of time with things like assets handling, sprites, etc..

> NOTE: Features were implemented with modularity and configurability in mind. The majority of features are independent on other features and can be therefore easily enabled or disabled.

### Stats
Each player has following stats:
- **Maximum health** - maximum health of the player
- **Damage** - base damage of all attacks
- **Defense** - more defense means less damage taken from attacks
- **Movement speed** - speed of movement
- **Critical chance** - chance that attacks deal more damage

> NOTE: All monsters have stats, too. They have very similar meaning to player's stats but are not visible anywhere.

### Items
Items modify player's base stats. There are currently 4 types of equipment - **helmets**, **body armors**, **weapons** and **shields**. Effects of each item can be seen in the *inventory* after equiping the item.

> NOTE: Items like *potions* are coming in future versions.

### Looting
There are currently 2 ways to obtain items - either by *killing monsters* or by *looting chests*. Both monsters and chests are assigned a *loot group*. Loot groups are collections of items where every item has its own chance to be dropped. It is therefore very easy to make harder enemies drop better items and make fights more rewarding.

### Monsters TODO
There is currently only one type of monster - TwoFace. It has two phases - one where it does not move at all and one where he chases the closests player and tries to attack him. The two phases change in a periodic manner.

> NOTE: All monsters can use Pathfinding which is implemented by an A* algorithm.

### Combat
All attacks are currently made as *bump to attack* which means that a player has to bump into an enemy to make the attack. The base damage is taken from the stats of the attacking entity.  Critical chance is then used to determine whether the attack is a critical hit which means that it deals 1.5 times the base damage. Finally, defense of the defender is taken to reduce the damage by                                                                                              given percentage.

### Lights
When enabled, light source can be easily added to entities. Every light source is configured with a radius of produced light. The light is brighest on the map tile where the source is located. It gets less bright the further from the light source you are. It doesn't spread through walls.

> NOTE: The only entity currently producing light is a torch.

> NOTE: It can be combined with *Fog of war*.

### Fog of war
When enabled, the whole map starts covered with shadow. Entities can be configured to reveal specified radius of map around them. The goal of this feature is to force players to explore the map and also make it quite dangerous.

> NOTE: It can be combined with *Light* feature.

# Multiplayer
### Overview
The main idea is to have one client act as an authoritative server and other clients are just regular clients. Clients send messages to server, server then processes them and broadcasts changes to all clients (including the client who sent the original message). The game state on all clients is then accordingly changed.

> NOTE: To make it easier, the server acts as a client, too. Both server and clients implement common abstract class **NetworkEntity** which provides easy-to-use api for sending and receiveing network messages. The majority of application then does not have to care if it is a regular client or the server.

Both the server and clients **run almost identical game-related code**. It is therefore possible to send *Action messages* (see below) and all clients can easily modify their state. The main difference is that some systems (like monsters' AI, processing combat, etc..) run only on the server because there is sometimes randomness involved and it would add unneeded complexity with synchronization.

### Technology used
All network communication is based on Unity's Transport Layer API (LLAPI). The main benefit is that it lets you easily create communication channels with different **Quality of Service**. Two channels are used in the game - one for control messages and the other one for action messages (see below). Both channels use **ReliableSequenced** quality of service which means that it delivers all messages and also preserves their order. The good think about LLAPI is that it lets you send directly arrays of bytes so you are in a complete control of what happens in the network.

> NOTE: LLAPI can be replaced with any similar library but it was used because it comes as a part of Unity so no other dependencies were needed.

#### Protobuf and message serialization
Protobuf-net is an excellent library for binary serialization. NetworkEntity lets you send high level messages which are then serialized into an array of bytes under the hood (with Protobuf), sent and finally deserialized on the other side.

### Control messages
Control messages are used for game control related actions such that the game has already started, that a player connected etc.. Majority of these messages are handled directly by NetworkController and NetworkEntity.

> NOTE: All control messages implement **IControlMessage** interface for easy manipulation and serialization.

#### Implementation overview
All messages in the *control channel* are directly cast into *IControlMessage*. The idea of *NetworkEntity* was that it should not contain more logic than is really needed. It therefore lets you register a handler for any kind of control message with *RegisterHandler* method. New features can be then add without the need to change the *NetworkEntity* itself.
```
public void RegisterHandler<T>(Action<T, Player> handler) where T : IControlMessage
```
When a message is received, all registered handlers for that message type are triggered. *NetworkEntity* has some convenient handlers itself but majority of handlers come from *NetworkController*.

#### Example usage
*NetworkController* register its handler for *ConnectedMessage*. When a player connects, the handler is triggered and *NetworkController* updates its list of players with the newly connected player.


### Action messages
Action messages are used for actions that are related to the game itself. The most basic example is probably *BasicMoveAction* which handles movement of game entities.

> NOTE: All control messages implement **IAction** interface for easy manipulation and serialization.

> NOTE: Action messages are meant to send changes instead of the whole game state so they can be very small when sending through network. *BasicMoveAction*, for example, only has information about given entity and its new position. All clients then interpret this action and change their game state.

There are currently 3 systems responsible for handling of action messages:

#### ClientSystem
*ClientSystem* takes all actions from the current update cycle. It sends them to the server and then destroys them. It then checks if it received any actions from the server. If so, received actions are added to the current update cycle and are further processed.

> NOTE: Actions are destroyed because the server is authoritative. All actions must be first checked and processed on the server and only after that they are sent to all clients and changes to the game state are made.

#### ServerSendSystem
*ServerSendSystem* runs in the cleanup phase and sends all actions to all clients.

#### ServerReceiveSystem
*ServerReceiveSystem* runs in the init phase and waits for all actions received from client and adds them to the udpate cycle.

> NOTE: These 3 systems alone are not enough to handle all multiplayer challenges. Maybe the most important and hardest thing is the system order. See more in the section dedicated to execution order of systems.

### Network tracking
There is a system dedicated to adding a unique id to all entities that are going to be referenced over the network. The id is assigned right when the entity is created so it is then really easy to reference any entity in action messages.

# Challenges

### Execute order of systems
#### Problem overview
One of the main challenges when developing games with *Entitas* is maintaing a correct order of systems. Systems are executed in the order they were initially registered. This introduces problems where systems cannot react to changes because they were already executed in the current update cycle. Which results in loosing these changes for good.

> EXAMPLE: *SetPositionSystem* is a *reactive* system that reacts to changes of entities' position components. It takes all changed entities and tells Unity to move these entities to their new positions. *ProcessBasicMoveSystem* is a *reactive* systems that checks if a player (or any monster) requested to move and if so, it changes *position* component of the given entity. Registering *ProcessBasicMoveSystem* after *SetPositionSystem* results in entities not moving at all.

The goal is to not depend on register order at all.

#### Original solution
*Entitas* itself doesn't provide any convenient solution.  You end up with a (potentially long) ordered list of systems and you hope to not break it all when adding a new system. It also doesn't let you use *features* (groups of systems) anymore because the order then depends on when you registered given feature which results in a major loss of flexibility.

#### Phases
At first, I divided my systems into *Phases*. Phase is just an enum that contains names of phases (like *Init*, *Input*, *ProcessActions*, etc..) which have specific order.  I also created *ExecutePhase* attribute that you can use to annotate your *execute* systems. You can now easily say that you want a specific system to run in for example the *Init* phase of the update loop. It adds a declarative way to specify an execute order and lets you use *features* to group systems by functionality. 

```
[ExecutePhase(ExecutePhase.ProcessActions)]
public class AddMonsterReferenceSystem
```

> NOTE: *Phases* do not have any universal meaning in Entitas or game dev. It is just a way for me to think about the systems order.

> NOTE: There are currently these phases: *Init, Input, ProcessActions, ValidateActions, Network, ReactToActions, ReactToComponents, Cleanup*

#### Explicit dependencies
*Phases* make it easier to handle execute order but they are not enough. Sometimes you need to specify order inside a phase. This is where *ExecutesAfter* and *ExecutesBefore* attributes come into play.

```
[ExecutePhase(ExecutePhase.ReactToComponents)]
[ExecutesAfter(typeof(AddViewSystem))]
public class SetPositionSystem
```

Graph is then built from all dependencies and topoligal order is found (if it exists).

> NOTE: Explicit dependencies are more powerful than *phases* and could completely replace them.  But I find it easier to use both layers to manage the order.

#### Result
You still can't avoid thinking about the order but I found it a lot easier than it was before.
