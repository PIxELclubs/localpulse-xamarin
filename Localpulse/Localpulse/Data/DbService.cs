using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Localpulse
{
    public class DbService
    {
		// ObjectId => data
		public static Dictionary<string, IssueDetail> Issues = new Dictionary<string, IssueDetail>();
		public static Dictionary<string, ObservableCollection<IssueComment>> IssueComments = new Dictionary<string, ObservableCollection<IssueComment>>();
	}
}
