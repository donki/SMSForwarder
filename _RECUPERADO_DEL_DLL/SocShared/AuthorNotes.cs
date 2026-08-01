using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;

namespace SocShared;

/// <summary>
/// Notas de autor integradas en la app (mismo criterio que sOC): un botón flotante 📝
/// visible SOLO en los dispositivos del autor (tablet Samsung / Xiaomi). Al pulsarlo se
/// escribe una nota que se GUARDA SOLA en los datos de la app (author_notes.json),
/// etiquetada con la pantalla actual y la fecha/hora. Se recupera por adb (run-as).
///
/// Implementación: se inyecta un Button real en el árbol visual de cada ContentPage
/// (envolviendo su contenido en un Grid), lo que es mucho más fiable que un IWindowOverlay.
/// Uso: en App.CreateWindow -&gt; var w = new Window(shell); AuthorNotes.Attach(w); return w;
/// </summary>
public static class AuthorNotes
{
	public class Note
	{
		public string time { get; set; } = "";

		public string context { get; set; } = "";

		public string text { get; set; } = "";
	}

	private class NotesFile
	{
		public string app { get; set; } = "?";

		public List<Note> notes { get; set; } = new List<Note>();
	}

	private static readonly string[] AllowedModels = new string[2] { "SM-X130", "24090RA29G" };

	private const string Marker = "__authorNotesRoot";

	private static Page? _hookedPage;

	public static bool DeviceAllowed
	{
		get
		{
			try
			{
				string[] allowedModels = AllowedModels;
				foreach (string b in allowedModels)
				{
					if (string.Equals(DeviceInfo.Model, b, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			catch
			{
			}
			return false;
		}
	}

	private static string FilePath => Path.Combine(FileSystem.AppDataDirectory, "author_notes.json");

	public static void Attach(Window window)
	{
		if (!DeviceAllowed)
		{
			return;
		}
		window.Created += delegate
		{
			Hook(window);
		};
		((BindableObject)window).PropertyChanged += delegate(object? _, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Page")
			{
				Hook(window);
			}
		};
		if (((Element)window).Handler != null)
		{
			Hook(window);
		}
	}

	private static void Hook(Window window)
	{
		try
		{
			Page page = window.Page;
			if (page == null || page == _hookedPage)
			{
				return;
			}
			_hookedPage = page;
			Shell val = (Shell)(object)((page is Shell) ? page : null);
			if (val == null)
			{
				NavigationPage val2 = (NavigationPage)(object)((page is NavigationPage) ? page : null);
				if (val2 != null)
				{
					val2.Pushed += delegate(object? _, NavigationEventArgs e)
					{
						EnsureButton(e.Page);
					};
					val2.Popped += delegate
					{
						EnsureButton(val2.CurrentPage);
					};
					EnsureButton(val2.CurrentPage);
				}
				else
				{
					EnsureButton(page);
				}
			}
			else
			{
				val.Navigated += delegate
				{
					EnsureButton(val.CurrentPage);
				};
				EnsureButton(val.CurrentPage);
			}
		}
		catch
		{
		}
	}

	/// <summary>Envuelve el contenido de la página en un Grid y añade el botón 📝 encima (una sola vez).</summary>
	private static void EnsureButton(Page? page)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		try
		{
			ContentPage cp = (ContentPage)(object)((page is ContentPage) ? page : null);
			if (cp == null || cp.Content == null)
			{
				return;
			}
			View content = cp.Content;
			Grid val = (Grid)(object)((content is Grid) ? content : null);
			if (val == null || !(((Element)val).StyleId == "__authorNotesRoot"))
			{
				View content2 = cp.Content;
				cp.Content = null;
				Grid val2 = new Grid
				{
					StyleId = "__authorNotesRoot"
				};
				((Layout)val2).Add((IView)(object)content2);
				Button val3 = new Button
				{
					Text = "\ud83d\udcdd",
					FontSize = 22.0,
					WidthRequest = 54.0,
					HeightRequest = 54.0,
					CornerRadius = 14,
					Padding = Thickness.op_Implicit(0.0),
					BackgroundColor = Color.FromRgba(20, 22, 30, 230),
					TextColor = Colors.White,
					BorderColor = Color.FromRgba(230, 180, 90, 235),
					BorderWidth = 1.0,
					HorizontalOptions = LayoutOptions.End,
					VerticalOptions = LayoutOptions.End,
					Margin = new Thickness(0.0, 0.0, 16.0, 90.0),
					ZIndex = 999
				};
				val3.Clicked += async delegate
				{
					await ShowEditor((Page)(object)cp);
				};
				((Layout)val2).Add((IView)(object)val3);
				cp.Content = (View)(object)val2;
			}
		}
		catch
		{
		}
	}

	private static async Task ShowEditor(Page page)
	{
		try
		{
			string ctx = ((object)page).GetType().Name;
			string text = await ModernDialog.PromptAsync(page, "\ud83d\udcdd Nota — " + ctx, "Se guarda sola al pulsar Guardar (pantalla y hora incluidas).", "Guardar", "Cancelar", null, "Escribe aquí…");
			if (!string.IsNullOrWhiteSpace(text))
			{
				Save(ctx, text);
			}
		}
		catch
		{
		}
	}

	public static void Save(string context, string text)
	{
		try
		{
			NotesFile notesFile = Load();
			notesFile.notes.Add(new Note
			{
				time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
				context = context,
				text = text
			});
			File.WriteAllText(FilePath, JsonSerializer.Serialize(notesFile, new JsonSerializerOptions
			{
				WriteIndented = true
			}));
		}
		catch
		{
		}
	}

	private static NotesFile Load()
	{
		try
		{
			if (File.Exists(FilePath))
			{
				return JsonSerializer.Deserialize<NotesFile>(File.ReadAllText(FilePath)) ?? Fresh();
			}
		}
		catch
		{
		}
		return Fresh();
	}

	private static NotesFile Fresh()
	{
		string app = "?";
		try
		{
			app = AppInfo.Name;
		}
		catch
		{
		}
		return new NotesFile
		{
			app = app
		};
	}
}
