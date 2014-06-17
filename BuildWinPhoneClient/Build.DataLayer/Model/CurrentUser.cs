#region Using Directives

using System;
using System.Data.Linq.Mapping;

#endregion

namespace Build.DataLayer.Model
{
    [Table]
    public class CurrentUser
    {
        #region Fields

        [Column]
        public string Login;

        [Column]
        public DateTime LoginTime;

        [Column]
        public string Password;

        private Guid id;

        #endregion

        #region Public Properties

        [Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public Guid Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        #endregion
    }
}