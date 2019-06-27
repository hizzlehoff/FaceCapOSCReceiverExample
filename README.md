# FaceCapOSCReceiverExample
A Unity example project that received Face Cap data via OSC.

http://www.bannaflak.com/face-cap/livemode.html

Use Unity v2018.3.14f1 or newer.

If not included add the 'extOSC' package via the Unity Asset store.

Open the scene "scenes/FaceCapGenericHeadOSCExample".

Click on the "Scripts" gameObject to set up your port.

Make sure Unity is allowed through your firewall.

Make sure the FaceCap iOS app and the Unity project are connected to the same network.

Enter your I.P. address in the Face Cap iOS app live mode.

![Alt text](FaceCap.gif?raw=true "Hello there.")

Latest update:
----------------------------
There are 2 example scenes: one that drives a face that has the same amount of blendshapes in the same order as the generic Face Cap face, and one that that drives a custom face.

To drive a custom face a FaceCap Remapping object must be created. In your project folder click 'create' and then from the top the FaceCap remapping object.

From you project folder assign the skinnedMeshRenderer to it that has the blendshapes that you want to drive. A list of available blendshapes will be generated. Link these to the FaceCap data by selecting the appropriate drive from the list.

Now create a new scene and add your face model, add the FaceCapOSCReceiverCustom script, assign the FaceCapRemappingObject you have created, the Skinned Mesh from you scene and the appropriate transforms. All done.

Warning:
----------------------------
If your mesh is attached to a skeleton it is possible it's joints have non-zero default rotations and different axis. I do not currently compensate for this but will do so in the future. For now you'll have to align those axis to world space in your 3d software.