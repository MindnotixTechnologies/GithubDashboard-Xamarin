using System;

namespace GithubDashboard
{
	/// <summary>
	/// A data view renders data of the given type - typically as a chart, grid or gauge
	/// </summary>
	public interface IDataView <T>
	{
		/// <summary>
		/// Renders the given data.
		/// </summary>
		void RenderData (T data);
	}
}

