using System.Runtime.InteropServices;

namespace DotnetFileSystemExceptions.Tests
{
	public static class Initialize
	{
		public static IDisposable TemporaryDirectory()
		{
			return new DirectoryCleaner();
		}

		internal sealed class DirectoryCleaner : IDisposable
		{
			private readonly string _basePath;

			public DirectoryCleaner()
			{
				_basePath = InitializeBasePath();
			}

			#region IDirectoryCleaner Members

			/// <inheritdoc cref="IDisposable.Dispose()" />
			public void Dispose()
			{
				try
				{
					// It is important to reset the current directory, as otherwise deleting the BasePath
					// results in a IOException, because the process cannot access the file.
					Directory.SetCurrentDirectory(Path.GetTempPath());

					for (var i = 10; i >= 0; i--)
						try
						{
							ForceDeleteDirectory(_basePath);
							break;
						}
						catch (Exception)
						{
							if (i == 0) throw;

							Thread.Sleep(100);
						}
				}
				catch (Exception)
				{
					// Ignore any exceptions in the dispose method.
				}
			}

			#endregion

			/// <summary>
			///   Force deletes the directory at the given <paramref name="path" />.<br />
			///   Removes the <see cref="FileAttributes.ReadOnly" /> flag, if necessary.
			///   <para />
			///   If <paramref name="recursive" /> is set (default <see langword="true" />), the sub directories are force deleted as
			///   well.
			/// </summary>
			private void ForceDeleteDirectory(string path, bool recursive = true)
			{
				if (!Directory.Exists(path)) return;

				var directory = new DirectoryInfo(path);
				directory.Attributes = FileAttributes.Normal;

				foreach (var info in directory.EnumerateFiles(
					         "*",
					         SearchOption.TopDirectoryOnly))
				{
					info.Attributes = FileAttributes.Normal;
					info.Delete();
				}

				if (recursive)
					foreach (var info in
					         directory.EnumerateDirectories(
						         "*",
						         SearchOption.TopDirectoryOnly))
						ForceDeleteDirectory(info.FullName, recursive);

				Directory.Delete(path);
			}

			private string InitializeBasePath()
			{
				string basePath;

				do
				{
					var localBasePath = Path.Combine(
						Path.GetTempPath(),
						Path.GetFileNameWithoutExtension(Path
							.GetRandomFileName()));
					if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
						localBasePath = "/private" + localBasePath;
					basePath = localBasePath;
				} while (Directory.Exists(basePath));

				Directory.CreateDirectory(basePath);

				Directory.SetCurrentDirectory(basePath);
				return basePath;
			}
		}
	}
}
