using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Car;
using CarDisplay.Annotations;
using FORCEBuild.Helper;
using FORCEBuild.UI.WPF;

namespace CarDisplay
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Brand> _brands;
        private IEnumerable<Series> _series;
        private IEnumerable<Model> _models;

        public IEnumerable<Brand> Brands {
            get { return _brands; }
            set {
                if (Equals(value, _brands)) return;
                _brands = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Series> Series {
            get { return _series; }
            set {
                if (Equals(value, _series)) return;
                _series = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Model> Models {
            get { return _models; }
            set {
                if (Equals(value, _models)) return;
                _models = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<SubModel> SubModels {
            get { return _subModels; }
            set {
                if (Equals(value, _subModels)) return;
                _subModels = value;
                OnPropertyChanged();
            }
        }

        public Brand SelectedBrand {
            get { return _selectedBrand; }
            set {
                if (value == _selectedBrand) {
                    return;
                }
                SelectedSeries = null;
                _selectedBrand = value;
                if (value != null) {
                    Series = value.Series;
                }
            }
        }

        public Series SelectedSeries {
            get { return _selectedSeries; }
            set {
                if (value == _selectedSeries) {
                    return;
                }
                SelectedModel = null;
                _selectedSeries = value;
                Models = value?.Models;
            }
        }

        public Model SelectedModel {
            get { return _selectedModel; }
            set {
                if (value == _selectedModel) {
                    return;
                }
                SelectedSubModel = null;
                _selectedModel = value;
                SubModels = value?.SubModels.OrderByDescending((model => model.Year));
            }
        }

        public SubModel SelectedSubModel { get; set; }

        /// <summary>
        /// 变动缓存
        /// </summary>
        public IList<PropertyList> PropertyLists { get; set; }

        public IEnumerable<PropertyList> BindingPropertyLists {
            get { return _bindingPropertyLists; }
            set {
                if (Equals(value, _bindingPropertyLists)) return;
                _bindingPropertyLists = value;
                OnPropertyChanged();
            }
        }
        
        private CarContext _context;
        private Brand _selectedBrand;
        private IEnumerable<SubModel> _subModels;
        private Series _selectedSeries;
        private Model _selectedModel;
        private IEnumerable<PropertyList> _bindingPropertyLists;

        public MainWindowViewModel(CarContext context)
        {
            this._context = context;
            Brands = _context.Brands.ToList();
        }

        /// <summary>
        /// 刷新到绑定源
        /// </summary>
        private void RefreshBinding()
        {
            BindingPropertyLists = (from propertyList in PropertyLists
                let propertyLines = propertyList.PropertyLines.Where(line => !line.IsHide)
                select new PropertyList() {
                    Name = propertyList.Name,
                    PropertyLines = propertyLines.ToArray()
                }).ToList();
        }

        public ICommand RemoveModelCommand => new AnotherCommandImplementation((o => {
            var indexOf = PropertyLists[0].PropertyLines[0].ValueList.IndexOf(o.ToString());
            foreach (var propertyList in PropertyLists) {
                foreach (var line in propertyList.PropertyLines) {
                    line.ValueList.RemoveAt(indexOf);
                }
            }
        }));

        public ICommand AddModelCommand => new AnotherCommandImplementation((o => {
            if (PropertyLists == null) {
                var enumerable = SelectedSubModel.PropertyGroups.Select((group => {
                    return new PropertyList() {
                        Name = @group.Name,
                        PropertyLines = group.PropertyFields.Select((field => new PropertyLine() {
                            Name = field.Name,
                            ValueList = new ObservableCollection<string>(new[] {field.Value})
                        })).ToArray()
                    };
                }));
                PropertyLists = new ObservableCollection<PropertyList>(enumerable);
                RefreshBinding();
            }
            else {
                for (int i = 0; i < PropertyLists.Count; i++) {
                    var lines = PropertyLists[i].PropertyLines;
                    var fields = SelectedSubModel.PropertyGroups[i].PropertyFields;
                    for (int j = 0; j < lines.Length; j++) {
                        lines[j].ValueList.Add(fields[j].Value);
                    }
                }
            }
        }), (o => SelectedSubModel != null));

        public ICommand HidePropertyCommand => new AnotherCommandImplementation((o => {
            if ((bool)o) {
                foreach (var propertyList in PropertyLists) {
                    foreach (var line in propertyList.PropertyLines) {
                        line.IsHide = line.ValueList.All(s => s == line.ValueList[0]);
                    }
                }
            }
            else {
                foreach (var propertyList in PropertyLists) {
                    foreach (var line in propertyList.PropertyLines) {
                        line.IsHide = false;
                    }
                }
            }
            RefreshBinding();
        }), (o => PropertyLists != null));

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PropertyList
    {
        public string Name { get; set; }

        public PropertyLine[] PropertyLines { get; set; }
    }

    public class PropertyLine : INotifyPropertyChanged
    {
        private bool _isHide;
        public string Name { get; set; }

        public ObservableCollection<string> ValueList { get; set; }

        public bool IsHide {
            get { return _isHide; }
            set {
                if (value == _isHide) return;
                _isHide = value;
                OnPropertyChanged();
            }
        }

        //public PropertyLine()     
        //{
        //    ValueList = new ObservableCollection<string>();
        //}
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}