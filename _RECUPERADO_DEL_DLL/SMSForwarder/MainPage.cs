using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using Android.Content;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Xaml.Internals;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;
using SMSForwarder.Services;
using SocShared;

namespace SMSForwarder;

[XamlFilePath("MainPage.xaml")]
public class MainPage : ContentPage
{
	[CompilerGenerated]
	private sealed class _003CInitializeComponent_003E_anonXamlCDataTemplate_1
	{
		internal object[] parentValues;

		internal MainPage root;

		internal NameScope _scope0;

		internal NameScope _scope1;

		internal NameScope _scope2;

		internal NameScope _scope3;

		internal NameScope _scope4;

		internal NameScope _scope5;

		internal NameScope _scope6;

		internal NameScope _scope7;

		internal NameScope _scope8;

		internal NameScope _scope9;

		internal NameScope _scope10;

		internal NameScope _scope11;

		internal NameScope _scope12;

		internal NameScope _scope13;

		internal NameScope _scope14;

		internal NameScope _scope15;

		internal NameScope _scope16;

		internal NameScope _scope17;

		internal NameScope _scope18;

		internal NameScope _scope19;

		internal NameScope _scope20;

		internal NameScope _scope21;

		internal NameScope _scope22;

		internal NameScope _scope23;

		internal NameScope _scope24;

		internal NameScope _scope25;

		internal NameScope _scope26;

		internal NameScope _scope27;

		internal NameScope _scope28;

		internal NameScope _scope29;

		internal NameScope _scope30;

		internal NameScope _scope31;

		internal NameScope _scope32;

		internal NameScope _scope33;

		internal NameScope _scope34;

		internal NameScope _scope35;

		internal NameScope _scope36;

		internal NameScope _scope37;

		internal NameScope _scope38;

		internal NameScope _scope39;

		internal NameScope _scope40;

		internal NameScope _scope41;

		internal NameScope _scope42;

		internal NameScope _scope43;

		internal NameScope _scope44;

		internal NameScope _scope45;

		internal NameScope _scope46;

		internal NameScope _scope47;

		internal NameScope _scope48;

		internal NameScope _scope49;

		internal NameScope _scope50;

		internal NameScope _scope51;

		internal NameScope _scope52;

		internal NameScope _scope53;

		internal NameScope _scope54;

		internal NameScope _scope55;

		internal NameScope _scope56;

		internal NameScope _scope57;

		internal NameScope _scope58;

		internal NameScope _scope59;

		internal NameScope _scope60;

		internal NameScope _scope61;

		internal NameScope _scope62;

		internal NameScope _scope63;

		internal NameScope _scope64;

		internal NameScope _scope65;

		internal NameScope _scope66;

		internal NameScope _scope67;

		internal NameScope _scope68;

		internal NameScope _scope69;

		internal NameScope _scope70;

		internal NameScope _scope71;

		internal NameScope _scope72;

		internal NameScope _scope73;

		internal NameScope _scope74;

		internal NameScope _scope75;

		internal NameScope _scope76;

		internal NameScope _scope77;

		internal NameScope _scope78;

		internal NameScope _scope79;

		internal NameScope _scope80;

		internal NameScope _scope81;

		internal NameScope _scope82;

		internal NameScope _scope83;

		internal NameScope _scope84;

		internal NameScope _scope85;

		internal NameScope _scope86;

		internal NameScope _scope87;

		internal NameScope _scope88;

		internal NameScope _scope89;

		internal NameScope _scope90;

		internal NameScope _scope91;

		internal NameScope _scope92;

		internal NameScope _scope93;

		internal NameScope _scope94;

		internal NameScope _scope95;

		internal NameScope _scope96;

		internal NameScope _scope97;

		internal NameScope _scope98;

		internal NameScope _scope99;

		internal NameScope _scope100;

		internal NameScope _scope101;

		internal NameScope _scope102;

		internal NameScope _scope103;

		internal NameScope _scope104;

		internal NameScope _scope105;

		internal NameScope _scope106;

		internal NameScope _scope107;

		internal NameScope _scope108;

		internal NameScope _scope109;

		internal NameScope _scope110;

		internal NameScope _scope111;

		internal NameScope _scope112;

		internal NameScope _scope113;

		internal NameScope _scope114;

		internal NameScope _scope115;

		internal NameScope _scope116;

		internal NameScope _scope117;

		internal NameScope _scope118;

		internal NameScope _scope119;

		internal NameScope _scope120;

		internal NameScope _scope121;

		internal NameScope _scope122;

		internal NameScope _scope123;

		internal NameScope _scope124;

		internal NameScope _scope125;

		internal NameScope _scope126;

		internal NameScope _scope127;

		internal NameScope _scope128;

		internal NameScope _scope129;

		internal NameScope _scope130;

		internal NameScope _scope131;

		internal NameScope _scope132;

		internal NameScope _scope133;

		internal NameScope _scope134;

		internal NameScope _scope135;

		internal NameScope _scope136;

		internal NameScope _scope137;

		internal NameScope _scope138;

		internal NameScope _scope139;

		internal NameScope _scope140;

		internal NameScope _scope141;

		internal NameScope _scope142;

		internal NameScope _scope143;

		internal NameScope _scope144;

		internal NameScope _scope145;

		internal NameScope _scope146;

		internal NameScope _scope147;

		internal NameScope _scope148;

		internal NameScope _scope149;

		internal NameScope _scope150;

		internal NameScope _scope151;

		internal NameScope _scope152;

		internal NameScope _scope153;

		internal NameScope _scope154;

		internal NameScope _scope155;

		internal NameScope _scope156;

		internal NameScope _scope157;

		internal NameScope _scope158;

		internal NameScope _scope159;

