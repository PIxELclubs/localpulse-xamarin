using System;
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

		string objectId = string.Empty;
		DateTime createdAt = new DateTime(0);
		DateTime updatedAt = new DateTime(0);
		string description = string.Empty;
		int votes = 0;
		string picture = string.Empty;

		#endregion

		public string ObjectId
		{
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
		public DateTime CreatedAt
		{
			get {
				return createdAt;
			}
			set {
				if (value != createdAt) {
					createdAt = value;
					NotifyPropertyChanged();
				}
			}
		}
		public DateTime UpdatedAt
		{
			get {
				return updatedAt;
			}
			set {
				if (value != updatedAt) {
					updatedAt = value;
					NotifyPropertyChanged();
				}
			}
		}
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

		public void Update(IssueDetail newer)
		{
			Description = newer.Description;
			Votes = newer.Votes;
			ObjectId = newer.ObjectId;
			Picture = newer.Picture;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType()) {
				return false;
			}

			return ObjectId == ((IssueDetail)obj).ObjectId;
		}
		public override int GetHashCode()
		{
			return ((ObjectId[3] << 24) | (ObjectId[2] << 16) | (ObjectId[1] << 8) | ObjectId[0]);
		}

		public IssueDetail()
		{
			;
		}
	}
}
