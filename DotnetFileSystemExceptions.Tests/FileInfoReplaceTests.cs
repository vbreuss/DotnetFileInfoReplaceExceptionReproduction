namespace DotnetFileSystemExceptions.Tests;

public class FileInfoReplaceTests
{
	[Fact]
	public void Replace_DirectoryMissing_ShouldThrow()
	{
		using (Initialize.TemporaryDirectory())
		{
			var destinationPath = Path.Combine(Directory.GetCurrentDirectory(),
				"not-existing-directory",
				"destination.txt");
			var sut = new FileInfo("some-path.txt");
			sut.Create();

			var exception = Record.Exception(() => sut.Replace(destinationPath, null));

			Assert.IsType<DirectoryNotFoundException>(exception);
		}
	}

	[Fact]
	public void Replace_FileMissing_ShouldThrow()
	{
		using (Initialize.TemporaryDirectory())
		{
			Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(),
				"existing-directory"));
			var destinationPath = Path.Combine(Directory.GetCurrentDirectory(),
				"existing-directory",
				"destination.txt");
			var sut = new FileInfo("some-path.txt");
			sut.Create();

			var exception = Record.Exception(() => sut.Replace(destinationPath, null));

			Assert.IsType<FileNotFoundException>(exception);
		}
	}
}
