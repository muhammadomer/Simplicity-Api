namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class NaturalForm
    {
        string formId;
        public string form_ref
        {
            get { return formId; }
            set { formId = value; }
        }

        string formTitle;
        public string title
        {
            get { return formTitle; }
            set { formTitle = value; }
        }

        bool isAssetRequired;
        public bool IsAssetRequired
        {
            get { return isAssetRequired; }
            set { isAssetRequired = value; }
        }

        bool isSupplierRequired;
        public bool IsSupplierRequired
        {
            get { return isSupplierRequired; }
            set { isSupplierRequired = value; }
        }

        public NaturalForm(string formId, string formName, bool isAsset, bool isSupplier)
        {
            this.formId = formId;
            this.title = formName;
            this.isAssetRequired = isAsset;
            this.isSupplierRequired = isSupplier;
        }
    }
}
