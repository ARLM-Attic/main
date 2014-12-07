namespace SmartPrint.Model
{
    public class PrintFormVM : BaseVM
    {
        private string _postScriptFilePath;
        private bool _isColor;
        private bool _isMonochrome;
        private bool _isGrayscale;

        public PrintFormVM()
        {
            PostScriptFilePath = "Design time value";
        }

        public string PostScriptFilePath
        {
            get { return _postScriptFilePath; }
            set
            {
                if (_postScriptFilePath == value) return;
                _postScriptFilePath = value;
                OnPropertyChanged(() => PostScriptFilePath);
            }
        }

        public bool IsColor
        {
            get { return _isColor; }
            set
            {
                if (_isColor == value) return;
                _isColor = value;
                OnPropertyChanged(() => IsColor);
            }
        }

        public bool IsMonochrome
        {
            get { return _isMonochrome; }
            set
            {
                if (_isMonochrome == value) return;
                _isMonochrome = value;
                OnPropertyChanged(() => IsMonochrome);
            }
        }

        public bool IsGrayscale
        {
            get { return _isGrayscale; }
            set
            {
                if (_isGrayscale == value) return;
                _isGrayscale = value;
                OnPropertyChanged(() => IsGrayscale);
            }
        }
    }
}