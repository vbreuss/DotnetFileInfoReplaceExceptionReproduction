namespace DotnetFileSystemExceptions.Tests.Helpers;

public static class Test
{
	public static bool IsNet8
#if NET8_0
		=> true;
#else
		=> false;
#endif
	public static bool IsNet7
#if NET7_0
		=> true;
#else
		=> false;
#endif
	public static bool IsNet7OrGreater
#if NET7_0_OR_GREATER
		=> true;
#else
		=> false;
#endif
	public static bool IsNet6
#if NET6_0
		=> true;
#else
		=> false;
#endif
	public static bool IsNet5
#if NET5_0
		=> true;
#else
		=> false;
#endif
	public static bool IsNet3
#if NETCOREAPP3_1
		=> true;
#else
		=> false;
#endif
}
