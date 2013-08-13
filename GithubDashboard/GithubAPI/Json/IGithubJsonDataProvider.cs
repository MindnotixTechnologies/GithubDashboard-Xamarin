using System;

namespace GithubAPI.Json
{
	/// <summary>
	/// Provides basuic access to repository data / statistics via the GitHub API. 
	/// </summary>
	/// <remarks>
	/// The primary purpose of this interface is to allow both online and offline usage of the application. In offline
	/// mode, this interface is implemented via GithubJsonFileDataProvider which returns data from files. 
	/// </remarks>
	public interface IGithubJsonDataProvider
	{
		void FetchData<TCallback>(string owner, string repo, string apiMethod, Action<TCallback> callback)
			where TCallback : new();
	}
}

