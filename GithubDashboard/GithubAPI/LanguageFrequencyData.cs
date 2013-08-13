using System;
using System.Collections.Generic;

namespace GithubAPI
{
	public class LanguageFrequencyData : Dictionary<string, long>
	{
		public LanguageFrequencyData (IDictionary<string, long> dictionary)
			: base(dictionary)
		{
		}
	}
}

