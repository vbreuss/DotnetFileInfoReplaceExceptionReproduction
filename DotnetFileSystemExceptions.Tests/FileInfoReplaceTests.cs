using DotnetFileSystemExceptions.Tests.Helpers;
using System.Runtime.InteropServices;

namespace DotnetFileSystemExceptions.Tests;

public class FileInfoReplaceTests
{
	[Fact]
	public void Replace_DestinationDirectoryMissing_ShouldThrow()
	{
		using (Initialize.TemporaryDirectory())
		{
			string destinationPath = Path.Combine(Directory.GetCurrentDirectory(),
				"not-existing-directory",
				"destination.txt");
			FileInfo sut = new("some-path.txt");
			sut.Create();

			Exception? exception = Record.Exception(() => sut.Replace(destinationPath, null));

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				Assert.IsType<FileNotFoundException>(exception);
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				Assert.IsType<FileNotFoundException>(exception);
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Assert.IsType<DirectoryNotFoundException>(exception);
			}
		}
	}

	[Fact]
	public void Replace_SourceDirectoryMissing_ShouldThrow()
	{
		using (Initialize.TemporaryDirectory())
		{
			string sourcePath = Path.Combine(Directory.GetCurrentDirectory(),
				"not-existing-directory",
				"source.txt");
			FileInfo sut = new(sourcePath);

			Exception? exception = Record.Exception(() => sut.Replace("destination.txt", null));

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				if (Test.IsNet7)
				{
					Assert.IsType<DirectoryNotFoundException>(exception);
				}
				else
				{
					Assert.IsType<FileNotFoundException>(exception);
				}
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				if (Test.IsNet7)
				{
					Assert.IsType<DirectoryNotFoundException>(exception);
				}
				else
				{
					Assert.IsType<FileNotFoundException>(exception);
				}
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Assert.IsType<FileNotFoundException>(exception);
			}
		}
	}

	[Fact]
	public void Replace_FileMissing_ShouldThrow()
	{
		using (Initialize.TemporaryDirectory())
		{
			Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(),
				"existing-directory"));
			string destinationPath = Path.Combine(Directory.GetCurrentDirectory(),
				"existing-directory",
				"destination.txt");
			FileInfo sut = new("some-path.txt");
			sut.Create();

			Exception? exception = Record.Exception(() => sut.Replace(destinationPath, null));

			Assert.IsType<FileNotFoundException>(exception);
		}
	}
}