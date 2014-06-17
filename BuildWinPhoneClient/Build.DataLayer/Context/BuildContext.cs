#region Using Directives

using System.Data.Linq;

using Build.DataLayer.Model;

#endregion

namespace Build.DataLayer.Context
{
    public class BuildContext : DataContext
    {
        #region Constants

        private const string ConnectionString = "DataSource=isostore:/Build.sdf";

        #endregion

        #region Fields

        public Table<CurrentUser> CurrentUser;

        #endregion

        #region Constructors and Destructors

        public BuildContext()
            : this(ConnectionString)
        {
        }

        public BuildContext(string connectionString)
            : base(connectionString)
        {
            if (!this.DatabaseExists())
            {
                this.CreateDatabase();
            }
        }

        #endregion
    }
}