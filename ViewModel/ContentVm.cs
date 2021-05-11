using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using task_09.MvvmBase;

namespace task_09.ViewModel
{
    class ContentVm : ViewModelBase
    {
        private StringBuilder _textSave = new StringBuilder();
        private StringBuilder _text;

        public string TextSave
        {
            get => _textSave.ToString();
            set
            {
                _textSave = new StringBuilder(value);
                OnPropertyChanged("TextSave");
                CompareText();
            }
        }

        private bool _textbox_enabled;
        public bool TextboxEnabled 
        {
            get { return _textbox_enabled; }
            set
            {
                _textbox_enabled = value;
                OnPropertyChanged("TextboxEnabled"); 
            }
        }

        private bool _quickDrawBarPinned;

        public bool QuickDrawBarPinned
        {
            get => _quickDrawBarPinned;
            set
            {
                _quickDrawBarPinned = value;
                OnPropertyChanged("QuickDrawBarPinned");
            }
        }

        private string filePath;

        public string WindowTitle { get; set; }

        public ICommand OpenDialog { get; set; }
        public ICommand SaveChangedText { get; set; }


        public ContentVm()
        {
            OpenDialog = new RelayCommand(GetFile, o => true);
            SaveChangedText = new RelayCommand(SaveChanged, o => true);
        }


        public void CompareText()
        {
            if (_text.ToString() != _textSave.ToString())
            {
                QuickDrawBarPinned = true;
            }
            else
            {
                QuickDrawBarPinned = false;
            }
            
        }

        public void GetFile(object param)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt"
            };

            TextboxEnabled = true;
            
            if (openFileDialog1.ShowDialog() == true)
            {
                filePath = openFileDialog1.FileName;

                WindowTitle = Path.GetFileName(filePath);
                OnPropertyChanged("WindowTitle");

                using var stream = new StreamReader(filePath);
                var text = stream.ReadToEnd();

                _text = new StringBuilder(text);
                TextSave = text;
            }
        }

        public void SaveChanged(object param)
        {
            try
            {
                using var stream = new StreamWriter(filePath, false);
                stream.Write(_textSave);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                MessageBox.Show(unauthorizedAccessException.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}