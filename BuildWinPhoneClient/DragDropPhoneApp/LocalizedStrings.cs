
#region Using Directives

using DragDropPhoneApp.Resources;

#endregion

namespace DragDropPhoneApp
{
    public class LocalizedStrings
    {
        #region Static Fields

        private static AppResources localizedResources = new AppResources();

        #endregion

        #region Public Properties

        public AppResources LocalizedResources
        {
            get
            {
                return localizedResources;
            }
        }

        #endregion
    }
}