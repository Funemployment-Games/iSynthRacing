/*
 *  RegisterClasses.h
 *  iPhone-target2
 *
 *  Created by Renaldas on 8/26/08.
 *  Copyright 2008 __MyCompanyName__. All rights reserved.
 *
 */
#define DEFINE_FORCE_INCLUDE(classname) void ForceInclude_##classname();
#define UNITY_TOUCH(classname) { void *p = (void*)ForceInclude_##classname; printf_console("Unity runtime class registered %d\n", p); }
#define UNITY_TOUCH_NS(classname) { void *p = (void*)Unity::ForceInclude_##classname; printf_console("Unity runtime class registered %d\n", p); }



DEFINE_FORCE_INCLUDE( Animation );
DEFINE_FORCE_INCLUDE( AnimationClip );
DEFINE_FORCE_INCLUDE( AnimationManager );
DEFINE_FORCE_INCLUDE( AudioClip );
DEFINE_FORCE_INCLUDE( AudioListener );
DEFINE_FORCE_INCLUDE( AudioManager );
DEFINE_FORCE_INCLUDE( AudioSource );
DEFINE_FORCE_INCLUDE( BaseBehaviourManager );
DEFINE_FORCE_INCLUDE( Behaviour );
DEFINE_FORCE_INCLUDE( BehaviourManager );
DEFINE_FORCE_INCLUDE( BoxCollider );
DEFINE_FORCE_INCLUDE( BuildSettings );
DEFINE_FORCE_INCLUDE( CGProgram );
DEFINE_FORCE_INCLUDE( Camera );
DEFINE_FORCE_INCLUDE( CapsuleCollider );
DEFINE_FORCE_INCLUDE( CharacterController );
namespace Unity { DEFINE_FORCE_INCLUDE( CharacterJoint ); }
DEFINE_FORCE_INCLUDE( Collider );
namespace Unity { DEFINE_FORCE_INCLUDE( Component ); }
DEFINE_FORCE_INCLUDE( ConstantForce );
DEFINE_FORCE_INCLUDE( Cubemap );
DEFINE_FORCE_INCLUDE( DelayedCallManager );
DEFINE_FORCE_INCLUDE( EditorExtension );
DEFINE_FORCE_INCLUDE( EllipsoidParticleEmitter );
DEFINE_FORCE_INCLUDE( Filter );
DEFINE_FORCE_INCLUDE( FixedBehaviourManager );
namespace Unity { DEFINE_FORCE_INCLUDE( FixedJoint ); }
DEFINE_FORCE_INCLUDE( Flare );
DEFINE_FORCE_INCLUDE( FlareLayer );
DEFINE_FORCE_INCLUDE( Font );
DEFINE_FORCE_INCLUDE( GUIElement );
DEFINE_FORCE_INCLUDE( GUILayer );
DEFINE_FORCE_INCLUDE( GUIText );
DEFINE_FORCE_INCLUDE( GUITexture );
DEFINE_FORCE_INCLUDE( GameManager );
namespace Unity { DEFINE_FORCE_INCLUDE( GameObject ); }
DEFINE_FORCE_INCLUDE( GlobalGameManager );
DEFINE_FORCE_INCLUDE( Halo );
DEFINE_FORCE_INCLUDE( HaloLayer );
DEFINE_FORCE_INCLUDE( HaloManager );
namespace Unity { DEFINE_FORCE_INCLUDE( HingeJoint ); }
DEFINE_FORCE_INCLUDE( InputManager );
namespace Unity { DEFINE_FORCE_INCLUDE( Joint ); }
DEFINE_FORCE_INCLUDE( LateBehaviourManager );
DEFINE_FORCE_INCLUDE( LensFlare );
DEFINE_FORCE_INCLUDE( LevelGameManager );
DEFINE_FORCE_INCLUDE( Light );
DEFINE_FORCE_INCLUDE( LineRenderer );
DEFINE_FORCE_INCLUDE( MasterServerInterface );
namespace Unity { DEFINE_FORCE_INCLUDE( Material ); }
DEFINE_FORCE_INCLUDE( Mesh );
DEFINE_FORCE_INCLUDE( MeshCollider );
DEFINE_FORCE_INCLUDE( MeshFilter );
DEFINE_FORCE_INCLUDE( MeshParticleEmitter );
DEFINE_FORCE_INCLUDE( MeshRenderer );
DEFINE_FORCE_INCLUDE( MonoBehaviour );
DEFINE_FORCE_INCLUDE( MonoManager );
DEFINE_FORCE_INCLUDE( MonoScript );
//DEFINE_FORCE_INCLUDE( MovieTexture );
DEFINE_FORCE_INCLUDE( NamedObject );
//DEFINE_FORCE_INCLUDE( NetworkManager );
//DEFINE_FORCE_INCLUDE( NetworkView );
DEFINE_FORCE_INCLUDE( NotificationManager );
DEFINE_FORCE_INCLUDE( ParticleAnimator );
DEFINE_FORCE_INCLUDE( ParticleEmitter );
DEFINE_FORCE_INCLUDE( ParticleRenderer );
DEFINE_FORCE_INCLUDE( PhysicMaterial );
DEFINE_FORCE_INCLUDE( PhysicsManager );
DEFINE_FORCE_INCLUDE( Pipeline );
DEFINE_FORCE_INCLUDE( PipelineManager );
DEFINE_FORCE_INCLUDE( PlayerSettings );
DEFINE_FORCE_INCLUDE( Projector );
DEFINE_FORCE_INCLUDE( QualitySettings );
DEFINE_FORCE_INCLUDE( RaycastCollider );
DEFINE_FORCE_INCLUDE( RenderLayer );
DEFINE_FORCE_INCLUDE( RenderManager );
DEFINE_FORCE_INCLUDE( RenderSettings );
DEFINE_FORCE_INCLUDE( RenderTexture );
DEFINE_FORCE_INCLUDE( Renderer );
DEFINE_FORCE_INCLUDE( ResourceFile );
DEFINE_FORCE_INCLUDE( ResourceManager );
DEFINE_FORCE_INCLUDE( Rigidbody );
namespace Unity { DEFINE_FORCE_INCLUDE( Scene ); }
DEFINE_FORCE_INCLUDE( ScriptMapper );
DEFINE_FORCE_INCLUDE( Shader );
DEFINE_FORCE_INCLUDE( SkinnedMeshRenderer );
DEFINE_FORCE_INCLUDE( Skybox );
DEFINE_FORCE_INCLUDE( SphereCollider );
namespace Unity { DEFINE_FORCE_INCLUDE( SpringJoint ); }
DEFINE_FORCE_INCLUDE( TagManager );
DEFINE_FORCE_INCLUDE( TerrainData );
DEFINE_FORCE_INCLUDE( TextAsset );
DEFINE_FORCE_INCLUDE( TextMesh );
DEFINE_FORCE_INCLUDE( Texture );
DEFINE_FORCE_INCLUDE( Texture2D );
DEFINE_FORCE_INCLUDE( Texture3D );
DEFINE_FORCE_INCLUDE( TextureRect );
DEFINE_FORCE_INCLUDE( TimeManager );
DEFINE_FORCE_INCLUDE( TrailRenderer );
DEFINE_FORCE_INCLUDE( Transform );
DEFINE_FORCE_INCLUDE( UpdateManager );
DEFINE_FORCE_INCLUDE( WheelCollider  );
DEFINE_FORCE_INCLUDE( WorldParticleCollider );

void RegisterAnimationClasses();
void RegisterAudioClasses();
void RegisterManagerClasses();
void RegisterDynamicsClasses();
void RegisterRenderingClasses();
void RegisterAuxClasses();
void RegisterParticleClasses();
void RegisterGUIClasses();
