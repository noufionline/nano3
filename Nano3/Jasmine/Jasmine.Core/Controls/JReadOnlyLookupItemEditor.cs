namespace Jasmine.Core.Controls
{
    public class JReadOnlyLookupItemEditor : LookupItemEditor
    {
        public JReadOnlyLookupItemEditor()
        {
            IsReadOnly = true;
            AllowDefaultButton = false;
        }

        protected override void OnIsReadOnlyChanged()
        {
            IsReadOnly = true;
            AllowDefaultButton = false;
        }
    }
}
