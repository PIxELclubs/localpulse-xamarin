using System;
using System.ComponentModel;

namespace Localpulse
{
    public class IssueViewModel : INotifyPropertyChanged
    {
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public string Description { get; set; }
		public int Votes { get; set; }
		// public string Location { get; set; }
		public string ObjectId { get; set; }

		public IssueViewModel ()
		{
			;
		}
	}
}
