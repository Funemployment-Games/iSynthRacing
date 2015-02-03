export AOT_COMPILER=../../../../build/Unity.app/Contents/Tools/MonoCompilerHost
export MSCORLIB_PATH=../../../../build/Unity.app/Contents/Frameworks/MonoCompiler.framework
$AOT_COMPILER $MSCORLIB_PATH/mscorlib.dll -d .