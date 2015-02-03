#import <UIKit/UIKit.h>

#include "RegisterClasses.h"
#include "RegisterMonoModules.h"

int main(int argc, char *argv[])
{
	RegisterAnimationClasses();
	RegisterAudioClasses();
	RegisterManagerClasses();
#if 1
	RegisterDynamicsClasses();
	RegisterRenderingClasses();
	RegisterAuxClasses();
	RegisterParticleClasses();
	RegisterGUIClasses();
#endif	
	NSLog(@"-> registered unity classes\n");
	
	RegisterMonoModules();
	NSLog(@"-> registered mono modules\n");
	
	NSAutoreleasePool*		pool = [NSAutoreleasePool new];
	
	UIApplicationMain(argc, argv, nil, @"AppController");
	
	[pool release];
	
	return 0;
}