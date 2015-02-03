#include "RegisterMonoModules.h"
#ifdef __cplusplus
extern "C" {
#endif
	typedef void* gpointer;
	typedef int gboolean;
	const char*			UnityIPhoneRuntimeVersion = "1.5.0f3";
	void				mono_aot_register_module(gpointer *aot_info);
	void				mono_dl_register_symbol (const char* name, void *addr);
	extern gboolean		mono_aot_only;
	extern int 			mono_ficall_flag;
	extern gpointer*	mono_aot_module_mscorlib_info; // mscorlib.dll
	extern gpointer*	mono_aot_module_UnityEngine_info; // UnityEngine.dll
	extern gpointer*	mono_aot_module_24f6c5924e52c49b19da64a24b86d410_info; // Assembly - CSharp.dll
#ifdef __cplusplus
}
#endif
void RegisterMonoModules()
{
	mono_aot_only = true;
	mono_ficall_flag = true;
	mono_aot_register_module(mono_aot_module_mscorlib_info);
	mono_aot_register_module(mono_aot_module_UnityEngine_info);
	mono_aot_register_module(mono_aot_module_24f6c5924e52c49b19da64a24b86d410_info);
}
