using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Media;
using System.Resources;
using System.Windows.Threading;
using System.IO;
using System.Security.AccessControl;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int MaxCharacterSlider = 12;
        public MainWindow()

        {
            
            InitializeComponent();
            initialize();
            
            LoadObjects(this, null);
            LoadObjectsTab2(this, null);
            LoadObjectsTab3(this, null);
            LoadObjectsTab4(this, null);
            LoadObjectsTab5(this, null);
            LoadObjectsTab6(this, null);
            LoadObjectsTab7(this, null);
        }

        
        public void initialize()
        {
            this.slider1textBoxMaxValue.Text = "Z";
            slide1MinValue.Text = "A";
            textBox4.Text = "C:\\";
            slider1.Minimum = 0;
            slider1.Maximum = MaxCharacterSlider;
            slider2.Maximum = MaxCharacterSlider;
            slider3.Maximum = MaxCharacterSlider;
            slider4.Maximum = MaxCharacterSlider;
            slider2.Value = MaxCharacterSlider;
            slider4.Value = MaxCharacterSlider;
            //slide1MinValue.Text = "";
            listBox1.ItemsSource = null;

        }

        private void MainBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            LoadObjects(sender, e);
            LoadObjectsTab2(this, null);
            LoadObjectsTab3(this, null); 
            LoadObjectsTab4(this, null);
            LoadObjectsTab5(this, null);
            LoadObjectsTab6(this, null);
            LoadObjectsTab7(this, null);
        }

        private void FileListComboBoxTab1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // int x = 0;
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox2.IsChecked == true)
            { textBox1.IsEnabled = true; }
            else
            { textBox1.IsEnabled = false; }
        }

        private void checkBox3_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox3.IsChecked == true)
            { textBox3.IsEnabled = true; }
            else { textBox3.IsEnabled = false; }
        }

        public bool ValidateDirectory(string s)
        {
            bool returnvalue = false;
            returnvalue = Directory.Exists(s);

            return returnvalue;
        }

        public bool ValidateIsRootDirectory(string s)
        {
            bool isRoot = false;
            if (Directory.GetDirectoryRoot(s) == s)
                isRoot = true;
            return isRoot;
        
        }

        public bool isThisHidden(string s)
        {
            bool isHidden = false;
            string tmp = textBox4.Text;
            DirectoryInfo directory = new DirectoryInfo(@tmp);                       
            int fileCount = 0;

            if (ValidateIsRootDirectory(tmp))
                tmp = tmp + s;
            else
                tmp = tmp + @"\" + s;
            if(ValidateDirectory(tmp))
            {
                DirectoryInfo[] lisdir = directory.GetDirectories(s );
                var t = lisdir.Select(f => f).Where(f => (f.Attributes & FileAttributes.Hidden) != 0);
                fileCount = t.Count(); 
            }
            else
            {
                FileInfo[] files = directory.GetFiles(s ); 
                var t = files.Select(f => f).Where(f => (f.Attributes & FileAttributes.Hidden) != 0);
                fileCount = t.Count();
            }

            if (fileCount != 0)
                isHidden = true;
            return isHidden;

        }

        public void LoadAllObjectTabs()
        {
            LoadObjects(this, null);
            LoadObjectsTab2(this, null);
            LoadObjectsTab3(this, null);
            LoadObjectsTab4(this, null);
            LoadObjectsTab5(this, null);
            LoadObjectsTab6(this, null);
            LoadObjectsTab7(this, null);
        
        }

        public void LoadObjects(object sender, RoutedEventArgs e)
        {
            
            string emptystring = string.Empty;
            string directoryName = string.Empty;
           directoryName = textBox4.Text;
           
            if ((directoryName != "") || (directoryName != string.Empty))
            {
                if (ValidateDirectory(directoryName))
                {
                    var list = Directory.EnumerateDirectories(directoryName);
                    var list2 = Directory.EnumerateFiles(directoryName);
                    List<string> directoryList = new List<string>();
                    List<string> fileList = new List<string>();
                    List<string> extensionList = new List<string>();
                    //do something.
                    foreach (string directorynameitem in list)
                    {
                        string temp=emptystring;
                        if (ValidateIsRootDirectory(@directoryName))
                        {
                            temp = directorynameitem.Replace(@directoryName, emptystring).ToString();
                        }
                        else
                        {
                            temp = directorynameitem.Replace(@directoryName + @"\", emptystring).ToString();
                        }
                        string MinLetter = slide1MinValue.Text;
                        string MaxLetter= slider1textBoxMaxValue.Text;


                        if (Char.ConvertToUtf32((MinLetter), 0) <= Char.ConvertToUtf32((temp[0].ToString().ToUpper()), 0)
                        &&
                        Char.ConvertToUtf32(MaxLetter, 0) >= Char.ConvertToUtf32(temp[0].ToString().ToUpper(), 0)

                        )
                        {
                            if (!isThisHidden(temp))
                            {

                                directoryList.Add(temp);
                            }
                        }                       

                    }
                    foreach (string filenameitem in list2)
                    {
                        string temp = filenameitem.ToString().Replace(@directoryName, emptystring);
                        string MinLetter = slide2MinValue.Text;
                        string MaxLetter = slide2MaxValue.Text; ;

                        if (Char.ConvertToUtf32(MinLetter, 0) <= Char.ConvertToUtf32(temp[0].ToString().ToUpper(), 0)
                            &&
                            Char.ConvertToUtf32(MaxLetter, 0) >= Char.ConvertToUtf32(temp[0].ToString().ToUpper(), 0)
                            )
                        {

                            fileList.Add(temp);
                        }

                    }
                    foreach (string extitem in fileList)
                    {
                        extensionList.Add(extitem.ToLower().Substring(extitem.IndexOf('.'), 4));
                    }
                    var extentions = extensionList.Distinct();
                    listBoxDirectoryTab1.ItemsSource = directoryList;
                    listBoxFileTab1.ItemsSource = fileList;                    
                    ExtensioncomboBox1Tab1.ItemsSource = extentions;

                    if (ExtensioncomboBox1Tab1.SelectedIndex != -1)
                    {
                        List<string> NewfileList = new List<string>();
                        string temp = ExtensioncomboBox1Tab1.SelectedValue.ToString();
                        foreach (string extitem in fileList)
                        {
                            if (extitem.Contains(temp))
                                NewfileList.Add(extitem);
                        }
                        listBoxFileTab1.ItemsSource = NewfileList;

                    }
                }


            }
          


        }

        public void LoadObjectsTab2(object sender, RoutedEventArgs e)
        {
            List<string> x = new List<string>(); 
            for(int i=0;i<listBoxFileTab1.Items.Count; i++)
            {string tmp=listBoxFileTab1.Items[i].ToString().ToLower();
            if (tmp.Contains(".mp3") || tmp.Contains(".wma") || tmp.Contains(".mp4"))
            {
                x.Add(listBoxFileTab1.Items[i].ToString());
            }
            
            }
            listBox1.ItemsSource = x;
        
        }

        public void LoadObjectsTab3(object sender, RoutedEventArgs e)
        { 
            List<string> x = new List<string>(); 
            for (int i = 0; i < listBoxDirectoryTab1.Items.Count; i++)
            {
                string tmp = listBoxDirectoryTab1.Items[i].ToString().ToLower();
                if(tmp!="$recycle.bin" && tmp!="documents and settings" && tmp!="recovery" && tmp!="system volume information")
                x.Add(tmp.ToString());                
            }
            listBox2.ItemsSource = null;
            listBox2.ItemsSource = x;
        }

        public void LoadObjectsTab4(object sender, RoutedEventArgs e)
        {
            string tmp = textBox4.Text;
            DirectoryInfo directory = new DirectoryInfo(@tmp);
            FileInfo[] files = directory.GetFiles();
            DirectoryInfo[] lisdir = directory.GetDirectories();
            List<string> x = new List<string>();
           var x2= Directory.EnumerateFileSystemEntries(tmp);

           var t = files.Select(f => f).Where(f => (f.Attributes & FileAttributes.Hidden) != 0);
           foreach (var fileItem in t)
           {
               x.Add(fileItem.Name);
           }

           listBox6.ItemsSource = x;
           var x3 = Directory.GetFileSystemEntries(tmp);
           var t2 = lisdir.Select(f => f).Where(f => (f.Attributes & FileAttributes.Hidden) != 0);
           List<string> dirList = new List<string>();
           foreach (var fileItem in t2)
           {
               dirList.Add(fileItem.Name);
           }

           listBox5.ItemsSource = dirList;
           textBox2.Text= "Root: "+ Directory.GetDirectoryRoot(tmp);
           textBox5.Text ="Current Directory: "+ Directory.GetCurrentDirectory();
           textBox6.Text= "Directory Root: "+Directory.GetDirectoryRoot(textBox5.Text); 
           // textBox7.Text="Parent: "+ Directory.GetParent(textBox5.Text).ToString();
           listBox7.ItemsSource = Directory.GetLogicalDrives();
           listView1.ItemsSource = Directory.GetLogicalDrives();
        }

        public void LoadObjectsTab5(object sender, RoutedEventArgs e)
        {
            string tmp = textBox4.Text;
            PopulateListView(listView2, tmp);            
        }

        public void LoadObjectsTab6(object sender, RoutedEventArgs e)
        {
            //
            listBox9.SelectionMode = SelectionMode.Multiple;
            List<string> x = new List<string>();
            List<string> x2 = new List<string>();
            for (int i = 0; i < listBoxFileTab1.Items.Count; i++)
            {
                string tmp = listBoxFileTab1.Items[i].ToString().ToLower();
                //if (tmp.Contains(".mp3") || tmp.Contains(".wma") || tmp.Contains(".mp4"))
                {
                    x.Add(listBoxFileTab1.Items[i].ToString());
                }

            }
            for (int i = 0; i < listBoxDirectoryTab1.Items.Count; i++)
            {
                string tmp = listBoxDirectoryTab1.Items[i].ToString();
                x2.Add(tmp);
            }

            listBox8.ItemsSource = x2;
            listBox9.ItemsSource = x;
            for (int i = 0; i < listBoxFileTab1.Items.Count; i++)
            {
                string tmp = listBoxFileTab1.Items[i].ToString().ToLower();
                if (tmp.StartsWith("."))
                {                    
                    listBox9.SelectedItems.Add(listBoxFileTab1.Items[i]);
                }
                
               
            }
        }

        public void LoadObjectsTab7(object sender, RoutedEventArgs e)
        {
            List<string> x = new List<string>();
            x.Add("(root)");
            for (int i = 0; i < listBoxDirectoryTab1.Items.Count; i++)
            {
                string tmp = listBoxDirectoryTab1.Items[i].ToString();
                x.Add(tmp);
            }
            listBox10.ItemsSource = x;
            listBox11.Visibility = Visibility.Hidden;

            //listBox11.ItemsSource = listBoxFileTab1.ItemsSource;
        }

        public void PopulateListView(ListView destination, string FileLocation)
        {
            string tmp = FileLocation;
            bool LoadView = true;
            destination.Items.Clear();
            if (destination.Name == "listView3") 
            {                
                listView4.Items.Clear();
            }
            

            if (!Directory.Exists(tmp))
                LoadView = false;
            //if (Directory.GetAccessControl(tmp).AreAccessRulesProtected)              LoadView = false;
            //if(Directory.GetDirectoryRoot(@tmp) == @tmp && Directory.GetAccessControl(tmp).AreAccessRulesProtected)                { LoadView = true; }
            //List<string> a = new List<string>();
           
            if(LoadView)
            {
                
                GridView gridview6 = new GridView();
                GridViewColumn c1 = new GridViewColumn();
                GridViewColumn c2 = new GridViewColumn();
                GridViewColumn c3 = new GridViewColumn();
                GridViewColumn c4 = new GridViewColumn();
                GridViewColumn c5 = new GridViewColumn();
                int columnwidth = 150;//int.Parse( (listView4.Width / 3).ToString());
                c1.Header = "File Name";
                c2.Header = "File Size";
                c3.Header = "File Type";
                c4.Header = "Last Modified";
                c1.Width = columnwidth;
                c2.Width = columnwidth;
                c3.Width = columnwidth;
                c4.Width = columnwidth;
                gridview6.Columns.Add(c1);
                gridview6.Columns.Add(c2);
                gridview6.Columns.Add(c3);
                gridview6.Columns.Add(c4);
                gridview6.Columns.Add(c5);
                c1.DisplayMemberBinding = new Binding("FName");
                c2.DisplayMemberBinding = new Binding("FSize");
                c3.DisplayMemberBinding = new Binding("FType");
                c4.DisplayMemberBinding = new Binding("FDate");
                c5.DisplayMemberBinding = new Binding("FD");
                
                destination.View = gridview6;
                loadViewItems( tmp, destination, "D" );
                loadViewItems(tmp, destination, "F");
                ListView SortView = new ListView();
               
            }
            if (!LoadView) {
                destination.Items.Clear();
            }
        }

        public void loadViewItems(string tmp, ListView destination, string FileOrDirectory )
        {
            List<string> x = new List<string>();                        
            DirectoryInfo directory = new DirectoryInfo(@tmp);
            FileInfo[] files = directory.GetFiles();
            DirectoryInfo[] lisdir = directory.GetDirectories();

            if (FileOrDirectory == "D")
            {
                var t = lisdir.Select(f => f) .Where(f => (f.Attributes & FileAttributes.Hidden) == 0);
                foreach (var fileItem in t) 
                {
                    x.Add(fileItem.FullName);
                }
            }
            else
            { 
                var t = files.Select(f => f).Where(f => (f.Attributes & FileAttributes.Hidden) == 0);
                foreach (var fileItem in t)
                {
                    x.Add(fileItem.FullName);
                }
            
            }
            foreach (var fileItem in x)
            {
                FileInfo f = new FileInfo(fileItem);                
                string filename;
                string comparisonstring;
                if (Directory.GetDirectoryRoot(@tmp) == @tmp)
                {
                    filename = fileItem.Replace(@tmp, "");
                    comparisonstring = @tmp + filename;
                }
                else
                {
                    filename = fileItem.Replace(@tmp + @"\", "");
                    comparisonstring = @tmp + @"\" + filename;
                }
                string isFile = "D";
                string filesize = string.Empty;
                string fileAttrib = string.Empty;

                if (!Directory.Exists(comparisonstring))
                {
                    filesize = ConvertFileSize(f.Length); 
                    isFile = "F";
                   
                }
                else 
                {
                    isFile = "D";
                }
                string lastModified = f.LastAccessTime.ToShortDateString() + " " + f.LastAccessTime.ToShortTimeString(); ; ;
                string fileType = showFileType(f.Extension);
                destination.Items.Add(new { FName = filename, FSize = filesize, FType = fileType, FDate = lastModified, FD = isFile });                
            }
        }

        public string createImage(string extension)
        {
            string tmp=string.Empty;
            return tmp;
        
        }

        public string showFileType(string extension)
        { string tmp=string.Empty;
         switch (extension.ToLower())
            {
             case "": return tmp="File Folder";
             case "docx": return tmp = "Word File";
             case ".doc": return tmp = "Word File";
             case ".htm": return tmp = "HTML Document";
             case ".html": return tmp = "HTML Document";
             case ".xhtml": return tmp = "HTML Document";
             case ".shtml": return tmp = "HTML Document";
            default: return tmp=extension.ToUpper().Replace(".","")+" File";
            }
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            double valueDouble = slider1.Value;
            int valueInteger = Int32.Parse(Math.Ceiling(valueDouble).ToString());
            slide1MinValue.Text = Char.ConvertFromUtf32(65 + valueInteger).ToString();
        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextBox tb = new TextBox();
            
            double valueDouble = slider2.Value;
            int valueInteger = Int32.Parse(Math.Ceiling(valueDouble).ToString());
            int mathvalue = 78 + valueInteger;
            string test = Char.ConvertFromUtf32(mathvalue);            
            tb.Text = test;

            slider1textBoxMaxValue.Text = test;
        }

        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            double valueDouble = slider3.Value;
            int valueInteger = Int32.Parse(Math.Ceiling(valueDouble).ToString());
            slide2MinValue.Text = Char.ConvertFromUtf32(65 + valueInteger).ToString();

        }

        private void slider4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double valueDouble = slider4.Value;
            int valueInteger = Int32.Parse(Math.Ceiling(valueDouble).ToString());
            slide2MaxValue.Text = Char.ConvertFromUtf32(65 + 13 + valueInteger).ToString();
        }

        private void ExtensioncomboBox1Tab1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get with extensions
            string extension = string.Empty;string emptystring = string.Empty;
            if (ExtensioncomboBox1Tab1.SelectedIndex != -1)
            {
                extension = ExtensioncomboBox1Tab1.SelectedValue.ToString(); ;
                List<string> fileList = new List<string>();
                string directoryName = directoryName = textBox4.Text;
                var list2 = Directory.EnumerateFiles(directoryName);
                string MinLetter =slide2MinValue.Text; ;
                string MaxLetter = slide2MaxValue.Text;
                foreach (string filenameitem in list2)
                {
                    string temp = filenameitem.ToString().Replace(@directoryName, emptystring);

                    if (Char.ConvertToUtf32(MinLetter, 0) <= Char.ConvertToUtf32(temp[0].ToString().ToUpper(), 0)
                                &&
                                Char.ConvertToUtf32(MaxLetter, 0) >= Char.ConvertToUtf32(temp[0].ToString().ToUpper(), 0))
                            {
                             if(temp.Contains(extension))   
                                fileList.Add(temp);
                            }
                        }
                listBoxFileTab1.ItemsSource = fileList;
            }
        }

        private void slide1MinValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tmp;
            try {
                tmp = textBox4.Text;
            }
            catch
            {
                return;
            }
            LoadObjects(this, null);
        }

        private void slider1textBoxMaxValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tmp;
            try
            {
                tmp = textBox4.Text;
            }
            catch
            {
                return;
            }
            LoadObjects(this, null);
        }

        private void slide2MinValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tmp;
            try
            {
                tmp = textBox4.Text;
            }
            catch
            {
                return;
            } 
            LoadObjects(this, null);
        }

        private void slide2MaxValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            string tmp;
            try
            {
                tmp = textBox4.Text;
            }
            catch
            {
                return;
            } LoadObjects(this, null);
        }

        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                string tmp = textBox4.Text + listBox2.SelectedValue.ToString();
                List<string> directoryList = new List<string>();
                var list = Directory.EnumerateDirectories(tmp);
                foreach (string DirItem in list)
                { 
                    directoryList.Add(DirItem.Replace(tmp+"\\", "")); 
                }
                listBox3.ItemsSource = directoryList;
            }
        }

        private void listBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // int x = 0;
            if (listBox3.SelectedIndex != -1)
            {
                string tmp = textBox4.Text + listBox2.SelectedValue.ToString() +"\\"+ listBox3.SelectedValue.ToString();
                List<string> FileList = new List<string>();
                var list = Directory.EnumerateFiles(tmp);
                foreach (string FileItem in list)
                {
                    FileList.Add(FileItem.Replace(tmp+"\\", ""));
                }
                listBox4.ItemsSource = FileList;
            }
        }

        public string ConvertFileSize(long fileSize)
        {
            int byteNum = 1024;
            string tmp = string.Empty;
            if (fileSize <= byteNum)
            { 
                return tmp = "1 KB"; 
            }
            if (fileSize < Math.Pow( byteNum,2))
            {
                return tmp = (fileSize / byteNum).ToString("N0") + " KB";
            }
            if (fileSize < Math.Pow(byteNum, 3))
            {
                return tmp = (fileSize / Math.Pow(byteNum, 2)).ToString("N0") + " MB";
            }
            if (fileSize < Math.Pow(byteNum, 4))
            {
                return tmp = (fileSize / Math.Pow(byteNum, 3)).ToString("N0") + " GB";
            }
            if (fileSize < Math.Pow(byteNum, 5))
            {
                return tmp = (fileSize / Math.Pow(byteNum, 4)).ToString("N0") + " TB";
            }
            return tmp;
        }

        private void listBox4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Int32 size = 0;
            if (listBox4.SelectedIndex != -1)
            {
                string tmp = String.Empty;
                if (listBox4.SelectedItems.Count == 1)
                {
                    tmp = textBox4.Text + listBox2.SelectedValue.ToString() + "\\" + listBox3.SelectedValue.ToString() + "\\" + listBox4.SelectedValue.ToString();
                    if (File.Exists(tmp))
                    {
                        //List<string> s = File.GetAttributes(tmp);
                        FileAttributes s = new FileAttributes();
                        s = File.GetAttributes(tmp);
                        FileInfo f = new FileInfo(tmp);

                        label11.Content = ConvertFileSize(f.Length);

                    }
                }
                else
                {
                    for (int i = 0; i < listBox4.SelectedItems.Count; i++)
                    {
                        tmp = textBox4.Text + listBox2.SelectedValue.ToString() + "\\" + listBox3.SelectedValue.ToString() + "\\" + listBox4.SelectedItems[i].ToString();
                        FileInfo f = new FileInfo(tmp);
                        size += Int32.Parse( f.Length.ToString());
                    }
                    label11.Content = ConvertFileSize(size);
                }
            
            }
        }

        private void listView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listView2.SelectedIndex != -1)
            {
                string tmp = listView2.SelectedValue.ToString();
                label12.Content = listView2.SelectedValue;
                List<string> selectedListItem = new List<string>();
                selectedListItem.Add(listView2.SelectedValue.ToString());
                int firstfewchar="{ FNAME= ".Count();
                tmp = tmp.Substring(firstfewchar, tmp.IndexOf(",")-firstfewchar);
                label12.Content = textBox4.Text+ tmp.Trim();
                tmp = textBox4.Text + tmp.Trim();
                PopulateListView(listView3, tmp);   
            
            }
        }

        private string getFileNamefromListView(string fileNameString)
        {
            string tmp=String.Empty;
            tmp = fileNameString;
            int firstfewchar = "{ FNAME= ".Count();
            tmp = tmp.Substring(firstfewchar, tmp.IndexOf(",") - firstfewchar);
            tmp = tmp.Trim();
            return tmp;
        }

        private void listView3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tmp;
            if (listView3.SelectedIndex != -1)
            {
                tmp = textBox4.Text + getFileNamefromListView(listView2.SelectedValue.ToString()) + @"\" + getFileNamefromListView(listView3.SelectedValue.ToString());
                label12.Content = tmp;
                PopulateListView(listView4, tmp);  
            
            }
        }

        private void listBoxDirectoryTab1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tmp=listBoxDirectoryTab1.SelectedValue.ToString();
            tmp=tmp+" is hidden: " + isThisHidden(tmp).ToString();
            statusBar1.Items.Clear();
            statusBar1.Items.Add(tmp);
            statusBar1.HorizontalAlignment=HorizontalAlignment.Right;
        }

       
        private void listBox10_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            string tmp=textBox4.Text;
            if (listBox10.SelectedIndex != -1) 
            {
                if (listBox10.SelectedIndex == 0)
                {
                    listBox10.SelectedIndex = -1;
                    if(textBox4.Text!=Directory.GetDirectoryRoot(textBox4.Text))
                    textBox4.Text = Directory.GetParent(textBox4.Text).FullName;
                    LoadAllObjectTabs();
                //do nothing for now.
                }
                else
                {
                    if (ValidateIsRootDirectory(tmp))
                    {
                        textBox4.Text = textBox4.Text  + listBox10.SelectedValue;
                        
                    }
                    else
                    { textBox4.Text = textBox4.Text + @"\" + listBox10.SelectedValue; }
                    LoadAllObjectTabs();
                }
            
            }
        }

        private void listBox9_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            label13.Content = "Index: " + listBox9.SelectedIndex;
            label14.Content = "Item: " + listBox9.SelectedItem;
        }


    }
}
