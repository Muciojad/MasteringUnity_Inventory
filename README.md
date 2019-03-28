
# Unity inventory system
#### UNITY VERSION - 2018.3.6F1
## Briefly
Working inventory system for Unity game.

![alt text](https://github.com/Muciojad/MasteringUnity_Inventory/blob/master/Screens/6.PNG)

**Core things mastered: Observer pattern, design class with interfaces, magic of inheritance**
Main purpose to do it was self-development in C#/Unity and learning game programming patterns.
It is first of my **Mastering Unity** projects, where I try to develop working game systems as Inventory/crafting etc and "*copy*" some great features from AAA or just well-known games.
## Inventory system - quick look
I aimed for "remake" inventory from RE2, but I end up with my own system, highly inspired by mentioned above RE2 inventory.
### Main features

 - Inventory bag for keeping items
 ![alt text](https://github.com/Muciojad/MasteringUnity_Inventory/blob/master/Screens/1.PNG)
 - Bag upgrade system - player can extend bag size by finding upgrades
 ![alt text](https://github.com/Muciojad/MasteringUnity_Inventory/blob/master/Screens/5.PNG)
 - Neat UI look - inventory view is clean and clear, items have their own description
 - Collecting items during gameplay - when player enters item's trigger
 - Throwing/using items via inventory UI - player can "use" item in inventory view - Medpack as in-game example. Item can be thrown simply by clicking H key when item is currently active.
 - Marking items as useless - player knows when item is useless and can be thrown
 ![alt text](https://github.com/Muciojad/MasteringUnity_Inventory/blob/master/Screens/4.PNG)
 - Fast use - item needed to do something else, for example access device which unlocks door, can be used outside UI view - player is informed when and which item can be used simply by pressing F key.
 ![alt text](https://github.com/Muciojad/MasteringUnity_Inventory/blob/master/Screens/2.PNG)
 - Item stacking
 
 ### Inventory data structure
 Top-bottom.
 Top - what player see
 Bottom - what's going on inside
 
 - Inventory UI window
	 - Inventory slots manager
		 - Inventory slot controller
			 - Item data field
			 - Onclick notifications
			 - OnUse notifications
			 - OnThrow notifications
		 - Inventory Icon Dispatcher
			 - Deal with icons for collected items
		 - Inventory
			 - ItemSlot - informations about single slot in bag
			 - Collecting/removing items
			 - Manage space - merge slots(sum the same items in one slot if possible)
			 - Look for item in bag
			 - Upgrade system
### Observer & subject & events
Observer is really simple base class with just one, abstract method - OnNotify.
Main task of Observer is getting event data as method param and performing some actions, depending on observer's duty.

Subject is base class for all subject objects. It contains list of observers inside, and subject can anytime subscribe to observer.
Subject's Notify() method is main method for sending messages to observers, with some event as param.

Event is always struct -> due to how garbage collector works. Using objects based on classes could bite in ass when it comes to disposing it from memory.
There is base struct for events and proper events as derived structs. Events contain some information about thing that is reported (like which slot was clicked).

### By the way
For inventory purposes, I developed item architecture using inheritance and interface features. Item can be really simple, and could have more extended derivatives, like MedPack or AccessDevice(both presented in my Unity project).

### Observer pattern - summary
Main purpose of this project was to learn observer pattern in practical way.
I thought inventory system will be great case to do that.
Most of core functionality is based on Observer-Subject pattern, but I know that not in all places it was neccessary. 
I chosen simple way to entire project - put observers and subjects in every single place where it can be used. 
With project growth I slowed down with observers amount and few times used a simpler ways to achieve goal.
Definitely code is not super clean, maybe it isn't clean at all through amount of observers and subjects - somewhere it's really messy.

For summarize, I think my current knowledge about Observer pattern is enough to consciously using it when it's necessary - without using unnecessary coupling between objects. I realized, that this pattern is really cool when it comes to code managment and it's important to have as least responsibilities on object as possible. Keep it simple.

Project download: https://drive.google.com/open?id=1zi5TfkJK1Lve9umXXpcVbQ-VEdc5KH2R

Video: https://youtu.be/mxCu-fYjBRA


