/*
 *  RegisterClasses.cpp
 *  iPhone-target2
 *
 *  Created by Renaldas on 8/26/08.
 *  Copyright 2008 __MyCompanyName__. All rights reserved.
 *
 */

#include <stdio.h>
#include "RegisterClasses.h"


void RegisterAnimationClasses()
{
	// Animation
	UNITY_TOUCH (Animation);
	UNITY_TOUCH (AnimationClip);
	UNITY_TOUCH (AnimationManager);
}	

void RegisterAudioClasses()
{  
	// Audio
	UNITY_TOUCH (AudioClip);
	UNITY_TOUCH (AudioListener);
	UNITY_TOUCH (AudioManager);
	UNITY_TOUCH (AudioSource);
}	

void RegisterManagerClasses()
{
	// Behaviours/Managers
	UNITY_TOUCH (BaseBehaviourManager);
	UNITY_TOUCH (Behaviour);
	UNITY_TOUCH (BehaviourManager);
	//UNITY_TOUCH_NS (Component);
	UNITY_TOUCH (FixedBehaviourManager);
	UNITY_TOUCH (DelayedCallManager);
	//UNITY_TOUCH (GameManager);
	//UNITY_TOUCH_NS (GameObject);
	//UNITY_TOUCH (GlobalGameManager);
	//UNITY_TOUCH (InputManager);
	UNITY_TOUCH (LateBehaviourManager);
	//UNITY_TOUCH (LevelGameManager);
	UNITY_TOUCH (MasterServerInterface);
	UNITY_TOUCH (MonoBehaviour);
	UNITY_TOUCH (MonoManager);
	UNITY_TOUCH (MonoScript);
	//UNITY_TOUCH (NamedObject);
	//UNITY_TOUCH (NotificationManager);
	//UNITY_TOUCH (ResourceFile);
	//UNITY_TOUCH (ResourceManager);
	UNITY_TOUCH (ScriptMapper);
	UNITY_TOUCH (TagManager);
	//UNITY_TOUCH (TimeManager);
	//UNITY_TOUCH (Transform);
	//UNITY_TOUCH (UpdateManager);
}

void RegisterRenderingClasses()
{
	// Rendering
	UNITY_TOUCH (Filter);
	UNITY_TOUCH (CGProgram);
	UNITY_TOUCH (Camera);
	UNITY_TOUCH (Cubemap);
	UNITY_TOUCH (Flare);
	UNITY_TOUCH (FlareLayer);
	UNITY_TOUCH (Font);
	UNITY_TOUCH (Halo);
	UNITY_TOUCH (HaloLayer);
	UNITY_TOUCH (HaloManager);
	UNITY_TOUCH (LensFlare);
	UNITY_TOUCH (Light);
	UNITY_TOUCH (LineRenderer);
	UNITY_TOUCH (Mesh);
	UNITY_TOUCH (MeshFilter);
	UNITY_TOUCH (MeshRenderer);
	UNITY_TOUCH (Projector);
	UNITY_TOUCH (PlayerSettings);
	UNITY_TOUCH (QualitySettings);
	UNITY_TOUCH (RenderLayer);
	UNITY_TOUCH (RenderManager);
	UNITY_TOUCH (RenderSettings);
	UNITY_TOUCH (RenderTexture);
	UNITY_TOUCH (Renderer);
	UNITY_TOUCH (Shader);
	UNITY_TOUCH (SkinnedMeshRenderer);
	UNITY_TOUCH (Skybox);
	UNITY_TOUCH (TextAsset);
	UNITY_TOUCH (TextMesh);
	UNITY_TOUCH (Texture);
	UNITY_TOUCH (Texture2D);
	UNITY_TOUCH (Texture3D);
	UNITY_TOUCH (TextureRect);
	UNITY_TOUCH (TrailRenderer);
	UNITY_TOUCH (EllipsoidParticleEmitter);
	UNITY_TOUCH (MeshParticleEmitter);
	UNITY_TOUCH (ParticleRenderer);
}

void RegisterDynamicsClasses()
{
	// Dynamics
	UNITY_TOUCH (CharacterController);
	UNITY_TOUCH_NS (CharacterJoint);
	UNITY_TOUCH (BoxCollider);
	UNITY_TOUCH (CapsuleCollider);
	UNITY_TOUCH (Collider);
	UNITY_TOUCH (ConstantForce);
	UNITY_TOUCH_NS (FixedJoint);
	UNITY_TOUCH_NS (HingeJoint);
	UNITY_TOUCH_NS (Joint);
//	UNITY_TOUCH_NS (Material);
	UNITY_TOUCH (MeshCollider);
	UNITY_TOUCH (RaycastCollider);
	UNITY_TOUCH (Rigidbody);
	UNITY_TOUCH (PhysicMaterial);
	UNITY_TOUCH (PhysicsManager);
	UNITY_TOUCH (SphereCollider);
	UNITY_TOUCH (WheelCollider);
	UNITY_TOUCH (WorldParticleCollider);
}

void RegisterAuxClasses()
{
	// Aux
	UNITY_TOUCH (BuildSettings);
	UNITY_TOUCH (EditorExtension);
	UNITY_TOUCH (Pipeline);
	UNITY_TOUCH (PipelineManager);
}

void RegisterParticleClasses()
{
	// Particles
	UNITY_TOUCH (EllipsoidParticleEmitter);
	UNITY_TOUCH (MeshParticleEmitter);
	UNITY_TOUCH (ParticleAnimator);
	UNITY_TOUCH (ParticleEmitter);
	UNITY_TOUCH (ParticleRenderer);
}

void RegisterGUIClasses()
{		
	// GUI
	UNITY_TOUCH (GUIElement);
	UNITY_TOUCH (GUILayer);
	UNITY_TOUCH (GUIText);
	UNITY_TOUCH (GUITexture);
}

//	 Unity::UNITY_TOUCH (Scene);
//	 Unity::UNITY_TOUCH (SpringJoint);
//	UNITY_TOUCH (MovieTexture);
//	UNITY_TOUCH (NetworkManager);
//	UNITY_TOUCH (NetworkView);

