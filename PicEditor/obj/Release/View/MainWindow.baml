   M S B A M L   `   `   `  ���� c  _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  D @NDragDrop, Version=0.3.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:NDragDrop;assembly=NDragDrop	NDragDrop D @PicEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:PicEditor.ViewModelPicEditor.ViewModel L HJetControlLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:JetControlLibrary;assembly=JetControlLibraryJetControlLibrary /clr-namespace:PicEditor.ViewPicEditor.View 9!clr-namespace:PicEditor.ViewModelPicEditor.ViewModel    PicEditor.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Z VDevExpress.Mvvm.UI, Version=19.2.0.0, Culture=neutral, PublicKeyToken=382ad28f6ae31f30F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     a	 ]GongSolutions.WPF.DragDrop, Version=2.0.0.0, Culture=neutral, PublicKeyToken=91f1945125b7a587ddurn:gong-wpf-dragdrop 	 b
 ^GalaSoft.MvvmLight.Platform, Version=5.4.1.0, Culture=neutral, PublicKeyToken=5f873c45e98af8a1(j http://www.galasoft.ch/mvvmlight 
 4ndd*clr-namespace:NDragDrop;assembly=NDragDrop 
 -local!clr-namespace:PicEditor.ViewModel 
 Dpic:clr-namespace:JetControlLibrary;assembly=JetControlLibrary 
 'viewclr-namespace:PicEditor.View 
 *vm!clr-namespace:PicEditor.ViewModel 
 -   $��MainWin��  � Title$  
MainWindow��$	��690��$��690.2��$	��690��$	��665�� � DataContext &  PicEditor.ViewModel.MainWindowVM  .   (System.Windows.Interactivity.Interaction   Triggers /   )System.Windows.Interactivity.EventTrigger     	EventName$ 	MouseMove��.   (System.Windows.Interactivity.TriggerBase   Actions. / 
 )GalaSoft.MvvmLight.Command.EventToCommand     PassEventArgsToCommand .    Command �� *WinMouseMove+  $ Loaded��. 6   0System.Windows.Interactivity.InvokeCommandAction     Command �� *WindowLoaded+.��� ���	#FF4F4F4F.+��� $��Button��$
��Left=�$��	25,72,0,0q�$	��Top=�$��95��$��45������ *Ok+�� $
��Left=�$	��Top=�$��165,55,0,20q�$	��580��$	��500��.���� $
��Left=�$	��Top=�$
��auto��$
��auto������ *
ImageItems+���� .R��  J� 	AllowDrop . $
��auto��$
��auto������ .R�� $	��165��.+���  . $��0,0,13,0q����� *Icon+���� *Width+���� *Height+   $ MouseLeftButtonDown��.    �� *
ImageClick+	   CommandParameter	 ��   $
 Drop��.    .  �� *	ImageDrop+  $ MouseRightButtonDown��.    �� *ShowFullImage+�� ���	#FF056EB6$��12(�$
��auto��$��15��$	��150��$��Center=�$	��Top=�$��-13,155,0,-5q����� *Name+�� $#��Открыть 
  папку��$
��Left=�$��
25,290,0,0q�$	��Top=�$��95��$��51�����	#FF056EB6$��Center=�$��Center=����� *
OpenFolder+�� $
��Left=�$	��150��$	��150��$��	6,126,0,0q�$	��Top=��� $
��Left=�$	��Top=�$��
165,14,0,0q�$	��120��$��24��
 �� SelectedIndex
 �� *SortParamIndex+.@��� $��По имени���� $"��По дате созданя���� $&�� По дате изменения��   $ SelectionChanged��.    �� *SortParamChanged+�� $
��Sort��$
��Left=�$��
297,14,0,0q�$	��Top=�$��56��$��24������ *Sort+�� $
��Left=�$	��Top=�$��24��$	��142��$��
415,14,0,0q� �� TextWrapping$ NoWrap=����� *	NewName+�� $��Rename��$
��Left=�$	��Top=�$��
574,16,0,0q�$��75������ *	RenameAll+&   JetControlLibrary.PictureControl  #  PicEditor.View.PictureControl     BackgroundOpacity$	 0.9Q�y��� *PictureVisibility+   ImageSource �� *PictureSource+   BGMouseLeftButtonDown �� *Test+