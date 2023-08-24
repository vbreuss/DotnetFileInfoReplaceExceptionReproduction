using DotnetFileSystemExceptions.Tests.Helpers;
using FluentAssertions;
using System.Runtime.InteropServices;

namespace DotnetFileSystemExceptions.Tests;

public class FileMoveTests
{
	[Fact]
	public void File_Copy_ShouldSetAccessTime()
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
			File.Copy(source, destination);

			DateTime sourceLastAccessTime = File.GetLastAccessTimeUtc(source);
			DateTime destinationLastAccessTime = File.GetLastAccessTimeUtc(destination);
#if NET8_0_OR_GREATER
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#else
			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
#endif
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

	[Fact]
	public void FileInfo_CopyTo_ShouldSetAccessTime()
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
			var fileInfo = new FileInfo(source);
			fileInfo.CopyTo(destination);

			DateTime sourceLastAccessTime = File.GetLastAccessTimeUtc(source);
			DateTime destinationLastAccessTime = File.GetLastAccessTimeUtc(destination);
#if NET8_0_OR_GREATER
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#else
			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
#endif
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