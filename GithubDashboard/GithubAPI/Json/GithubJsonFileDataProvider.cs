using System;
using RestSharp.Deserializers;
using System.IO;
using Newtonsoft.Json;

namespace GithubAPI.Json
{
	/// <summary>
	/// Provides Github API data from files, allowing off line usage.
	/// </summary>
	public class GithubJsonFileDataProvider : IGithubJsonDataProvider
	{
		GithubJsonWebDataProvider _webDataProvider = new GithubJsonWebDataProvider();

		public GithubJsonFileDataProvider ()
		{
		}

		#region IGithubRawDataProvider implementation

		public void FetchData<TCallback> (string owner, string repo, string apiMethod, Action<TCallback> callback)
			where TCallback : new()
		{
			// convert the API method into a file path
			string filePath = "./GithubAPI/Json/Data/" + apiMethod.Replace ("{repo}", repo).Replace ("{owner}", owner) + ".json";

			if (File.Exists (filePath))
			{
				// if the file is found, deserialize and callback
				string data = File.ReadAllText (filePath);
				var deserializedData = JsonConvert.DeserializeObject<TCallback>(data);
				callback (deserializedData);
			}
			else
			{
				// otherwise fallback to a web request
				_webDataProvider.FetchData (owner, repo, apiMethod, callback);
			}
		}

		#endregion
	}
}

