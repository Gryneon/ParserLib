namespace Terminal.Menu;

public class MenuHandler;

/// <summary>The menu controller. Can be static since there is only one console.</summary>
public static class MenuController
{
  #region Public Members

  public static void StartMenu (string menu_name)
  {
    IsMenuActive = true;
    HideCursor();
    SetCurrentMenu(menu_name);
    Draw(MenuDrawType.None);
  }
  public static void StartMenu (MenuBase menu)
  {
    IsMenuActive = true;
    HideCursor();
    SetCurrentMenu(menu);
    while (IsMenuActive && !IsExiting)
    {
      Draw(MenuDrawType.None);
      Await();
    }
  }
  #endregion
  #region Internal Properties
  internal static bool IsExiting { get; set; }
  internal static bool IsMenuActive { get; set; }
  [MemberNotNullWhen(true, nameof(NextMenu))]
  internal static bool IsChangingMenu { get; set; }
  internal static bool HasMoved { get; set; }
  internal static string? NextMenu { get; set; }
  internal static ConsoleKeyInfo UserKey { get; set; }
  internal static MenuBase? ActiveMenu { get; set; }
  internal static MenuItem? SelectedMenuItem => ActiveMenu?.Items[ActiveMenu.Index];
  internal static int ActiveMenuItemCount => ActiveMenu?.Items.Count ?? 0;
  internal static int LineCount { get; set; }
  internal static int FirstItemLine { get; set; }
  internal static string? CurrentMenuName { get; set; }
  internal static IEnumerable<MenuBase> AllMenus { get; private set; } = [];
  internal static void AddNewMenu (MenuBase menu) => AllMenus = AllMenus.Concat([menu]);
  #endregion
  [MemberNotNull(nameof(ActiveMenu), nameof(CurrentMenuName))]
  internal static void SetCurrentMenu ([NotNull] MenuBase menu)
  {
    menu.ThrowIfNull();
    CurrentMenuName = menu.Name;
    ActiveMenu = menu;
    LineCount = ActiveMenuItemCount;
  }
  [MemberNotNull(nameof(ActiveMenu), nameof(CurrentMenuName))]
  internal static void SetCurrentMenu (string menu_name)
  {
    CurrentMenuName = menu_name;
    ActiveMenu = AllMenus.FirstOrDefault(m => m.Name == CurrentMenuName) ?? throw new ArgumentException($"menu_name: \"{menu_name}\" was not found.");
    FirstItemLine = ActiveMenu.Items.IsEmpty ? ErrVal : ActiveMenu.OptionLines.Start.Value;
    LineCount = ActiveMenuItemCount;
  }
  [MemberNotNull(nameof(NextMenu))]
  internal static void SetNextMenu (string next_menu)
  {
    MenuBase? next = AllMenus.FirstOrDefault(item => item.Name.Like(next_menu));
    next.ThrowIfNull($"No menu with name of '{next_menu}' found.");
    NextMenu = next_menu;
    IsChangingMenu = true;
  }
  internal static void HideCursor () => Console.CursorVisible = false;
  internal static void ShowCursor () => Console.CursorVisible = true;
  internal static void Draw (MenuDrawType type)
  {
    if (type is MenuDrawType.Erase)
    {
      goto MenuLoop;
    }
    if (type is MenuDrawType.Redraw)
    {
      IsChangingMenu = false;
    }

    ActiveMenu.ThrowIfNull();

    if (IsChangingMenu)
    {
      SetCurrentMenu(NextMenu);
      Draw(type: MenuDrawType.Redraw);
      return;
    }

  MenuLoop:

    for (int i = 0; i < ActiveMenuItemCount; i++)
    {
      if (type is MenuDrawType.Erase || ActiveMenu is null)
      {
        EraseLine(FirstItemLine + i, false);
      }
      else if (ActiveMenu.Items[i].IsDirty || type is MenuDrawType.Redraw)
      {
        ActiveMenu.Items[i].Draw(ActiveMenu.Index);
        ActiveMenu.Items[i].IsDirty = false;
      }
    }
  }
  internal static void EraseLine (int line, bool keep_original_pos)
  {
    int current_line = Console.CursorTop;
    int current_pos = Console.CursorLeft;
    Console.CursorTop = line;
    Console.CursorLeft = 0;
    string result = SE;
    int length = Console.WindowWidth;
    for (int i = 0; i < length; i++)
    {
      result += " ";
    }
    Console.WriteLine(result);
    Console.CursorTop = keep_original_pos ? current_line : line;
    Console.CursorLeft = keep_original_pos ? current_pos : 0;
  }
  internal static void CloseMenu ()
  {
    IsMenuActive = false;
    ShowCursor();
    CurrentMenuName = null;
    ActiveMenu = null;
  }
  internal static void Await ()
  {
    ActiveMenu.ThrowIfNull();

    Collection<MenuAction> all = ActiveMenu.AllActions;

    MenuAction? checkForValidKey (ConsoleKeyInfo key)
    {
      ConsoleKey ckey = key.Key;
      foreach (MenuAction data in all)
      {
        if (data.Key == ckey)
        {
          return data;
        }
      }
      return null;
    }
    MenuAction? checkForSelection ()
    {
      foreach (MenuAction data in all)
      {
        if (data.OnSelect)
        {
          return data;
        }
      }
      return null;
    }
    MenuAction? action;
    do
    {
      UserKey = Console.ReadKey();
      action = checkForValidKey(UserKey);
    }
    while (action is null);

    action.Execute();

    if (IsExiting)
      return;

    if (HasMoved && checkForSelection() is MenuAction a)
    {
      a.Execute();
    }

    return;
  }
}
