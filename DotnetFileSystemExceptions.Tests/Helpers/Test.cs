namespace DotnetFileSystemExceptions.Tests.Helpers;

public static class Test
{
	public static bool IsNet7OrGreater
#if NET7_0_OR_GREATER
		=> true;
#else
		=> false;
#endif
}
