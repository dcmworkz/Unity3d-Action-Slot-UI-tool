# Unity3d Action Slot UI

Features:
    - Supports three different Action states
        - The Active state can be used while an Action is alive, or while an Action is being cast.
        - The Cooldown state can be used while an Action cannot be used for a certain duration
        - The Disabled state can be used when interaction with the Action Slot should not trigger the Action to fire
    - Native Unity3D functionality
    - Everything from the setup to its’ basic functionality is all native in Unity3D. This means that there are no surprises, and also no       frustration is necessary to make a certain feature work.
    - Easy four step implementation:
        - Add the ActionSlotUI class to a GameObject (or drag one of eight predefined objects from the project into your scene)
        - Set which UI objects you want to use for the Action Slots in the Unity3D inspector
        - Add the IActionObject interface to your Action’s class
        - Call the UpdateActionSlot() method

For more information (and a playable demo), please visit http://lairinus.com/?p=880
