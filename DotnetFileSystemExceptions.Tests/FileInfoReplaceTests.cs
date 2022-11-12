using System.Runtime.InteropServices;

namespace DotnetFileSystemExceptions.Tests;

public class FileInfoReplaceTests
{
	[Fact]
	public void Replace_DirectoryMissing_ShouldThrow()
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
				Assert.IsType<FileNotFoundException>(exception);
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Assert.IsType<DirectoryNotFoundException>(exception);
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