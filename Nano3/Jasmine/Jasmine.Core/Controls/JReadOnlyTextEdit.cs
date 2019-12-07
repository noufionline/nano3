using DevExpress.Xpf.Editors;

namespace Jasmine.Core.Controls
{
    public class JReadOnlyTextEdit : TextEdit
    {
        public JReadOnlyTextEdit()
        {
            IsReadOnly = true;
        }

        protected override void OnIsReadOnlyChanged()
        {
            IsReadOnly = true;
        }
    }
}
