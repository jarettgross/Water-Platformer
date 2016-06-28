// Setup // (OPEN IN VISUAL STUDIO ETC TO VIEW THIS EASIER)

1)	You will need to start by dragging the prefab "InputController" into your scene.

2)	Now you simply just need to view the inspector on the "InputController". Just open the "Inputs" dropdown and there you can add, change and remove key bindings.
	By default some should be on there but feel free to do what you wish.

	NOTE FOR MOUSE, WHEEL AND AXIS SUPPORT.
		- Currently the only axis supported are "Mouse Wheel Up", "Mouse Wheel Down", "Mouse Position X", and "Mouse Position Y".
		- Simply use the dropdown to select the axis you want to use. Using an axis will wipe the inputs for the specific key, this means that only that axis will work for the key. 
				This is intented, aka you cant have "Mouse Wheel Up" and "X" bound to the same key. I may change this in the future, but it works for now.

3)	In the script you want to use the inputs in you need to first define a new variable like so : " hardInput hInput = null; "
	And then in the start function you need to asign this to an object. This can be dont like so : " hInput = GameObject.FindObjectOfType<hardInput>(); "

3)	Now when ever you want to use an input you have created you can call "hInput." and the type you require.
	Currently Supported:
		hInput.GetKey("KeyName");						-	This simply returns true like the unity default when a key is held down.
		hInput.GetKeyDown("KeyName");					-	This will fire only once when a key is pressed. Just like the default unity one.
		hInput.GetAixs("KeyName", "KeyName2", force);	-	The axis works a little differently but basically the same. You have to feed it two Keys that you have already defined.
															The first KeyName is the positive key, the second is the negative key, and the thrid is the negative key.
																Example:
																	- hInput.GetAxis("ZoomIn", "ZoomOut", 5);

4)	That's it! Video tutorials to come and they will be linked here. Just look at the default bindings I have added in the code, then just add, change, and remove what you like :)
	

// InGame Rebinding Keys //

1)	All you need to rebind a key is a UI Button or a script that calls the rebind function on the "InputController".
	In the prefabs folder there is an example UI for rebinding using buttons.
	Simply add the script "hardInputUI" onto a button and drag it to the "OnClick()" event in the inspector window. Now select the function "hardInputUI.remapKey".

	Now in the inspector just type in the KeyName you want the button to affect. This MUST BE DEFINED in the "hardInput" script as shown in step (2) of // Setup //
	Define a "Text" element (normally just select the text element that is a child to the button by default) in the inspector, this will be showing the player what keys are assigned etc.
	They "Want Second" boolean is if you want the button to use the secondary input of the key you have selected.


NOTE:	To use mouse positions the code needs to use the default Input.GetAxis() code. 
		This should not be a problem as long as you have not changed the name of the default inputs for the mouse axis in the unity input settings.