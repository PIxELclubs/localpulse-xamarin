using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Localpulse
{
    public class IssueDetail : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		string description = string.Empty;
		int votes = 0;
		string objectId = string.Empty;
		string picture = string.Empty;

		#endregion

		public string Description
		{
			get {
				return description;
			}
			set {
				if (value != description) {
					description = value;
					NotifyPropertyChanged();
				}
			}
		}

		public int Votes
		{
			get {
				return votes;
			}
			set {
				if (value != votes) {
					votes = value;
					NotifyPropertyChanged();
				}
			}
		}

		public string ObjectId {
			get {
				return objectId;
			}
			set {
				if (value != objectId) {
					objectId = value;
					NotifyPropertyChanged();
				}
			}
		}

		public string Picture
		{
			get {
				return picture;
			}
			set {
				if (value != picture) {
					picture = value;
					NotifyPropertyChanged();
				}
			}
		}

		public IssueDetail()
		{
			;
		}
	}
}
