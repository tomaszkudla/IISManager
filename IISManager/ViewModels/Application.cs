using System.ComponentModel;

namespace IISManager.ViewModels
{
    public class Application : INotifyPropertyChanged
    {
        private string applicationPoolName;
        private string path;
        private string dirPath;
        private string webPath;

        public Application(string applicationPoolName, string path, string dirPath, string webPath)
        {
            this.applicationPoolName = applicationPoolName;
            this.path = path;
            this.dirPath = dirPath;
            this.webPath = webPath;
        }

        public string ApplicationPoolName
        {
            get { return applicationPoolName; }
            set
            {
                if (this.applicationPoolName != value)
                {
                    this.applicationPoolName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ApplicationPoolName"));
                }
            }
        }

        public string Path
        {
            get { return path; }
            set
            {
                if (this.path != value)
                {
                    this.path = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Path"));
                }
            }
        }

        public string DirPath
        {
            get { return dirPath; }
            set
            {
                if (this.dirPath != value)
                {
                    this.dirPath = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("DirPath"));
                }
            }
        }

        public string WebPath
        {
            get { return webPath; }
            set
            {
                if (this.webPath != value)
                {
                    this.webPath = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("WebPath"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
