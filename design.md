* game design document
	* graphics
		* draw: circles (asteroids, powerups)
		* draw: polygons (player, projectiles)
		* colors in the command line, with text overlay
	* logic
		* simulation runs in real time, even while player isn't providing input
		* objects move using simple physics
		* objects interact when they collide
		* player character is an object, controlled by user
	* player choices
		* move in cardinal directions (up/down/left/right)
		* shoot projectile, if sufficient ammo
		* avoid moving into asteroids, or else be destroyed
