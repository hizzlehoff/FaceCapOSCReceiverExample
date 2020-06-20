# FaceCapOSCReceiverExample
A Unity example project that received Face Cap data via OSC.

http://www.bannaflak.com/face-cap/livemode.html

Use Unity v2019.3.11 or newer.

If not included add the 'extOSC' package via the Unity Asset store.

Open the scene "FaceCap/Scenes/FaceCapGenericHeadOSCExample".

Click on the "FaceCapHeadGeneric" gameObject to set up your OSC port.

Make sure Unity is allowed through your firewall.

Make sure the FaceCap iOS app and the Unity project are connected to the same wifi network, or even better create a hotspot on you iPhone or iPad and connect the device running Unity to the hotspot via a USB cable.

Press play in Unity and on the "FaceCapHeadGeneric" gameObject you will find an "extOSC" component that shows the I.P. address of your device, enter this in the Face Cap iOS app live mode.

![Alt text](FaceCap.gif?raw=true "Well, hello there.")

Latest update:
----------------------------
This example project has been completely rewritten and simplified.

There is now just one script that controls everything, be it a mesh with blendshapes that have the same naming as FaceCap or custom head with custom naming. It is called "FaceCapLiveModeReceiver" and it to your character and assign the desired properties, it will throw warnings if anything is missing.

A second example "FaceCap/Scenes/FaceCapCustomAvatarExample" shows you how to divide the position and rotation values across the skeleton of a rigged character.
