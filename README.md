# Mini Fighter 3D Key Scripts

----

C# Scripts used in "Mini Fighter 3D", the Casual Fighting Action Game for Android made with Unity3D 



## About Mini Fighter 3D (2020)

----

Mini Fighter 3D (2020) is a 3D casual action fighting game for Android. The game is made with Unity 5. It currently supports 4 Play modes: Story Mode, Custom Battle Mode, Practice Mode and Tutorial Mode, and plans to update 3 modes (Special Match, Ranked Battle, Online Battle) in the future. Also it supports Customization & Upgrade Abilities.
The game is under development from January 2020, and will be released and serviced through the Google Play Store in the near future.



## Contents

----

This contains the Key Scripts that implement various functions in each scene.



### Descriptions about each Scene (Functions&Scripts)

This Unity Project contains 11 Scenes.

- **scTitle** - The first scene that appears when the game starts. The text 'TOUCH SCREEN' flashes every certain frame. The time to initialize the DB of the current user and game.
- **scMain** - Scene 'Main menu'. Various DB information such as owned resources(Gold, Ruby, Energy), character level and costume are displayed, and scripts for various functions such as UI information and animation are operated.
- scSelect - Scene 'Select'. It is the time to select and initialize the game data information such as the player and enemy's selected character, map and difficulty level.
- **mapMatrix** - In-Game Stage: Matrix Map
- **mapOctagon** - In-Game Stage: Octagon Map
- **mapSkycity** - In-Game Stage: Sky City Map
- **scTutorial** - Scene 'Tutorial'. Separate player and enemy scripts were applied to record mission achievements by certain conditions and proceed with the tutorial.
- **scCustomize** - Scene 'Customization'. DB controls such as possession/purchase/equipment of the player's costume were applied.
- **scStory** - Scene 'Story'. Includes missions and actions for each story stage. (game round progression and camera control, etc.)
- **scUpgrade** - Scene 'Upgrade'. User's character-specific stat DB control applied.
- **scShop** - Scene 'Shop'. Scene related to DB for owned resources. Currently, the code for purchasing goods through the Google Store is not implemented.



### Core Scripts by Function

- **Player Control**(PlayerCtrl) : Player character costume application and situational motion control. (Move, avoidance, combo linkage, hit box creation, death, etc.) + Virtual Joystick.cs

- **AI Control** (AI): AI motion control scripts through state pattern.

- **Camera Control(SH_camera)** : Camera control using Cinemachine. Implementation of screen shake (strike effect) change through singleton and state patterns.

- **DB** : Various data classes and data control/management scripts.

  > Game.cs : Coupon info / Game mode / Option / Selected character, map, difficulty, victory, etc.
  >
  > roundDB.cs : In-Game information (damage, guard, reflect, victory, etc.)
  >
  > userData : User information (userID, owned resources, character info)
  >
  > dataStatCharacter.cs : Information about each character (level, rank, costume, exp, etc.)

- **Customization**(Customize) : Managing possession/purchase/equipment of items

- UI Control, Event Management, Hitbox Control, Button Control & Etc.
