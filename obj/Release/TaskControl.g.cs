﻿#pragma checksum "..\..\TaskControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D1267417A0137AFD07CE8C3F8CFB2A714517A0D1C527127B1E14CAF582907F18"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TestMakerWPF_FirstInstance;


namespace TestMakerWPF_FirstInstance {
    
    
    /// <summary>
    /// TaskControl
    /// </summary>
    public partial class TaskControl : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\TaskControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TitleTB;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\TaskControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RichTextBox TaskTestTB;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\TaskControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox AnswerFieldTB;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\TaskControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ApplyB;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\TaskControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox DB_Dropdown;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TestMakerWPF_FirstInstance;component/taskcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\TaskControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.TitleTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.TaskTestTB = ((System.Windows.Controls.RichTextBox)(target));
            return;
            case 3:
            this.AnswerFieldTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.ApplyB = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\TaskControl.xaml"
            this.ApplyB.Click += new System.Windows.RoutedEventHandler(this.ApplyB_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.DB_Dropdown = ((System.Windows.Controls.ComboBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