		internal object LoadDataTemplate()
		{
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Expected O, but got Unknown
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Expected O, but got Unknown
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Expected O, but got Unknown
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Expected O, but got Unknown
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Expected O, but got Unknown
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Expected O, but got Unknown
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Expected O, but got Unknown
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0535: Expected O, but got Unknown
			//IL_0535: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Expected O, but got Unknown
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Expected O, but got Unknown
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_054a: Expected O, but got Unknown
			//IL_054a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Expected O, but got Unknown
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_0558: Expected O, but got Unknown
			//IL_0558: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Expected O, but got Unknown
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Expected O, but got Unknown
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Expected O, but got Unknown
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Expected O, but got Unknown
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0605: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0677: Unknown result type (might be due to invalid IL or missing references)
			//IL_067c: Unknown result type (might be due to invalid IL or missing references)
			//IL_067f: Expected O, but got Unknown
			//IL_0684: Expected O, but got Unknown
			//IL_0684: Unknown result type (might be due to invalid IL or missing references)
			//IL_0696: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c1: Expected O, but got Unknown
			//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c6: Expected O, but got Unknown
			//IL_06cb: Expected O, but got Unknown
			//IL_0716: Unknown result type (might be due to invalid IL or missing references)
			//IL_0742: Unknown result type (might be due to invalid IL or missing references)
			//IL_0747: Unknown result type (might be due to invalid IL or missing references)
			//IL_0752: Unknown result type (might be due to invalid IL or missing references)
			//IL_0757: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ed: Expected O, but got Unknown
			//IL_07f2: Expected O, but got Unknown
			//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0804: Unknown result type (might be due to invalid IL or missing references)
			//IL_0816: Unknown result type (might be due to invalid IL or missing references)
			//IL_0825: Unknown result type (might be due to invalid IL or missing references)
			//IL_082f: Expected O, but got Unknown
			//IL_082a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0834: Expected O, but got Unknown
			//IL_0839: Expected O, but got Unknown
			//IL_0891: Unknown result type (might be due to invalid IL or missing references)
			//IL_0896: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08da: Expected O, but got Unknown
			//IL_08d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_08df: Expected O, but got Unknown
			//IL_08e4: Expected O, but got Unknown
			//IL_0924: Unknown result type (might be due to invalid IL or missing references)
			//IL_0942: Unknown result type (might be due to invalid IL or missing references)
			//IL_0947: Unknown result type (might be due to invalid IL or missing references)
			//IL_094d: Expected O, but got Unknown
			//IL_094f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0954: Unknown result type (might be due to invalid IL or missing references)
			//IL_095a: Expected O, but got Unknown
			//IL_095c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0961: Unknown result type (might be due to invalid IL or missing references)
			//IL_0967: Expected O, but got Unknown
			//IL_0967: Unknown result type (might be due to invalid IL or missing references)
			//IL_0971: Expected O, but got Unknown
			//IL_098f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0994: Unknown result type (might be due to invalid IL or missing references)
			//IL_099f: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3a: Expected O, but got Unknown
			//IL_0a3f: Expected O, but got Unknown
			//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7c: Expected O, but got Unknown
			//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a81: Expected O, but got Unknown
			//IL_0a86: Expected O, but got Unknown
			//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b12: Expected O, but got Unknown
			//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c14: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c38: Expected O, but got Unknown
			//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c3d: Expected O, but got Unknown
			//IL_0c42: Expected O, but got Unknown
			//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d01: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d09: Expected O, but got Unknown
			//IL_0d0e: Expected O, but got Unknown
			//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d41: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d4b: Expected O, but got Unknown
			//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d50: Expected O, but got Unknown
			//IL_0d55: Expected O, but got Unknown
			//IL_0da8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0de2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e48: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e58: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e5d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef3: Expected O, but got Unknown
			//IL_0ef8: Expected O, but got Unknown
			//IL_0ef8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f2b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f35: Expected O, but got Unknown
			//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3a: Expected O, but got Unknown
			//IL_0f3f: Expected O, but got Unknown
			//IL_0f78: Unknown result type (might be due to invalid IL or missing references)
			NameScope val = _scope0;
			NameScope val2 = _scope1;
			NameScope val3 = _scope2;
			NameScope val4 = _scope3;
			NameScope val5 = _scope4;
			NameScope val6 = _scope5;
			NameScope val7 = _scope6;
			NameScope val8 = _scope7;
			NameScope val9 = _scope8;
			NameScope val10 = _scope9;
			NameScope val11 = _scope10;
			NameScope val12 = _scope11;
			NameScope val13 = _scope12;
			NameScope val14 = _scope13;
			NameScope val15 = _scope14;
			NameScope val16 = _scope15;
			NameScope val17 = _scope16;
			NameScope val18 = _scope17;
			NameScope val19 = _scope18;
			NameScope val20 = _scope19;
			NameScope val21 = _scope20;
			NameScope val22 = _scope21;
			NameScope val23 = _scope22;
			NameScope val24 = _scope23;
			NameScope val25 = _scope24;
			NameScope val26 = _scope25;
			NameScope val27 = _scope26;
			NameScope val28 = _scope27;
			NameScope val29 = _scope28;
			NameScope val30 = _scope29;
			NameScope val31 = _scope30;
			NameScope val32 = _scope31;
			NameScope val33 = _scope32;
			NameScope val34 = _scope33;
			NameScope val35 = _scope34;
			NameScope val36 = _scope35;
			NameScope val37 = _scope36;
			NameScope val38 = _scope37;
			NameScope val39 = _scope38;
			NameScope val40 = _scope39;
			NameScope val41 = _scope40;
			NameScope val42 = _scope41;
			NameScope val43 = _scope42;
			NameScope val44 = _scope43;
			NameScope val45 = _scope44;
			NameScope val46 = _scope45;
			NameScope val47 = _scope46;
			NameScope val48 = _scope47;
			NameScope val49 = _scope48;
			NameScope val50 = _scope49;
			NameScope val51 = _scope50;
			NameScope val52 = _scope51;
			NameScope val53 = _scope52;
			NameScope val54 = _scope53;
			NameScope val55 = _scope54;
			NameScope val56 = _scope55;
			NameScope val57 = _scope56;
			NameScope val58 = _scope57;
			NameScope val59 = _scope58;
			NameScope val60 = _scope59;
			NameScope val61 = _scope60;
			NameScope val62 = _scope61;
			NameScope val63 = _scope62;
			NameScope val64 = _scope63;
			NameScope val65 = _scope64;
			NameScope val66 = _scope65;
			NameScope val67 = _scope66;
			NameScope val68 = _scope67;
			NameScope val69 = _scope68;
			NameScope val70 = _scope69;
			NameScope val71 = _scope70;
			NameScope val72 = _scope71;
			NameScope val73 = _scope72;
			NameScope val74 = _scope73;
			NameScope val75 = _scope74;
			NameScope val76 = _scope75;
			NameScope val77 = _scope76;
			NameScope val78 = _scope77;
			NameScope val79 = _scope78;
			NameScope val80 = _scope79;
			NameScope val81 = _scope80;
			NameScope val82 = _scope81;
			NameScope val83 = _scope82;
			NameScope val84 = _scope83;
			NameScope val85 = _scope84;
			NameScope val86 = _scope85;
			NameScope val87 = _scope86;
			NameScope val88 = _scope87;
			NameScope val89 = _scope88;
			NameScope val90 = _scope89;
			NameScope val91 = _scope90;
			NameScope val92 = _scope91;
			NameScope val93 = _scope92;
			NameScope val94 = _scope93;
			NameScope val95 = _scope94;
			NameScope val96 = _scope95;
			NameScope val97 = _scope96;
			NameScope val98 = _scope97;
			NameScope val99 = _scope98;
			NameScope val100 = _scope99;
			NameScope val101 = _scope100;
			NameScope val102 = _scope101;
			NameScope val103 = _scope102;
			NameScope val104 = _scope103;
			NameScope val105 = _scope104;
			NameScope val106 = _scope105;
			NameScope val107 = _scope106;
			NameScope val108 = _scope107;
			NameScope val109 = _scope108;
			NameScope val110 = _scope109;
			NameScope val111 = _scope110;
			NameScope val112 = _scope111;
			NameScope val113 = _scope112;
			NameScope val114 = _scope113;
			NameScope val115 = _scope114;
			NameScope val116 = _scope115;
			NameScope val117 = _scope116;
			NameScope val118 = _scope117;
			NameScope val119 = _scope118;
			NameScope val120 = _scope119;
			NameScope val121 = _scope120;
			NameScope val122 = _scope121;
			NameScope val123 = _scope122;
			NameScope val124 = _scope123;
			NameScope val125 = _scope124;
			NameScope val126 = _scope125;
			NameScope val127 = _scope126;
			NameScope val128 = _scope127;
			NameScope val129 = _scope128;
			NameScope val130 = _scope129;
			NameScope val131 = _scope130;
			NameScope val132 = _scope131;
			NameScope val133 = _scope132;
			NameScope val134 = _scope133;
			NameScope val135 = _scope134;
			NameScope val136 = _scope135;
			NameScope val137 = _scope136;
			NameScope val138 = _scope137;
			NameScope val139 = _scope138;
			NameScope val140 = _scope139;
			NameScope val141 = _scope140;
			NameScope val142 = _scope141;
			NameScope val143 = _scope142;
			NameScope val144 = _scope143;
			NameScope val145 = _scope144;
			NameScope val146 = _scope145;
			NameScope val147 = _scope146;
			NameScope val148 = _scope147;
			NameScope val149 = _scope148;
			NameScope val150 = _scope149;
			NameScope val151 = _scope150;
			NameScope val152 = _scope151;
			NameScope val153 = _scope152;
			NameScope val154 = _scope153;
			NameScope val155 = _scope154;
			NameScope val156 = _scope155;
			NameScope val157 = _scope156;
			NameScope val158 = _scope157;
			NameScope val159 = _scope158;
			NameScope val160 = _scope159;
			StaticResourceExtension val161 = new StaticResourceExtension();
			StaticResourceExtension val162 = new StaticResourceExtension();
			BindingExtension val163 = new BindingExtension();
			SwipeItem val164 = new SwipeItem();
			SwipeItems val165 = new SwipeItems();
			StaticResourceExtension val166 = new StaticResourceExtension();
			Label val167 = new Label();
			Border val168 = new Border();
			BindingExtension val169 = new BindingExtension();
			StaticResourceExtension val170 = new StaticResourceExtension();
			Label val171 = new Label();
			StaticResourceExtension val172 = new StaticResourceExtension();
			Label val173 = new Label();
			Grid val174 = new Grid();
			SwipeView val175 = new SwipeView();
			Border val176 = new Border();
			NameScope val177 = new NameScope();
			NameScope.SetNameScope((BindableObject)(object)val176, (INameScope)(object)val177);
			((Element)val175).transientNamescope = (INameScope)(object)val177;
			((Element)val165).transientNamescope = (INameScope)(object)val177;
			((Element)val164).transientNamescope = (INameScope)(object)val177;
			((INameScope)val177).RegisterName("DeleteSwipeItem", (object)val164);
			if (((Element)val164).StyleId == null)
			{
				((Element)val164).StyleId = "DeleteSwipeItem";
			}
			((Element)val174).transientNamescope = (INameScope)(object)val177;
			((Element)val168).transientNamescope = (INameScope)(object)val177;
			((Element)val167).transientNamescope = (INameScope)(object)val177;
			((Element)val171).transientNamescope = (INameScope)(object)val177;
			((Element)val173).transientNamescope = (INameScope)(object)val177;
			val161.Key = "Card";
			StaticResourceExtension val178 = new StaticResourceExtension
			{
				Key = "Card"
			};
			XamlServiceProvider val179 = new XamlServiceProvider();
			Type? typeFromHandle = typeof(IProvideValueTarget);
			int length;
			object[] array = new object[(length = parentValues.Length) + 1];
			Array.Copy(parentValues, 0, array, 1, length);
			array[0] = val176;
			SimpleValueTargetProvider val180 = new SimpleValueTargetProvider(array, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[8] { val177, val177, val108, val105, val91, val4, val3, val }, (object)root);
			object obj = (object)val180;
			val179.Add(typeFromHandle, (object)val180);
			val179.Add(typeof(IReferenceProvider), obj);
			val179.Add(typeof(IRootObjectProvider), obj);
			val179.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(89, 37)));
			object obj2 = val178.ProvideValue((IServiceProvider)val179);
			((BindableObject)val176).SetValue(VisualElement.StyleProperty, (obj2 == null || !typeof(BindingBase).IsAssignableFrom(obj2.GetType())) ? obj2 : obj2);
			((BindableObject)val176).SetValue(View.MarginProperty, (object)new Thickness(0.0, 4.0));
			((BindableObject)val164).SetValue(MenuItem.TextProperty, (object)"Eliminar");
			val162.Key = "Danger";
			StaticResourceExtension val181 = new StaticResourceExtension
			{
				Key = "Danger"
			};
			XamlServiceProvider val182 = new XamlServiceProvider();
			Type? typeFromHandle2 = typeof(IProvideValueTarget);
			int length2;
			object[] array2 = new object[(length2 = parentValues.Length) + 4];
			Array.Copy(parentValues, 0, array2, 4, length2);
			array2[0] = val164;
			array2[1] = val165;
			array2[2] = val175;
			array2[3] = val176;
			SimpleValueTargetProvider val183 = new SimpleValueTargetProvider(array2, (object)SwipeItem.BackgroundColorProperty, (INameScope[])(object)new NameScope[11]
			{
				val177, val177, val177, val177, val177, val108, val105, val91, val4, val3,
				val
			}, (object)root);
			object obj3 = (object)val183;
			val182.Add(typeFromHandle2, (object)val183);
			val182.Add(typeof(IReferenceProvider), obj3);
			val182.Add(typeof(IRootObjectProvider), obj3);
			val182.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(95, 56)));
			object obj4 = val181.ProvideValue((IServiceProvider)val182);
			((BindableObject)val164).SetValue(SwipeItem.BackgroundColorProperty, (obj4 == null || !typeof(BindingBase).IsAssignableFrom(obj4.GetType())) ? obj4 : obj4);
			val164.Invoked += root.OnDeleteClicked;
			val163.Path = ".";
			XamlServiceProvider val184 = new XamlServiceProvider();
			Type? typeFromHandle3 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val185 = new XmlNamespaceResolver();
			val185.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val185.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val184.Add(typeFromHandle3, (object)new XamlTypeResolver((IXmlNamespaceResolver)val185, typeof(_003CInitializeComponent_003E_anonXamlCDataTemplate_1).Assembly));
			BindingBase val186 = ((IMarkupExtension<BindingBase>)(object)val163).ProvideValue((IServiceProvider)val184);
			((BindableObject)val164).SetBinding(MenuItem.CommandParameterProperty, val186);
			val165.Add((ISwipeItem)(object)val164);
			((BindableObject)val175).SetValue(SwipeView.RightItemsProperty, (object)val165);
			((BindableObject)val174).SetValue(Layout.PaddingProperty, (object)new Thickness(16.0, 12.0));
			((BindableObject)val174).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[3]
			{
				new ColumnDefinition(GridLength.Auto),
				new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(GridLength.Auto)
			}));
			((BindableObject)val168).SetValue(Grid.ColumnProperty, (object)0);
			val166.Key = "PrimaryLight";
			StaticResourceExtension val187 = new StaticResourceExtension
			{
				Key = "PrimaryLight"
			};
			XamlServiceProvider val188 = new XamlServiceProvider();
			Type? typeFromHandle4 = typeof(IProvideValueTarget);
			int length3;
			object[] array3 = new object[(length3 = parentValues.Length) + 4];
			Array.Copy(parentValues, 0, array3, 4, length3);
			array3[0] = val168;
			array3[1] = val174;
			array3[2] = val175;
			array3[3] = val176;
			SimpleValueTargetProvider val189 = new SimpleValueTargetProvider(array3, (object)VisualElement.BackgroundColorProperty, (INameScope[])(object)new NameScope[11]
			{
				val177, val177, val177, val177, val177, val108, val105, val91, val4, val3,
				val
			}, (object)root);
			object obj5 = (object)val189;
			val188.Add(typeFromHandle4, (object)val189);
			val188.Add(typeof(IReferenceProvider), obj5);
			val188.Add(typeof(IRootObjectProvider), obj5);
			val188.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(102, 49)));
			object obj6 = val187.ProvideValue((IServiceProvider)val188);
			((BindableObject)val168).SetValue(VisualElement.BackgroundColorProperty, (obj6 == null || !typeof(BindingBase).IsAssignableFrom(obj6.GetType())) ? obj6 : obj6);
			((BindableObject)val168).SetValue(Border.StrokeThicknessProperty, (object)0.0);
			((BindableObject)val168).SetValue(Border.StrokeShapeProperty, (object)new RoundRectangle
			{
				CornerRadius = new CornerRadius(16.0, 16.0, 16.0, 16.0)
			});
			((BindableObject)val168).SetValue(VisualElement.WidthRequestProperty, (object)32.0);
			((BindableObject)val168).SetValue(VisualElement.HeightRequestProperty, (object)32.0);
			((BindableObject)val168).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val167).SetValue(Label.TextProperty, (object)"\ud83d\udcf1");
			((BindableObject)val167).SetValue(Label.FontSizeProperty, (object)16.0);
			((BindableObject)val167).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val167).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val168).SetValue(Border.ContentProperty, (object)val167);
			((Layout)val174).Children.Add((IView)(object)val168);
			((BindableObject)val171).SetValue(Grid.ColumnProperty, (object)1);
			val169.Path = ".";
			XamlServiceProvider val190 = new XamlServiceProvider();
			Type? typeFromHandle5 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val191 = new XmlNamespaceResolver();
			val191.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val191.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val190.Add(typeFromHandle5, (object)new XamlTypeResolver((IXmlNamespaceResolver)val191, typeof(_003CInitializeComponent_003E_anonXamlCDataTemplate_1).Assembly));
			BindingBase val192 = ((IMarkupExtension<BindingBase>)(object)val169).ProvideValue((IServiceProvider)val190);
			((BindableObject)val171).SetBinding(Label.TextProperty, val192);
			val170.Key = "BodyText";
			StaticResourceExtension val193 = new StaticResourceExtension
			{
				Key = "BodyText"
			};
			XamlServiceProvider val194 = new XamlServiceProvider();
			Type? typeFromHandle6 = typeof(IProvideValueTarget);
			int length4;
			object[] array4 = new object[(length4 = parentValues.Length) + 4];
			Array.Copy(parentValues, 0, array4, 4, length4);
			array4[0] = val171;
			array4[1] = val174;
			array4[2] = val175;
			array4[3] = val176;
			SimpleValueTargetProvider val195 = new SimpleValueTargetProvider(array4, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[11]
			{
				val177, val177, val177, val177, val177, val108, val105, val91, val4, val3,
				val
			}, (object)root);
			object obj7 = (object)val195;
			val194.Add(typeFromHandle6, (object)val195);
			val194.Add(typeof(IReferenceProvider), obj7);
			val194.Add(typeof(IRootObjectProvider), obj7);
			val194.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(115, 48)));
			object obj8 = val193.ProvideValue((IServiceProvider)val194);
			((BindableObject)val171).SetValue(VisualElement.StyleProperty, (obj8 == null || !typeof(BindingBase).IsAssignableFrom(obj8.GetType())) ? obj8 : obj8);
			((BindableObject)val171).SetValue(Label.FontSizeProperty, (object)16.0);
			((BindableObject)val171).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val171).SetValue(View.MarginProperty, (object)new Thickness(12.0, 0.0, 0.0, 0.0));
			((Layout)val174).Children.Add((IView)(object)val171);
			((BindableObject)val173).SetValue(Grid.ColumnProperty, (object)2);
			((BindableObject)val173).SetValue(Label.TextProperty, (object)"›");
			((BindableObject)val173).SetValue(Label.FontSizeProperty, (object)22.0);
			val172.Key = "HintText";
			StaticResourceExtension val196 = new StaticResourceExtension
			{
				Key = "HintText"
			};
			XamlServiceProvider val197 = new XamlServiceProvider();
			Type? typeFromHandle7 = typeof(IProvideValueTarget);
			int length5;
			object[] array5 = new object[(length5 = parentValues.Length) + 4];
			Array.Copy(parentValues, 0, array5, 4, length5);
			array5[0] = val173;
			array5[1] = val174;
			array5[2] = val175;
			array5[3] = val176;
			SimpleValueTargetProvider val198 = new SimpleValueTargetProvider(array5, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[11]
			{
				val177, val177, val177, val177, val177, val108, val105, val91, val4, val3,
				val
			}, (object)root);
			object obj9 = (object)val198;
			val197.Add(typeFromHandle7, (object)val198);
			val197.Add(typeof(IReferenceProvider), obj9);
			val197.Add(typeof(IRootObjectProvider), obj9);
			val197.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(122, 48)));
			object obj10 = val196.ProvideValue((IServiceProvider)val197);
			((BindableObject)val173).SetValue(VisualElement.StyleProperty, (obj10 == null || !typeof(BindingBase).IsAssignableFrom(obj10.GetType())) ? obj10 : obj10);
			((BindableObject)val173).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((Layout)val174).Children.Add((IView)(object)val173);
			((BindableObject)val175).SetValue(ContentView.ContentProperty, (object)val174);
			((BindableObject)val176).SetValue(Border.ContentProperty, (object)val175);
			return val176;
		}
	}

	private readonly ILoggingService _loggingService;

	private readonly IContactPicker _contactPicker;

	private readonly ILocalizationService _localizationService;

	private readonly IMessageStore _messageStore;

	private bool _isApplyingLanguageSelection;

	private ObservableCollection<string> phones = new ObservableCollection<string>();

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label TitleLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label SubtitleLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label LanguageLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Button SpanishButton;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Button EnglishButton;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Entry PhoneEntry;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Button AddButton;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Button ContactsButton;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label NumbersListLabel;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private CollectionView PhoneList;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label InfoTitle;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label InfoText;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Border DefaultSmsCard;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label DefaultSmsTitle;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Label DefaultSmsStatus;

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	private Button MakeDefaultButton;

	public MainPage(ILoggingService loggingService, IContactPicker contactPicker, ILocalizationService localizationService, IMessageStore messageStore)
	{
		InitializeComponent();
		_loggingService = loggingService;
		_contactPicker = contactPicker;
		_localizationService = localizationService;
		_messageStore = messageStore;
		List<string> list = JsonSerializer.Deserialize<List<string>>(Preferences.Default.Get<string>("phones", "[]", (string)null));
		if (list != null)
		{
			foreach (string item in list)
			{
				phones.Add(item);
			}
		}
		((ItemsView)PhoneList).ItemsSource = phones;
		UpdateLocalizedStrings();
		_localizationService.LanguageChanged += OnLanguageChanged;
		UpdateLanguageButtons();
		UpdateDefaultStatus();
	}

	protected override void OnAppearing()
	{
		((Page)this).OnAppearing();
		UpdateDefaultStatus();
	}

	private void UpdateDefaultStatus()
	{
		try
		{
			if (_messageStore == null || !_messageStore.IsSupported || !_messageStore.CanBeDefault)
			{
				((VisualElement)DefaultSmsCard).IsVisible = false;
				return;
			}
			((VisualElement)DefaultSmsCard).IsVisible = true;
			bool flag = _localizationService.CurrentLanguage == "es-ES";
			bool isDefaultSmsApp = _messageStore.IsDefaultSmsApp;
			DefaultSmsTitle.Text = (flag ? "App de SMS predeterminada" : "Default SMS app");
			if (isDefaultSmsApp)
			{
				DefaultSmsStatus.Text = (flag ? "✓ SMS Forwarder es tu app de SMS predeterminada." : "✓ SMS Forwarder is your default SMS app.");
				((VisualElement)MakeDefaultButton).IsVisible = false;
			}
			else
			{
				DefaultSmsStatus.Text = (flag ? "Para recibir, gestionar y reenviar SMS de forma fiable, ponla como app de SMS predeterminada." : "To reliably receive, manage and forward SMS, set it as your default SMS app.");
				MakeDefaultButton.Text = (flag ? "Predeterminada" : "Set as default");
				((VisualElement)MakeDefaultButton).IsVisible = true;
			}
		}
		catch (Exception ex)
		{
			_loggingService.LogError("UpdateDefaultStatus: " + ex.Message);
		}
	}

	private async void OnMakeDefaultClicked(object sender, EventArgs e)
	{
		bool es = _localizationService.CurrentLanguage == "es-ES";
		try
		{
			if (!_messageStore.CanBeDefault)
			{
				await ModernDialog.AlertAsync((Page)(object)this, es ? "No disponible" : "Not available", es ? "Este dispositivo no admite ser app de SMS predeterminada (no tiene telefonía/SIM). Pruébalo en un teléfono con SIM." : "This device can't be a default SMS app (no telephony/SIM). Try it on a phone with a SIM.", "OK");
				return;
			}
			await _messageStore.RequestDefaultAsync();
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al solicitar app SMS por defecto", ex);
			await ModernDialog.AlertAsync((Page)(object)this, es ? "Error" : "Error", es ? "No se pudo abrir el selector de app de SMS predeterminada." : "Couldn't open the default SMS app chooser.", "OK");
		}
		UpdateDefaultStatus();
	}

	private void UpdateLocalizedStrings()
	{
		TitleLabel.Text = _localizationService.GetString("main.title");
		SubtitleLabel.Text = _localizationService.GetString("main.subtitle");
		LanguageLabel.Text = _localizationService.GetString("main.language");
		((InputView)PhoneEntry).Placeholder = _localizationService.GetString("main.placeholder");
		AddButton.Text = _localizationService.GetString("main.add_number");
		ContactsButton.Text = _localizationService.GetString("main.from_contacts");
		NumbersListLabel.Text = _localizationService.GetString("main.numbers_list");
		InfoTitle.Text = "\ud83d\udca1 " + _localizationService.GetString("menu.settings");
		if (_localizationService.CurrentLanguage == "es-ES")
		{
			InfoText.Text = "• Los SMS recibidos se reenviarán automáticamente a estos números\n• Puedes escribir números manualmente o seleccionarlos desde tus contactos\n• Para configurar permisos avanzados, ve a la sección Diagnósticos\n• Desliza hacia la izquierda en un número para eliminarlo";
		}
		else
		{
			InfoText.Text = "• Received SMS will be automatically forwarded to these numbers\n• You can enter numbers manually or select them from your contacts\n• For advanced permission settings, go to the Diagnostics section\n• Swipe left on a number to delete it";
		}
	}

	private void OnSpanishClicked(object? sender, EventArgs e)
	{
		ApplyLanguage("es-ES");
	}

	private void OnEnglishClicked(object? sender, EventArgs e)
	{
		ApplyLanguage("en-US");
	}

	private void ApplyLanguage(string languageCode)
	{
		if (!_isApplyingLanguageSelection && !(languageCode == _localizationService.CurrentLanguage))
		{
			_localizationService.SetLanguage(languageCode);
			UpdateLocalizedStrings();
		}
	}

	private void UpdateLanguageButtons()
	{
		_isApplyingLanguageSelection = true;
		bool flag = _localizationService.CurrentLanguage == "es-ES";
		((StyleableElement)SpanishButton).Style = LookupStyle(flag ? "PrimaryButton" : "OutlineButton");
		((StyleableElement)EnglishButton).Style = LookupStyle(flag ? "OutlineButton" : "PrimaryButton");
		_isApplyingLanguageSelection = false;
	}

	private static Style? LookupStyle(string key)
	{
		Application current = Application.Current;
		object obj = default(object);
		if (current == null || !current.Resources.TryGetValue(key, ref obj))
		{
			return null;
		}
		return (Style?)((obj is Style) ? obj : null);
	}

	private void OnLanguageChanged(object? sender, EventArgs e)
	{
		MainThread.BeginInvokeOnMainThread((Action)delegate
		{
			UpdateLanguageButtons();
			UpdateLocalizedStrings();
			UpdateDefaultStatus();
		});
	}

	private void OnAddClicked(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(((InputView)PhoneEntry).Text))
			{
				return;
			}
			string text = ((InputView)PhoneEntry).Text.Replace(" ", "").Trim();
			if (IsValidPhoneNumber(text))
			{
				if (!phones.Contains(text))
				{
					phones.Add(text);
					SavePhones();
					((InputView)PhoneEntry).Text = string.Empty;
					_loggingService.LogInfo("Número agregado: " + text);
				}
				else if (_localizationService.CurrentLanguage == "es-ES")
				{
					ModernDialog.AlertAsync((Page)(object)this, "Número duplicado", "Este número ya está en la lista.", "OK");
				}
				else
				{
					ModernDialog.AlertAsync((Page)(object)this, "Duplicate number", "This number is already in the list.", "OK");
				}
			}
			else
			{
				if (_localizationService.CurrentLanguage == "es-ES")
				{
					ModernDialog.AlertAsync((Page)(object)this, "Número no válido", "Por favor, introduce un número de teléfono válido (7-15 dígitos).", "OK");
				}
				else
				{
					ModernDialog.AlertAsync((Page)(object)this, "Invalid number", "Please enter a valid phone number (7-15 digits).", "OK");
				}
				_loggingService.LogWarning("Intento de agregar número inválido: " + text);
			}
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al agregar número", ex);
			if (_localizationService.CurrentLanguage == "es-ES")
			{
				ModernDialog.AlertAsync((Page)(object)this, "Error", "Error al agregar el número", "OK");
			}
			else
			{
				ModernDialog.AlertAsync((Page)(object)this, "Error", "Error adding the number", "OK");
			}
		}
	}

	private void OnDeleteClicked(object sender, EventArgs e)
	{
		try
		{
			SwipeItem val = (SwipeItem)((sender is SwipeItem) ? sender : null);
			if (val != null && ((MenuItem)val).CommandParameter is string text)
			{
				phones.Remove(text);
				SavePhones();
				_loggingService.LogInfo("Número eliminado: " + text);
			}
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al eliminar número", ex);
			if (_localizationService.CurrentLanguage == "es-ES")
			{
				ModernDialog.AlertAsync((Page)(object)this, "Error", "Error al eliminar el número", "OK");
			}
			else
			{
				ModernDialog.AlertAsync((Page)(object)this, "Error", "Error deleting the number", "OK");
			}
		}
	}

	private void SavePhones()
	{
		string text = JsonSerializer.Serialize(phones);
		Preferences.Default.Set<string>("phones", text, (string)null);
		Application current = Application.Current;
		object obj;
		if (current == null)
		{
			obj = null;
		}
		else
		{
			IElementHandler handler = ((Element)current).Handler;
			if (handler == null)
			{
				obj = null;
			}
			else
			{
				IMauiContext mauiContext = handler.MauiContext;
				obj = ((mauiContext != null) ? mauiContext.Context : null);
			}
		}
		Context val = (Context)obj;
		if (val == null)
		{
			return;
		}
		try
		{
			ISharedPreferences sharedPreferences = val.GetSharedPreferences(val.PackageName + "_preferences", (FileCreationMode)0);
			ISharedPreferencesEditor obj2 = ((sharedPreferences != null) ? sharedPreferences.Edit() : null);
			if (obj2 != null)
			{
				obj2.PutString("phones", text);
			}
			if (obj2 != null)
			{
				obj2.Apply();
			}
			_loggingService.LogInfo("Números guardados en preferencias de Android: " + text);
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al guardar en preferencias de Android: " + ex.Message);
		}
	}

	private void OnItemSelected(object sender, SelectionChangedEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((SelectableItemsView)(CollectionView)sender).SelectedItem = null;
	}

	private bool IsValidPhoneNumber(string phoneNumber)
	{
		string text = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "")
			.Replace(")", "");
		if (new Regex("^\\+?[1-9]\\d{6,14}$").IsMatch(text) && text.Length >= 7)
		{
			return text.Length <= 15;
		}
		return false;
	}

	private async void OnSelectFromContactsClicked(object sender, EventArgs e)
	{
		try
		{
			string text = await _contactPicker.PickPhoneNumberAsync();
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			AddPhoneNumberFromContact(text);
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al abrir contactos", ex);
			if (!(_localizationService.CurrentLanguage == "es-ES"))
			{
				await ModernDialog.AlertAsync((Page)(object)this, "Error", "Error opening contacts list", "OK");
			}
			else
			{
				await ModernDialog.AlertAsync((Page)(object)this, "Error", "Error al abrir la lista de contactos", "OK");
			}
		}
	}

	private void AddPhoneNumberFromContact(string phoneNumber)
	{
		try
		{
			_loggingService.LogInfo("Numero recibido desde el selector de contactos");
			if (string.IsNullOrWhiteSpace(phoneNumber))
			{
				return;
			}
			string cleanNumber = phoneNumber.Replace(" ", "").Trim();
			if (IsValidPhoneNumber(cleanNumber))
			{
				if (!phones.Contains(cleanNumber))
				{
					phones.Add(cleanNumber);
					SavePhones();
					_loggingService.LogInfo("Número agregado desde contactos: " + cleanNumber);
					MainThread.BeginInvokeOnMainThread((Action)async delegate
					{
						if (_localizationService.CurrentLanguage == "es-ES")
						{
							await ModernDialog.AlertAsync((Page)(object)this, "Número agregado", "El número " + cleanNumber + " ha sido agregado exitosamente", "OK");
						}
						else
						{
							await ModernDialog.AlertAsync((Page)(object)this, "Number added", "The number " + cleanNumber + " has been successfully added", "OK");
						}
					});
					return;
				}
				MainThread.BeginInvokeOnMainThread((Action)async delegate
				{
					if (_localizationService.CurrentLanguage == "es-ES")
					{
						await ModernDialog.AlertAsync((Page)(object)this, "Número duplicado", "Este número ya está en la lista.", "OK");
					}
					else
					{
						await ModernDialog.AlertAsync((Page)(object)this, "Duplicate number", "This number is already in the list.", "OK");
					}
				});
				return;
			}
			_loggingService.LogWarning("Número inválido desde contactos: " + cleanNumber);
			MainThread.BeginInvokeOnMainThread((Action)async delegate
			{
				if (_localizationService.CurrentLanguage == "es-ES")
				{
					await ModernDialog.AlertAsync((Page)(object)this, "Número no válido", "El número seleccionado no es válido.", "OK");
				}
				else
				{
					await ModernDialog.AlertAsync((Page)(object)this, "Invalid number", "The selected number is invalid.", "OK");
				}
			});
		}
		catch (Exception ex)
		{
			_loggingService.LogError("Error al procesar contacto seleccionado", ex);
		}
	}

	protected override void OnDisappearing()
	{
		((Page)this).OnDisappearing();
	}

	[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
	[MemberNotNull("TitleLabel")]
	[MemberNotNull("SubtitleLabel")]
	[MemberNotNull("LanguageLabel")]
	[MemberNotNull("SpanishButton")]
	[MemberNotNull("EnglishButton")]
	[MemberNotNull("PhoneEntry")]
	[MemberNotNull("AddButton")]
	[MemberNotNull("ContactsButton")]
	[MemberNotNull("NumbersListLabel")]
	[MemberNotNull("PhoneList")]
	[MemberNotNull("InfoTitle")]
	[MemberNotNull("InfoText")]
	[MemberNotNull("DefaultSmsCard")]
	[MemberNotNull("DefaultSmsTitle")]
	[MemberNotNull("DefaultSmsStatus")]
	[MemberNotNull("MakeDefaultButton")]
	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Expected O, but got Unknown
		//IL_06a0: Expected O, but got Unknown
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Expected O, but got Unknown
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Expected O, but got Unknown
		//IL_06e7: Expected O, but got Unknown
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Expected O, but got Unknown
		//IL_078c: Expected O, but got Unknown
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Expected O, but got Unknown
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_07d3: Expected O, but got Unknown
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Expected O, but got Unknown
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_0821: Expected O, but got Unknown
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Expected O, but got Unknown
		//IL_08e8: Expected O, but got Unknown
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Expected O, but got Unknown
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Expected O, but got Unknown
		//IL_092f: Expected O, but got Unknown
		//IL_0982: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Expected O, but got Unknown
		//IL_0a5d: Expected O, but got Unknown
		//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Expected O, but got Unknown
		//IL_0a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9f: Expected O, but got Unknown
		//IL_0aa4: Expected O, but got Unknown
		//IL_0b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b10: Expected O, but got Unknown
		//IL_0b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1d: Expected O, but got Unknown
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b27: Expected O, but got Unknown
		//IL_0c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb5: Expected O, but got Unknown
		//IL_0cba: Expected O, but got Unknown
		//IL_0cba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Expected O, but got Unknown
		//IL_0cf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfc: Expected O, but got Unknown
		//IL_0d01: Expected O, but got Unknown
		//IL_0d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e17: Expected O, but got Unknown
		//IL_0e1c: Expected O, but got Unknown
		//IL_0e1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e59: Expected O, but got Unknown
		//IL_0e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5e: Expected O, but got Unknown
		//IL_0e63: Expected O, but got Unknown
		//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f11: Expected O, but got Unknown
		//IL_0f16: Expected O, but got Unknown
		//IL_0f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f53: Expected O, but got Unknown
		//IL_0f4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f58: Expected O, but got Unknown
		//IL_0f5d: Expected O, but got Unknown
		//IL_0f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8b: Expected O, but got Unknown
		//IL_0f8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa4: Expected O, but got Unknown
		//IL_0f9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa9: Expected O, but got Unknown
		//IL_0fae: Expected O, but got Unknown
		//IL_0fca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1059: Unknown result type (might be due to invalid IL or missing references)
		//IL_105e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1061: Expected O, but got Unknown
		//IL_1066: Expected O, but got Unknown
		//IL_1066: Unknown result type (might be due to invalid IL or missing references)
		//IL_1078: Unknown result type (might be due to invalid IL or missing references)
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1099: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a3: Expected O, but got Unknown
		//IL_109e: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a8: Expected O, but got Unknown
		//IL_10ad: Expected O, but got Unknown
		//IL_10c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1153: Unknown result type (might be due to invalid IL or missing references)
		//IL_1158: Unknown result type (might be due to invalid IL or missing references)
		//IL_115b: Expected O, but got Unknown
		//IL_1160: Expected O, but got Unknown
		//IL_1160: Unknown result type (might be due to invalid IL or missing references)
		//IL_1172: Unknown result type (might be due to invalid IL or missing references)
		//IL_1184: Unknown result type (might be due to invalid IL or missing references)
		//IL_1193: Unknown result type (might be due to invalid IL or missing references)
		//IL_119d: Expected O, but got Unknown
		//IL_1198: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a2: Expected O, but got Unknown
		//IL_11a7: Expected O, but got Unknown
		//IL_11b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d5: Expected O, but got Unknown
		//IL_11d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ee: Expected O, but got Unknown
		//IL_11e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f3: Expected O, but got Unknown
		//IL_11f8: Expected O, but got Unknown
		//IL_124d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1252: Unknown result type (might be due to invalid IL or missing references)
		//IL_1258: Expected O, but got Unknown
		//IL_125a: Unknown result type (might be due to invalid IL or missing references)
		//IL_125f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1265: Expected O, but got Unknown
		//IL_1265: Unknown result type (might be due to invalid IL or missing references)
		//IL_126f: Expected O, but got Unknown
		//IL_12cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1342: Unknown result type (might be due to invalid IL or missing references)
		//IL_1347: Unknown result type (might be due to invalid IL or missing references)
		//IL_134a: Expected O, but got Unknown
		//IL_134f: Expected O, but got Unknown
		//IL_134f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1361: Unknown result type (might be due to invalid IL or missing references)
		//IL_1373: Unknown result type (might be due to invalid IL or missing references)
		//IL_1382: Unknown result type (might be due to invalid IL or missing references)
		//IL_138c: Expected O, but got Unknown
		//IL_1387: Unknown result type (might be due to invalid IL or missing references)
		//IL_1391: Expected O, but got Unknown
		//IL_1396: Expected O, but got Unknown
		//IL_1433: Unknown result type (might be due to invalid IL or missing references)
		//IL_1438: Unknown result type (might be due to invalid IL or missing references)
		//IL_1443: Unknown result type (might be due to invalid IL or missing references)
		//IL_1448: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b1: Expected O, but got Unknown
		//IL_14b6: Expected O, but got Unknown
		//IL_14b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14da: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f3: Expected O, but got Unknown
		//IL_14ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f8: Expected O, but got Unknown
		//IL_14fd: Expected O, but got Unknown
		//IL_153b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1540: Unknown result type (might be due to invalid IL or missing references)
		//IL_154b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1550: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b9: Expected O, but got Unknown
		//IL_15be: Expected O, but got Unknown
		//IL_15be: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fb: Expected O, but got Unknown
		//IL_15f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1600: Expected O, but got Unknown
		//IL_1605: Expected O, but got Unknown
		//IL_1698: Unknown result type (might be due to invalid IL or missing references)
		//IL_169d: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1704: Unknown result type (might be due to invalid IL or missing references)
		//IL_1709: Unknown result type (might be due to invalid IL or missing references)
		//IL_170c: Expected O, but got Unknown
		//IL_1711: Expected O, but got Unknown
		//IL_1711: Unknown result type (might be due to invalid IL or missing references)
		//IL_1723: Unknown result type (might be due to invalid IL or missing references)
		//IL_1735: Unknown result type (might be due to invalid IL or missing references)
		//IL_1744: Unknown result type (might be due to invalid IL or missing references)
		//IL_174e: Expected O, but got Unknown
		//IL_1749: Unknown result type (might be due to invalid IL or missing references)
		//IL_1753: Expected O, but got Unknown
		//IL_1758: Expected O, but got Unknown
		//IL_1796: Unknown result type (might be due to invalid IL or missing references)
		//IL_179b: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_181b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1820: Unknown result type (might be due to invalid IL or missing references)
		//IL_1823: Expected O, but got Unknown
		//IL_1828: Expected O, but got Unknown
		//IL_1828: Unknown result type (might be due to invalid IL or missing references)
		//IL_183a: Unknown result type (might be due to invalid IL or missing references)
		//IL_184c: Unknown result type (might be due to invalid IL or missing references)
		//IL_185b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1865: Expected O, but got Unknown
		//IL_1860: Unknown result type (might be due to invalid IL or missing references)
		//IL_186a: Expected O, but got Unknown
		//IL_186f: Expected O, but got Unknown
		//IL_1886: Unknown result type (might be due to invalid IL or missing references)
		//IL_188b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1896: Unknown result type (might be due to invalid IL or missing references)
		//IL_189b: Unknown result type (might be due to invalid IL or missing references)
		//IL_190b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1910: Unknown result type (might be due to invalid IL or missing references)
		//IL_1913: Expected O, but got Unknown
		//IL_1918: Expected O, but got Unknown
		//IL_1918: Unknown result type (might be due to invalid IL or missing references)
		//IL_192a: Unknown result type (might be due to invalid IL or missing references)
		//IL_193c: Unknown result type (might be due to invalid IL or missing references)
		//IL_194b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1955: Expected O, but got Unknown
		//IL_1950: Unknown result type (might be due to invalid IL or missing references)
		//IL_195a: Expected O, but got Unknown
		//IL_195f: Expected O, but got Unknown
		//IL_196c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1971: Unknown result type (might be due to invalid IL or missing references)
		//IL_1983: Unknown result type (might be due to invalid IL or missing references)
		//IL_198d: Expected O, but got Unknown
		//IL_198d: Unknown result type (might be due to invalid IL or missing references)
		//IL_199c: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a6: Expected O, but got Unknown
		//IL_19a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ab: Expected O, but got Unknown
		//IL_19b0: Expected O, but got Unknown
		//IL_19c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2013: Unknown result type (might be due to invalid IL or missing references)
		//IL_2018: Unknown result type (might be due to invalid IL or missing references)
		//IL_201b: Expected O, but got Unknown
		//IL_2020: Expected O, but got Unknown
		//IL_2020: Unknown result type (might be due to invalid IL or missing references)
		//IL_2032: Unknown result type (might be due to invalid IL or missing references)
		//IL_2044: Unknown result type (might be due to invalid IL or missing references)
		//IL_2056: Unknown result type (might be due to invalid IL or missing references)
		//IL_2060: Expected O, but got Unknown
		//IL_205b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2065: Expected O, but got Unknown
		//IL_206a: Expected O, but got Unknown
		//IL_20a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_20bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2123: Unknown result type (might be due to invalid IL or missing references)
		//IL_2128: Unknown result type (might be due to invalid IL or missing references)
		//IL_212b: Expected O, but got Unknown
		//IL_2130: Expected O, but got Unknown
		//IL_2130: Unknown result type (might be due to invalid IL or missing references)
		//IL_2142: Unknown result type (might be due to invalid IL or missing references)
		//IL_2154: Unknown result type (might be due to invalid IL or missing references)
		//IL_2166: Unknown result type (might be due to invalid IL or missing references)
		//IL_2170: Expected O, but got Unknown
		//IL_216b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2175: Expected O, but got Unknown
		//IL_217a: Expected O, but got Unknown
		//IL_2191: Unknown result type (might be due to invalid IL or missing references)
		//IL_2196: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_220c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2211: Unknown result type (might be due to invalid IL or missing references)
		//IL_2214: Expected O, but got Unknown
		//IL_2219: Expected O, but got Unknown
		//IL_2219: Unknown result type (might be due to invalid IL or missing references)
		//IL_222b: Unknown result type (might be due to invalid IL or missing references)
		//IL_223d: Unknown result type (might be due to invalid IL or missing references)
		//IL_224f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2259: Expected O, but got Unknown
		//IL_2254: Unknown result type (might be due to invalid IL or missing references)
		//IL_225e: Expected O, but got Unknown
		//IL_2263: Expected O, but got Unknown
		//IL_2270: Unknown result type (might be due to invalid IL or missing references)
		//IL_2275: Unknown result type (might be due to invalid IL or missing references)
		//IL_2287: Unknown result type (might be due to invalid IL or missing references)
		//IL_2291: Expected O, but got Unknown
		//IL_2291: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ad: Expected O, but got Unknown
		//IL_22a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b2: Expected O, but got Unknown
		//IL_22b7: Expected O, but got Unknown
		//IL_22d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2311: Unknown result type (might be due to invalid IL or missing references)
		//IL_2357: Unknown result type (might be due to invalid IL or missing references)
		//IL_235c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2367: Unknown result type (might be due to invalid IL or missing references)
		//IL_236c: Unknown result type (might be due to invalid IL or missing references)
		//IL_23cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d5: Expected O, but got Unknown
		//IL_23da: Expected O, but got Unknown
		//IL_23da: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_23fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2410: Unknown result type (might be due to invalid IL or missing references)
		//IL_241a: Expected O, but got Unknown
		//IL_2415: Unknown result type (might be due to invalid IL or missing references)
		//IL_241f: Expected O, but got Unknown
		//IL_2424: Expected O, but got Unknown
		//IL_249b: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2511: Unknown result type (might be due to invalid IL or missing references)
		//IL_2516: Unknown result type (might be due to invalid IL or missing references)
		//IL_2519: Expected O, but got Unknown
		//IL_251e: Expected O, but got Unknown
		//IL_251e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2530: Unknown result type (might be due to invalid IL or missing references)
		//IL_2542: Unknown result type (might be due to invalid IL or missing references)
		//IL_2554: Unknown result type (might be due to invalid IL or missing references)
		//IL_255e: Expected O, but got Unknown
		//IL_2559: Unknown result type (might be due to invalid IL or missing references)
		//IL_2563: Expected O, but got Unknown
		//IL_2568: Expected O, but got Unknown
		//IL_25e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2644: Unknown result type (might be due to invalid IL or missing references)
		//IL_2649: Unknown result type (might be due to invalid IL or missing references)
		//IL_264c: Expected O, but got Unknown
		//IL_2651: Expected O, but got Unknown
		//IL_2651: Unknown result type (might be due to invalid IL or missing references)
		//IL_2663: Unknown result type (might be due to invalid IL or missing references)
		//IL_2675: Unknown result type (might be due to invalid IL or missing references)
		//IL_2687: Unknown result type (might be due to invalid IL or missing references)
		//IL_2691: Expected O, but got Unknown
		//IL_268c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2696: Expected O, but got Unknown
		//IL_269b: Expected O, but got Unknown
		//IL_26dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2717: Unknown result type (might be due to invalid IL or missing references)
		//IL_275d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2762: Unknown result type (might be due to invalid IL or missing references)
		//IL_276d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2772: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_27db: Expected O, but got Unknown
		//IL_27e0: Expected O, but got Unknown
		//IL_27e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_27f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2804: Unknown result type (might be due to invalid IL or missing references)
		//IL_2816: Unknown result type (might be due to invalid IL or missing references)
		//IL_2820: Expected O, but got Unknown
		//IL_281b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2825: Expected O, but got Unknown
		//IL_282a: Expected O, but got Unknown
		//IL_28a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2917: Unknown result type (might be due to invalid IL or missing references)
		//IL_291c: Unknown result type (might be due to invalid IL or missing references)
		//IL_291f: Expected O, but got Unknown
		//IL_2924: Expected O, but got Unknown
		//IL_2924: Unknown result type (might be due to invalid IL or missing references)
		//IL_2936: Unknown result type (might be due to invalid IL or missing references)
		//IL_2948: Unknown result type (might be due to invalid IL or missing references)
		//IL_295a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2964: Expected O, but got Unknown
		//IL_295f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2969: Expected O, but got Unknown
		//IL_296e: Expected O, but got Unknown
		//IL_2a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a89: Expected O, but got Unknown
		//IL_2a8e: Expected O, but got Unknown
		//IL_2a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ace: Expected O, but got Unknown
		//IL_2ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad3: Expected O, but got Unknown
		//IL_2ad8: Expected O, but got Unknown
		StaticResourceExtension val = new StaticResourceExtension();
		StaticResourceExtension val2 = new StaticResourceExtension();
		AppThemeBindingExtension val3 = new AppThemeBindingExtension();
		Label val4 = new Label();
		StaticResourceExtension val5 = new StaticResourceExtension();
		Label val6 = new Label();
		VerticalStackLayout val7 = new VerticalStackLayout();
		StaticResourceExtension val8 = new StaticResourceExtension();
		Label val9 = new Label();
		Button val10 = new Button();
		Button val11 = new Button();
		Grid val12 = new Grid();
		VerticalStackLayout val13 = new VerticalStackLayout();
		StaticResourceExtension val14 = new StaticResourceExtension();
		StaticResourceExtension val15 = new StaticResourceExtension();
		StaticResourceExtension val16 = new StaticResourceExtension();
		AppThemeBindingExtension val17 = new AppThemeBindingExtension();
		StaticResourceExtension val18 = new StaticResourceExtension();
		StaticResourceExtension val19 = new StaticResourceExtension();
		AppThemeBindingExtension val20 = new AppThemeBindingExtension();
		Entry val21 = new Entry();
		Border val22 = new Border();
		StaticResourceExtension val23 = new StaticResourceExtension();
		Button val24 = new Button();
		StaticResourceExtension val25 = new StaticResourceExtension();
		StaticResourceExtension val26 = new StaticResourceExtension();
		Button val27 = new Button();
		Grid val28 = new Grid();
		VerticalStackLayout val29 = new VerticalStackLayout();
		StaticResourceExtension val30 = new StaticResourceExtension();
		StaticResourceExtension val31 = new StaticResourceExtension();
		StaticResourceExtension val32 = new StaticResourceExtension();
		AppThemeBindingExtension val33 = new AppThemeBindingExtension();
		Label val34 = new Label();
		DataTemplate val35 = new DataTemplate();
		CollectionView val36 = new CollectionView();
		VerticalStackLayout val37 = new VerticalStackLayout();
		StaticResourceExtension val38 = new StaticResourceExtension();
		StaticResourceExtension val39 = new StaticResourceExtension();
		StaticResourceExtension val40 = new StaticResourceExtension();
		AppThemeBindingExtension val41 = new AppThemeBindingExtension();
		StaticResourceExtension val42 = new StaticResourceExtension();
		Label val43 = new Label();
		StaticResourceExtension val44 = new StaticResourceExtension();
		Label val45 = new Label();
		VerticalStackLayout val46 = new VerticalStackLayout();
		Border val47 = new Border();
		StaticResourceExtension val48 = new StaticResourceExtension();
		StaticResourceExtension val49 = new StaticResourceExtension();
		Label val50 = new Label();
		StaticResourceExtension val51 = new StaticResourceExtension();
		Label val52 = new Label();
		StaticResourceExtension val53 = new StaticResourceExtension();
		Button val54 = new Button();
		VerticalStackLayout val55 = new VerticalStackLayout();
		Border val56 = new Border();
		VerticalStackLayout val57 = new VerticalStackLayout();
		ScrollView val58 = new ScrollView();
		MainPage mainPage;
		NameScope val59 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(mainPage = this))) ?? ((object)new NameScope()));
		NameScope.SetNameScope((BindableObject)(object)mainPage, (INameScope)(object)val59);
		((Element)val58).transientNamescope = (INameScope)(object)val59;
		((Element)val57).transientNamescope = (INameScope)(object)val59;
		((Element)val7).transientNamescope = (INameScope)(object)val59;
		((Element)val4).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("TitleLabel", (object)val4);
		if (((Element)val4).StyleId == null)
		{
			((Element)val4).StyleId = "TitleLabel";
		}
		((Element)val6).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("SubtitleLabel", (object)val6);
		if (((Element)val6).StyleId == null)
		{
			((Element)val6).StyleId = "SubtitleLabel";
		}
		((Element)val13).transientNamescope = (INameScope)(object)val59;
		((Element)val9).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("LanguageLabel", (object)val9);
		if (((Element)val9).StyleId == null)
		{
			((Element)val9).StyleId = "LanguageLabel";
		}
		((Element)val12).transientNamescope = (INameScope)(object)val59;
		((Element)val10).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("SpanishButton", (object)val10);
		if (((Element)val10).StyleId == null)
		{
			((Element)val10).StyleId = "SpanishButton";
		}
		((Element)val11).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("EnglishButton", (object)val11);
		if (((Element)val11).StyleId == null)
		{
			((Element)val11).StyleId = "EnglishButton";
		}
		((Element)val29).transientNamescope = (INameScope)(object)val59;
		((Element)val22).transientNamescope = (INameScope)(object)val59;
		((Element)val21).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("PhoneEntry", (object)val21);
		if (((Element)val21).StyleId == null)
		{
			((Element)val21).StyleId = "PhoneEntry";
		}
		((Element)val28).transientNamescope = (INameScope)(object)val59;
		((Element)val24).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("AddButton", (object)val24);
		if (((Element)val24).StyleId == null)
		{
			((Element)val24).StyleId = "AddButton";
		}
		((Element)val27).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("ContactsButton", (object)val27);
		if (((Element)val27).StyleId == null)
		{
			((Element)val27).StyleId = "ContactsButton";
		}
		((Element)val37).transientNamescope = (INameScope)(object)val59;
		((Element)val34).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("NumbersListLabel", (object)val34);
		if (((Element)val34).StyleId == null)
		{
			((Element)val34).StyleId = "NumbersListLabel";
		}
		((Element)val36).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("PhoneList", (object)val36);
		if (((Element)val36).StyleId == null)
		{
			((Element)val36).StyleId = "PhoneList";
		}
		((Element)val47).transientNamescope = (INameScope)(object)val59;
		((Element)val46).transientNamescope = (INameScope)(object)val59;
		((Element)val43).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("InfoTitle", (object)val43);
		if (((Element)val43).StyleId == null)
		{
			((Element)val43).StyleId = "InfoTitle";
		}
		((Element)val45).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("InfoText", (object)val45);
		if (((Element)val45).StyleId == null)
		{
			((Element)val45).StyleId = "InfoText";
		}
		((Element)val56).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("DefaultSmsCard", (object)val56);
		if (((Element)val56).StyleId == null)
		{
			((Element)val56).StyleId = "DefaultSmsCard";
		}
		((Element)val55).transientNamescope = (INameScope)(object)val59;
		((Element)val50).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("DefaultSmsTitle", (object)val50);
		if (((Element)val50).StyleId == null)
		{
			((Element)val50).StyleId = "DefaultSmsTitle";
		}
		((Element)val52).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("DefaultSmsStatus", (object)val52);
		if (((Element)val52).StyleId == null)
		{
			((Element)val52).StyleId = "DefaultSmsStatus";
		}
		((Element)val54).transientNamescope = (INameScope)(object)val59;
		((INameScope)val59).RegisterName("MakeDefaultButton", (object)val54);
		if (((Element)val54).StyleId == null)
		{
			((Element)val54).StyleId = "MakeDefaultButton";
		}
		TitleLabel = val4;
		SubtitleLabel = val6;
		LanguageLabel = val9;
		SpanishButton = val10;
		EnglishButton = val11;
		PhoneEntry = val21;
		AddButton = val24;
		ContactsButton = val27;
		NumbersListLabel = val34;
		PhoneList = val36;
		InfoTitle = val43;
		InfoText = val45;
		DefaultSmsCard = val56;
		DefaultSmsTitle = val50;
		DefaultSmsStatus = val52;
		MakeDefaultButton = val54;
		((BindableObject)mainPage).SetValue(Page.TitleProperty, (object)"Configuración");
		((BindableObject)val57).SetValue(Layout.PaddingProperty, (object)new Thickness(24.0));
		((BindableObject)val57).SetValue(StackBase.SpacingProperty, (object)20.0);
		((BindableObject)val7).SetValue(StackBase.SpacingProperty, (object)8.0);
		((BindableObject)val4).SetValue(Label.TextProperty, (object)"Configuración");
		((BindableObject)val4).SetValue(Label.FontSizeProperty, (object)24.0);
		((BindableObject)val4).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		val.Key = "TextPrimaryLight";
		StaticResourceExtension val60 = new StaticResourceExtension
		{
			Key = "TextPrimaryLight"
		};
		XamlServiceProvider val61 = new XamlServiceProvider();
		Type? typeFromHandle = typeof(IProvideValueTarget);
		object[] array = new object[0 + 6];
		array[0] = val3;
		array[1] = val4;
		array[2] = val7;
		array[3] = val57;
		array[4] = val58;
		array[5] = mainPage;
		SimpleValueTargetProvider val62 = new SimpleValueTargetProvider(array, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj = (object)val62;
		val61.Add(typeFromHandle, (object)val62);
		val61.Add(typeof(IReferenceProvider), obj);
		val61.Add(typeof(IRootObjectProvider), obj);
		val61.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(16, 24)));
		object light = val60.ProvideValue((IServiceProvider)val61);
		val3.Light = light;
		val2.Key = "TextPrimaryDark";
		StaticResourceExtension val63 = new StaticResourceExtension
		{
			Key = "TextPrimaryDark"
		};
		XamlServiceProvider val64 = new XamlServiceProvider();
		Type? typeFromHandle2 = typeof(IProvideValueTarget);
		object[] array2 = new object[0 + 6];
		array2[0] = val3;
		array2[1] = val4;
		array2[2] = val7;
		array2[3] = val57;
		array2[4] = val58;
		array2[5] = mainPage;
		SimpleValueTargetProvider val65 = new SimpleValueTargetProvider(array2, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj2 = (object)val65;
		val64.Add(typeFromHandle2, (object)val65);
		val64.Add(typeof(IReferenceProvider), obj2);
		val64.Add(typeof(IRootObjectProvider), obj2);
		val64.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(16, 24)));
		object dark = val63.ProvideValue((IServiceProvider)val64);
		val3.Dark = dark;
		XamlServiceProvider val66 = new XamlServiceProvider();
		val66.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val4, (object)Label.TextColorProperty));
		val66.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(16, 24)));
		BindingBase val67 = ((IMarkupExtension<BindingBase>)(object)val3).ProvideValue((IServiceProvider)val66);
		((BindableObject)val4).SetBinding(Label.TextColorProperty, val67);
		((BindableObject)val4).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val7).Children.Add((IView)(object)val4);
		((BindableObject)val6).SetValue(Label.TextProperty, (object)"Configura los números donde reenviar SMS");
		val5.Key = "HintText";
		StaticResourceExtension val68 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val69 = new XamlServiceProvider();
		Type? typeFromHandle3 = typeof(IProvideValueTarget);
		object[] array3 = new object[0 + 5];
		array3[0] = val6;
		array3[1] = val7;
		array3[2] = val57;
		array3[3] = val58;
		array3[4] = mainPage;
		SimpleValueTargetProvider val70 = new SimpleValueTargetProvider(array3, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj3 = (object)val70;
		val69.Add(typeFromHandle3, (object)val70);
		val69.Add(typeof(IReferenceProvider), obj3);
		val69.Add(typeof(IRootObjectProvider), obj3);
		val69.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(20, 24)));
		object obj4 = val68.ProvideValue((IServiceProvider)val69);
		((BindableObject)val6).SetValue(VisualElement.StyleProperty, (obj4 == null || !typeof(BindingBase).IsAssignableFrom(obj4.GetType())) ? obj4 : obj4);
		((BindableObject)val6).SetValue(Label.FontSizeProperty, (object)14.0);
		((BindableObject)val6).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((Layout)val7).Children.Add((IView)(object)val6);
		((Layout)val57).Children.Add((IView)(object)val7);
		((BindableObject)val13).SetValue(StackBase.SpacingProperty, (object)8.0);
		((BindableObject)val9).SetValue(Label.TextProperty, (object)"Idioma");
		val8.Key = "BodyText";
		StaticResourceExtension val71 = new StaticResourceExtension
		{
			Key = "BodyText"
		};
		XamlServiceProvider val72 = new XamlServiceProvider();
		Type? typeFromHandle4 = typeof(IProvideValueTarget);
		object[] array4 = new object[0 + 5];
		array4[0] = val9;
		array4[1] = val13;
		array4[2] = val57;
		array4[3] = val58;
		array4[4] = mainPage;
		SimpleValueTargetProvider val73 = new SimpleValueTargetProvider(array4, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj5 = (object)val73;
		val72.Add(typeFromHandle4, (object)val73);
		val72.Add(typeof(IReferenceProvider), obj5);
		val72.Add(typeof(IRootObjectProvider), obj5);
		val72.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(29, 24)));
		object obj6 = val71.ProvideValue((IServiceProvider)val72);
		((BindableObject)val9).SetValue(VisualElement.StyleProperty, (obj6 == null || !typeof(BindingBase).IsAssignableFrom(obj6.GetType())) ? obj6 : obj6);
		((BindableObject)val9).SetValue(Label.FontAttributesProperty, (object)(FontAttributes)1);
		((Layout)val13).Children.Add((IView)(object)val9);
		((BindableObject)val12).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[2]
		{
			new ColumnDefinition(GridLength.Star),
			new ColumnDefinition(GridLength.Star)
		}));
		((BindableObject)val12).SetValue(Grid.ColumnSpacingProperty, (object)10.0);
		((BindableObject)val10).SetValue(Grid.ColumnProperty, (object)0);
		((BindableObject)val10).SetValue(Button.TextProperty, (object)"\ud83c\uddea\ud83c\uddf8 Español");
		((BindableObject)val10).SetValue(Button.FontSizeProperty, (object)14.0);
		val10.Clicked += mainPage.OnSpanishClicked;
		((Layout)val12).Children.Add((IView)(object)val10);
		((BindableObject)val11).SetValue(Grid.ColumnProperty, (object)1);
		((BindableObject)val11).SetValue(Button.TextProperty, (object)"\ud83c\uddec\ud83c\udde7 English");
		((BindableObject)val11).SetValue(Button.FontSizeProperty, (object)14.0);
		val11.Clicked += mainPage.OnEnglishClicked;
		((Layout)val12).Children.Add((IView)(object)val11);
		((Layout)val13).Children.Add((IView)(object)val12);
		((Layout)val57).Children.Add((IView)(object)val13);
		((BindableObject)val29).SetValue(StackBase.SpacingProperty, (object)12.0);
		val14.Key = "Card";
		StaticResourceExtension val74 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val75 = new XamlServiceProvider();
		Type? typeFromHandle5 = typeof(IProvideValueTarget);
		object[] array5 = new object[0 + 5];
		array5[0] = val22;
		array5[1] = val29;
		array5[2] = val57;
		array5[3] = val58;
		array5[4] = mainPage;
		SimpleValueTargetProvider val76 = new SimpleValueTargetProvider(array5, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj7 = (object)val76;
		val75.Add(typeFromHandle5, (object)val76);
		val75.Add(typeof(IReferenceProvider), obj7);
		val75.Add(typeof(IRootObjectProvider), obj7);
		val75.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(50, 25)));
		object obj8 = val74.ProvideValue((IServiceProvider)val75);
		((BindableObject)val22).SetValue(VisualElement.StyleProperty, (obj8 == null || !typeof(BindingBase).IsAssignableFrom(obj8.GetType())) ? obj8 : obj8);
		((BindableObject)val22).SetValue(Border.PaddingProperty, (object)new Thickness(4.0));
		((BindableObject)val21).SetValue(Entry.PlaceholderProperty, (object)"Ej: +34 600 123 456");
		((BindableObject)val21).SetValue(VisualElement.BackgroundColorProperty, (object)Colors.Transparent);
		val15.Key = "TextPrimaryLight";
		StaticResourceExtension val77 = new StaticResourceExtension
		{
			Key = "TextPrimaryLight"
		};
		XamlServiceProvider val78 = new XamlServiceProvider();
		Type? typeFromHandle6 = typeof(IProvideValueTarget);
		object[] array6 = new object[0 + 7];
		array6[0] = val17;
		array6[1] = val21;
		array6[2] = val22;
		array6[3] = val29;
		array6[4] = val57;
		array6[5] = val58;
		array6[6] = mainPage;
		SimpleValueTargetProvider val79 = new SimpleValueTargetProvider(array6, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[8] { val59, val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj9 = (object)val79;
		val78.Add(typeFromHandle6, (object)val79);
		val78.Add(typeof(IReferenceProvider), obj9);
		val78.Add(typeof(IRootObjectProvider), obj9);
		val78.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(54, 28)));
		object light2 = val77.ProvideValue((IServiceProvider)val78);
		val17.Light = light2;
		val16.Key = "TextPrimaryDark";
		StaticResourceExtension val80 = new StaticResourceExtension
		{
			Key = "TextPrimaryDark"
		};
		XamlServiceProvider val81 = new XamlServiceProvider();
		Type? typeFromHandle7 = typeof(IProvideValueTarget);
		object[] array7 = new object[0 + 7];
		array7[0] = val17;
		array7[1] = val21;
		array7[2] = val22;
		array7[3] = val29;
		array7[4] = val57;
		array7[5] = val58;
		array7[6] = mainPage;
		SimpleValueTargetProvider val82 = new SimpleValueTargetProvider(array7, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[8] { val59, val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj10 = (object)val82;
		val81.Add(typeFromHandle7, (object)val82);
		val81.Add(typeof(IReferenceProvider), obj10);
		val81.Add(typeof(IRootObjectProvider), obj10);
		val81.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(54, 28)));
		object dark2 = val80.ProvideValue((IServiceProvider)val81);
		val17.Dark = dark2;
		XamlServiceProvider val83 = new XamlServiceProvider();
		val83.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val21, (object)Entry.TextColorProperty));
		val83.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(54, 28)));
		BindingBase val84 = ((IMarkupExtension<BindingBase>)(object)val17).ProvideValue((IServiceProvider)val83);
		((BindableObject)val21).SetBinding(Entry.TextColorProperty, val84);
		val18.Key = "TextSecondaryLight";
		StaticResourceExtension val85 = new StaticResourceExtension
		{
			Key = "TextSecondaryLight"
		};
		XamlServiceProvider val86 = new XamlServiceProvider();
		Type? typeFromHandle8 = typeof(IProvideValueTarget);
		object[] array8 = new object[0 + 7];
		array8[0] = val20;
		array8[1] = val21;
		array8[2] = val22;
		array8[3] = val29;
		array8[4] = val57;
		array8[5] = val58;
		array8[6] = mainPage;
		SimpleValueTargetProvider val87 = new SimpleValueTargetProvider(array8, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[8] { val59, val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj11 = (object)val87;
		val86.Add(typeFromHandle8, (object)val87);
		val86.Add(typeof(IReferenceProvider), obj11);
		val86.Add(typeof(IRootObjectProvider), obj11);
		val86.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(55, 28)));
		object light3 = val85.ProvideValue((IServiceProvider)val86);
		val20.Light = light3;
		val19.Key = "TextSecondaryDark";
		StaticResourceExtension val88 = new StaticResourceExtension
		{
			Key = "TextSecondaryDark"
		};
		XamlServiceProvider val89 = new XamlServiceProvider();
		Type? typeFromHandle9 = typeof(IProvideValueTarget);
		object[] array9 = new object[0 + 7];
		array9[0] = val20;
		array9[1] = val21;
		array9[2] = val22;
		array9[3] = val29;
		array9[4] = val57;
		array9[5] = val58;
		array9[6] = mainPage;
		SimpleValueTargetProvider val90 = new SimpleValueTargetProvider(array9, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[8] { val59, val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj12 = (object)val90;
		val89.Add(typeFromHandle9, (object)val90);
		val89.Add(typeof(IReferenceProvider), obj12);
		val89.Add(typeof(IRootObjectProvider), obj12);
		val89.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(55, 28)));
		object dark3 = val88.ProvideValue((IServiceProvider)val89);
		val20.Dark = dark3;
		XamlServiceProvider val91 = new XamlServiceProvider();
		val91.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val21, (object)Entry.PlaceholderColorProperty));
		val91.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(55, 28)));
		BindingBase val92 = ((IMarkupExtension<BindingBase>)(object)val20).ProvideValue((IServiceProvider)val91);
		((BindableObject)val21).SetBinding(Entry.PlaceholderColorProperty, val92);
		((BindableObject)val21).SetValue(Entry.FontSizeProperty, (object)16.0);
		((BindableObject)val22).SetValue(Border.ContentProperty, (object)val21);
		((Layout)val29).Children.Add((IView)(object)val22);
		((BindableObject)val28).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[2]
		{
			new ColumnDefinition(GridLength.Star),
			new ColumnDefinition(GridLength.Star)
		}));
		((BindableObject)val28).SetValue(Grid.ColumnSpacingProperty, (object)8.0);
		((BindableObject)val24).SetValue(Grid.ColumnProperty, (object)0);
		((BindableObject)val24).SetValue(Button.TextProperty, (object)"\ud83d\udcdd Agregar Número");
		val24.Clicked += mainPage.OnAddClicked;
		val23.Key = "PrimaryButton";
		StaticResourceExtension val93 = new StaticResourceExtension
		{
			Key = "PrimaryButton"
		};
		XamlServiceProvider val94 = new XamlServiceProvider();
		Type? typeFromHandle10 = typeof(IProvideValueTarget);
		object[] array10 = new object[0 + 6];
		array10[0] = val24;
		array10[1] = val28;
		array10[2] = val29;
		array10[3] = val57;
		array10[4] = val58;
		array10[5] = mainPage;
		SimpleValueTargetProvider val95 = new SimpleValueTargetProvider(array10, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj13 = (object)val95;
		val94.Add(typeFromHandle10, (object)val95);
		val94.Add(typeof(IReferenceProvider), obj13);
		val94.Add(typeof(IRootObjectProvider), obj13);
		val94.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(65, 29)));
		object obj14 = val93.ProvideValue((IServiceProvider)val94);
		((BindableObject)val24).SetValue(VisualElement.StyleProperty, (obj14 == null || !typeof(BindingBase).IsAssignableFrom(obj14.GetType())) ? obj14 : obj14);
		((BindableObject)val24).SetValue(Button.FontSizeProperty, (object)14.0);
		((Layout)val28).Children.Add((IView)(object)val24);
		((BindableObject)val27).SetValue(Grid.ColumnProperty, (object)1);
		((BindableObject)val27).SetValue(Button.TextProperty, (object)"\ud83d\udc65 Contactos");
		val27.Clicked += mainPage.OnSelectFromContactsClicked;
		val25.Key = "PrimaryButton";
		StaticResourceExtension val96 = new StaticResourceExtension
		{
			Key = "PrimaryButton"
		};
		XamlServiceProvider val97 = new XamlServiceProvider();
		Type? typeFromHandle11 = typeof(IProvideValueTarget);
		object[] array11 = new object[0 + 6];
		array11[0] = val27;
		array11[1] = val28;
		array11[2] = val29;
		array11[3] = val57;
		array11[4] = val58;
		array11[5] = mainPage;
		SimpleValueTargetProvider val98 = new SimpleValueTargetProvider(array11, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj15 = (object)val98;
		val97.Add(typeFromHandle11, (object)val98);
		val97.Add(typeof(IReferenceProvider), obj15);
		val97.Add(typeof(IRootObjectProvider), obj15);
		val97.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(72, 29)));
		object obj16 = val96.ProvideValue((IServiceProvider)val97);
		((BindableObject)val27).SetValue(VisualElement.StyleProperty, (obj16 == null || !typeof(BindingBase).IsAssignableFrom(obj16.GetType())) ? obj16 : obj16);
		val26.Key = "Accent";
		StaticResourceExtension val99 = new StaticResourceExtension
		{
			Key = "Accent"
		};
		XamlServiceProvider val100 = new XamlServiceProvider();
		Type? typeFromHandle12 = typeof(IProvideValueTarget);
		object[] array12 = new object[0 + 6];
		array12[0] = val27;
		array12[1] = val28;
		array12[2] = val29;
		array12[3] = val57;
		array12[4] = val58;
		array12[5] = mainPage;
		SimpleValueTargetProvider val101 = new SimpleValueTargetProvider(array12, (object)VisualElement.BackgroundColorProperty, (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj17 = (object)val101;
		val100.Add(typeFromHandle12, (object)val101);
		val100.Add(typeof(IReferenceProvider), obj17);
		val100.Add(typeof(IRootObjectProvider), obj17);
		val100.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(73, 29)));
		object obj18 = val99.ProvideValue((IServiceProvider)val100);
		((BindableObject)val27).SetValue(VisualElement.BackgroundColorProperty, (obj18 == null || !typeof(BindingBase).IsAssignableFrom(obj18.GetType())) ? obj18 : obj18);
		((Layout)val28).Children.Add((IView)(object)val27);
		((Layout)val29).Children.Add((IView)(object)val28);
		((Layout)val57).Children.Add((IView)(object)val29);
		((BindableObject)val37).SetValue(StackBase.SpacingProperty, (object)12.0);
		((BindableObject)val34).SetValue(Label.TextProperty, (object)"Números configurados");
		val30.Key = "CardTitle";
		StaticResourceExtension val102 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val103 = new XamlServiceProvider();
		Type? typeFromHandle13 = typeof(IProvideValueTarget);
		object[] array13 = new object[0 + 5];
		array13[0] = val34;
		array13[1] = val37;
		array13[2] = val57;
		array13[3] = val58;
		array13[4] = mainPage;
		SimpleValueTargetProvider val104 = new SimpleValueTargetProvider(array13, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[6] { val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj19 = (object)val104;
		val103.Add(typeFromHandle13, (object)val104);
		val103.Add(typeof(IReferenceProvider), obj19);
		val103.Add(typeof(IRootObjectProvider), obj19);
		val103.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(81, 24)));
		object obj20 = val102.ProvideValue((IServiceProvider)val103);
		((BindableObject)val34).SetValue(VisualElement.StyleProperty, (obj20 == null || !typeof(BindingBase).IsAssignableFrom(obj20.GetType())) ? obj20 : obj20);
		val31.Key = "TextPrimaryLight";
		StaticResourceExtension val105 = new StaticResourceExtension
		{
			Key = "TextPrimaryLight"
		};
		XamlServiceProvider val106 = new XamlServiceProvider();
		Type? typeFromHandle14 = typeof(IProvideValueTarget);
		object[] array14 = new object[0 + 6];
		array14[0] = val33;
		array14[1] = val34;
		array14[2] = val37;
		array14[3] = val57;
		array14[4] = val58;
		array14[5] = mainPage;
		SimpleValueTargetProvider val107 = new SimpleValueTargetProvider(array14, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj21 = (object)val107;
		val106.Add(typeFromHandle14, (object)val107);
		val106.Add(typeof(IReferenceProvider), obj21);
		val106.Add(typeof(IRootObjectProvider), obj21);
		val106.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(82, 24)));
		object light4 = val105.ProvideValue((IServiceProvider)val106);
		val33.Light = light4;
		val32.Key = "TextPrimaryDark";
		StaticResourceExtension val108 = new StaticResourceExtension
		{
			Key = "TextPrimaryDark"
		};
		XamlServiceProvider val109 = new XamlServiceProvider();
		Type? typeFromHandle15 = typeof(IProvideValueTarget);
		object[] array15 = new object[0 + 6];
		array15[0] = val33;
		array15[1] = val34;
		array15[2] = val37;
		array15[3] = val57;
		array15[4] = val58;
		array15[5] = mainPage;
		SimpleValueTargetProvider val110 = new SimpleValueTargetProvider(array15, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj22 = (object)val110;
		val109.Add(typeFromHandle15, (object)val110);
		val109.Add(typeof(IReferenceProvider), obj22);
		val109.Add(typeof(IRootObjectProvider), obj22);
		val109.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(82, 24)));
		object dark4 = val108.ProvideValue((IServiceProvider)val109);
		val33.Dark = dark4;
		XamlServiceProvider val111 = new XamlServiceProvider();
		val111.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val34, (object)Label.TextColorProperty));
		val111.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(82, 24)));
		BindingBase val112 = ((IMarkupExtension<BindingBase>)(object)val33).ProvideValue((IServiceProvider)val111);
		((BindableObject)val34).SetBinding(Label.TextColorProperty, val112);
		((BindableObject)val34).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
		((BindableObject)val34).SetValue(View.MarginProperty, (object)new Thickness(0.0, 8.0, 0.0, 0.0));
		((Layout)val37).Children.Add((IView)(object)val34);
		((BindableObject)val36).SetValue(SelectableItemsView.SelectionModeProperty, (object)(SelectionMode)0);
		NameScope _scope0 = val59;
		NameScope _scope1 = val59;
		NameScope _scope2 = val59;
		NameScope _scope3 = val59;
		NameScope _scope4 = val59;
		NameScope _scope5 = val59;
		NameScope _scope6 = val59;
		NameScope _scope7 = val59;
		NameScope _scope8 = val59;
		NameScope _scope9 = val59;
		NameScope _scope10 = val59;
		NameScope _scope11 = val59;
		NameScope _scope12 = val59;
		NameScope _scope13 = val59;
		NameScope _scope14 = val59;
		NameScope _scope15 = val59;
		NameScope _scope16 = val59;
		NameScope _scope17 = val59;
		NameScope _scope18 = val59;
		NameScope _scope19 = val59;
		NameScope _scope20 = val59;
		NameScope _scope21 = val59;
		NameScope _scope22 = val59;
		NameScope _scope23 = val59;
		NameScope _scope24 = val59;
		NameScope _scope25 = val59;
		NameScope _scope26 = val59;
		NameScope _scope27 = val59;
		NameScope _scope28 = val59;
		NameScope _scope29 = val59;
		NameScope _scope30 = val59;
		NameScope _scope31 = val59;
		NameScope _scope32 = val59;
		NameScope _scope33 = val59;
		NameScope _scope34 = val59;
		NameScope _scope35 = val59;
		NameScope _scope36 = val59;
		NameScope _scope37 = val59;
		NameScope _scope38 = val59;
		NameScope _scope39 = val59;
		NameScope _scope40 = val59;
		NameScope _scope41 = val59;
		NameScope _scope42 = val59;
		NameScope _scope43 = val59;
		NameScope _scope44 = val59;
		NameScope _scope45 = val59;
		NameScope _scope46 = val59;
		NameScope _scope47 = val59;
		NameScope _scope48 = val59;
		NameScope _scope49 = val59;
		NameScope _scope50 = val59;
		NameScope _scope51 = val59;
		NameScope _scope52 = val59;
		NameScope _scope53 = val59;
		NameScope _scope54 = val59;
		NameScope _scope55 = val59;
		NameScope _scope56 = val59;
		NameScope _scope57 = val59;
		NameScope _scope58 = val59;
		NameScope _scope59 = val59;
		NameScope _scope60 = val59;
		NameScope _scope61 = val59;
		NameScope _scope62 = val59;
		NameScope _scope63 = val59;
		NameScope _scope64 = val59;
		NameScope _scope65 = val59;
		NameScope _scope66 = val59;
		NameScope _scope67 = val59;
		NameScope _scope68 = val59;
		NameScope _scope69 = val59;
		NameScope _scope70 = val59;
		NameScope _scope71 = val59;
		NameScope _scope72 = val59;
		NameScope _scope73 = val59;
		NameScope _scope74 = val59;
		NameScope _scope75 = val59;
		NameScope _scope76 = val59;
		NameScope _scope77 = val59;
		NameScope _scope78 = val59;
		NameScope _scope79 = val59;
		NameScope _scope80 = val59;
		NameScope _scope81 = val59;
		NameScope _scope82 = val59;
		NameScope _scope83 = val59;
		NameScope _scope84 = val59;
		NameScope _scope85 = val59;
		NameScope _scope86 = val59;
		NameScope _scope87 = val59;
		NameScope _scope88 = val59;
		NameScope _scope89 = val59;
		NameScope _scope90 = val59;
		NameScope _scope91 = val59;
		NameScope _scope92 = val59;
		NameScope _scope93 = val59;
		NameScope _scope94 = val59;
		NameScope _scope95 = val59;
		NameScope _scope96 = val59;
		NameScope _scope97 = val59;
		NameScope _scope98 = val59;
		NameScope _scope99 = val59;
		NameScope _scope100 = val59;
		NameScope _scope101 = val59;
		NameScope _scope102 = val59;
		NameScope _scope103 = val59;
		NameScope _scope104 = val59;
		NameScope _scope105 = val59;
		NameScope _scope106 = val59;
		NameScope _scope107 = val59;
		NameScope _scope108 = val59;
		NameScope _scope109 = val59;
		NameScope _scope110 = val59;
		NameScope _scope111 = val59;
		NameScope _scope112 = val59;
		NameScope _scope113 = val59;
		NameScope _scope114 = val59;
		NameScope _scope115 = val59;
		NameScope _scope116 = val59;
		NameScope _scope117 = val59;
		NameScope _scope118 = val59;
		NameScope _scope119 = val59;
		NameScope _scope120 = val59;
		NameScope _scope121 = val59;
		NameScope _scope122 = val59;
		NameScope _scope123 = val59;
		NameScope _scope124 = val59;
		NameScope _scope125 = val59;
		NameScope _scope126 = val59;
		NameScope _scope127 = val59;
		NameScope _scope128 = val59;
		NameScope _scope129 = val59;
		NameScope _scope130 = val59;
		NameScope _scope131 = val59;
		NameScope _scope132 = val59;
		NameScope _scope133 = val59;
		NameScope _scope134 = val59;
		NameScope _scope135 = val59;
		NameScope _scope136 = val59;
		NameScope _scope137 = val59;
		NameScope _scope138 = val59;
		NameScope _scope139 = val59;
		NameScope _scope140 = val59;
		NameScope _scope141 = val59;
		NameScope _scope142 = val59;
		NameScope _scope143 = val59;
		NameScope _scope144 = val59;
		NameScope _scope145 = val59;
		NameScope _scope146 = val59;
		NameScope _scope147 = val59;
		NameScope _scope148 = val59;
		NameScope _scope149 = val59;
		NameScope _scope150 = val59;
		NameScope _scope151 = val59;
		NameScope _scope152 = val59;
		NameScope _scope153 = val59;
		NameScope _scope154 = val59;
		NameScope _scope155 = val59;
		NameScope _scope156 = val59;
		NameScope _scope157 = val59;
		NameScope _scope158 = val59;
		NameScope _scope159 = val59;
		object[] array16 = new object[0 + 6];
		array16[0] = val35;
		array16[1] = val36;
		array16[2] = val37;
		array16[3] = val57;
		array16[4] = val58;
		array16[5] = mainPage;
		object[] parentValues = array16;
		MainPage root = mainPage;
		((ElementTemplate)val35).LoadTemplate = delegate
		{
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Expected O, but got Unknown
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Expected O, but got Unknown
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Expected O, but got Unknown
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Expected O, but got Unknown
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Expected O, but got Unknown
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Expected O, but got Unknown
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Expected O, but got Unknown
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0535: Expected O, but got Unknown
			//IL_0535: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Expected O, but got Unknown
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Expected O, but got Unknown
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_054a: Expected O, but got Unknown
			//IL_054a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Expected O, but got Unknown
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_0558: Expected O, but got Unknown
			//IL_0558: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Expected O, but got Unknown
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Expected O, but got Unknown
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Expected O, but got Unknown
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Expected O, but got Unknown
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0605: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0677: Unknown result type (might be due to invalid IL or missing references)
			//IL_067c: Unknown result type (might be due to invalid IL or missing references)
			//IL_067f: Expected O, but got Unknown
			//IL_0684: Expected O, but got Unknown
			//IL_0684: Unknown result type (might be due to invalid IL or missing references)
			//IL_0696: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c1: Expected O, but got Unknown
			//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c6: Expected O, but got Unknown
			//IL_06cb: Expected O, but got Unknown
			//IL_0716: Unknown result type (might be due to invalid IL or missing references)
			//IL_0742: Unknown result type (might be due to invalid IL or missing references)
			//IL_0747: Unknown result type (might be due to invalid IL or missing references)
			//IL_0752: Unknown result type (might be due to invalid IL or missing references)
			//IL_0757: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ed: Expected O, but got Unknown
			//IL_07f2: Expected O, but got Unknown
			//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0804: Unknown result type (might be due to invalid IL or missing references)
			//IL_0816: Unknown result type (might be due to invalid IL or missing references)
			//IL_0825: Unknown result type (might be due to invalid IL or missing references)
			//IL_082f: Expected O, but got Unknown
			//IL_082a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0834: Expected O, but got Unknown
			//IL_0839: Expected O, but got Unknown
			//IL_0891: Unknown result type (might be due to invalid IL or missing references)
			//IL_0896: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08da: Expected O, but got Unknown
			//IL_08d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_08df: Expected O, but got Unknown
			//IL_08e4: Expected O, but got Unknown
			//IL_0924: Unknown result type (might be due to invalid IL or missing references)
			//IL_0942: Unknown result type (might be due to invalid IL or missing references)
			//IL_0947: Unknown result type (might be due to invalid IL or missing references)
			//IL_094d: Expected O, but got Unknown
			//IL_094f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0954: Unknown result type (might be due to invalid IL or missing references)
			//IL_095a: Expected O, but got Unknown
			//IL_095c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0961: Unknown result type (might be due to invalid IL or missing references)
			//IL_0967: Expected O, but got Unknown
			//IL_0967: Unknown result type (might be due to invalid IL or missing references)
			//IL_0971: Expected O, but got Unknown
			//IL_098f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0994: Unknown result type (might be due to invalid IL or missing references)
			//IL_099f: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3a: Expected O, but got Unknown
			//IL_0a3f: Expected O, but got Unknown
			//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7c: Expected O, but got Unknown
			//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a81: Expected O, but got Unknown
			//IL_0a86: Expected O, but got Unknown
			//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b12: Expected O, but got Unknown
			//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c14: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c38: Expected O, but got Unknown
			//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c3d: Expected O, but got Unknown
			//IL_0c42: Expected O, but got Unknown
			//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d01: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d09: Expected O, but got Unknown
			//IL_0d0e: Expected O, but got Unknown
			//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d41: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d4b: Expected O, but got Unknown
			//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d50: Expected O, but got Unknown
			//IL_0d55: Expected O, but got Unknown
			//IL_0da8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0de2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e48: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e58: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e5d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef3: Expected O, but got Unknown
			//IL_0ef8: Expected O, but got Unknown
			//IL_0ef8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f2b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f35: Expected O, but got Unknown
			//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3a: Expected O, but got Unknown
			//IL_0f3f: Expected O, but got Unknown
			//IL_0f78: Unknown result type (might be due to invalid IL or missing references)
			NameScope val142 = _scope0;
			NameScope val143 = _scope1;
			NameScope val144 = _scope2;
			NameScope val145 = _scope3;
			NameScope val146 = _scope4;
			NameScope val147 = _scope5;
			NameScope val148 = _scope6;
			NameScope val149 = _scope7;
			NameScope val150 = _scope8;
			NameScope val151 = _scope9;
			NameScope val152 = _scope10;
			NameScope val153 = _scope11;
			NameScope val154 = _scope12;
			NameScope val155 = _scope13;
			NameScope val156 = _scope14;
			NameScope val157 = _scope15;
			NameScope val158 = _scope16;
			NameScope val159 = _scope17;
			NameScope val160 = _scope18;
			NameScope val161 = _scope19;
			NameScope val162 = _scope20;
			NameScope val163 = _scope21;
			NameScope val164 = _scope22;
			NameScope val165 = _scope23;
			NameScope val166 = _scope24;
			NameScope val167 = _scope25;
			NameScope val168 = _scope26;
			NameScope val169 = _scope27;
			NameScope val170 = _scope28;
			NameScope val171 = _scope29;
			NameScope val172 = _scope30;
			NameScope val173 = _scope31;
			NameScope val174 = _scope32;
			NameScope val175 = _scope33;
			NameScope val176 = _scope34;
			NameScope val177 = _scope35;
			NameScope val178 = _scope36;
			NameScope val179 = _scope37;
			NameScope val180 = _scope38;
			NameScope val181 = _scope39;
			NameScope val182 = _scope40;
			NameScope val183 = _scope41;
			NameScope val184 = _scope42;
			NameScope val185 = _scope43;
			NameScope val186 = _scope44;
			NameScope val187 = _scope45;
			NameScope val188 = _scope46;
			NameScope val189 = _scope47;
			NameScope val190 = _scope48;
			NameScope val191 = _scope49;
			NameScope val192 = _scope50;
			NameScope val193 = _scope51;
			NameScope val194 = _scope52;
			NameScope val195 = _scope53;
			NameScope val196 = _scope54;
			NameScope val197 = _scope55;
			NameScope val198 = _scope56;
			NameScope val199 = _scope57;
			NameScope val200 = _scope58;
			NameScope val201 = _scope59;
			NameScope val202 = _scope60;
			NameScope val203 = _scope61;
			NameScope val204 = _scope62;
			NameScope val205 = _scope63;
			NameScope val206 = _scope64;
			NameScope val207 = _scope65;
			NameScope val208 = _scope66;
			NameScope val209 = _scope67;
			NameScope val210 = _scope68;
			NameScope val211 = _scope69;
			NameScope val212 = _scope70;
			NameScope val213 = _scope71;
			NameScope val214 = _scope72;
			NameScope val215 = _scope73;
			NameScope val216 = _scope74;
			NameScope val217 = _scope75;
			NameScope val218 = _scope76;
			NameScope val219 = _scope77;
			NameScope val220 = _scope78;
			NameScope val221 = _scope79;
			NameScope val222 = _scope80;
			NameScope val223 = _scope81;
			NameScope val224 = _scope82;
			NameScope val225 = _scope83;
			NameScope val226 = _scope84;
			NameScope val227 = _scope85;
			NameScope val228 = _scope86;
			NameScope val229 = _scope87;
			NameScope val230 = _scope88;
			NameScope val231 = _scope89;
			NameScope val232 = _scope90;
			NameScope val233 = _scope91;
			NameScope val234 = _scope92;
			NameScope val235 = _scope93;
			NameScope val236 = _scope94;
			NameScope val237 = _scope95;
			NameScope val238 = _scope96;
			NameScope val239 = _scope97;
			NameScope val240 = _scope98;
			NameScope val241 = _scope99;
			NameScope val242 = _scope100;
			NameScope val243 = _scope101;
			NameScope val244 = _scope102;
			NameScope val245 = _scope103;
			NameScope val246 = _scope104;
			NameScope val247 = _scope105;
			NameScope val248 = _scope106;
			NameScope val249 = _scope107;
			NameScope val250 = _scope108;
			NameScope val251 = _scope109;
			NameScope val252 = _scope110;
			NameScope val253 = _scope111;
			NameScope val254 = _scope112;
			NameScope val255 = _scope113;
			NameScope val256 = _scope114;
			NameScope val257 = _scope115;
			NameScope val258 = _scope116;
			NameScope val259 = _scope117;
			NameScope val260 = _scope118;
			NameScope val261 = _scope119;
			NameScope val262 = _scope120;
			NameScope val263 = _scope121;
			NameScope val264 = _scope122;
			NameScope val265 = _scope123;
			NameScope val266 = _scope124;
			NameScope val267 = _scope125;
			NameScope val268 = _scope126;
			NameScope val269 = _scope127;
			NameScope val270 = _scope128;
			NameScope val271 = _scope129;
			NameScope val272 = _scope130;
			NameScope val273 = _scope131;
			NameScope val274 = _scope132;
			NameScope val275 = _scope133;
			NameScope val276 = _scope134;
			NameScope val277 = _scope135;
			NameScope val278 = _scope136;
			NameScope val279 = _scope137;
			NameScope val280 = _scope138;
			NameScope val281 = _scope139;
			NameScope val282 = _scope140;
			NameScope val283 = _scope141;
			NameScope val284 = _scope142;
			NameScope val285 = _scope143;
			NameScope val286 = _scope144;
			NameScope val287 = _scope145;
			NameScope val288 = _scope146;
			NameScope val289 = _scope147;
			NameScope val290 = _scope148;
			NameScope val291 = _scope149;
			NameScope val292 = _scope150;
			NameScope val293 = _scope151;
			NameScope val294 = _scope152;
			NameScope val295 = _scope153;
			NameScope val296 = _scope154;
			NameScope val297 = _scope155;
			NameScope val298 = _scope156;
			NameScope val299 = _scope157;
			NameScope val300 = _scope158;
			NameScope val301 = _scope159;
			StaticResourceExtension val302 = new StaticResourceExtension();
			StaticResourceExtension val303 = new StaticResourceExtension();
			BindingExtension val304 = new BindingExtension();
			SwipeItem val305 = new SwipeItem();
			SwipeItems val306 = new SwipeItems();
			StaticResourceExtension val307 = new StaticResourceExtension();
			Label val308 = new Label();
			Border val309 = new Border();
			BindingExtension val310 = new BindingExtension();
			StaticResourceExtension val311 = new StaticResourceExtension();
			Label val312 = new Label();
			StaticResourceExtension val313 = new StaticResourceExtension();
			Label val314 = new Label();
			Grid val315 = new Grid();
			SwipeView val316 = new SwipeView();
			Border val317 = new Border();
			NameScope val318 = new NameScope();
			NameScope.SetNameScope((BindableObject)(object)val317, (INameScope)(object)val318);
			((Element)val316).transientNamescope = (INameScope)(object)val318;
			((Element)val306).transientNamescope = (INameScope)(object)val318;
			((Element)val305).transientNamescope = (INameScope)(object)val318;
			((INameScope)val318).RegisterName("DeleteSwipeItem", (object)val305);
			if (((Element)val305).StyleId == null)
			{
				((Element)val305).StyleId = "DeleteSwipeItem";
			}
			((Element)val315).transientNamescope = (INameScope)(object)val318;
			((Element)val309).transientNamescope = (INameScope)(object)val318;
			((Element)val308).transientNamescope = (INameScope)(object)val318;
			((Element)val312).transientNamescope = (INameScope)(object)val318;
			((Element)val314).transientNamescope = (INameScope)(object)val318;
			val302.Key = "Card";
			StaticResourceExtension val319 = new StaticResourceExtension
			{
				Key = "Card"
			};
			XamlServiceProvider val320 = new XamlServiceProvider();
			Type? typeFromHandle25 = typeof(IProvideValueTarget);
			int length;
			object[] array26 = new object[(length = parentValues.Length) + 1];
			Array.Copy(parentValues, 0, array26, 1, length);
			array26[0] = val317;
			SimpleValueTargetProvider val321 = new SimpleValueTargetProvider(array26, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[8] { val318, val318, val249, val246, val232, val145, val144, val142 }, (object)root);
			object obj39 = (object)val321;
			val320.Add(typeFromHandle25, (object)val321);
			val320.Add(typeof(IReferenceProvider), obj39);
			val320.Add(typeof(IRootObjectProvider), obj39);
			val320.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(89, 37)));
			object obj40 = val319.ProvideValue((IServiceProvider)val320);
			((BindableObject)val317).SetValue(VisualElement.StyleProperty, (obj40 == null || !typeof(BindingBase).IsAssignableFrom(obj40.GetType())) ? obj40 : obj40);
			((BindableObject)val317).SetValue(View.MarginProperty, (object)new Thickness(0.0, 4.0));
			((BindableObject)val305).SetValue(MenuItem.TextProperty, (object)"Eliminar");
			val303.Key = "Danger";
			StaticResourceExtension val322 = new StaticResourceExtension
			{
				Key = "Danger"
			};
			XamlServiceProvider val323 = new XamlServiceProvider();
			Type? typeFromHandle26 = typeof(IProvideValueTarget);
			int length2;
			object[] array27 = new object[(length2 = parentValues.Length) + 4];
			Array.Copy(parentValues, 0, array27, 4, length2);
			array27[0] = val305;
			array27[1] = val306;
			array27[2] = val316;
			array27[3] = val317;
			SimpleValueTargetProvider val324 = new SimpleValueTargetProvider(array27, (object)SwipeItem.BackgroundColorProperty, (INameScope[])(object)new NameScope[11]
			{
				val318, val318, val318, val318, val318, val249, val246, val232, val145, val144,
				val142
			}, (object)root);
			object obj41 = (object)val324;
			val323.Add(typeFromHandle26, (object)val324);
			val323.Add(typeof(IReferenceProvider), obj41);
			val323.Add(typeof(IRootObjectProvider), obj41);
			val323.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(95, 56)));
			object obj42 = val322.ProvideValue((IServiceProvider)val323);
			((BindableObject)val305).SetValue(SwipeItem.BackgroundColorProperty, (obj42 == null || !typeof(BindingBase).IsAssignableFrom(obj42.GetType())) ? obj42 : obj42);
			val305.Invoked += root.OnDeleteClicked;
			val304.Path = ".";
			XamlServiceProvider val325 = new XamlServiceProvider();
			Type? typeFromHandle27 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val326 = new XmlNamespaceResolver();
			val326.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val326.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val325.Add(typeFromHandle27, (object)new XamlTypeResolver((IXmlNamespaceResolver)val326, typeof(_003CInitializeComponent_003E_anonXamlCDataTemplate_1).Assembly));
			BindingBase val327 = ((IMarkupExtension<BindingBase>)(object)val304).ProvideValue((IServiceProvider)val325);
			((BindableObject)val305).SetBinding(MenuItem.CommandParameterProperty, val327);
			val306.Add((ISwipeItem)(object)val305);
			((BindableObject)val316).SetValue(SwipeView.RightItemsProperty, (object)val306);
			((BindableObject)val315).SetValue(Layout.PaddingProperty, (object)new Thickness(16.0, 12.0));
			((BindableObject)val315).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[3]
			{
				new ColumnDefinition(GridLength.Auto),
				new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(GridLength.Auto)
			}));
			((BindableObject)val309).SetValue(Grid.ColumnProperty, (object)0);
			val307.Key = "PrimaryLight";
			StaticResourceExtension val328 = new StaticResourceExtension
			{
				Key = "PrimaryLight"
			};
			XamlServiceProvider val329 = new XamlServiceProvider();
			Type? typeFromHandle28 = typeof(IProvideValueTarget);
			int length3;
			object[] array28 = new object[(length3 = parentValues.Length) + 4];
			Array.Copy(parentValues, 0, array28, 4, length3);
			array28[0] = val309;
			array28[1] = val315;
			array28[2] = val316;
			array28[3] = val317;
			SimpleValueTargetProvider val330 = new SimpleValueTargetProvider(array28, (object)VisualElement.BackgroundColorProperty, (INameScope[])(object)new NameScope[11]
			{
				val318, val318, val318, val318, val318, val249, val246, val232, val145, val144,
				val142
			}, (object)root);
			object obj43 = (object)val330;
			val329.Add(typeFromHandle28, (object)val330);
			val329.Add(typeof(IReferenceProvider), obj43);
			val329.Add(typeof(IRootObjectProvider), obj43);
			val329.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(102, 49)));
			object obj44 = val328.ProvideValue((IServiceProvider)val329);
			((BindableObject)val309).SetValue(VisualElement.BackgroundColorProperty, (obj44 == null || !typeof(BindingBase).IsAssignableFrom(obj44.GetType())) ? obj44 : obj44);
			((BindableObject)val309).SetValue(Border.StrokeThicknessProperty, (object)0.0);
			((BindableObject)val309).SetValue(Border.StrokeShapeProperty, (object)new RoundRectangle
			{
				CornerRadius = new CornerRadius(16.0, 16.0, 16.0, 16.0)
			});
			((BindableObject)val309).SetValue(VisualElement.WidthRequestProperty, (object)32.0);
			((BindableObject)val309).SetValue(VisualElement.HeightRequestProperty, (object)32.0);
			((BindableObject)val309).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val308).SetValue(Label.TextProperty, (object)"\ud83d\udcf1");
			((BindableObject)val308).SetValue(Label.FontSizeProperty, (object)16.0);
			((BindableObject)val308).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val308).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val309).SetValue(Border.ContentProperty, (object)val308);
			((Layout)val315).Children.Add((IView)(object)val309);
			((BindableObject)val312).SetValue(Grid.ColumnProperty, (object)1);
			val310.Path = ".";
			XamlServiceProvider val331 = new XamlServiceProvider();
			Type? typeFromHandle29 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val332 = new XmlNamespaceResolver();
			val332.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val332.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val331.Add(typeFromHandle29, (object)new XamlTypeResolver((IXmlNamespaceResolver)val332, typeof(_003CInitializeComponent_003E_anonXamlCDataTemplate_1).Assembly));
			BindingBase val333 = ((IMarkupExtension<BindingBase>)(object)val310).ProvideValue((IServiceProvider)val331);
			((BindableObject)val312).SetBinding(Label.TextProperty, val333);
			val311.Key = "BodyText";
			StaticResourceExtension val334 = new StaticResourceExtension
			{
				Key = "BodyText"
			};
			XamlServiceProvider val335 = new XamlServiceProvider();
			Type? typeFromHandle30 = typeof(IProvideValueTarget);
			int length4;
			object[] array29 = new object[(length4 = parentValues.Length) + 4];
			Array.Copy(parentValues, 0, array29, 4, length4);
			array29[0] = val312;
			array29[1] = val315;
			array29[2] = val316;
			array29[3] = val317;
			SimpleValueTargetProvider val336 = new SimpleValueTargetProvider(array29, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[11]
			{
				val318, val318, val318, val318, val318, val249, val246, val232, val145, val144,
				val142
			}, (object)root);
			object obj45 = (object)val336;
			val335.Add(typeFromHandle30, (object)val336);
			val335.Add(typeof(IReferenceProvider), obj45);
			val335.Add(typeof(IRootObjectProvider), obj45);
			val335.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(115, 48)));
			object obj46 = val334.ProvideValue((IServiceProvider)val335);
			((BindableObject)val312).SetValue(VisualElement.StyleProperty, (obj46 == null || !typeof(BindingBase).IsAssignableFrom(obj46.GetType())) ? obj46 : obj46);
			((BindableObject)val312).SetValue(Label.FontSizeProperty, (object)16.0);
			((BindableObject)val312).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val312).SetValue(View.MarginProperty, (object)new Thickness(12.0, 0.0, 0.0, 0.0));
			((Layout)val315).Children.Add((IView)(object)val312);
			((BindableObject)val314).SetValue(Grid.ColumnProperty, (object)2);
			((BindableObject)val314).SetValue(Label.TextProperty, (object)"›");
			((BindableObject)val314).SetValue(Label.FontSizeProperty, (object)22.0);
			val313.Key = "HintText";
			StaticResourceExtension val337 = new StaticResourceExtension
			{
				Key = "HintText"
			};
			XamlServiceProvider val338 = new XamlServiceProvider();
			Type? typeFromHandle31 = typeof(IProvideValueTarget);
			int length5;
			object[] array30 = new object[(length5 = parentValues.Length) + 4];
			Array.Copy(parentValues, 0, array30, 4, length5);
			array30[0] = val314;
			array30[1] = val315;
			array30[2] = val316;
			array30[3] = val317;
			SimpleValueTargetProvider val339 = new SimpleValueTargetProvider(array30, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[11]
			{
				val318, val318, val318, val318, val318, val249, val246, val232, val145, val144,
				val142
			}, (object)root);
			object obj47 = (object)val339;
			val338.Add(typeFromHandle31, (object)val339);
			val338.Add(typeof(IReferenceProvider), obj47);
			val338.Add(typeof(IRootObjectProvider), obj47);
			val338.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(122, 48)));
			object obj48 = val337.ProvideValue((IServiceProvider)val338);
			((BindableObject)val314).SetValue(VisualElement.StyleProperty, (obj48 == null || !typeof(BindingBase).IsAssignableFrom(obj48.GetType())) ? obj48 : obj48);
			((BindableObject)val314).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((Layout)val315).Children.Add((IView)(object)val314);
			((BindableObject)val316).SetValue(ContentView.ContentProperty, (object)val315);
			((BindableObject)val317).SetValue(Border.ContentProperty, (object)val316);
			return val317;
		};
		((BindableObject)val36).SetValue(ItemsView.ItemTemplateProperty, (object)val35);
		((Layout)val37).Children.Add((IView)(object)val36);
		((Layout)val57).Children.Add((IView)(object)val37);
		val38.Key = "Card";
		StaticResourceExtension val113 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val114 = new XamlServiceProvider();
		Type? typeFromHandle16 = typeof(IProvideValueTarget);
		object[] array17 = new object[0 + 4];
		array17[0] = val47;
		array17[1] = val57;
		array17[2] = val58;
		array17[3] = mainPage;
		SimpleValueTargetProvider val115 = new SimpleValueTargetProvider(array17, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj23 = (object)val115;
		val114.Add(typeFromHandle16, (object)val115);
		val114.Add(typeof(IReferenceProvider), obj23);
		val114.Add(typeof(IRootObjectProvider), obj23);
		val114.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(133, 21)));
		object obj24 = val113.ProvideValue((IServiceProvider)val114);
		((BindableObject)val47).SetValue(VisualElement.StyleProperty, (obj24 == null || !typeof(BindingBase).IsAssignableFrom(obj24.GetType())) ? obj24 : obj24);
		val39.Key = "PageBackgroundLight";
		StaticResourceExtension val116 = new StaticResourceExtension
		{
			Key = "PageBackgroundLight"
		};
		XamlServiceProvider val117 = new XamlServiceProvider();
		Type? typeFromHandle17 = typeof(IProvideValueTarget);
		object[] array18 = new object[0 + 5];
		array18[0] = val41;
		array18[1] = val47;
		array18[2] = val57;
		array18[3] = val58;
		array18[4] = mainPage;
		SimpleValueTargetProvider val118 = new SimpleValueTargetProvider(array18, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Light"), (INameScope[])(object)new NameScope[6] { val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj25 = (object)val118;
		val117.Add(typeFromHandle17, (object)val118);
		val117.Add(typeof(IReferenceProvider), obj25);
		val117.Add(typeof(IRootObjectProvider), obj25);
		val117.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(134, 21)));
		object light5 = val116.ProvideValue((IServiceProvider)val117);
		val41.Light = light5;
		val40.Key = "PageBackgroundDark";
		StaticResourceExtension val119 = new StaticResourceExtension
		{
			Key = "PageBackgroundDark"
		};
		XamlServiceProvider val120 = new XamlServiceProvider();
		Type? typeFromHandle18 = typeof(IProvideValueTarget);
		object[] array19 = new object[0 + 5];
		array19[0] = val41;
		array19[1] = val47;
		array19[2] = val57;
		array19[3] = val58;
		array19[4] = mainPage;
		SimpleValueTargetProvider val121 = new SimpleValueTargetProvider(array19, (object)typeof(AppThemeBindingExtension).GetRuntimeProperty("Dark"), (INameScope[])(object)new NameScope[6] { val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj26 = (object)val121;
		val120.Add(typeFromHandle18, (object)val121);
		val120.Add(typeof(IReferenceProvider), obj26);
		val120.Add(typeof(IRootObjectProvider), obj26);
		val120.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(134, 21)));
		object dark5 = val119.ProvideValue((IServiceProvider)val120);
		val41.Dark = dark5;
		XamlServiceProvider val122 = new XamlServiceProvider();
		val122.Add(typeof(IProvideValueTarget), (object)new ValueTargetProvider((object)val47, (object)VisualElement.BackgroundColorProperty));
		val122.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(134, 21)));
		BindingBase val123 = ((IMarkupExtension<BindingBase>)(object)val41).ProvideValue((IServiceProvider)val122);
		((BindableObject)val47).SetBinding(VisualElement.BackgroundColorProperty, val123);
		((BindableObject)val47).SetValue(Border.PaddingProperty, (object)new Thickness(16.0));
		((BindableObject)val47).SetValue(View.MarginProperty, (object)new Thickness(0.0, 8.0, 0.0, 0.0));
		((BindableObject)val46).SetValue(StackBase.SpacingProperty, (object)8.0);
		((BindableObject)val43).SetValue(Label.TextProperty, (object)"\ud83d\udca1 Información");
		val42.Key = "CardTitle";
		StaticResourceExtension val124 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val125 = new XamlServiceProvider();
		Type? typeFromHandle19 = typeof(IProvideValueTarget);
		object[] array20 = new object[0 + 6];
		array20[0] = val43;
		array20[1] = val46;
		array20[2] = val47;
		array20[3] = val57;
		array20[4] = val58;
		array20[5] = mainPage;
		SimpleValueTargetProvider val126 = new SimpleValueTargetProvider(array20, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj27 = (object)val126;
		val125.Add(typeFromHandle19, (object)val126);
		val125.Add(typeof(IReferenceProvider), obj27);
		val125.Add(typeof(IRootObjectProvider), obj27);
		val125.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(140, 28)));
		object obj28 = val124.ProvideValue((IServiceProvider)val125);
		((BindableObject)val43).SetValue(VisualElement.StyleProperty, (obj28 == null || !typeof(BindingBase).IsAssignableFrom(obj28.GetType())) ? obj28 : obj28);
		((BindableObject)val43).SetValue(Label.FontSizeProperty, (object)14.0);
		((Layout)val46).Children.Add((IView)(object)val43);
		((BindableObject)val45).SetValue(Label.TextProperty, (object)"• Los SMS recibidos se reenviarán automáticamente a estos números\n• Puedes escribir números manualmente o seleccionarlos desde tus contactos\n• Para configurar permisos avanzados, ve a la sección Diagnósticos\n• Desliza hacia la izquierda en un número para eliminarlo");
		val44.Key = "HintText";
		StaticResourceExtension val127 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val128 = new XamlServiceProvider();
		Type? typeFromHandle20 = typeof(IProvideValueTarget);
		object[] array21 = new object[0 + 6];
		array21[0] = val45;
		array21[1] = val46;
		array21[2] = val47;
		array21[3] = val57;
		array21[4] = val58;
		array21[5] = mainPage;
		SimpleValueTargetProvider val129 = new SimpleValueTargetProvider(array21, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj29 = (object)val129;
		val128.Add(typeFromHandle20, (object)val129);
		val128.Add(typeof(IReferenceProvider), obj29);
		val128.Add(typeof(IRootObjectProvider), obj29);
		val128.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(144, 28)));
		object obj30 = val127.ProvideValue((IServiceProvider)val128);
		((BindableObject)val45).SetValue(VisualElement.StyleProperty, (obj30 == null || !typeof(BindingBase).IsAssignableFrom(obj30.GetType())) ? obj30 : obj30);
		((BindableObject)val45).SetValue(Label.LineBreakModeProperty, (object)(LineBreakMode)1);
		((Layout)val46).Children.Add((IView)(object)val45);
		((BindableObject)val47).SetValue(Border.ContentProperty, (object)val46);
		((Layout)val57).Children.Add((IView)(object)val47);
		val48.Key = "Card";
		StaticResourceExtension val130 = new StaticResourceExtension
		{
			Key = "Card"
		};
		XamlServiceProvider val131 = new XamlServiceProvider();
		Type? typeFromHandle21 = typeof(IProvideValueTarget);
		object[] array22 = new object[0 + 4];
		array22[0] = val56;
		array22[1] = val57;
		array22[2] = val58;
		array22[3] = mainPage;
		SimpleValueTargetProvider val132 = new SimpleValueTargetProvider(array22, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[5] { val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj31 = (object)val132;
		val131.Add(typeFromHandle21, (object)val132);
		val131.Add(typeof(IReferenceProvider), obj31);
		val131.Add(typeof(IRootObjectProvider), obj31);
		val131.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(151, 21)));
		object obj32 = val130.ProvideValue((IServiceProvider)val131);
		((BindableObject)val56).SetValue(VisualElement.StyleProperty, (obj32 == null || !typeof(BindingBase).IsAssignableFrom(obj32.GetType())) ? obj32 : obj32);
		((BindableObject)val56).SetValue(Border.PaddingProperty, (object)new Thickness(16.0));
		((BindableObject)val56).SetValue(View.MarginProperty, (object)new Thickness(0.0, 8.0, 0.0, 0.0));
		((BindableObject)val55).SetValue(StackBase.SpacingProperty, (object)10.0);
		((BindableObject)val50).SetValue(Label.TextProperty, (object)"App de SMS predeterminada");
		val49.Key = "CardTitle";
		StaticResourceExtension val133 = new StaticResourceExtension
		{
			Key = "CardTitle"
		};
		XamlServiceProvider val134 = new XamlServiceProvider();
		Type? typeFromHandle22 = typeof(IProvideValueTarget);
		object[] array23 = new object[0 + 6];
		array23[0] = val50;
		array23[1] = val55;
		array23[2] = val56;
		array23[3] = val57;
		array23[4] = val58;
		array23[5] = mainPage;
		SimpleValueTargetProvider val135 = new SimpleValueTargetProvider(array23, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj33 = (object)val135;
		val134.Add(typeFromHandle22, (object)val135);
		val134.Add(typeof(IReferenceProvider), obj33);
		val134.Add(typeof(IRootObjectProvider), obj33);
		val134.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(157, 28)));
		object obj34 = val133.ProvideValue((IServiceProvider)val134);
		((BindableObject)val50).SetValue(VisualElement.StyleProperty, (obj34 == null || !typeof(BindingBase).IsAssignableFrom(obj34.GetType())) ? obj34 : obj34);
		((BindableObject)val50).SetValue(Label.FontSizeProperty, (object)15.0);
		((Layout)val55).Children.Add((IView)(object)val50);
		((BindableObject)val52).SetValue(Label.TextProperty, (object)"");
		val51.Key = "HintText";
		StaticResourceExtension val136 = new StaticResourceExtension
		{
			Key = "HintText"
		};
		XamlServiceProvider val137 = new XamlServiceProvider();
		Type? typeFromHandle23 = typeof(IProvideValueTarget);
		object[] array24 = new object[0 + 6];
		array24[0] = val52;
		array24[1] = val55;
		array24[2] = val56;
		array24[3] = val57;
		array24[4] = val58;
		array24[5] = mainPage;
		SimpleValueTargetProvider val138 = new SimpleValueTargetProvider(array24, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj35 = (object)val138;
		val137.Add(typeFromHandle23, (object)val138);
		val137.Add(typeof(IReferenceProvider), obj35);
		val137.Add(typeof(IRootObjectProvider), obj35);
		val137.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(161, 28)));
		object obj36 = val136.ProvideValue((IServiceProvider)val137);
		((BindableObject)val52).SetValue(VisualElement.StyleProperty, (obj36 == null || !typeof(BindingBase).IsAssignableFrom(obj36.GetType())) ? obj36 : obj36);
		((BindableObject)val52).SetValue(Label.FontSizeProperty, (object)13.0);
		((BindableObject)val52).SetValue(Label.LineBreakModeProperty, (object)(LineBreakMode)1);
		((Layout)val55).Children.Add((IView)(object)val52);
		((BindableObject)val54).SetValue(Button.TextProperty, (object)"Predeterminada");
		val54.Clicked += mainPage.OnMakeDefaultClicked;
		val53.Key = "PrimaryButton";
		StaticResourceExtension val139 = new StaticResourceExtension
		{
			Key = "PrimaryButton"
		};
		XamlServiceProvider val140 = new XamlServiceProvider();
		Type? typeFromHandle24 = typeof(IProvideValueTarget);
		object[] array25 = new object[0 + 6];
		array25[0] = val54;
		array25[1] = val55;
		array25[2] = val56;
		array25[3] = val57;
		array25[4] = val58;
		array25[5] = mainPage;
		SimpleValueTargetProvider val141 = new SimpleValueTargetProvider(array25, (object)VisualElement.StyleProperty, (INameScope[])(object)new NameScope[7] { val59, val59, val59, val59, val59, val59, val59 }, (object)mainPage);
		object obj37 = (object)val141;
		val140.Add(typeFromHandle24, (object)val141);
		val140.Add(typeof(IReferenceProvider), obj37);
		val140.Add(typeof(IRootObjectProvider), obj37);
		val140.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(167, 29)));
		object obj38 = val139.ProvideValue((IServiceProvider)val140);
		((BindableObject)val54).SetValue(VisualElement.StyleProperty, (obj38 == null || !typeof(BindingBase).IsAssignableFrom(obj38.GetType())) ? obj38 : obj38);
		((BindableObject)val54).SetValue(Button.FontSizeProperty, (object)14.0);
		((Layout)val55).Children.Add((IView)(object)val54);
		((BindableObject)val56).SetValue(Border.ContentProperty, (object)val55);
		((Layout)val57).Children.Add((IView)(object)val56);
		val58.Content = (View)(object)val57;
		((BindableObject)mainPage).SetValue(ContentPage.ContentProperty, (object)val58);
	}
}
