using DotnetFileSystemExceptions.Tests.Helpers;
using FluentAssertions;
using System.Runtime.InteropServices;

namespace DotnetFileSystemExceptions.Tests;

[Collection("Sequential")]
public class FileCopyTests
{
	[Fact]
	public void File_Copy_ShouldSetAccessTime()
	{
		ExecuteFileCopyTest((source, destination) =>
		{
			File.Copy(source, destination);
		});
	}

	[Fact]
	public void FileInfo_CopyTo_ShouldSetAccessTime()
	{
		ExecuteFileCopyTest((source, destination) =>
		{
			var fileInfo = new FileInfo(source);
			fileInfo.CopyTo(destination);
		});
	}

	private static void ExecuteFileCopyTest(Action<string, string> fileCopyAction)
	{
		string source = "source.txt";
		string destination = "destination.txt";
		using (Initialize.TemporaryDirectory())
		{
			DateTime creationTimeStart = DateTime.UtcNow;
			File.WriteAllText(source, "some content");
			DateTime creationTimeEnd = DateTime.UtcNow;
			Thread.Sleep(2500);
			var updateTime = DateTime.UtcNow;
			fileCopyAction(source, destination);

			DateTime sourceLastAccessTime = File.GetLastAccessTimeUtc(source);
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
#if NET8_0_OR_GREATER
				sourceLastAccessTime.Should()
					.BeOnOrAfter(creationTimeStart.AddMilliseconds(-40)).And
					.BeOnOrBefore(creationTimeEnd);
#else
				sourceLastAccessTime.Should()
					.BeOnOrAfter(updateTime.AddMilliseconds(-40));
#endif
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				sourceLastAccessTime.Should()
					.BeOnOrAfter(creationTimeStart.AddMilliseconds(-40)).And
					.BeOnOrBefore(creationTimeEnd);
			}
			else
			{
				sourceLastAccessTime.Should()
					.BeOnOrAfter(updateTime.AddMilliseconds(-40));
			}
		}
	}
}