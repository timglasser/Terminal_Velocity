# Terminal_Velocity
A 4 player racing game for Unity.
Unique Gameplay - Arcade style. shooting from passenger to damage and slow down opponents car(180 degree line of site), puddles to cause skids. Car who finishes first after one lap wins  1 - 2 minutes of play. If you win you get another free play. ( Black white blue and yellow color coded). 

Players - 4 cars , 1 lap, (2 animated passengers in players car )(i.e.shooting and driving).Single player

Similar games - Wipeout, GTA, Mario Cart, Gran Torismo

Market - Location based entertainment - VR

Development Features

Current demo  + modular features below = Meccanim for all FSM's. Frame Rate - at least 60 fps on standard machines

Car - rear view mirror, animating steering wheel,emissive shaders for headlights and brake lights. Put light in car cabin?
	need Singleton Object Pool for efficient bullet and skid instantiation
	car metallic shader?

Characters - ?????

AI and CarControl- FSM - 3 cars - waypoint system - possibilty of machine learning framework
Input - for Steering wheel, gears, accelerator pedal keyboard and gamepad.
Editor options - Control attributes of waypoints and AI by dialog box

UI - speedometer, tachymeter, track HUD, timer, field position, Ready Set Go, checkered flag, you win

Gamemanager - Singleton - FSM - Control the states of the arcade experience
