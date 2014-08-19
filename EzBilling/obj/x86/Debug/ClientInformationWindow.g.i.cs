﻿#pragma checksum "..\..\..\ClientInformationWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E4686F4A0B6A16D47B91B2CBF372F61F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
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


namespace EzBilling {
    
    
    /// <summary>
    /// ClientInformationWindow
    /// </summary>
    public partial class ClientInformationWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\ClientInformationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox clients_ComboBox;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\ClientInformationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox clientName_TextBox;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\ClientInformationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox clientStreet_TextBox;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\ClientInformationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox clientCity_TextBox;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\ClientInformationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox clientPostalCode_TextBox;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\ClientInformationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button saveClientInformation_Button;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\ClientInformationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button deleteClientInformation_Button;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\ClientInformationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button resetFields_Button;
        
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
            System.Uri resourceLocater = new System.Uri("/EzBilling;component/clientinformationwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ClientInformationWindow.xaml"
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
            this.clients_ComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 39 "..\..\..\ClientInformationWindow.xaml"
            this.clients_ComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.clients_ComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.clientName_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 43 "..\..\..\ClientInformationWindow.xaml"
            this.clientName_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.clientName_TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.clientStreet_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.clientCity_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.clientPostalCode_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.saveClientInformation_Button = ((System.Windows.Controls.Button)(target));
            
            #line 61 "..\..\..\ClientInformationWindow.xaml"
            this.saveClientInformation_Button.Click += new System.Windows.RoutedEventHandler(this.saveClientInformation_Button_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.deleteClientInformation_Button = ((System.Windows.Controls.Button)(target));
            
            #line 62 "..\..\..\ClientInformationWindow.xaml"
            this.deleteClientInformation_Button.Click += new System.Windows.RoutedEventHandler(this.deleteClientInformation_Button_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.resetFields_Button = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\..\ClientInformationWindow.xaml"
            this.resetFields_Button.Click += new System.Windows.RoutedEventHandler(this.resetFields_Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

