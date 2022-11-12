using DotnetFileSystemExceptions.Tests.Helpers;
using System.Runtime.InteropServices;

namespace DotnetFileSystemExceptions.Tests;

public class SetCreationTimeTests
{
	[Fact]
	public void Replace_DestinationDirectoryMissing_ShouldThrow()
	{
		using (Initialize.TemporaryDirectory())
		{
			Exception? exception = Record.Exception(() =>
			{
				Directory.SetCreationTime("missing-file.txt", DateTime.Now);
			});


			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				if (Test.IsNet7)
				{
					Assert.IsType<FileNotFoundException>(exception);
				}
				else
				{
					Assert.IsType<DirectoryNotFoundException>(exception);
				}
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				Assert.IsType<DirectoryNotFoundException>(exception);
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Assert.IsType<FileNotFoundException>(exception);
			}
		}
	}
}