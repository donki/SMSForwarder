using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace SocShared;

/// <summary>
/// Diálogos NO nativos (modernos) compartidos por todas las apps sOCratic: una tarjeta con esquinas
/// redondeadas sobre un velo oscuro, con animación de entrada, en lugar del AlertDialog del sistema.
/// Reemplaza a Page.DisplayAlert / DisplayActionSheet.
///
/// Uso:
///   bool ok = await SocShared.ModernDialog.AlertAsync(this, "Título", "Mensaje", "Aceptar", "Cancelar");
///   string? sel = await SocShared.ModernDialog.ActionSheetAsync(this, "Título", "Cancelar", "A", "B", "C");
///
/// Tema-aware (claro/oscuro) y usa el color "Primary" de la app si existe.
/// </summary>
public static class ModernDialog
{
	private const string OverlayId = "__modernDialogOverlay";

	private static bool IsDark
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			Application current = Application.Current;
			if (current == null)
			{
				return false;
			}
			return (int)current.RequestedTheme == 2;
		}
	}

	private static Color CardColor
	{
		get
		{
			if (!IsDark)
			{
				return Colors.White;
			}
			return Color.FromArgb("#1E2228");
		}
	}

	private static Color TextColor
	{
		get
		{
			if (!IsDark)
			{
				return Color.FromArgb("#1C2530");
			}
			return Color.FromArgb("#ECEFF3");
		}
	}

	private static Color MutedColor
	{
		get
		{
			if (!IsDark)
			{
				return Color.FromArgb("#5B6773");
			}
			return Color.FromArgb("#9AA6B2");
		}
	}

	private static Color ScrimColor => Color.FromRgba(0.0, 0.0, 0.0, IsDark ? 0.62 : 0.45);

	private static Color Accent()
	{
		Application current = Application.Current;
		object obj = default(object);
		if (((current != null) ? current.Resources : null) != null && Application.Current.Resources.TryGetValue("Primary", ref obj))
		{
			Color val = (Color)((obj is Color) ? obj : null);
			if (val != null)
			{
				return val;
			}
		}
		return Color.FromArgb("#3B82F6");
	}

	/// <summary>Aviso/confirmación. Devuelve true si se pulsa "accept"; false si "cancel" o se descarta.</summary>
	public static Task<bool> AlertAsync(Page page, string title, string message, string accept, string? cancel = null)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
		Button val = MakeButton(accept, Accent(), Colors.White);
		Button val2 = ((cancel == null) ? null : MakeButton(cancel, Color.FromArgb(IsDark ? "#2A2F37" : "#EDF0F3"), TextColor));
		HorizontalStackLayout val3 = new HorizontalStackLayout
		{
			Spacing = 10.0,
			HorizontalOptions = LayoutOptions.End
		};
		if (val2 != null)
		{
			((Layout)val3).Add((IView)(object)val2);
		}
		((Layout)val3).Add((IView)(object)val);
		Border card = BuildCard(title, message, (View)(object)val3);
		Grid overlay = BuildOverlay(page, card, delegate
		{
			Close(page, delegate
			{
				tcs.TrySetResult(result: false);
			});
		});
		val.Clicked += delegate
		{
			Close(page, delegate
			{
				tcs.TrySetResult(result: true);
			});
		};
		if (val2 != null)
		{
			val2.Clicked += delegate
			{
				Close(page, delegate
				{
					tcs.TrySetResult(result: false);
				});
			};
		}
		Present(page, overlay, card);
		return tcs.Task;
	}

	/// <summary>Lista de opciones (reemplaza DisplayActionSheet). Devuelve la opción elegida o null.</summary>
	public static Task<string?> ActionSheetAsync(Page page, string? title, string cancel, params string[] options)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		TaskCompletionSource<string?> tcs = new TaskCompletionSource<string>();
		VerticalStackLayout val = new VerticalStackLayout
		{
			Spacing = 8.0
		};
		foreach (string text in options)
		{
			Button val2 = MakeButton(text, Color.FromArgb(IsDark ? "#2A2F37" : "#F1F4F7"), TextColor);
			((View)val2).HorizontalOptions = LayoutOptions.Fill;
			string captured = text;
			val2.Clicked += delegate
			{
				Close(page, delegate
				{
					tcs.TrySetResult(captured);
				});
			};
			((Layout)val).Add((IView)(object)val2);
		}
		Button val3 = MakeButton(cancel, Color.FromArgb(IsDark ? "#3A2A2E" : "#FBECEC"), Color.FromArgb("#C0392B"));
		((View)val3).HorizontalOptions = LayoutOptions.Fill;
		val3.Clicked += delegate
		{
			Close(page, delegate
			{
				tcs.TrySetResult(null);
			});
		};
		((Layout)val).Add((IView)(object)val3);
		Border card = BuildCard(title, null, (View)(object)val);
		Grid overlay = BuildOverlay(page, card, delegate
		{
			Close(page, delegate
			{
				tcs.TrySetResult(null);
			});
		});
		Present(page, overlay, card);
		return tcs.Task;
	}

	/// <summary>Entrada de texto (reemplaza DisplayPromptAsync). Devuelve el texto o null si se cancela.</summary>
	public static Task<string?> PromptAsync(Page page, string title, string? message, string accept = "OK", string cancel = "Cancel", string? initialValue = null, string? placeholder = null)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		TaskCompletionSource<string?> tcs = new TaskCompletionSource<string>();
		Entry entry = new Entry
		{
			Text = (initialValue ?? string.Empty),
			Placeholder = (placeholder ?? string.Empty),
			TextColor = TextColor,
			BackgroundColor = Color.FromArgb(IsDark ? "#12151A" : "#F1F4F7")
		};
		Button val = MakeButton(accept, Accent(), Colors.White);
		Button val2 = MakeButton(cancel, Color.FromArgb(IsDark ? "#2A2F37" : "#EDF0F3"), TextColor);
		HorizontalStackLayout val3 = new HorizontalStackLayout
		{
			Spacing = 10.0,
			HorizontalOptions = LayoutOptions.End
		};
		((Layout)val3).Add((IView)(object)val2);
		((Layout)val3).Add((IView)(object)val);
		VerticalStackLayout val4 = new VerticalStackLayout
		{
			Spacing = 12.0
		};
		((Layout)val4).Add((IView)(object)entry);
		((Layout)val4).Add((IView)(object)val3);
		Border card = BuildCard(title, message, (View)(object)val4);
		Grid overlay = BuildOverlay(page, card, delegate
		{
			Close(page, delegate
			{
				tcs.TrySetResult(null);
			});
		});
		val.Clicked += delegate
		{
			Close(page, delegate
			{
				tcs.TrySetResult(((InputView)entry).Text ?? string.Empty);
			});
		};
		val2.Clicked += delegate
		{
			Close(page, delegate
			{
				tcs.TrySetResult(null);
			});
		};
		Present(page, overlay, card);
		return tcs.Task;
	}

	private static Border BuildCard(string? title, string? message, View content)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		VerticalStackLayout val = new VerticalStackLayout
		{
			Spacing = 14.0
		};
		if (!string.IsNullOrEmpty(title))
		{
			((Layout)val).Add((IView)new Label
			{
				Text = title,
				FontSize = 19.0,
				FontAttributes = (FontAttributes)1,
				TextColor = TextColor
			});
		}
		if (!string.IsNullOrEmpty(message))
		{
			((Layout)val).Add((IView)new Label
			{
				Text = message,
				FontSize = 15.0,
				TextColor = MutedColor
			});
		}
		((Layout)val).Add((IView)(object)content);
		return new Border
		{
			Content = (View)(object)val,
			BackgroundColor = CardColor,
			Stroke = Brush.op_Implicit(Color.FromRgba(255.0, 255.0, 255.0, IsDark ? 0.08 : 0.0)),
			StrokeThickness = (IsDark ? 1 : 0),
			StrokeShape = (IShape)new RoundRectangle
			{
				CornerRadius = CornerRadius.op_Implicit(18.0)
			},
			Padding = new Thickness(22.0, 20.0),
			Margin = new Thickness(28.0, 0.0),
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center,
			MaximumWidthRequest = 420.0,
			Shadow = new Shadow
			{
				Brush = (Brush)new SolidColorBrush(Colors.Black),
				Opacity = 0.35f,
				Radius = 24f,
				Offset = new Point(0.0, 8.0)
			},
			Opacity = 0.0,
			Scale = 0.92
		};
	}

	private static Grid BuildOverlay(Page page, Border card, Action onScrim)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		BoxView val = new BoxView
		{
			Color = ScrimColor,
			Opacity = 0.0
		};
		TapGestureRecognizer val2 = new TapGestureRecognizer();
		val2.Tapped += delegate
		{
			onScrim();
		};
		((View)val).GestureRecognizers.Add((IGestureRecognizer)(object)val2);
		Grid val3 = new Grid
		{
			StyleId = "__modernDialogOverlay"
		};
		((Layout)val3).Add((IView)(object)val);
		((Layout)val3).Add((IView)(object)card);
		return val3;
	}

	private static Button MakeButton(string text, Color bg, Color fg)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		return new Button
		{
			Text = text,
			BackgroundColor = bg,
			TextColor = fg,
			FontSize = 15.0,
			FontAttributes = (FontAttributes)1,
			CornerRadius = 12,
			Padding = new Thickness(18.0, 10.0),
			MinimumHeightRequest = 44.0
		};
	}

	private static void Present(Page page, Grid overlay, Border card)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		Grid val = HostGrid(page);
		if (val != null)
		{
			if (((DefinitionCollection<RowDefinition>)(object)val.RowDefinitions).Count > 0)
			{
				Grid.SetRowSpan((BindableObject)(object)overlay, ((DefinitionCollection<RowDefinition>)(object)val.RowDefinitions).Count);
			}
			if (((DefinitionCollection<ColumnDefinition>)(object)val.ColumnDefinitions).Count > 0)
			{
				Grid.SetColumnSpan((BindableObject)(object)overlay, ((DefinitionCollection<ColumnDefinition>)(object)val.ColumnDefinitions).Count);
			}
			((Layout)val).Add((IView)(object)overlay);
			ViewExtensions.FadeTo((VisualElement)(BoxView)((Layout)overlay).Children[0], 1.0, 160u, Easing.CubicOut);
			ViewExtensions.FadeTo((VisualElement)(object)card, 1.0, 180u, Easing.CubicOut);
			ViewExtensions.ScaleTo((VisualElement)(object)card, 1.0, 200u, Easing.CubicOut);
		}
	}

	private static async void Close(Page page, Action complete)
	{
		Grid host = HostGrid(page);
		Grid overlay = ((host != null) ? ((Layout)host).Children.OfType<Grid>().FirstOrDefault((Grid g) => ((Element)g).StyleId == "__modernDialogOverlay") : null);
		if (overlay != null)
		{
			Border val = ((Layout)overlay).Children.OfType<Border>().FirstOrDefault();
			if (val != null)
			{
				ViewExtensions.ScaleTo((VisualElement)(object)val, 0.92, 120u, Easing.CubicIn);
				await ViewExtensions.FadeTo((VisualElement)(object)val, 0.0, 120u, Easing.CubicIn);
			}
			((Layout)host).Remove((IView)(object)overlay);
		}
		complete();
	}

	/// <summary>Rejilla anfitriona: envuelve el contenido de la página en un Grid (una vez) para poder
	/// superponer el overlay. Reutiliza el Grid si ya lo hay (incluido el de AuthorNotes).</summary>
	private static Grid? HostGrid(Page page)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		ContentPage val = (ContentPage)(object)((page is ContentPage) ? page : null);
		if (val == null)
		{
			return null;
		}
		View content = val.Content;
		Grid val2 = (Grid)(object)((content is Grid) ? content : null);
		if (val2 != null)
		{
			return val2;
		}
		View content2 = val.Content;
		Grid val3 = new Grid();
		val.Content = null;
		if (content2 != null)
		{
			((Layout)val3).Add((IView)(object)content2);
		}
		val.Content = (View)(object)val3;
		return val3;
	}
}
