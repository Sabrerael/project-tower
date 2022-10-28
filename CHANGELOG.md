CHANGELOG

v0.4.2:
  Content Additions:
     - Added 2 new enemies to the Ice Floor

  Characters: 
  	- Adjusted movement speed values
	- Updated animator controllers
	- Made it so Attacking and using Active Abilities will stop movement. Movement will be allowed once the attack/ability is complete
	- Made it so the roll can only be triggered in the direction of movement instead of the direction of the mouse
	  > This was just awkward and made the movement feel off
     - Adjusted player character hitboxes
     - Added a shadow to player characters
     - Added a particle system to kick up dust while moving

  Enemies: 
     - Adjusted movement speed values
     - Adjusted enemy hitboxes
     - Added a shadow to enemies
     - Modified how certain enemies trigger damage

  Weapons:
     - Adjusted hitboxes


  Bug Fixes:
	- Fixed an issue where the player always flipped to the right
	- Fixed an issue where the enemy health bars would flip when the enemy turned to face the player

-----------------------------------------------------------------

v0.4.1:
  Content Additions:
	- Added the Ice Floor
	- Added three enemies for the Ice Floor

  UI:
	- Added a panel behind the Character Stats UI to allow for better readability
	- The Boss' Health Bar will fade from view after the Boss dies

  Bug Fixes:
	- Fixed an issue where the Gelatinous Cube got tired after one jump and stopped moving
	- Fixed an issue where the Exit Door did not open after defeating the boss
	- Fixed an issue where the menus rendered at improper resolutions
	- Fixed an issue where other rooms could be seen on the periphery of the camera

  Known Issues:
	- Enemy Health Bar flip when the Enemy flips to follow the Player
	- Had one instance where the Floor Layout looped on itself, preventing the player from proceeding
	- Quit Button from the Pause Menu caused some broken behavior
	